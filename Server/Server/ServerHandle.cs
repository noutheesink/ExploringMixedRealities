using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Device.Location;
using System.Numerics;
using System.Text;

namespace GameServer
{
    class ServerHandle
    {
        public static void WelcomeReceived(int _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();
            bool seeker = _packet.ReadBool();

            Console.WriteLine($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {_fromClient}. Seeker: {seeker}");
            if (_fromClient != _clientIdCheck)
            {
                Console.WriteLine($"Player with ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})!");
            }
            Server.clients[_fromClient].SendIntoGame(seeker);
        }

        public static void PlayerMovement(int _fromClient, Packet _packet)
        {
            bool[] _inputs = new bool[_packet.ReadInt()];
            for (int i = 0; i < _inputs.Length; i++)
            {
                _inputs[i] = _packet.ReadBool();
            }

            Server.clients[_fromClient].player.SetInput(_inputs);
        }
        public static void ClientGeoCoordinate(int _fromClient, Packet _packet)
        {
            Server.clients[_fromClient].player.coordinate = _packet.ReadGeoCoordinate();;
        }

        public static void Button(int _fromClient, Packet _packet)
        {
            GameLogic.HandleButton(_fromClient, _packet.ReadString());
        }
    }
}
