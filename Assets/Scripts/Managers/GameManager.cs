using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using FMODUnity;

public class GameManager : MonoBehaviour
{
	public Player[] Players = new Player[4];
	public GameObject slimePrefab;
	public GameObject apple, avocado, banana, cherris, lemon,
		peach, peanut, pear, strawberry, watermelon;

	private GameObject currentPrefab;
	private GameObject[] otherPlayers = new GameObject[4];

	private HashSet<GameObject> fruitSet = new HashSet<GameObject>();
	private GameObject[] fruits2 = new GameObject[10];

	private int currentPlayer = 1;

	private NetworkManager networkManager;

	void Start()
	{
		DontDestroyOnLoad(gameObject);
		fruitSet.Add(apple);
		fruitSet.Add(avocado);
		fruitSet.Add(banana);
		fruitSet.Add(cherris);
		fruitSet.Add(lemon);
		fruitSet.Add(peach);
		fruitSet.Add(peanut);
		fruitSet.Add(pear);
		fruitSet.Add(strawberry);
		fruitSet.Add(watermelon);

		networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
		MessageQueue msgQueue = networkManager.GetComponent<MessageQueue>();
		msgQueue.AddCallback(Constants.SMSG_MOVEMENT, OnResponseMovement);
		msgQueue.AddCallback(Constants.SMSG_PICK, OnResponsePick);
		msgQueue.AddCallback(Constants.SMSG_THROW, OnResponseThrow);
		msgQueue.AddCallback(Constants.SMSG_ART, OnResponseArt);
		msgQueue.AddCallback(Constants.SMSG_FRUIT, OnResponseFruitUpdate);
	}

/*
	public Player GetCurrentPlayer()
	{
		// return Players[currentPlayer - 1];
	}
*/
	
	public void Init(Player t1p1, Player t1p2, Player t2p1, Player t2p2, int currentPlayerId)
	{
		Players[0] = t1p1;
		Players[1] = t1p2;
        Players[2] = t2p1;
        Players[3] = t2p2;
		currentPlayer = currentPlayerId;
		Debug.Log("Current player id detected by GameManager is: " + currentPlayerId);
		// useNetwork = (!player1.IsMouseControlled || !player2.IsMouseControlled);
	}
	
	public void createFruits(){
		Debug.Log("Fruits are being instantiated*************************************");
		// an efficient way to do it is find the empty gameobject in the map to init
		Vector3[] fruitPos = new Vector3[] {
			new Vector3(70, 0, 70), new Vector3(80, 0, 70), new Vector3(90, 0, 70), new Vector3(100, 0, 70), new Vector3(120, 0, 70), 
			new Vector3(70, 0, 70), new Vector3(70, 0, 80), new Vector3(70, 0, 90), new Vector3(70, 0, 100), new Vector3(70, 0, 120)
		};
		for (int i=0; i<10; i++) {
			fruits2[i] = Instantiate(fruitSet.ElementAt(i), fruitPos[i], Quaternion.identity);
		}
		StartCoroutine(updateFruitLocation());
	}

	IEnumerator updateFruitLocation()
    {
		while (true)
        {
			networkManager.SendFruitUpdateRequest(fruits2);
			yield return new WaitForSeconds(0.1f);
		}
    }

	public void createCharacters() {
		Debug.Log("Current player id when create characters is: " + currentPlayer);
		for(int i = 0; i < 4; i++){
			if (i == currentPlayer-1) {
				currentPrefab = Instantiate(slimePrefab, getPosition(i+1), Quaternion.identity);
			}
			else {
				otherPlayers[i] = Instantiate(slimePrefab, getPosition(i+1), Quaternion.identity);
				otherPlayers[i].GetComponentInChildren<Camera>().enabled=false;
            	otherPlayers[i].GetComponent<PlayerController>().enabled = false;
			}
		}
	}

	public void OnResponseMovement(ExtendedEventArgs eventArgs)
	{
		ResponseMovementEventArgs args = eventArgs as ResponseMovementEventArgs;
		Debug.Log("OnResponseMovement is activated in the Game Manager....with user_id: " + args.user_id);
		if (args.user_id != Constants.USER_ID)
		{
			Transform transform = otherPlayers[args.user_id-1].transform;
			transform.position = Vector3.Lerp(new Vector3(args.move_x, args.move_y, args.move_z), transform.position, 0.03f);
			transform.rotation = Quaternion.Lerp(new Quaternion(args.rotate_x, args.rotate_y, args.rotate_z, args.rotate_w), transform.rotation, 0.03f);
		}
	}

	public Vector3 getPosition(int usr_id){
		switch(usr_id){
			case 1:
				return new Vector3(100, 0, 110);
			case 2:
				return new Vector3(100, 0, 90);
			case 3:
				return new Vector3(110, 0, 100);
			case 4:
				return new Vector3(90, 0, 100);
			default:
				Debug.Log("Something went wrong when calling getPosition() in Player.cs");
				return new Vector3(0, 0, 0);
		}
	}

	public void OnResponsePick(ExtendedEventArgs eventArgs) {
		ResponsePickEventArgs args = eventArgs as ResponsePickEventArgs;
		Debug.Log("In OnResponsePick, the received user id is: " + args.user_id);
		Debug.Log("In OnResponsePick, the received fruitTag is: " + args.index);

		if (args.index < 0 || args.index >= fruits2.Length) return;
		if (args.user_id != Constants.USER_ID)
		{
			GameObject pickedFruit = fruits2[args.index];
			Rigidbody pickedFruitRB = pickedFruit.GetComponent<Rigidbody>();
			Pickable pickable = pickedFruit.GetComponent<Pickable>();
			if (!pickable.isPicked)
			{
				pickable.isPicked = true;
				pickedFruitRB.useGravity = false;
				pickedFruitRB.drag = 10;
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
			GameObject throwFruit = fruits2[args.index];
			Rigidbody throwFruitRB = throwFruit.GetComponent<Rigidbody>();

			throwFruit.GetComponent<Pickable>().isPicked = false;
            throwFruitRB.useGravity = true;
            throwFruitRB.drag = 0;
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
			for (int i = 0; i < fruits2.Length; i++)
            {
				Transform transform = fruits2[i].transform;
				transform.position = Vector3.Lerp(transform.position, args.positions[i], 0.1f);
            }
		}
	}

}
