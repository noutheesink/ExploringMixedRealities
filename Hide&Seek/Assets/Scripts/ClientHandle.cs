using System.Collections;
using System.Collections.Generic;
using System.Device.Location;
using System.Net;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
    public static void Welcome(Packet _packet)
    {
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();

        Debug.Log($"Message from server: {_msg}");
        Client.instance.myId = _myId;
        ClientSend.WelcomeReceived();

        Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
    }

    public static void SpawnPlayer(Packet _packet)
    {
        int _id = _packet.ReadInt();
        GeoCoordinate _coordinate = _packet.ReadGeoCoordinate();

        GameManager.instance.SpawnPlayer(_id, _coordinate);
    }

    public static void PlayerPosition(Packet _packet)
    {
        int _id = _packet.ReadInt();
        GeoCoordinate _coordinate = _packet.ReadGeoCoordinate();

        GameManager.players[_id] = _coordinate;
    }
    
    public static void Button(Packet _packet)
    {
        string buttonFunction = _packet.ReadString();
        
    }

    public static void PlayerDisconnected(Packet _packet)
    {
        int _id = _packet.ReadInt();
        GameManager.players.Remove(_id);
    }
}
