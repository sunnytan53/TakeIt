using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestFruitPoint : NetworkRequest
{
	public RequestFruitPoint()
	{
		request_id = Constants.CMSG_FRUITPOINT;
	}

	public void send(int index, int points)
	{
		packet = new GamePacket(request_id);
		packet.addInt32(index);
		packet.addInt32(points);
	}
}
