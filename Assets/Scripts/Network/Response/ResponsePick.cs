using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;

public class ResponsePickEventArgs : ExtendedEventArgs
{
	public int user_id { get; set; } // The user_id of whom who sent the request
	public int index { get; set; }

	public ResponsePickEventArgs()
	{
		event_id = Constants.SMSG_PICK;
	}
}

public class ResponsePick : NetworkResponse
{
	private int user_id;
	private int index;

	public ResponsePick()
	{
	}

	public override void parse()
	{
		user_id = DataReader.ReadInt(dataStream);
		Debug.Log("user_id got from ResponsePick is: " + user_id);
		index = DataReader.ReadInt(dataStream);
	}

	public override ExtendedEventArgs process()
	{
		ResponsePickEventArgs args = new ResponsePickEventArgs
		{
			user_id = user_id,
			index = index
		};

		return args;
	}

}
