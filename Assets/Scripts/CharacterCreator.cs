using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCreator : MonoBehaviour
{
	private static GameManager gameManager;

	// Start is called before the first frame update
	void Start()
	{
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		gameManager.createCharacters();
	}

	// Update is called once per frame
	void Update()
    {
        
    }
}