using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public Player[] Players = new Player[4];
	public GameObject playerPrefab;
	private GameObject playerObjT1P1;
	private GameObject playerObjT1P2;
	private GameObject playerObjT2P1;
	private GameObject playerObjT2P2;

	//private Hero[,] gameBoard = new Hero[6,5];

	private int currentPlayer = 1;
	//private bool canInteract = false;
	//private bool choosingInteraction = false;

	//private bool useNetwork;
	private NetworkManager networkManager;

	void Start()
	{
		DontDestroyOnLoad(gameObject);
		networkManager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
		MessageQueue msgQueue = networkManager.GetComponent<MessageQueue>();
		msgQueue.AddCallback(Constants.SMSG_MOVEMENT, OnResponseMovement);
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
	
	public void createCharacters() {
		Debug.Log("Current player id when create characters is: " + currentPlayer);
		if (currentPlayer==1){
			playerObjT1P1 = Instantiate(playerPrefab, new Vector3(0, 0, 10), Quaternion.identity);
			
			playerObjT1P2 = Instantiate(playerPrefab, new Vector3(0, 0, -10), Quaternion.identity);
			playerObjT1P2.GetComponentInChildren<Camera>().enabled=false;
			playerObjT1P2.GetComponent<PlayerController>().enabled = false;

			playerObjT2P1 = Instantiate(playerPrefab, new Vector3(10, 0, 0), Quaternion.identity);
			playerObjT2P1.GetComponentInChildren<Camera>().enabled=false;
			playerObjT2P1.GetComponent<PlayerController>().enabled = false;

			playerObjT2P2 = Instantiate(playerPrefab, new Vector3(-10, 0, 0), Quaternion.identity);
			playerObjT2P2.GetComponentInChildren<Camera>().enabled=false;
			playerObjT2P2.GetComponent<PlayerController>().enabled = false;
		}
		else if (currentPlayer==2){
			playerObjT1P2 = Instantiate(playerPrefab, new Vector3(0, 0, -10), Quaternion.identity);

			playerObjT1P1 = Instantiate(playerPrefab, new Vector3(0, 0, 10), Quaternion.identity);
			playerObjT1P1.GetComponentInChildren<Camera>().enabled=false;
			playerObjT1P1.GetComponent<PlayerController>().enabled = false;

			playerObjT2P1 = Instantiate(playerPrefab, new Vector3(10, 0, 0), Quaternion.identity);
			playerObjT2P1.GetComponentInChildren<Camera>().enabled=false;
			playerObjT2P1.GetComponent<PlayerController>().enabled = false;

			playerObjT2P2 = Instantiate(playerPrefab, new Vector3(-10, 0, 0), Quaternion.identity);
			playerObjT2P2.GetComponentInChildren<Camera>().enabled=false;
			playerObjT2P2.GetComponent<PlayerController>().enabled = false;
		}
		else if (currentPlayer==3){
			playerObjT2P1 = Instantiate(playerPrefab, new Vector3(10, 0, 0), Quaternion.identity);
		
			playerObjT1P1 = Instantiate(playerPrefab, new Vector3(0, 0, 10), Quaternion.identity);
			playerObjT1P1.GetComponentInChildren<Camera>().enabled=false;
			playerObjT1P1.GetComponent<PlayerController>().enabled = false;

			playerObjT1P2 = Instantiate(playerPrefab, new Vector3(0, 0, -10), Quaternion.identity);
			playerObjT1P2.GetComponentInChildren<Camera>().enabled=false;
			playerObjT1P2.GetComponent<PlayerController>().enabled = false;

			playerObjT2P2 = Instantiate(playerPrefab, new Vector3(-10, 0, 0), Quaternion.identity);
			playerObjT2P2.GetComponentInChildren<Camera>().enabled=false;
			playerObjT2P2.GetComponent<PlayerController>().enabled = false;
		}
		else if (currentPlayer==4){
			playerObjT2P2 = Instantiate(playerPrefab, new Vector3(-10, 0, 0), Quaternion.identity);
		
			playerObjT1P1 = Instantiate(playerPrefab, new Vector3(0, 0, 10), Quaternion.identity);
			playerObjT1P1.GetComponentInChildren<Camera>().enabled=false;
			playerObjT1P1.GetComponent<PlayerController>().enabled = false;
			
			playerObjT1P2 = Instantiate(playerPrefab, new Vector3(0, 0, -10), Quaternion.identity);
			playerObjT1P2.GetComponentInChildren<Camera>().enabled=false;
			playerObjT1P2.GetComponent<PlayerController>().enabled = false;

			playerObjT2P1 = Instantiate(playerPrefab, new Vector3(10, 0, 0), Quaternion.identity);
			playerObjT2P1.GetComponentInChildren<Camera>().enabled=false;
			playerObjT2P1.GetComponent<PlayerController>().enabled = false;
		}
		else {
			Debug.Log("Something went wrong when trying to instantiate character prefabs...");
		}
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

	public void ProcessClick(GameObject hitObject)
	{
        /*
		if (hitObject.tag == "Tile")
		{
			if (ObjectSelector.SelectedObject)
			{
				Hero hero = ObjectSelector.SelectedObject.GetComponentInParent<Hero>();
				if (hero)
				{
					int x = (int)hitObject.transform.position.x;
					int y = (int)hitObject.transform.position.z;
					if (gameBoard[x, y] == null)
					{
						if (hero.CanMoveTo(x, y))
						{
							if (useNetwork)
							{
								networkManager.SendMoveRequest(hero.Index, x, y);
							}
							gameBoard[hero.x, hero.y] = null;
							hero.Move(x, y);
							gameBoard[x, y] = hero;
						}
					}
				}
			}
		}
		else
		{
			Hero hero = hitObject.GetComponentInParent<Hero>();
			if (hero)
			{
				if (choosingInteraction)
				{
					Hero selectedHero = ObjectSelector.SelectedObject?.GetComponentInParent<Hero>();
					if (selectedHero)
					{
						if (AreNeighbors(hero, selectedHero) && hero.Owner != selectedHero.Owner)
						{
							if (useNetwork)
							{
								networkManager.SendInteractRequest(selectedHero.Index, hero.Index);
							}
							selectedHero.Interact(hero);
							choosingInteraction = false;
						}
					}
				}
				else if (hero.gameObject == ObjectSelector.SelectedObject)
				{
					ObjectSelector.SetSelectedObject(null);
				}
				else if (hero.Owner.IsMouseControlled && hero.Owner == Players[currentPlayer - 1])
				{
					ObjectSelector.SetSelectedObject(hitObject);
				}
			}
		}
        */
	}
 /*
	public bool HighlightEnabled(GameObject gameObject)
	{
       
		if (gameObject.tag == "Tile")
		{
			Hero hero = ObjectSelector.SelectedObject?.GetComponentInParent<Hero>();
			if (hero)
			{
				int x = (int)gameObject.transform.position.x;
				int y = (int)gameObject.transform.position.z;
				return (gameBoard[x, y] == null);
			}
		}
		else if (choosingInteraction)
		{
			Hero hero = gameObject.GetComponentInParent<Hero>();
			Hero selectedHero = ObjectSelector.SelectedObject?.GetComponentInParent<Hero>();
			if (hero && selectedHero)
			{
				return AreNeighbors(hero, selectedHero) && hero.Owner != selectedHero.Owner;
			}
			else
			{
				return false;
			}
		}
		else
		{
			Hero hero = gameObject.GetComponentInParent<Hero>();
			if (hero)
			{
				return (hero.Owner.IsMouseControlled && hero.Owner == Players[currentPlayer - 1]);
			}
		}
		return true;

	}
 
	private bool AreNeighbors(Hero hero1, Hero hero2)
	{
       
		return (Math.Abs(hero1.x - hero2.x) + Math.Abs(hero1.y - hero2.y) == 1);
        
	}
*/

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
			if(args.user_id == 1){
				transform = playerObjT1P1.transform;
			}
			else if (args.user_id == 2) {
				transform = playerObjT1P2.transform;
			}
			else if (args.user_id == 3) {
				transform = playerObjT2P1.transform;
			}
			else {
				transform = playerObjT2P2.transform;
			}
			// Set the position and rotation of the transform
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
}
