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
public class ResponsePick extends GameResponse {
    private Player player;
    private int fruitTag;
    private float move_x;
    private float move_y;
    private float move_z;
    private float velocity_x;
    private float velocity_y;
    private float velocity_z;

    public ResponsePick() {
        responseCode = Constants.SMSG_PICK;
    }

    @Override
    public byte[] constructResponseInBytes() {
        GamePacket packet = new GamePacket(responseCode);
        packet.addInt32(player.getID());
        packet.addInt32(fruitTag);
        packet.addFloat(move_x);
        packet.addFloat(move_y);
        packet.addFloat(move_z);
        packet.addFloat(velocity_x);
        packet.addFloat(velocity_y);
        packet.addFloat(velocity_z);

        Log.printf("Player with id %d has picked a fruit with tag: %s", player.getID(), fruitTag);
 
        return packet.getBytes();
    }

    public void setPlayer(Player player) {
        this.player = player;
    }

    public void setData(int fruitTag, float move_x, float move_y, float move_z, float velocity_x, float velocity_y, float velocity_z) {
        this.fruitTag = fruitTag;
        this.move_x = move_x;
        this.move_y = move_y; 
        this.move_z = move_z;
        this.velocity_x = velocity_x;
        this.velocity_y = velocity_y;
        this.velocity_z = velocity_z;
    }
}