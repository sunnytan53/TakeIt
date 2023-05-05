using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestPick : NetworkRequest
{
	public RequestPick()
	{
		request_id = Constants.CMSG_PICK;
	}

	public void send(int index)
	{
		packet = new GamePacket(request_id);
		packet.addInt32(index);
		if (index < 0 || index >= 10)
        {
			Debug.LogError("abnormal index");
        }
	}
}
