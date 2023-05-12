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
public class ResponseFruitPoint extends GameResponse {
    private Player player;
    private int index;
    private int points;

    public ResponseFruitPoint() {
        responseCode = Constants.SMSG_FRUITPOINT;
    }

    @Override
    public byte[] constructResponseInBytes() {
        GamePacket packet = new GamePacket(responseCode);
        packet.addInt32(player.getID());
        packet.addInt32(index);
        packet.addInt32(points);

        // Log.printf("In ResponseFruitPoint, Player with id %d FruitPoints a fruit with tag: %s with force: (%f, %f, %f)", player.getID(), index, force_x, force_y, force_z);
 
        return packet.getBytes();
    }

    public void setPlayer(Player player) {
        this.player = player;
    }

    public void setData(int index, int points) {
        this.index = index;
        this.points = points;
    }
}