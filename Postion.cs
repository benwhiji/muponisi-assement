using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClosestVehicles
{
    internal class Position
    {
        private string _name;
        private float _longitude;
        private float _latitude;

        public Position(string name, float latitude, float longitude)
        {
            _name = name;
            _longitude = longitude;
            _latitude = latitude;
        }

        public string Name { get => _name; set => _name = value; }

        public float Langitude { get => _longitude; set => _longitude = value; }

        public float Latitude { get => _latitude; set => _latitude = value; }


    }
}