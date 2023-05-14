using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestFruitUpdate : NetworkRequest
{
	public RequestFruitUpdate()
	{
		request_id = Constants.CMSG_FRUIT;
	}

	public void send(GameObject[] fruits)
	{
		packet = new GamePacket(request_id);
		packet.addInt32(fruits.Length*3);
		foreach (GameObject fru in fruits)
        {
			if (fru != null) {
				Vector3 pos = fru.transform.position;
				packet.addFloat32(pos.x);
				packet.addFloat32(pos.y);
				packet.addFloat32(pos.z);
			}
		}
	}
}
