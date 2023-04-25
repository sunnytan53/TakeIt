package networking.request;

// Java Imports
import java.io.IOException;

// Other Imports
import model.Player;
import networking.response.ResponseMovement;
import utility.DataReader;
import core.NetworkManager;
import utility.Log;

public class RequestMovement extends GameRequest {
    private float move_x, move_y, move_z, rotate_x, rotate_y, rotate_z, rotate_w;
    // Responses
    private ResponseMovement responseMovement;

    public RequestMovement() {
        responses.add(responseMovement = new ResponseMovement());
    }

    @Override
    public void parse() throws IOException {
        move_x = DataReader.readFloat(dataInput);
        move_y = DataReader.readFloat(dataInput);
        move_z = DataReader.readFloat(dataInput);
        rotate_x = DataReader.readFloat(dataInput);
        rotate_y = DataReader.readFloat(dataInput);
        rotate_z = DataReader.readFloat(dataInput);
        rotate_w = DataReader.readFloat(dataInput);
    }

    @Override
    public void doBusiness() throws Exception {
        
        Player player = client.getPlayer();
        // Log.printf("In request movement, Player with id %d has moved to (%f, %f, %f)", player.getID(), move_y, move_y, move_z);

        responseMovement.setPlayer(player);
        responseMovement.setData(move_x, move_y, move_z, rotate_x, rotate_y, rotate_z, rotate_w);
        NetworkManager.addResponseForAllOnlinePlayers(player.getID(), responseMovement);
    }
}