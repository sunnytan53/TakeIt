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
    public int UserId { get; }
    public string UserName { get; }
    public bool UserReady { get; }

    public UserData(int userId, string userName, bool userReady)
    {
        UserId = userId;
        UserName = userName;
        UserReady = userReady;
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
			Debug.Log("In ResponseJoin, the user_id is: " + user_id);
			while (dataStream.Position < dataStream.Length) {
				int userId = DataReader.ReadInt(dataStream);
				Debug.Log("In ResponseJoin while loop, the userId is: " + userId);
				if (userId != 0) {
					string userName = DataReader.ReadString(dataStream);
		    		bool userReady = DataReader.ReadBool(dataStream);

		    		UserData user = new UserData(userId, userName, userReady);
		    		users.Add(user);
				}
				else {
					break;
				}
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
