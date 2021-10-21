using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace GameServer
{
    class GameLogic
    {
        public static void Update()
        {
            foreach (Client _client in Server.clients.Values)
            {
                if (_client.player != null)
                {
                    _client.player.Update();
                }
            }

            ThreadManager.UpdateMain();
        }

        public static void HandleButton(int _fromClient, string _buttonFunction)
        {
            switch (_buttonFunction)
            {
                case "jamButton":
                    JamButton(_fromClient);
                    break;
                default:
                    break;
            }
        }

        private static void JamButton(int _fromClient)
        {
            GeoCoordinate _fromClientGeoCoordinate = Server.clients[_fromClient].player.coordinate;

            int _toClient = _fromClient;
            
            double minDistance = double.PositiveInfinity;
            // Server.clients.Where(client => client.Value.player.seeker).Min(client =>
            //     _fromClientGeoCoordinate.GetDistanceTo(client.Value.player.coordinate));
            foreach (var client in Server.clients)
            {
                if (client.Value.player == null) continue;
                if (client.Value.player.seeker)
                {
                    double currentDistance = _fromClientGeoCoordinate.GetDistanceTo(client.Value.player.coordinate);
                    if (currentDistance < minDistance)
                    {
                        minDistance = currentDistance;
                        _toClient = client.Value.id;
                    }
                }
            }

            Console.WriteLine("found something? i guess    " + _toClient + " and " + _fromClient);
            if (_toClient == _fromClient) return;
            
            ServerSend.Button(_toClient, "jamButton");
        }
    }
}
