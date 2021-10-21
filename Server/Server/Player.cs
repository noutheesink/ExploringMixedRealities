using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Text;
using System.Numerics;

namespace GameServer
{
    class Player
    {
        public int id;

        public GeoCoordinate coordinate;

        private bool[] inputs;
        public readonly bool seeker;

        public Player(int _id, bool _seeker)
        {
            id = _id;
            coordinate = new GeoCoordinate(0,0);
            seeker = _seeker;
            inputs = new bool[4];
        }

        public void Update()
        {
            // if (inputs[0])
            // {
            //     //do whatever this needs to be doing
            // }

            if (seeker)
            {
            }
            
            SendInformation();
        }

        private void SendInformation()
        {
            ServerSend.PlayerPosition(this);
        }

        public void SetInput(bool[] _inputs)
        {
            inputs = _inputs;
        }
    }
}
