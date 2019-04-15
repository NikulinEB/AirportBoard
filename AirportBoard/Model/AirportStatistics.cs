using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportBoard.Model
{
    public class AirportStatistics
    {
        public int TotalPassengers { get; private set; }
        public int LastFlightPassengers { get; private set; }
        public int LastDayPassengers { get; private set; }
        public int[] HoursPassengers { get; private set; } = new int[24];
        private LinkedList<CompletedFlight> _doneFlights = new LinkedList<CompletedFlight>();
    
        public void FlightCompleted(CompletedFlight flight, long currentTime)
        {
            _doneFlights.AddFirst(flight);
            TotalPassengers += flight.PassengersCount;
            LastFlightPassengers = flight.PassengersCount;
            LastDayPassengers = ComputeLastDayPassengers(_doneFlights, currentTime);
            HoursPassengers = ComputeHoursPassengers(_doneFlights, currentTime);
        }

        private int ComputeLastDayPassengers(LinkedList<CompletedFlight> doneFlights, long currentTime)
        {
            int lastDayPassengers = 0;
            var lastDay = new TimeSpan(currentTime).Subtract(new TimeSpan(24, 0, 0)).Ticks;
            foreach(var flight in doneFlights)
            {
                if (flight.Time.Ticks > lastDay)
                {
                    lastDayPassengers += flight.PassengersCount;
                }
                else
                {
                    break;
                }
            }
            return lastDayPassengers;
        }

        private int[] ComputeHoursPassengers(LinkedList<CompletedFlight> doneFlights, long currentTime)
        {
            int[] hoursPassengers = new int[24];
            var time = new TimeSpan(currentTime).Ticks;
            var timeHourAgo = new TimeSpan(time).Subtract(TimeSpan.FromHours(1)).Ticks;
            for (int i = hoursPassengers.Count(); i >= 0; i--)
            {
                foreach (var flight in doneFlights)
                {
                    if (flight.Time.Ticks > time && flight.Time.Ticks < timeHourAgo)
                    {
                        hoursPassengers[i] += flight.PassengersCount;
                    }
                }
                time = timeHourAgo;
                timeHourAgo = new TimeSpan(timeHourAgo).Subtract(TimeSpan.FromHours(1)).Ticks;
            }
            return hoursPassengers;
        }
    }
}
