package networking.request;

// Java Imports
import java.io.IOException;

// Other Imports
import model.Player;
import networking.response.ResponseFruitPoint;
import utility.DataReader;
import core.NetworkManager;
import utility.Log;

public class RequestFruitPoint extends GameRequest {
    private int index;
    private int points;

    // Responses
    private ResponseFruitPoint responseFruitPoint;

    public RequestFruitPoint() {
        responses.add(responseFruitPoint = new ResponseFruitPoint());
    }

    @Override
    public void parse() throws IOException {
        index = DataReader.readInt(dataInput);
        points = DataReader.readInt(dataInput);
    }

    @Override
    public void doBusiness() throws Exception {
        Player player = client.getPlayer();

        responseFruitPoint.setPlayer(player);
        responseFruitPoint.setData(index, points);

        NetworkManager.addResponseForAllOnlinePlayers(player.getID(), responseFruitPoint);
    }
}