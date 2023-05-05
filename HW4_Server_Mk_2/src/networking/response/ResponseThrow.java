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
public class ResponseThrow extends GameResponse {
    private Player player;
    private int index;
    private float force_x;
    private float force_y;
    private float force_z;

    public ResponseThrow() {
        responseCode = Constants.SMSG_THROW;
    }

    @Override
    public byte[] constructResponseInBytes() {
        GamePacket packet = new GamePacket(responseCode);
        packet.addInt32(player.getID());
        packet.addInt32(index);
        packet.addFloat(force_x);
        packet.addFloat(force_y);
        packet.addFloat(force_z);

        // Log.printf("In ResponseThrow, Player with id %d throws a fruit with tag: %s with force: (%f, %f, %f)", player.getID(), index, force_x, force_y, force_z);
 
        return packet.getBytes();
    }

    public void setPlayer(Player player) {
        this.player = player;
    }

    public void setData(int index, float force_x, float force_y, float force_z) {
        this.index = index;
        this.force_x = force_x;
        this.force_y = force_y; 
        this.force_z = force_z;
    }
}