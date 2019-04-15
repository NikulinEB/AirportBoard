using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportBoard.Model
{
    public class Schedule
    {
        [JsonProperty]
        public Queue<ScheduleRecord> Records { get; private set; } = new Queue<ScheduleRecord>();
        public ScheduleRecord LastFlight { get; private set; }

        public void AddFlight(ScheduleRecord record)
        {
            Records.Enqueue(record);
        }

        public ScheduleRecord RemoveLastRecord()
        {
            LastFlight = Records.Dequeue();
            return LastFlight;
        }

        public bool TryGetNextFlight(out ScheduleRecord nextFlight)
        {
            try
            {
                if (Records.Count > 0)
                {
                    nextFlight = Records.Peek();
                    return true;
                }
                else
                {
                    nextFlight = new ScheduleRecord();
                    return false;
                }
            }
            catch
            {
                nextFlight = new ScheduleRecord();
                return false;
            }
        }
    }
}
