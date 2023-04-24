using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;

public class ResponsePickEventArgs : ExtendedEventArgs
{
	public int user_id { get; set; } // The user_id of whom who sent the request
	public int fruitTag { get; set; } // The fruitTag sent back from server
	public float move_x { get; set; } // The movement in x direction
	public float move_y { get; set; } // The movement in y direction
	public float move_z { get; set; } // The movement in z direction
	public float velocity_x { get; set; } 
	public float velocity_y { get; set; } 
	public float velocity_z { get; set; }

	public ResponsePickEventArgs()
	{
		event_id = Constants.SMSG_PICK;
	}
}

public class ResponsePick : NetworkResponse
{
	private int user_id;
	private int fruitTag;
	public float move_x;
	public float move_y;
	public float move_z;
	public float velocity_x;
	public float velocity_y;
	public float velocity_z;

	public ResponsePick()
	{
	}

	public override void parse()
	{
		user_id = DataReader.ReadInt(dataStream);
		Debug.Log("user_id got from ResponsePick is: " + user_id);
		fruitTag = DataReader.ReadInt(dataStream);
		move_x = DataReader.ReadFloat(dataStream);
		move_y = DataReader.ReadFloat(dataStream);
		move_z = DataReader.ReadFloat(dataStream);
		velocity_x = DataReader.ReadFloat(dataStream);
		velocity_y = DataReader.ReadFloat(dataStream);
		velocity_z = DataReader.ReadFloat(dataStream);
	}

	public override ExtendedEventArgs process()
	{
		ResponsePickEventArgs args = new ResponsePickEventArgs
		{
			user_id = user_id,
			fruitTag = fruitTag,
			move_x = move_x,
			move_y = move_y,
			move_z = move_z,
			velocity_x = velocity_x,
			velocity_y = velocity_y,
			velocity_z = velocity_z
		};

		return args;
	}

}
