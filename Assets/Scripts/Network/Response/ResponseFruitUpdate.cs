using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseFruitUpdateEventArgs : ExtendedEventArgs
{
	public int user_id { get; set; } // The user_id of whom who sent the request
	public Vector3[] positions { get; set; } 

	public ResponseFruitUpdateEventArgs()
	{
		event_id = Constants.SMSG_FRUIT;
	}
}

public class ResponseFruitUpdate : NetworkResponse
{
	private int user_id;
	private Vector3[] positions;

	public ResponseFruitUpdate()
	{
	}

	public override void parse()
	{
		user_id = DataReader.ReadInt(dataStream);
		positions = new Vector3[DataReader.ReadInt(dataStream)/3];
		for (int i = 0; i < positions.Length; i++)
        {
			positions[i] = new Vector3(
				DataReader.ReadFloat(dataStream),
				DataReader.ReadFloat(dataStream),
				DataReader.ReadFloat(dataStream));
		}
	}

	public override ExtendedEventArgs process()
	{
		ResponseFruitUpdateEventArgs args = new ResponseFruitUpdateEventArgs
		{
			user_id = user_id,
			positions = positions
		};

		return args;
	}
}
