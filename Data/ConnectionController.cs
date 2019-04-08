using System.Collections.Generic;
class ConnectionController {
    Dictionary<string, string> gateComponentsConnection;
    public void add(string gateComponent, string gateName) =>
        gateComponentsConnection.Add(gateComponent, gateName);
}