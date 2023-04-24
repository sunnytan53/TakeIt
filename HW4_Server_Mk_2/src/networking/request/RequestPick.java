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
    private int fruitTag;
    private float move_x, move_y, move_z, velocity_x, velocity_y, velocity_z;

    // Responses
    private ResponsePick responsePick;

    public RequestPick() {
        responses.add(responsePick = new ResponsePick());
    }

    @Override
    public void parse() throws IOException {
        fruitTag = DataReader.readInt(dataInput);
        move_x = DataReader.readFloat(dataInput);
        move_y = DataReader.readFloat(dataInput);
        move_z = DataReader.readFloat(dataInput);
        velocity_x = DataReader.readFloat(dataInput);
        velocity_y = DataReader.readFloat(dataInput);
        velocity_z = DataReader.readFloat(dataInput);
    }

    @Override
    public void doBusiness() throws Exception {
        Player player = client.getPlayer();

        responsePick.setPlayer(player);
        responsePick.setData(fruitTag, move_x, move_y, move_z, velocity_x, velocity_y, velocity_z);

        // Log.printf("Player id got from RequestChat is: %d ", player.getID());
        
        NetworkManager.addResponseForAllOnlinePlayers(player.getID(), responsePick);
    }
}