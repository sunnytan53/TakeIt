using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
	private ConnectionManager cManager;

	void Awake()
	{
		DontDestroyOnLoad(gameObject);

		gameObject.AddComponent<MessageQueue>();
		gameObject.AddComponent<ConnectionManager>();

		NetworkRequestTable.init();
		NetworkResponseTable.init();
	}

	// Start is called before the first frame update
	void Start()
    {
		cManager = GetComponent<ConnectionManager>();
		connect();
	}

	public bool isConnected()
    {
		return cManager.IsConnected();

	}

	public void connect()
    {
		if (cManager)
		{
			cManager.setupSocket();
			StartCoroutine(RequestHeartbeat(0.1f));
		}
	}

	public bool SendJoinRequest(int i1, int i2)
	{
		if (cManager && cManager.IsConnected())
		{
			RequestJoin request = new RequestJoin();
			request.send(i1, i2);
			cManager.send(request);
			return true;
		}
		return false;
	}

	public bool SendLeaveRequest()
	{
		if (cManager && cManager.IsConnected())
		{
			RequestLeave request = new RequestLeave();
			request.send();
			cManager.send(request);
			return true;
		}
		return false;
	}

	public bool SendSetNameRequest(string name)
	{
		Debug.Log("network manager received the name: "+ name);
		if (cManager && cManager.IsConnected())
		{
			RequestSetName request = new RequestSetName();
			request.send(name);
			cManager.send(request);
			return true;
		}
		return false;
	}

	public bool SendReadyRequest()
	{
		if (cManager && cManager.IsConnected())
		{
			RequestReady request = new RequestReady();
			request.send();
			cManager.send(request);
			return true;
		}
		return false;
	}

	public IEnumerator RequestHeartbeat(float time)
	{
		yield return new WaitForSeconds(time);

		if (cManager)
		{
			RequestHeartbeat request = new RequestHeartbeat();
			request.send();
			cManager.send(request);
		}

		StartCoroutine(RequestHeartbeat(time));
	}

	public bool SendChatRequest(string msg)
	{
		if (cManager && cManager.IsConnected())
		{
			RequestChat request = new RequestChat();
			request.send(msg);
			cManager.send(request);
			return true;
		}
		return false;
	}

	public bool SendMovementRequest(Vector3 position, Quaternion rotation)
	{
		if (cManager && cManager.IsConnected())
		{
			//Debug.Log("Send Movement Request is activated in network manager......");
			RequestMovement request = new RequestMovement();
			request.send(position.x, position.y, position.z, rotation.x, rotation.y, rotation.z, rotation.w);
			cManager.send(request);
			return true;
		}
		return false;
	}

	public bool SendPickRequest(int index)
	{
		if (cManager && cManager.IsConnected())
		{
			Debug.Log("Fruit pick request is sent out from network manager...");
			RequestPick request = new RequestPick();
			request.send(index);
			cManager.send(request);
			return true;
		}
		return false;
	}

	public bool SendThrowRequest(int index, Vector3 force){
        if (cManager && cManager.IsConnected())
		{
			Debug.Log("Fruit throw request is sent out from network manager...");
			RequestThrow request = new RequestThrow();
			float force_x = force.x;
			float force_y = force.y;
			float force_z = force.z;
			request.send(index, force_x, force_y, force_z);
			cManager.send(request);
			return true;
		}
		return false;
    }


	public bool SendArtRequest(AnimationCodeEnum code)
	{
		if (cManager && cManager.IsConnected())
		{
			RequestArt request = new RequestArt();
			request.send(code);
			cManager.send(request);
			return true;
		}
		return false;
	}

	public bool SendFruitUpdateRequest(GameObject[] fruits)
	{
		if (cManager && cManager.IsConnected())
		{
			RequestFruitUpdate request = new RequestFruitUpdate();
            request.send(fruits);
            cManager.send(request);
            return true;
        }
		return true;
    }

    public bool SendFruitPointRequest(int index, int points)
    {
		if (cManager && cManager.IsConnected())
		{
			RequestFruitPoint request = new RequestFruitPoint();
			request.send(index, points);
			cManager.send(request);
			return true;
		}
		return true;
	}
}
