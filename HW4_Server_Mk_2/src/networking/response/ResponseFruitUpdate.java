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
public class ResponseFruitUpdate extends GameResponse {
    private Player player;
    private float[] positions;

    public ResponseFruitUpdate() {
        responseCode = Constants.SMSG_FRUIT;
    }

    @Override
    public byte[] constructResponseInBytes() {
        GamePacket packet = new GamePacket(responseCode);
        packet.addInt32(player.getID());
        packet.addInt32(positions.length);
        for (int i = 0; i < positions.length; i++) {
            packet.addFloat(positions[i]);;
        }

        return packet.getBytes();
    }

    public void setPlayer(Player player) {
        this.player = player;
    }

    public void setData(float[] positions) {
        this.positions = positions;
    }
}