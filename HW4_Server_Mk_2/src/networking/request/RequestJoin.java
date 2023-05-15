package networking.request;

// Java Imports
import java.io.IOException;

// Other Imports
import core.GameServer;
import core.NetworkManager;
import model.Player;
import networking.response.ResponseJoin;
import networking.response.ResponseName;
import utility.DataReader;
import utility.Log;

/**
 * The RequestLogin class authenticates the user information to log in. Other
 * tasks as part of the login process lies here as well.
 */

public class RequestJoin extends GameRequest {
    // Data
    private Player player;
    private int i1, i2;

    // Responses
    private ResponseJoin responseJoin;

    public RequestJoin() {
        responses.add(responseJoin = new ResponseJoin());
    }

    @Override
    public void parse() throws IOException {
        i1 = DataReader.readInt(dataInput);
        i2 = DataReader.readInt(dataInput);
    }

    @Override
    public void doBusiness() throws Exception {
        GameServer gs = GameServer.getInstance();
        int id = gs.getID();
        if(id != 0) {
            player = new Player(id, "Player " + id);
            player.setID(id);
            player.setI1(i1);
            player.setI2(i2);
            player.setClient(client);
            gs.setActivePlayer(player);
            // Pass Player reference into thread
            client.setPlayer(player);
            // Set response information
            responseJoin.setStatus((short) 0); // Login is a success
            responseJoin.setPlayer(player);
            Log.printf("User '%s' has successfully logged in.", player.getName());
        } else {
            Log.printf("A user has tried to join, but failed to do so.");
            responseJoin.setStatus((short) 1); 
        }
    }
}
