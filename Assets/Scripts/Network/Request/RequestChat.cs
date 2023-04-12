using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestChat : NetworkRequest
{
	public RequestChat()
	{
		request_id = Constants.CMSG_CHAT;
	}

	public void send(string msg)
	{
		packet = new GamePacket(request_id);
		packet.addString(msg);
	}
}
