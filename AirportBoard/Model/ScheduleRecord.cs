using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportBoard.Model
{
    public enum FlightType
    {
        Arrival,
        Departure
    }
    
    public class ScheduleRecord
    {
        [JsonProperty]
        public Airplane Airplane { get; private set; }
        [JsonProperty]
        public FlightType FlightType { get; private set; }
        [JsonProperty]
        public DateTime Time { get; private set; }
        [JsonProperty]
        public string City { get; private set; }

        public ScheduleRecord()
        {

        }

        public ScheduleRecord(Airplane airplane, FlightType flightType, DateTime time, string city)
        {
            Airplane = airplane;
            FlightType = flightType;
            Time = time;
            City = city;
        }
    }

    public class CompletedFlight: ScheduleRecord
    {
        public int PassengersCount { get; private set; }

        public CompletedFlight(ScheduleRecord record, int passengersCount) : 
            base(record.Airplane, record.FlightType, record.Time, record.City)
        {
            PassengersCount = passengersCount;
        }
    }
}
