package networking.request;

// Java Imports

import core.NetworkManager;
import model.Player;
import networking.response.ResponsePlayerControl;
import utility.DataReader;
import utility.Log;

import java.io.IOException;

public class RequestPlayerControl extends GameRequest {
    private float move_x, move_y, move_z, rotate_x, rotate_y, rotate_z, rotate_w;
    private short aCode, sCode;
    private ResponsePlayerControl responsePlayerControl;

    public RequestPlayerControl() {
        responses.add(responsePlayerControl = new ResponsePlayerControl());
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
        aCode = DataReader.readShort(dataInput);
        sCode = DataReader.readShort(dataInput);
    }

    @Override
    public void doBusiness() throws Exception {
        
        Player player = client.getPlayer();
        Log.printf(player.getID() + " request with (%f, %f, %f, %f, %f, %f, %f, %d, %d)", move_x, move_y, move_z, rotate_x, rotate_y, rotate_z, rotate_w, aCode, sCode);

        responsePlayerControl.setPlayer(player);
        responsePlayerControl.setData(move_x, move_y, move_z, rotate_x, rotate_y, rotate_z, rotate_w, aCode, sCode);
        NetworkManager.addResponseForAllOnlinePlayers(player.getID(), responsePlayerControl);
    }
}