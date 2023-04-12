using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;

public class ResponseChatEventArgs : ExtendedEventArgs
{
	public int user_id { get; set; } // The user_id of whom who sent the request
	public string message { get; set; } // The message sent back from server

	public ResponseChatEventArgs()
	{
		event_id = Constants.SMSG_CHAT;
	}
}

public class ResponseChat : NetworkResponse
{
	private int user_id;
	private string message;

	public ResponseChat()
	{
	}


	public override void parse()
	{
		user_id = DataReader.ReadInt(dataStream);
		Debug.Log("user_id got from ResponseChat is: " + user_id);
		message = DataReader.ReadString(dataStream);
	}

	public override ExtendedEventArgs process()
	{
		ResponseChatEventArgs args = new ResponseChatEventArgs
		{
			user_id = user_id,
			message = message,
		};

		return args;
	}

}
