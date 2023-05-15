using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using FMODUnity;

public class MainMenu : MonoBehaviour
{
	private GameObject rootMenuPanel;
	private GameObject preparePanel;
	private GameObject networkPanel;
	private GameObject messageBox;
	private TextMeshProUGUI messageBoxMsg;

	public GameObject[] bodies;
	public Texture2D[] faces;

	private Slider bodySlider;
	private Slider faceSlider;
	public Vector3 previewPlace;
	public Vector3[] allPreviewPlaces;
	private GameObject previewPlayer;
	private GameObject[] allPreviewPlayers;
	private int[,] previewIndicies;

	private TMP_InputField nameInput;

	private TextMeshProUGUI playerName;
	private TextMeshProUGUI t1p1Name;
	private TextMeshProUGUI t1p2Name;
	private TextMeshProUGUI t2p1Name;
	private TextMeshProUGUI t2p2Name;

	private NetworkManager networkManager;
	private MessageQueue msgQueue;
	private GameManager gameManager;

	private TMP_InputField chatInput;
    private TMP_Text chatOutput;
    private Button sendButton;

	public EventReference soundJoin;
	public EventReference soundReady;
	public EventReference soundQuit;
	public EventReference soundSendMsg;
	public EventReference soundMenu;
	private FMOD.Studio.EventInstance instance;

	void Start()
	{
		instance = RuntimeManager.CreateInstance(soundMenu);
		instance.start();

		previewIndicies = new int[4, 2]
		{
			{ 0, 0 },
			{ 0, 0 },
			{ 0, 0 },
			{ 0, 0 }
		};
		allPreviewPlayers = new GameObject[4];

		rootMenuPanel = GameObject.Find("RootMenu");
		preparePanel = GameObject.Find("PrepareMenu");
		networkPanel = GameObject.Find("NetworkMenu");
        messageBox = GameObject.Find("MessageBox");
        messageBoxMsg = messageBox.transform.Find("Message").gameObject.GetComponent<TextMeshProUGUI>();

		nameInput = GameObject.Find("NameInput").GetComponent<TMP_InputField>();

		bodySlider = GameObject.Find("BodySlider").GetComponent<Slider>();
		bodySlider.maxValue = bodies.Length - 1;
		faceSlider = GameObject.Find("FaceSlider").GetComponent<Slider>();
		faceSlider.maxValue = faces.Length - 1;

		t1p1Name = GameObject.Find("T1P1Name").GetComponent<TextMeshProUGUI>();
        t1p2Name = GameObject.Find("T1P2Name").GetComponent<TextMeshProUGUI>();
        t2p1Name = GameObject.Find("T2P1Name").GetComponent<TextMeshProUGUI>();
        t2p2Name = GameObject.Find("T2P2Name").GetComponent<TextMeshProUGUI>();

        networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
		msgQueue = networkManager.GetComponent<MessageQueue>();
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

		msgQueue.AddCallback(Constants.SMSG_JOIN, OnResponseJoin);
        msgQueue.AddCallback(Constants.SMSG_SETNAME, OnResponseSetName);
		msgQueue.AddCallback(Constants.SMSG_LEAVE, OnResponseLeave);
		msgQueue.AddCallback(Constants.SMSG_READY, OnResponseStart);
        msgQueue.AddCallback(Constants.SMSG_CHAT, OnResponseChat);

        chatInput = GameObject.Find("msgInput").GetComponent<TMP_InputField>();
        chatOutput = GameObject.Find("msgDisplay").GetComponent<TMP_Text>();
        sendButton = GameObject.Find("msgEnter").GetComponentInChildren<Button>();

        sendButton.onClick.AddListener(delegate { OnSendButton(); });
        chatInput.onEndEdit.AddListener(delegate { OnChatInputEnd (); });

		rootMenuPanel.SetActive(true);
		preparePanel.SetActive(false);
		networkPanel.SetActive(false);
        messageBox.SetActive(false);
    }


