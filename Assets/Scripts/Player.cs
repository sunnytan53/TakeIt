using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour 
{
	public int UserID { get; set; }
	public string Name { get; set; }
	public Color Color { get; set; }
	public bool IsMouseControlled { get; set; }
	public GameObject characterPrefab; 
	public GameObject slimePrefab; 
	// public Vector3 playerPosition { get; set; }

	// private int heroCount = 0;
	private GameObject playerPrefab;

	public Player(int userID, string name, Color color, bool isMouseControlled)
	{
		UserID = userID;
		Name = name;
		Color = color;
		IsMouseControlled = isMouseControlled;
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

	public void setMainPlayer(){
		Debug.Log("Going to set the main player");
		playerPrefab = Instantiate(characterPrefab, getPosition(UserID), Quaternion.identity);
		return;
	}

	public void setPlayer(){
		playerPrefab = Instantiate(characterPrefab, getPosition(UserID), Quaternion.identity);
		playerPrefab.GetComponentInChildren<Camera>().enabled=false;
		playerPrefab.GetComponent<PlayerController>().enabled = false;
		return;
	}

	public GameObject getPlayer(){
		return playerPrefab;
	}
}
