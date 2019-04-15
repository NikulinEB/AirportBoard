using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportBoard.Model
{
    public class Airplane
    {
        public string Name { get; private set; }
        public int Capacity { get; private set; }

        public Airplane(string name, int capacity)
        {
            Name = name;
            Capacity = capacity;
        }
    }
}
