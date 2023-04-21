using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestPlayerControl : NetworkRequest
{
	public RequestPlayerControl()
	{
		request_id = Constants.CMSG_PLAYER_CONTROL;
	}

	public void send(Vector3 pos, Quaternion rot, animationCode aCode, soundCode sCode)
	{
		packet = new GamePacket(request_id);
		packet.addFloat32(pos.x);
		packet.addFloat32(pos.y);
		packet.addFloat32(pos.z);
		packet.addFloat32(rot.x);
		packet.addFloat32(rot.y);
		packet.addFloat32(rot.z);
		packet.addFloat32(rot.w);
		packet.addShort16((short) sCode);
		packet.addShort16((short) aCode);
	}
}
