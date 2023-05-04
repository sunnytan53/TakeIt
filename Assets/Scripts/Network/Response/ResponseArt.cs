using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseArtEventArgs : ExtendedEventArgs
{
	public int user_id { get; set; }
	public short code { get; set; }

	public ResponseArtEventArgs()
	{
		event_id = Constants.SMSG_ART;
	}
}

public class ResponseArt : NetworkResponse
{
	private int user_id;
	private short code;

	public ResponseArt()
	{
	}

	public override void parse()
	{
		user_id = DataReader.ReadInt(dataStream);
		code = DataReader.ReadShort(dataStream);
	}

	public override ExtendedEventArgs process()
	{
		ResponseArtEventArgs args = new ResponseArtEventArgs
		{
			user_id = user_id,
			code = code
		};

		return args;
	}
}
