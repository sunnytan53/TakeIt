using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestPick : NetworkRequest
{
	public RequestPick()
	{
		request_id = Constants.CMSG_PICK;
	}

	public void send(int fruitTag, float move_x, float move_y, float move_z, 
	float velocity_x, float velocity_y, float velocity_z)
	{
		packet = new GamePacket(request_id);
		packet.addInt32(fruitTag);
		packet.addFloat32(move_x);
		packet.addFloat32(move_y);
		packet.addFloat32(move_z);
		packet.addFloat32(velocity_x);
		packet.addFloat32(velocity_y);
		packet.addFloat32(velocity_z);
	}
}