	public void OnPrepareClick()
	{
		networkManager.connect();
        if (!networkManager.isConnected())
		{
			messageBoxMsg.text = "Unable to connect to server.";
			rootMenuPanel.SetActive(false);
			messageBox.SetActive(true);
		}
		else {
			rootMenuPanel.SetActive(false);
			preparePanel.SetActive(true);
			onBodyChange();
		}
	}

	public void OnExitClick()
	{
		#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
		#else
			Application.Quit();
		#endif
	}

	public void onBodyChange()
    {
		Destroy(previewPlayer);
		previewPlayer = Instantiate(bodies[(int)bodySlider.value], previewPlace, Quaternion.Euler(0,180,0));
		previewPlayer.transform.localScale = new Vector3(5, 5, 5);
		onFaceChange();
	}

	public void onFaceChange()
	{
		previewPlayer.transform.GetChild(1).GetComponent<Renderer>().materials[1].SetTexture("_MainTex", faces[(int) faceSlider.value]);
	}

	public void onJoinClick()
    {
		Debug.Log("Joining the room");
        networkManager.SendJoinRequest();
		Destroy(previewPlayer);
	}

	public void OnResponseJoin(ExtendedEventArgs eventArgs)
	{
		Debug.Log("OnResponseJoin called");
		ResponseJoinEventArgs args = eventArgs as ResponseJoinEventArgs;
		if (args.status == 0)
		{
			RuntimeManager.PlayOneShot(soundJoin);
			List<UserData> users = args.users;

			if (args.user_id == 1)
			{
				playerName = t1p1Name;
			}
			else if (args.user_id == 2)
			{
				playerName = t1p2Name;
			}
			else if (args.user_id == 3)
			{
				playerName = t2p1Name;
			}
			else 
			{
				playerName = t2p2Name;
			}

			foreach (UserData user in args.users) {
				if (user.UserId == 1) {
					t1p1Name.text = user.UserName;
				}
				else if (user.UserId == 2) {
					t1p2Name.text = user.UserName;
				}
				else if (user.UserId == 3) {
					t2p1Name.text = user.UserName;
				}
				else if (user.UserId == 4) {
					t2p2Name.text = user.UserName;
				}
				else {
					Debug.Log("Something went wrong with setting other players name when a new player join");
				}
			}

			Constants.USER_ID = args.user_id;
			Debug.Log("Constants.USER_ID is " + Constants.USER_ID);
			string name = nameInput.text;
			if (name == "") name = "No Name";
			playerName.text = name;
			t2p2Name.text = name;
			
			Debug.Log("playerName.text is " + playerName.text);

			networkManager.SendSetNameRequest((int)bodySlider.value, (int)faceSlider.value, name);
			// playerName.text = nameInput.text;

			preparePanel.SetActive(false);
			networkPanel.SetActive(true);
		}
		
		else
		{
			messageBoxMsg.text = "Server is full.";
			messageBox.SetActive(true);
			preparePanel.SetActive(false);
		}

	}

	public void OnResponseSetName(ExtendedEventArgs eventArgs)
	{
		ResponseSetNameEventArgs args = eventArgs as ResponseSetNameEventArgs;
		RuntimeManager.PlayOneShot(soundJoin);
		if (args.user_id == 1)
		{
			t1p1Name.text = args.name;
		}
		else if (args.user_id == 2)
		{
			t1p2Name.text = args.name;
		}
		else if (args.user_id == 3)
		{
			t2p1Name.text = args.name;
		}
		else
		{
			t2p2Name.text = args.name;
		}

		int curUser = args.user_id - 1;
		previewIndicies[curUser, 0] = args.bodyIndex;
		previewIndicies[curUser, 1] = args.faceIndex;
		allPreviewPlayers[curUser] = Instantiate(bodies[args.bodyIndex], allPreviewPlaces[curUser], Quaternion.Euler(0, 180, 0));
		allPreviewPlayers[curUser].transform.GetChild(1).GetComponent<Renderer>().materials[1].SetTexture("_MainTex", faces[args.faceIndex]);
	}

