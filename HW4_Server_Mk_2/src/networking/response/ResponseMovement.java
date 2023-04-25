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
public class ResponseMovement extends GameResponse {
    private Player player;
    private float move_x;
    private float move_y;
    private float move_z;
    private float rotate_x;
    private float rotate_y;
    private float rotate_z;
    private float rotate_w;

    public ResponseMovement() {
        responseCode = Constants.SMSG_MOVEMENT;
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

        // Log.printf("In response movement, Player with id %d has moved to (%f, %f, %f)", player.getID(), move_y, move_y, move_z);
 
        return packet.getBytes();
    }

    public void setPlayer(Player player) {
        this.player = player;
    }

    public void setData(float move_x, float move_y, float move_z, float rotate_x, float rotate_y, float rotate_z, float rotate_w) {
        this.move_x = move_x;
        this.move_y = move_y; 
        this.move_z = move_z;
        this.rotate_x = rotate_x;
        this.rotate_y = rotate_y;
        this.rotate_z = rotate_z;
        this.rotate_w = rotate_w;
    }
}