using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseJoinEventArgs : ExtendedEventArgs
{
	public short status { get; set; } // 0 = success
	public int user_id { get; set; } // 1 is first to join, 2 is second, anything else is not valid!
	public List<UserData> users { get; set; }

	public ResponseJoinEventArgs()
	{
		event_id = Constants.SMSG_JOIN;
	}
}

public class UserData
{
    public int UserId { get; set; }
    public string UserName { get; set; }
	public int bodyIndex { get; set; }
	public int faceIndex { get; set; }

	public UserData()
    {
    }
}

public class ResponseJoin : NetworkResponse
{
	private short status;
	private int user_id;
	private List<UserData> users;

	public ResponseJoin()
	{
	}

	public override void parse()
	{
		users = new List<UserData>();
		status = DataReader.ReadShort(dataStream);
		if (status == 0)
		{
			user_id = DataReader.ReadInt(dataStream);
			while (dataStream.Position < dataStream.Length) {
				UserData user = new UserData();
				user.UserId = DataReader.ReadInt(dataStream);
				user.UserName = DataReader.ReadString(dataStream);
		    	user.bodyIndex = DataReader.ReadInt(dataStream);
				user.faceIndex = DataReader.ReadInt(dataStream);

				users.Add(user);
			}
		}
	}

	public override ExtendedEventArgs process()
	{
		ResponseJoinEventArgs args = new ResponseJoinEventArgs
		{
			status = status
		};
		if (status == 0)
		{
			args.user_id = user_id;
			args.users = users;
		}

		return args;
	}
}
