package networking.response;

// Other Imports
import metadata.Constants;
import model.Player;
import utility.GamePacket;
import utility.Log;

/**
 * The ResponseLogin class contains information about the authentication
 * process.
 */
public class ResponsePlayerControl extends GameResponse {
    private Player player;
    private float move_x, move_y, move_z, rotate_x, rotate_y, rotate_z, rotate_w;
    private short aCode, sCode;

    public ResponsePlayerControl() {
        responseCode = Constants.SMSG_PLAYER_CONTROL;
    }

    @Override
    public byte[] constructResponseInBytes() {
        GamePacket packet = new GamePacket(responseCode);
        packet.addInt32(player.getID());
        packet.addFloat(move_x);
        packet.addFloat(move_y);
        packet.addFloat(move_z);
        packet.addFloat(rotate_x);
        packet.addFloat(rotate_y);
        packet.addFloat(rotate_z);
        packet.addFloat(rotate_w);
        packet.addShort16(aCode);
        packet.addShort16(sCode);

        return packet.getBytes();
    }

    public void setPlayer(Player player) {
        this.player = player;
    }

    public void setData(float move_x, float move_y, float move_z, float rotate_x, float rotate_y, float rotate_z, float rotate_w, short aCode, short sCode) {
        this.move_x = move_x;
        this.move_y = move_y; 
        this.move_z = move_z;
        this.rotate_x = rotate_x;
        this.rotate_y = rotate_y;
        this.rotate_z = rotate_z;
        this.rotate_w = rotate_w;
        this.aCode = aCode;
        this.sCode = sCode;
    }
}