using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseSetNameEventArgs : ExtendedEventArgs
{
	public int user_id { get; set; } // The user_id of whom who sent the request
	public int bodyIndex { get; set; }
	public int faceIndex { get; set; }
	public string name { get; set; } // Their new name

	public ResponseSetNameEventArgs()
	{
		event_id = Constants.SMSG_SETNAME;
	}
}

public class ResponseSetName : NetworkResponse
{
	private int user_id;
	private int bodyIndex;
	private int faceIndex;
	private string name;

	public ResponseSetName()
	{
	}

	public override void parse()
	{
		user_id = DataReader.ReadInt(dataStream);
		name = DataReader.ReadString(dataStream);
		bodyIndex = DataReader.ReadInt(dataStream);
		faceIndex = DataReader.ReadInt(dataStream);
	}

	public override ExtendedEventArgs process()
	{
		ResponseSetNameEventArgs args = new ResponseSetNameEventArgs
		{
			user_id = user_id,
			bodyIndex = bodyIndex,
			faceIndex = faceIndex,
			name = name
		};

		return args;
	}
}
