using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestMovement : NetworkRequest
{
	public RequestMovement()
	{
		request_id = Constants.CMSG_MOVEMENT;
	}

	public void send(float move_x, float move_y, float move_z, 
	float rotate_x, float rotate_y, float rotate_z, float rotate_w)
	{
		packet = new GamePacket(request_id);
		packet.addFloat32(move_x);
		packet.addFloat32(move_y);
		packet.addFloat32(move_z);
		packet.addFloat32(rotate_x);
		packet.addFloat32(rotate_y);
		packet.addFloat32(rotate_z);
		packet.addFloat32(rotate_w);
	}
}