	private void deleteAllPreviews()
	{
		for (int i = 0; i < allPreviewPlayers.Length; i++)
		{
			if (allPreviewPlayers[i] != null)
			{
				Destroy(allPreviewPlayers[i]);
				allPreviewPlayers[i] = null;
			}
		}
	}

	public void OnLeavePrepare()
	{
		Destroy(previewPlayer);
		rootMenuPanel.SetActive(true);
		preparePanel.SetActive(false);
	}

	public void OnLeaveRoom()
	{
		Debug.Log("Send LeaveReq");
		networkManager.SendLeaveRequest();
		rootMenuPanel.SetActive(true);
		networkPanel.SetActive(false);
		deleteAllPreviews();
		RuntimeManager.PlayOneShot(soundQuit);
	}

	public void OnResponseLeave(ExtendedEventArgs eventArgs)
	{
		ResponseLeaveEventArgs args = eventArgs as ResponseLeaveEventArgs;
		if (args.user_id != Constants.USER_ID)
		{
			if (args.user_id == 1) {
				t1p1Name.text = "Waiting for others";
			}
			else if (args.user_id == 2) {
				t1p2Name.text = "Waiting for others";
			}
			else if (args.user_id == 3) {
				t2p1Name.text = "Waiting for others";
			}
			else{
				t2p2Name.text = "Waiting for others";
			}

			int curUser = args.user_id - 1;
			Destroy(allPreviewPlayers[curUser]);
			allPreviewPlayers[curUser] = null;
		}
		
	}

    public void OnStartClick()
	{
		networkManager.SendReadyRequest();
	}

	public void OnResponseStart(ExtendedEventArgs eventArgs)
	{
		RuntimeManager.PlayOneShot(soundReady);

		instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		instance.release();
		StartCoroutine(DoDelay());
	}


	public void OnSendButton() {
        if (chatInput.text.Length > 0) {
			string message = playerName.text + ": " + chatInput.text;
			Debug.Log("chat message received: " + message);
            networkManager.SendChatRequest(message);
            //chatOutput.text += "\n" + message;
            chatInput.text = "";
        }
		
    }

    public void OnChatInputEnd () {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) {
        	Debug.Log("OnChatInputEnd() is called by enter the input");
			OnSendButton ();
        }
    }

	public void OnResponseChat(ExtendedEventArgs eventArgs) {
		ResponseChatEventArgs args = eventArgs as ResponseChatEventArgs;
		chatOutput.text += "\n" + args.message;

		if (args.user_id != Constants.USER_ID)
		{
			RuntimeManager.PlayOneShot(soundSendMsg);
		}
	}

	public void OnOKClick()
	{
		rootMenuPanel.SetActive(true);
		messageBox.SetActive(false);
		preparePanel.SetActive(false);
		networkPanel.SetActive(false);
	}

	private void StartNetworkGame()
	{
		Player t1p1 = new Player(1, t1p1Name.text, new Color(0.9f, 0.1f, 0.1f), Constants.USER_ID == 1);
		Player t1p2 = new Player(2, t1p2Name.text, new Color(0.2f, 0.2f, 1.0f), Constants.USER_ID == 2);
		Player t2p1 = new Player(3, t2p1Name.text, new Color(0.9f, 0.1f, 0.1f), Constants.USER_ID == 3);
		Player t2p2 = new Player(4, t2p2Name.text, new Color(0.2f, 0.2f, 1.0f), Constants.USER_ID == 4);
		gameManager.Init(t1p1, t1p2, t2p1, t2p2, Constants.USER_ID, bodies, faces, previewIndicies);
		
		SceneManager.LoadScene("map");
	}

	IEnumerator DoDelay()
    {
		messageBox.SetActive(true);
		//messageBoxMsg.text = "Game Will Start in 3 Seconds!";
  //      yield return new WaitForSeconds(1.0f);
  //      messageBoxMsg.text = "Game Will Start in 2 Seconds!";
  //      yield return new WaitForSeconds(1.0f);
		messageBoxMsg.text = "Game Will Start in 1 Seconds!";
		yield return new WaitForSeconds(1.0f);
		StartNetworkGame();
    }
	
}
