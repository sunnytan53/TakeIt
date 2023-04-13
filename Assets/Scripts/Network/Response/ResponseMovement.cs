using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseMovementEventArgs : ExtendedEventArgs
{
	public int user_id { get; set; } // The user_id of whom who sent the request
	public float move_x { get; set; } // The movement in x direction
	public float move_y { get; set; } // The movement in y direction
	public float move_z { get; set; } // The movement in z direction
	public float rotate_x { get; set; } 
	public float rotate_y { get; set; } 
	public float rotate_z { get; set; } 
	public float rotate_w { get; set; } 

	public ResponseMovementEventArgs()
	{
		event_id = Constants.SMSG_MOVEMENT;
	}
}

public class ResponseMovement : NetworkResponse
{
	private int user_id;
	private float move_x;
	private float move_y;
	private float move_z;
	private float rotate_x;
	private float rotate_y;
	private float rotate_z;
	private float rotate_w;

	public ResponseMovement()
	{
	}

	public override void parse()
	{
		user_id = DataReader.ReadInt(dataStream);
		move_x = DataReader.ReadFloat(dataStream);
		move_y = DataReader.ReadFloat(dataStream);;
		move_z = DataReader.ReadFloat(dataStream);;
		rotate_x = DataReader.ReadFloat(dataStream);;
		rotate_y = DataReader.ReadFloat(dataStream);;
		rotate_z = DataReader.ReadFloat(dataStream);;
		rotate_w = DataReader.ReadFloat(dataStream);;
	}

	public override ExtendedEventArgs process()
	{
		ResponseMovementEventArgs args = new ResponseMovementEventArgs
		{
			user_id = user_id,
			move_x = move_x,
			move_y = move_y,
			move_z = move_z,
			rotate_x = rotate_x,
			rotate_y = rotate_y,
			rotate_z = rotate_z,
			rotate_w = rotate_w
		};

		return args;
	}
}
