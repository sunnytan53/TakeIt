using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponsePlayerControlEventArgs : ExtendedEventArgs
{
	public int user_id { get; set; }
	public Vector3 position { get; set; }
	public Quaternion rotation { get; set; }
	public animationCode aCode { get; set; }
	public soundCode sCode { get; set; }

	public ResponsePlayerControlEventArgs()
	{
		event_id = Constants.SMSG_PLAYER_CONTROL;
	}
}

public class ResponsePlayerControl : NetworkResponse
{
	private int user_id;
	private float pos_x;
	private float pos_y;
	private float pos_z;
	private float rotate_x;
	private float rotate_y;
	private float rotate_z;
	private float rotate_w;
	private short sCode;
	private short aCode;

	public ResponsePlayerControl()
	{
	}

	public override void parse()
	{
		user_id = DataReader.ReadInt(dataStream);
		pos_x = DataReader.ReadFloat(dataStream);
		pos_y = DataReader.ReadFloat(dataStream);
		pos_z = DataReader.ReadFloat(dataStream);
		rotate_x = DataReader.ReadFloat(dataStream);
		rotate_y = DataReader.ReadFloat(dataStream);
		rotate_z = DataReader.ReadFloat(dataStream);
		rotate_w = DataReader.ReadFloat(dataStream);
		sCode = DataReader.ReadShort(dataStream);
		aCode = DataReader.ReadShort(dataStream);
	}

	public override ExtendedEventArgs process()
	{
		ResponsePlayerControlEventArgs args = new ResponsePlayerControlEventArgs()
		{
			user_id = user_id,
			position = new Vector3(pos_x, pos_y, pos_z),
			rotation = new Quaternion(rotate_x, rotate_y, rotate_z, rotate_w),
			aCode = (animationCode) aCode,
			sCode = (soundCode) sCode
		};
		return args;
	}
}
