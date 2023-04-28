package networking.request;

// Java Imports
import java.io.IOException;

// Other Imports
import model.Player;
import networking.response.ResponseThrow;
import utility.DataReader;
import core.NetworkManager;
import utility.Log;

public class RequestThrow extends GameRequest {
    private int fruitTag;
    private float force_x, force_y, force_z;

    // Responses
    private ResponseThrow responsePick;

    public RequestThrow() {
        responses.add(responsePick = new ResponseThrow());
    }

    @Override
    public void parse() throws IOException {
        fruitTag = DataReader.readInt(dataInput);
        force_x = DataReader.readFloat(dataInput);
        force_y = DataReader.readFloat(dataInput);
        force_z = DataReader.readFloat(dataInput);
    }

    @Override
    public void doBusiness() throws Exception {
        Player player = client.getPlayer();

        responsePick.setPlayer(player);
        responsePick.setData(fruitTag, force_x, force_y, force_z);

        Log.printf("In request pick, Player with id %d has taken the fruit to (%f, %f, %f)", player.getID(), force_x, force_y, force_z);

        NetworkManager.addResponseForAllOnlinePlayers(player.getID(), responsePick);
    }
}