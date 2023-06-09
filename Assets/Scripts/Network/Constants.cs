public class Constants
{
	// Constants
	public static readonly string CLIENT_VERSION = "1.00";
	//public static readonly string REMOTE_HOST = "localhost";
	public static readonly string REMOTE_HOST = "18.144.87.42";
	public static readonly int REMOTE_PORT = 1729;
	
	// Request (1xx) + Response (2xx)
	public static readonly short CMSG_JOIN = 101;
	public static readonly short SMSG_JOIN = 201;
	public static readonly short CMSG_LEAVE = 102;
	public static readonly short SMSG_LEAVE = 202;
	public static readonly short CMSG_SETNAME = 103;
	public static readonly short SMSG_SETNAME = 203;
	public static readonly short CMSG_READY = 104;
	public static readonly short SMSG_READY = 204;
	//public static readonly short CMSG_MOVE = 105;
	//public static readonly short SMSG_MOVE = 205;
	//public static readonly short CMSG_INTERACT = 106;
	//public static readonly short SMSG_INTERACT = 206;
	public static readonly short CMSG_CHAT = 107;
	public static readonly short SMSG_CHAT = 207;
	public static readonly short CMSG_MOVEMENT = 108;
	public static readonly short SMSG_MOVEMENT = 208;
	public static readonly short CMSG_PICK = 109;
	public static readonly short SMSG_PICK = 209;
	public static readonly short CMSG_THROW = 110;
	public static readonly short SMSG_THROW = 210;

	public static readonly short CMSG_HEARTBEAT = 111;

	public static readonly short CMSG_ART = 120;
	public static readonly short SMSG_ART = 220;
	public static readonly short CMSG_FRUIT = 121;
	public static readonly short SMSG_FRUIT = 221;
	public static readonly short CMSG_FRUITPOINT = 122;
	public static readonly short SMSG_FRUITPOINT = 222;


	public static int USER_ID = -1;
	public static int OP_ID = -1;
}