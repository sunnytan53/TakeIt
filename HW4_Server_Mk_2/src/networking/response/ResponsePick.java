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
    private int index;

    public ResponsePick() {
        responseCode = Constants.SMSG_PICK;
    }

    @Override
    public byte[] constructResponseInBytes() {
        GamePacket packet = new GamePacket(responseCode);
        packet.addInt32(player.getID());
        packet.addInt32(index);

        // Log.printf("In ResponsePick, Player with id %d has picked a fruit with tag: %s to (%f, %f, %f)", player.getID(), fruitTag, move_x, move_y, move_z);
 
        return packet.getBytes();
    }

    public void setPlayer(Player player) {
        this.player = player;
    }

    public void setData(int index) {
        this.index = index;
    }
}