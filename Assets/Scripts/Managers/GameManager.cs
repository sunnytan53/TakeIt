using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
	public Player[] Players = new Player[4];
	public GameObject playerPrefab;
	public GameObject slimePrefab;
	public GameObject apple;
	public GameObject avocado;
	public GameObject banana;
	public GameObject cherris;
	public GameObject lemon;
	public GameObject peach;
	public GameObject peanut;
	public GameObject pear;
	public GameObject strawberry;
	public GameObject watermelon;

	private GameObject currentPrefab;
	private GameObject[] otherPlayers = new GameObject[4];

	private HashSet<GameObject> fruitSet = new HashSet<GameObject>();
	private GameObject[] fruits = new GameObject[20];
	private GameObject[] fruits2 = new GameObject[10];

	private int currentPlayer = 1;
	//private bool canInteract = false;
	//private bool choosingInteraction = false;

	//private bool useNetwork;
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
		msgQueue.AddCallback(Constants.SMSG_INTERACT, OnResponseInteract);
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
		System.Random rand = new System.Random();
		Debug.Log("Fruits are being instantiated*************************************"); 
		/* // can't generate fruits randomly, since each player may generate different assets
		for (int i = 0; i<fruits.Length; i++){
			// Debug.Log("Fruits are being instantiated with i == ************************************* : " + i);
			int randomX = rand.Next(70,130);
        	int randomY = rand.Next(-10,0);
        	int randomZ = rand.Next(70,130);
        	Vector3 genPosition = new Vector3(randomX, randomY, randomZ);
			fruits[i] = Instantiate(fruitSet.ElementAt(rand.Next(0, fruitSet.Count)), genPosition, Quaternion.identity);
			fruits[i].GetComponent<Pickable>().index = i;
			fruits[i].tag = i.ToString();
			// Debug.Log("fruits[i].GetComponent<Pickable>().tag is *************************************: "+fruits[i].GetComponent<Pickable>().tag); 
		}
		*/
		Vector3[] fruitPos = new Vector3[] {
			new Vector3(70, 0, 70), new Vector3(80, 0, 70), new Vector3(90, 0, 70), new Vector3(100, 0, 70), new Vector3(120, 0, 70), 
			new Vector3(70, 0, 70), new Vector3(70, 0, 80), new Vector3(70, 0, 90), new Vector3(70, 0, 100), new Vector3(70, 0, 120)
		};
		for (int i=0; i<10; i++) {
			fruits2[i] = Instantiate(fruitSet.ElementAt(i), fruitPos[i], Quaternion.identity);
			Debug.Log("i is *************************************: "+i); 
			fruits2[i].GetComponent<Pickable>().index = i;
			Debug.Log("fruits2[i].GetComponent<Pickable>().tag is *************************************: "+fruits2[i].GetComponent<Pickable>().tag); 
			fruits2[i].tag = i.ToString();
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


		// when calling Players[currentPlayer], reference object is not set to object, why?
		// Debug.Log("Player[0] when create characters is: " + Players[currentPlayer].getPosition(1));
		/* // this piece of code didn't work, need to check
		for (int i=0; i<4; i++) {
			if (i == currentPlayer-1) {
				// playerObjT1P1 = Instantiate(playerPrefab, new Vector3(100, 0, 110), Quaternion.identity);
				Players[i].setMainPlayer();
			}
			else {
				Players[i].setPlayer();
			}
		}
		*/
	}

/*
	public bool CanInteract()
	{
        
		return canInteract;
        
	}
*/
	public void StartInteraction()
	{
        /*
		if (canInteract)
		{
			choosingInteraction = true;
		}
        */
	}
