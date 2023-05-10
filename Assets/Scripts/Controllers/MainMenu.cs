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
	private GameObject networkMenuPanel;
	private GameObject chatPanel;
	private GameObject messageBox;
	private TMPro.TextMeshProUGUI messageBoxMsg;

	private TMPro.TextMeshProUGUI t1p1Name;
	private TMPro.TextMeshProUGUI t1p2Name;
	private TMPro.TextMeshProUGUI t2p1Name;
	private TMPro.TextMeshProUGUI t2p2Name;

	private GameObject t1p1Input;
	private GameObject t1p2Input;
	private GameObject t2p1Input;
	private GameObject t2p2Input;

	public TMP_InputField inputField;

	private TMPro.TextMeshProUGUI playerName;
	private GameObject playerInput;

	private string t1p1NameDefault = "T1 Player 1";
	private string t1p2NameDefault = "T1 Player 2";
	private string t2p1NameDefault = "T2 Player 1";
	private string t2p2NameDefault = "T2 Player 2";
	private string playerNameDefault;

	private NetworkManager networkManager;
	private MessageQueue msgQueue;

	private bool t1p1Ready = false;
	private bool t1p2Ready = false;
	private bool t2p1Ready = false;
	private bool t2p2Ready = false;

	private TMP_InputField chatInput;
    private TMP_Text chatOutput;
    private Button sendButton;

	public EventReference soundRegular;
	public EventReference soundJoin;
	public EventReference soundReady;
	public EventReference soundQuit;
	public EventReference soundSendMsg;

	void Start()
    {
		rootMenuPanel = GameObject.Find("RootMenu");
		networkMenuPanel = GameObject.Find("NetworkMenu");
		chatPanel = GameObject.Find("ChatMenu");

		messageBox = GameObject.Find("MessageBox");
		messageBoxMsg = messageBox.transform.Find("Message").gameObject.GetComponent<TMPro.TextMeshProUGUI>();

		t1p1Name = GameObject.Find("T1P1Name").GetComponent<TMPro.TextMeshProUGUI>();
		t1p2Name = GameObject.Find("T1P2Name").GetComponent<TMPro.TextMeshProUGUI>();
		t2p1Name = GameObject.Find("T2P1Name").GetComponent<TMPro.TextMeshProUGUI>();
		t2p2Name = GameObject.Find("T2P2Name").GetComponent<TMPro.TextMeshProUGUI>();
		
		t1p1Input = GameObject.Find("T1P1Input");
		t1p2Input = GameObject.Find("T1P2Input");
		t2p1Input = GameObject.Find("T2P1Input");
		t2p2Input = GameObject.Find("T2P2Input");

		networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
		msgQueue = networkManager.GetComponent<MessageQueue>();

		msgQueue.AddCallback(Constants.SMSG_JOIN, OnResponseJoin);
		msgQueue.AddCallback(Constants.SMSG_LEAVE, OnResponseLeave);
		msgQueue.AddCallback(Constants.SMSG_SETNAME, OnResponseSetName);
		msgQueue.AddCallback(Constants.SMSG_READY, OnResponseReady);
		msgQueue.AddCallback(Constants.SMSG_CHAT, OnResponseChat);

		chatInput = GameObject.Find("msgInput").GetComponent<TMP_InputField>();
        chatOutput = GameObject.Find("msgDisplay").GetComponent<TMP_Text>();
        sendButton = GameObject.Find("msgEnter").GetComponentInChildren<Button>();

		sendButton.onClick.AddListener (delegate { OnSendButton (); });
        chatInput.onEndEdit.AddListener (delegate { OnChatInputEnd (); });

		rootMenuPanel.SetActive(true);
		networkMenuPanel.SetActive(false);
		chatPanel.SetActive(false);
		messageBox.SetActive(false);
	}


	#region RootMenu
	public void OnNetworkClick()
	{
		RuntimeManager.PlayOneShot(soundRegular);
		Debug.Log("Send JoinReq");
		bool connected = networkManager.SendJoinRequest();
		if (!connected)
		{
			messageBoxMsg.text = "Unable to connect to server.";
			rootMenuPanel.SetActive(false);
			messageBox.SetActive(true);
		}
		else {
			rootMenuPanel.SetActive(false);
			chatPanel.SetActive(true);
			networkMenuPanel.SetActive(true);
		}
	}

	public void OnExitClick()
	{
		RuntimeManager.PlayOneShot(soundRegular);
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}
	#endregion


	#region NetworkMenu
	public void OnResponseJoin(ExtendedEventArgs eventArgs)
	{
		t1p1Input.SetActive(false);
		t1p1Name.gameObject.SetActive(true);
		t1p2Input.SetActive(false);
		t1p2Name.gameObject.SetActive(true);
		t2p1Input.SetActive(false);
		t2p1Name.gameObject.SetActive(true);
		t2p2Input.SetActive(false);
		t2p2Name.gameObject.SetActive(true);

		ResponseJoinEventArgs args = eventArgs as ResponseJoinEventArgs;
		if (args.status == 0)
		{
			RuntimeManager.PlayOneShot(soundJoin);
			if (args.user_id == 1)
			{
				playerName = t1p1Name;
				playerInput = t1p1Input;
				playerNameDefault = t1p1NameDefault;
			}
			else if (args.user_id == 2)
			{
				playerName = t1p2Name;
				playerInput = t1p2Input;
				playerNameDefault = t1p2NameDefault;
			}
			else if (args.user_id == 3)
			{
				playerName = t2p1Name;
				playerInput = t2p1Input;
				playerNameDefault = t2p1NameDefault;
			}
			else if (args.user_id == 4)
			{
				playerName = t2p2Name;
				playerInput = t2p2Input;
				playerNameDefault = t2p2NameDefault;
			}
			else
			{
				Debug.Log("ERROR: Invalid user_id in ResponseJoin: " + args.user_id);
				messageBoxMsg.text = "Error joining game. Network returned invalid response.";
				messageBox.SetActive(true);
				return;
			}
			Constants.USER_ID = args.user_id;
			
			playerInput.SetActive(true);
			inputField = playerInput.GetComponent<TMP_InputField>();
			inputField.onEndEdit.AddListener(OnPlayerNameSet);
			
			playerName.gameObject.SetActive(false);

			rootMenuPanel.SetActive(false);
			networkMenuPanel.SetActive(true);
			chatPanel.SetActive(true);
		}
		
		else
		{
			messageBoxMsg.text = "Server is full.";
			messageBox.SetActive(true);
			networkMenuPanel.SetActive(false);
			chatPanel.SetActive(false);
		}

	}

	public void OnLeave()
	{
		Debug.Log("Send LeaveReq");
		networkManager.SendLeaveRequest();
		rootMenuPanel.SetActive(true);
		networkMenuPanel.SetActive(false);
		chatPanel.SetActive(false);
		//ready = false;
	}

	public void OnResponseLeave(ExtendedEventArgs eventArgs)
	{
		RuntimeManager.PlayOneShot(soundQuit);
		ResponseLeaveEventArgs args = eventArgs as ResponseLeaveEventArgs;
		if (args.user_id != Constants.USER_ID)
		{
			if (args.user_id == 1) {
				t1p1Name.text = "Waiting for others";
				t1p1Ready = false;
			}
			else if (args.user_id == 2) {
				t1p2Name.text = "Waiting for others";
				t1p2Ready = false;
			}
			else if (args.user_id == 3) {
				t2p1Name.text = "Waiting for others";
				t2p1Ready = false;
			}
			else{
				t2p2Name.text = "Waiting for others";
				t2p2Ready = false;
			}
		}
		
	}

	public void OnPlayerNameSet(string name)
	{
		Debug.Log("OnPlayerNameSet received name: " + name);
		networkManager.SendSetNameRequest(name);
		playerInput.SetActive(false);
		playerName.gameObject.SetActive(true);
		
		if (Constants.USER_ID == 1)
		{
			t1p1Name.text = name;
		}
		else if(Constants.USER_ID == 2)
		{
			t1p2Name.text = name;
		}
		else if(Constants.USER_ID == 3){
			t2p1Name.text = name;
		} 
		else {
			t2p2Name.text = name;
		}
	}

	public void OnResponseSetName(ExtendedEventArgs eventArgs)
	{
		ResponseSetNameEventArgs args = eventArgs as ResponseSetNameEventArgs;
		if (args.user_id != Constants.USER_ID)
		{
			if (args.user_id == 1)
			{
				t1p1Input.SetActive(false);
				t1p1Name.text = args.name;
			}
			else if (args.user_id == 2)
			{
				t1p2Input.SetActive(false);
				t1p2Name.text = args.name;
			}
			else if (args.user_id == 3){
				t2p1Input.SetActive(false);
				t2p1Name.text = args.name;
			}
			else{
				t2p2Input.SetActive(false);
				t2p2Name.text = args.name;
			}
		}
	}

	public void OnReadyClick()
	{
		Debug.Log("Send ReadyReq");
		string message = "I'm Ready!";
		if (playerName.text == "Waiting for others") {
			message = playerNameDefault + ": " + message;
		}
		else {
			message = playerName.text + ": " + message;
		}
        networkManager.SendChatRequest(message);
		networkManager.SendReadyRequest();
	}

	public void OnResponseReady(ExtendedEventArgs eventArgs)
	{
		RuntimeManager.PlayOneShot(soundReady);
		ResponseReadyEventArgs args = eventArgs as ResponseReadyEventArgs;
		if (Constants.USER_ID == -1) // Haven't joined, but got ready message
		{
			// do something here
			//opReady = true;
		}
		else
		{
			if (args.user_id == 1)
			{
				t1p1Ready = true;
			}
			else if (args.user_id == 2)
			{
				t1p2Ready = true;
			}
			else if (args.user_id == 3)
			{
				t2p1Ready = true;
			}
			else if (args.user_id == 4) {
				t2p2Ready = true;
			}
			else
			{
				Debug.Log("ERROR: Invalid user_id in ResponseReady: " + args.user_id);
				messageBoxMsg.text = "Error starting game. Network returned invalid response.";
				messageBox.SetActive(true);
				return;
			}
		}

		if (t1p1Ready || t1p2Ready || t2p1Ready || t2p2Ready)
		{
			StartCoroutine(DoDelay(3.0f));
			
		}
	}
	#endregion

	#region ChatMenu
	public void OnSendButton() {
		
        if (chatInput.text.Length > 0) {
			string message;
			Debug.Log("playerNameDefault: " + playerNameDefault);
			if (playerName.text == "Waiting for others") {
				message = playerNameDefault + ": " + chatInput.text;
			}
			else {
				message = playerName.text + ": " + chatInput.text;
			}
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
	#endregion

	public void OnOKClick()
	{
		RuntimeManager.PlayOneShot(soundRegular);
		rootMenuPanel.SetActive(true);
		messageBox.SetActive(false);
		networkMenuPanel.SetActive(false);
		chatPanel.SetActive(false);
	}

	private void StartNetworkGame()
	{
		
		GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		if (t1p1Name.text.Length == 0)
		{
			t1p1Name.text = "t1 Player1";
		}
		if (t1p2Name.text.Length == 0)
		{
			t1p2Name.text = "t1 Player 2";
		}
		if (t2p1Name.text.Length == 0)
		{
			t2p1Name.text = "t2 Player 2";
		}
		if (t2p2Name.text.Length == 0)
		{
			t2p1Name.text = "t2 Player 2";
		}
		
		Player t1p1 = new Player(1, t1p1Name.text, new Color(0.9f, 0.1f, 0.1f), Constants.USER_ID == 1);
		Player t1p2 = new Player(2, t1p2Name.text, new Color(0.2f, 0.2f, 1.0f), Constants.USER_ID == 2);
		Player t2p1 = new Player(3, t2p1Name.text, new Color(0.9f, 0.1f, 0.1f), Constants.USER_ID == 3);
		Player t2p2 = new Player(4, t2p2Name.text, new Color(0.2f, 0.2f, 1.0f), Constants.USER_ID == 4);
		gameManager.Init(t1p1, t1p2, t2p1, t2p2, Constants.USER_ID);
		
		SceneManager.LoadScene("map");
	}

	IEnumerator DoDelay(float time)
    {
		messageBox.SetActive(true);
		//messageBoxMsg.text = "Game Will Start in 3 Seconds!";
  //      yield return new WaitForSeconds(1.0f);
		//messageBoxMsg.text = "Game Will Start in 2 Seconds!";
		//yield return new WaitForSeconds(1.0f);
		messageBoxMsg.text = "Game Will Start in 1 Seconds!";
		yield return new WaitForSeconds(1.0f);
		StartNetworkGame();
    }
	
}
