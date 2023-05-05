using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;

public class ResponseThrowEventArgs : ExtendedEventArgs
{
	public int user_id { get; set; } // The user_id of whom who sent the request
	public int index { get; set; } // The fruitTag sent back from server
	public float force_x { get; set; } // The force in x direction
	public float force_y { get; set; } // The force in y direction
	public float force_z { get; set; } // The force in z direction

	public ResponseThrowEventArgs()
	{
		event_id = Constants.SMSG_THROW;
	}
}

public class ResponseThrow : NetworkResponse
{
	private int user_id;
	private int index;
	public float force_x;
	public float force_y;
	public float force_z;

	public ResponseThrow()
	{
	}

	public override void parse()
	{
		user_id = DataReader.ReadInt(dataStream);
		Debug.Log("user_id got from ResponseThrow is: " + user_id);
		index = DataReader.ReadInt(dataStream);
		force_x = DataReader.ReadFloat(dataStream);
		force_y = DataReader.ReadFloat(dataStream);
		force_z = DataReader.ReadFloat(dataStream);
	}

	public override ExtendedEventArgs process()
	{
		ResponseThrowEventArgs args = new ResponseThrowEventArgs
		{
			user_id = user_id,
			index = index,
			force_x = force_x,
			force_y = force_y,
			force_z = force_z
		};

		return args;
	}

}
