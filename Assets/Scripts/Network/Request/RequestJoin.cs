using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestJoin : NetworkRequest
{
	public RequestJoin()
	{
		request_id = Constants.CMSG_JOIN;
	}

	public void send(int i1, int i2)
	{
		packet = new GamePacket(request_id);
		packet.addInt32(i1);
		packet.addInt32(i2);
	}
}
