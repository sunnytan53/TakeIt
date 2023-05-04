using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestArt : NetworkRequest
{
	public RequestArt()
	{
		request_id = Constants.CMSG_ART;
	}

	public void send(AnimationCodeEnum code)
	{
		packet = new GamePacket(request_id);
		packet.addShort16((short) code);
	}
}
