package networking.request;

// Java Imports
import java.io.IOException;

// Other Imports
import model.Player;
import networking.response.ResponseChat;
import utility.DataReader;
import core.NetworkManager;
import utility.Log;

public class RequestChat extends GameRequest {
    private String msg;
    // Responses
    private ResponseChat responseChat;

    public RequestChat() {
        responses.add(responseChat = new ResponseChat());
    }

    @Override
    public void parse() throws IOException {
        msg = DataReader.readString(dataInput);
    }

    @Override
    public void doBusiness() throws Exception {
        Player player = client.getPlayer();

        responseChat.setPlayer(player);
        responseChat.setData(msg);

        // Log.printf("Player id got from RequestChat is: %d ", player.getID());
        
        NetworkManager.addResponseForAllOnlinePlayers(player.getID(), responseChat);
    }
}