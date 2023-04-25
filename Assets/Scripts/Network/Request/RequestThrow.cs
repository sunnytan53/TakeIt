using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestThrow : NetworkRequest
{
	public RequestThrow()
	{
		request_id = Constants.CMSG_THROW;
	}

	public void send(int fruitTag, float force_x, float force_y, float force_z)
	{
		packet = new GamePacket(request_id);
		packet.addInt32(fruitTag);
		packet.addFloat32(force_x);
		packet.addFloat32(force_y);
		packet.addFloat32(force_z);
	}
}
