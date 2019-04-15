using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirportBoard.Model;

namespace AirportBoard.View
{
    interface IAirportView
    {
        void UpdateStats(Airport airport);
        event EventHandler<int> SpeedChanged;
        void UpdateSpeed(int speed);
        void UpdateTime(DateTime currentTime);
    }
}
