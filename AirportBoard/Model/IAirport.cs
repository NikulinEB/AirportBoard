using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportBoard.Model
{
    interface IAirport
    {
        Task StartWorking();
        CompletedFlight LastFlight { get; }
        DateTime CurrentTime { get; }
        event EventHandler<DateTime> TimeUpdated;
        event EventHandler<Airport> ScheduleUpdated;
        event EventHandler<int> SpeedChanged;
        void ChangeSpeed(int speed);
        AirportStatistics ArrivalsStatistics { get; }
        AirportStatistics DeparturesStatistics { get; }
    }
}
