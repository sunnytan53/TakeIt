package networking.request;

// Java Imports
import java.io.IOException;

// Other Imports
import model.Player;
import networking.response.ResponseArt;
import utility.DataReader;
import core.NetworkManager;
import utility.Log;

public class RequestArt extends GameRequest {
    private short code;
    // Responses
    private ResponseArt responseArt;

    public RequestArt() {
        responses.add(responseArt = new ResponseArt());
    }

    @Override
    public void parse() throws IOException {
        this.code = DataReader.readShort(dataInput);
    }

    @Override
    public void doBusiness() throws Exception {
        
        Player player = client.getPlayer();
        responseArt.setPlayer(player);
        responseArt.setData(this.code);
        NetworkManager.addResponseForAllOnlinePlayers(player.getID(), responseArt);
    }
}