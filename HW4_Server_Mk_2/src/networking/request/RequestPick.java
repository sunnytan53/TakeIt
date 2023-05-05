package networking.request;

// Java Imports
import java.io.IOException;

// Other Imports
import model.Player;
import networking.response.ResponsePick;
import utility.DataReader;
import core.NetworkManager;
import utility.Log;

public class RequestPick extends GameRequest {
    private int index;

    // Responses
    private ResponsePick responsePick;

    public RequestPick() {
        responses.add(responsePick = new ResponsePick());
    }

    @Override
    public void parse() throws IOException {
        index = DataReader.readInt(dataInput);
    }

    @Override
    public void doBusiness() throws Exception {
        Player player = client.getPlayer();

        responsePick.setPlayer(player);
        responsePick.setData(index);

        // Log.printf("In request pick, Player with id %d has taken the fruit to (%f, %f, %f)", player.getID(), move_x, move_y, move_z);

        NetworkManager.addResponseForAllOnlinePlayers(player.getID(), responsePick);
    }
}