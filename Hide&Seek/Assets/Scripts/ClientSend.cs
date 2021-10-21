using System.Collections;
using System.Collections.Generic;
using System.Device.Location;
using UnityEngine;

public class ClientSend : MonoBehaviour
{
    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.tcp.SendData(_packet);
    }

    private static void SendUDPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.udp.SendData(_packet);
    }

    #region Packets
    public static void WelcomeReceived()
    {
        using (Packet _packet = new Packet((int)ClientPackets.welcomeReceived))
        {
            _packet.Write(Client.instance.myId);
            _packet.Write(UIManager.instance.seeker);

            SendTCPData(_packet);
        }
    }

    public static void ClientCoordinates(GeoCoordinate coordinate)
    {
        using (Packet _packet = new Packet((int)ClientPackets.clientGeoCoordinates))
        {
            _packet.Write(coordinate);
            SendUDPData(_packet);
        }
    }

    public static void PlayerMovement(bool[] _inputs)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerMovement))
        {
            _packet.Write(_inputs.Length);
            foreach (bool _input in _inputs)
            {
                _packet.Write(_input);
            }

            SendUDPData(_packet);
        }
    }

    public static void Button(string buttonFunction)
    {
        using (Packet _packet = new Packet((int)ClientPackets.actionButton))
        {
            _packet.Write(buttonFunction);
            SendTCPData(_packet);
        }
    }
    #endregion
}
