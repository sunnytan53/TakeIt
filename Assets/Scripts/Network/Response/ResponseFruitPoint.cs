using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;

public class ResponseFruitPointEventArgs : ExtendedEventArgs
{
	public int user_id { get; set; } // The user_id of whom who sent the request
	public int index { get; set; }
	public int points { get; set; }

	public ResponseFruitPointEventArgs()
	{
		event_id = Constants.SMSG_FRUITPOINT;
	}
}

public class ResponseFruitPoint : NetworkResponse
{
	private int user_id;
	public int index;
	public int points;

	public ResponseFruitPoint()
	{
	}

	public override void parse()
	{
		user_id = DataReader.ReadInt(dataStream);
		index = DataReader.ReadInt(dataStream);
		points = DataReader.ReadInt(dataStream);
	}

	public override ExtendedEventArgs process()
	{
		ResponseFruitPointEventArgs args = new ResponseFruitPointEventArgs
		{
			user_id = user_id,
			index = index,
			points = points
		};

		return args;
	}

}
