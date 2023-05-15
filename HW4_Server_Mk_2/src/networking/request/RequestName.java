package networking.request;

// Java Imports
import java.io.IOException;
import utility.Log;


// Other Imports
import model.Player;
import networking.response.ResponseName;
import utility.DataReader;
import core.NetworkManager;

public class RequestName extends GameRequest {
    // Data
    private int i1, i2;
    private String name;

    // Responses
    private ResponseName responseName;

    public RequestName() {
        responses.add(responseName = new ResponseName());
    }

    @Override
    public void parse() throws IOException {
        i1 = DataReader.readInt(dataInput);
        i2 = DataReader.readInt(dataInput);
        name = DataReader.readString(dataInput).trim();
        
        Log.printf("In requestName, the i1 is %d, i2 is %d, name is %s", i1, i2, name);
    }

    @Override
    public void doBusiness() throws Exception {
        Player player = client.getPlayer();
       
        player.setName(name);
        responseName.setPlayer(player);
        responseName.setData(i1, i2);

        NetworkManager.addResponseForAllOnlinePlayers(player.getID(), responseName);
    }
}