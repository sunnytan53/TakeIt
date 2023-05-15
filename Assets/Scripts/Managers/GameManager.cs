using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public Player[] Players = new Player[4];
	public GameObject playerPrefab;
	public GameObject[] fruitToSpawn;
	public GameObject[] rockToSpawn;
	private GameObject[] fruitAndRock;

	private GameObject currentPlayer;
	private GameObject[] otherPlayers = new GameObject[4];

	private int currentPlayerID;

	private NetworkManager networkManager;

	private GameObject[] bodies;
	private Texture2D[] faces;
	private int[,] indicies;


	void Start()
	{
		fruitAndRock = new GameObject[fruitToSpawn.Length + rockToSpawn.Length];

		DontDestroyOnLoad(gameObject);
		networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
		MessageQueue msgQueue = networkManager.GetComponent<MessageQueue>();
		msgQueue.AddCallback(Constants.SMSG_MOVEMENT, OnResponseMovement);
		msgQueue.AddCallback(Constants.SMSG_PICK, OnResponsePick);
		msgQueue.AddCallback(Constants.SMSG_THROW, OnResponseThrow);
		msgQueue.AddCallback(Constants.SMSG_ART, OnResponseArt);
		msgQueue.AddCallback(Constants.SMSG_FRUIT, OnResponseFruitUpdate);
		msgQueue.AddCallback(Constants.SMSG_FRUITPOINT, OnResponseFruitPoint);
	}


    public void Init(Player t1p1, Player t1p2, Player t2p1, Player t2p2, int currentPlayerId, GameObject[] body, Texture2D[] face, int[,] index)
	{
		Players[0] = t1p1;
		Players[1] = t1p2;
        Players[2] = t2p1;
        Players[3] = t2p2;
		currentPlayerID = currentPlayerId;
		bodies = body;
		faces = face;
        indicies = index;
		Debug.Log("Current player id detected by GameManager is: " + currentPlayerId);
	}
	
	public void initMap(Vector3 middle, Vector3 t1, Vector3 t2){
		int totalLength = fruitToSpawn.Length + rockToSpawn.Length;
		float angle = 360f / totalLength;

		List<Vector3> fruitPos = new List<Vector3>();
		List<Vector3> rockPos = new List<Vector3>();
		for (int i = 0; i < totalLength; i++)
        {
			float angleRad = i * angle * Mathf.Deg2Rad;
			Vector3 spawn = middle + new Vector3(Mathf.Cos(angleRad), 0.5f, Mathf.Sin(angleRad)) * 20f;
			if (i % 2 == 0 && rockPos.Count < fruitToSpawn.Length)
            {
				fruitPos.Add(spawn);
            }
			else
            {
				rockPos.Add(spawn);
            }
		}

		// fruits
		for (int i = 0; i < fruitToSpawn.Length; i++) {
			fruitAndRock[i] = Instantiate(fruitToSpawn[i], fruitPos[i], Quaternion.identity);
			Pickable pickable = fruitAndRock[i].GetComponent<Pickable>();
			pickable.index = i;
			pickable.isFruit = true;
		}

		// rocks
		for (int i = 0; i < rockToSpawn.Length; i++)
		{
			int real_index = i + fruitToSpawn.Length;
			fruitAndRock[real_index] = Instantiate(rockToSpawn[i], rockPos[i], Quaternion.identity);
			Pickable pickable = fruitAndRock[real_index].GetComponent<Pickable>();
			pickable.index = real_index;
		}

		// player
		Vector3 offset = new Vector3(0, -10f, 0);
		for (int i = 0; i < 4; i++)
		{
			Vector3 pos = (i < 2) ? t1 : t2;
			// initialize body and face
			if (i == currentPlayerID - 1)
			{
				GameObject copy = Instantiate(playerPrefab, pos, Quaternion.identity);
				GameObject body = Instantiate(bodies[indicies[i, 0]], pos, Quaternion.identity);
				body.transform.GetChild(1).GetComponent<Renderer>().materials[1].SetTexture("_MainTex", faces[indicies[i, 1]]);
				body.transform.SetParent(copy.transform);
				copy.GetComponentInChildren<PlayerController>().horizontalRotation = (i < 2) ? -45 : 135;

				currentPlayer = Instantiate(copy);
				Destroy(copy);
			}
			else
			{
				pos = pos + offset;
				GameObject copy = Instantiate(playerPrefab, pos, Quaternion.identity);
				GameObject body = Instantiate(bodies[indicies[i, 0]], pos, Quaternion.identity);
				body.transform.GetChild(1).GetComponent<Renderer>().materials[1].SetTexture("_MainTex", faces[indicies[i, 1]]);
				body.transform.SetParent(copy.transform);
				copy.GetComponentInChildren<Camera>().enabled = false;
				copy.GetComponentInChildren<PlayerController>().enabled = false;
				copy.GetComponentInChildren<CapsuleCollider>().enabled = false;

				otherPlayers[i] = Instantiate(copy);
				Destroy(copy);
			}
		}

		StartCoroutine(updateLocation());
	}

	IEnumerator updateLocation()
    {
		while (true)
        {
			networkManager.SendFruitUpdateRequest(fruitAndRock);
			networkManager.SendMovementRequest(currentPlayer.transform.position, currentPlayer.transform.rotation);
			yield return new WaitForSeconds(0.1f);
		}
    }

	public void OnResponseMovement(ExtendedEventArgs eventArgs)
	{
		ResponseMovementEventArgs args = eventArgs as ResponseMovementEventArgs;
		//Debug.Log("OnResponseMovement is activated in the Game Manager....with user_id: " + args.user_id);
		if (args.user_id != Constants.USER_ID)
		{
			Transform transform = otherPlayers[args.user_id-1].transform;
			transform.position = Vector3.Lerp(new Vector3(args.move_x, args.move_y, args.move_z), transform.position, 0.1f);
			transform.rotation = Quaternion.Lerp(new Quaternion(args.rotate_x, args.rotate_y, args.rotate_z, args.rotate_w), transform.rotation, 0.1f);
		}
	}

	public void OnResponsePick(ExtendedEventArgs eventArgs) {
		ResponsePickEventArgs args = eventArgs as ResponsePickEventArgs;
		Debug.Log("In OnResponsePick, the received user id is: " + args.user_id);
		Debug.Log("In OnResponsePick, the received fruit index is: " + args.index);

		if (args.user_id != Constants.USER_ID)
		{
			GameObject pickedFruit = fruitAndRock[args.index];
			Rigidbody pickedFruitRB = pickedFruit.GetComponent<Rigidbody>();
			Pickable pickable = pickedFruit.GetComponent<Pickable>();
			if (!pickable.isPicked)
			{
				pickable.isPicked = true;
				pickedFruitRB.useGravity = false;
				//pickedFruitRB.drag = 10;
				pickedFruitRB.constraints = RigidbodyConstraints.FreezeRotation;
			}
		}
	}

	public void OnResponseThrow(ExtendedEventArgs eventArgs) {
		ResponseThrowEventArgs args = eventArgs as ResponseThrowEventArgs;
		Debug.Log("In OnResponseThrow, the received user id is: " + args.user_id);
		Debug.Log("In OnResponseThrow, the received fruitTag is: " + args.index);
		
		if (args.user_id != Constants.USER_ID)
		{
			GameObject throwFruit = fruitAndRock[args.index];
			Rigidbody throwFruitRB = throwFruit.GetComponent<Rigidbody>();

			throwFruit.GetComponent<Pickable>().isPicked = false;
            throwFruitRB.useGravity = true;
            //throwFruitRB.drag = 0;
            throwFruitRB.constraints = RigidbodyConstraints.None;

			throwFruitRB.AddForce(new Vector3(args.force_x, args.force_y, args.force_z));
		}
	}


	public void OnResponseArt(ExtendedEventArgs eventArgs)
	{
		ResponseArtEventArgs args = eventArgs as ResponseArtEventArgs;
		Debug.Log("OnResponseArt is activated in the Game Manager....with user_id: " + args.user_id);
		if (args.user_id != Constants.USER_ID)
		{
			otherPlayers[args.user_id - 1].GetComponent<PlayerArtController>().setAnimationCode((AnimationCodeEnum)args.code, true);
		}
	}

	public void OnResponseFruitUpdate(ExtendedEventArgs eventArgs)
	{
		ResponseFruitUpdateEventArgs args = eventArgs as ResponseFruitUpdateEventArgs;
		Debug.Log("OnResponseResponseFruitUpdateEventArgs is activated in the Game Manager....with user_id: " + args.user_id);
		if (args.user_id != Constants.USER_ID)
		{
			for (int i = 0; i < fruitAndRock.Length; i++)
            {
				Transform transform = fruitAndRock[i].transform;
                transform.position = Vector3.Lerp(transform.position, args.positions[i], 0.1f);
			}
		}
	}


	public void OnResponseFruitPoint(ExtendedEventArgs eventArgs)
	{
		ResponseFruitPointEventArgs args = eventArgs as ResponseFruitPointEventArgs;
		Debug.Log("ResponseFruitPointEventArgs is activated in the Game Manager....with user_id: " + args.user_id);
		if (args.user_id != Constants.USER_ID)
		{
			fruitAndRock[args.index].GetComponent<Pickable>().points = args.points;
		}
	}
}
