package networking.request;

// Java Imports
import java.io.IOException;

// Other Imports
import model.Player;
import networking.response.ResponseFruitUpdate;
import utility.DataReader;
import core.NetworkManager;
import utility.Log;

public class RequestFruitUpdate extends GameRequest {
    private float[] positions;
    // Responses
    private ResponseFruitUpdate responseFruitUpdate;

    public RequestFruitUpdate() {
        responses.add(responseFruitUpdate = new ResponseFruitUpdate());
    }

    @Override
    public void parse() throws IOException {
        positions = new float[DataReader.readInt(dataInput)];
        for (int i = 0; i < positions.length; i++) {
            positions[i] = DataReader.readFloat(dataInput);
        }
    }

    @Override
    public void doBusiness() throws Exception {
        
        Player player = client.getPlayer();
        // Log.printf("In request FruitUpdate, Player with id %d has moved to (%f, %f, %f)", player.getID(), move_y, move_y, move_z);

        responseFruitUpdate.setPlayer(player);
        responseFruitUpdate.setData(positions);
        NetworkManager.addResponseForAllOnlinePlayers(player.getID(), responseFruitUpdate);
    }
}