/*
	public void EndInteraction(Hero hero)
	{
		EndTurn();
	}

	public void EndInteractedWith(Hero hero)
	{
		// Do nothing
	}

	public void EndMove(Hero hero)
	{
        
		bool heroCanInteract = false;
		int[] deltaX = { 1, 0, -1, 0 };
		int[] deltaY = { 0, 1, 0, -1 };
		for (int i = 0; i < 4; ++i)
		{
			int x = hero.x + deltaX[i];
			int y = hero.y + deltaY[i];
			if (x >= 0 && x < 6 && y >= 0 && y < 5)
			{
				if (gameBoard[x, y] && gameBoard[x, y].Owner != hero.Owner)
				{
					heroCanInteract = true;
					break;
				}
			}
		}
		if (hero.Owner.IsMouseControlled)
		{
			canInteract = heroCanInteract;
		}

		if (!heroCanInteract)
		{
			EndTurn();
		}
       
	}
 */
	public void EndTurn()
	{
        /*
		ObjectSelector.SetSelectedObject(null);
		canInteract = false;
		currentPlayer = 3 - currentPlayer;
        */
	}


	public void OnResponseMovement(ExtendedEventArgs eventArgs)
	{
		ResponseMovementEventArgs args = eventArgs as ResponseMovementEventArgs;
		Debug.Log("OnResponseMovement is activated in the Game Manager....with user_id: " + args.user_id);
		if (args.user_id != Constants.USER_ID)
		{
			float move_x = args.move_x;
			float move_y = args.move_y;
			float move_z = args.move_z;
			float rotate_x = args.rotate_x;
			float rotate_y = args.rotate_y;
			float rotate_z = args.rotate_z;
			float rotate_w = args.rotate_w;
			Vector3 position = new Vector3(move_x, move_y, move_z);
            Quaternion rotation = new Quaternion(rotate_x, rotate_y, rotate_z, rotate_w);
			Transform transform;
			transform = otherPlayers[args.user_id-1].transform;
			transform.position = position;
			transform.rotation = rotation;
		}
		else if (args.user_id == Constants.USER_ID)
		{
			// Ignore
		}
		else
		{
			Debug.Log("ERROR: Invalid user_id in ResponseReady: " + args.user_id);
		}
	}

	public void OnResponseInteract(ExtendedEventArgs eventArgs)
	{
        /*
		ResponseInteractEventArgs args = eventArgs as ResponseInteractEventArgs;
		if (args.user_id == Constants.OP_ID)
		{
			int pieceIndex = args.piece_idx;
			int targetIndex = args.target_idx;
			Hero hero = Players[args.user_id - 1].Heroes[pieceIndex];
			Hero target = Players[Constants.USER_ID - 1].Heroes[targetIndex];
			hero.Interact(target);
		}
		else if (args.user_id == Constants.USER_ID)
		{
			// Ignore
		}
		else
		{
			Debug.Log("ERROR: Invalid user_id in ResponseReady: " + args.user_id);
		}
        */
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
		Debug.Log("In OnResponsePick, the received fruitTag is: " + args.fruitTag);
		int user_id = args.user_id;

		if (args.user_id != Constants.USER_ID)
		{
			int fruitTag = args.fruitTag;
			float move_x = args.move_x;
			float move_y = args.move_y;
			float move_z = args.move_z;
			float velocity_x = args.velocity_x;
			float velocity_y = args.velocity_y;
			float velocity_z = args.velocity_z;
			Debug.Log("In OnResponsePick, received move_x: " + move_x);
		
			// attach fruit with fruitTag to the player with user_id
			// GameObject playerPick = otherPlayers[user_id-1];
			GameObject pickedFruit = fruits2[fruitTag];
			// GameObject.Find(fruitTag.ToString());
			Rigidbody pickedFruitRB = pickedFruit.GetComponent<Rigidbody>();
			// pickedFruitRB.useGravity = false;
            // pickedFruitRB.drag = 10;
            // pickedFruitRB.constraints = RigidbodyConstraints.FreezeRotation;

			Debug.Log("In OnResponsePick, the pickedFruit is: " + pickedFruit.name);
			Debug.Log("In OnResponsePick, pickedFruit.transform.position: " + pickedFruit.transform.position);
			// Debug.Log("In OnResponsePick, pickedFruitRB.velocity: " + pickedFruitRB.velocity);

			Vector3 position = new Vector3(move_x, move_y, move_z);
			Vector3 velocity = new Vector3(velocity_x, velocity_y, velocity_z);
			Debug.Log("In OnResponsePick, received transform.position: " + position);
			Debug.Log("In OnResponsePick, received velocity: " + velocity);
			pickedFruit.transform.position = position;
    		pickedFruitRB.velocity = velocity;
		}
	}

	public void OnResponseThrow(ExtendedEventArgs eventArgs) {
		// ResponsePickEventArgs args = eventArgs as ResponsePickEventArgs;
		// Debug.Log("In OnResponsePick, the received user id is: " + args.user_id);
		// Debug.Log("In OnResponsePick, the received fruitTag is: " + args.fruitTag);
	}
}
