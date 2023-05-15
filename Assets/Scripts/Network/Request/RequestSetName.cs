using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestSetName : NetworkRequest
{
	public RequestSetName()
	{
		request_id = Constants.CMSG_SETNAME;
	}

	public void send(int i1, int i2, string name)
	{
		packet = new GamePacket(request_id);
		packet.addInt32(i1);
		packet.addInt32(i2);
		packet.addString(name);
	}
}
