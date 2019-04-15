using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AirportBoard.Model
{
    public class Airport: IAirport
    {
        public Schedule Schedule { get; private set; }
        public event EventHandler<Airport> ScheduleUpdated;
        public event EventHandler<int> SpeedChanged;
        public event EventHandler<DateTime> TimeUpdated;
        public AirportStatistics ArrivalsStatistics { get; private set; } = new AirportStatistics();
        public AirportStatistics DeparturesStatistics { get; private set; } = new AirportStatistics();
        public CompletedFlight LastFlight { get; private set; }
        public DateTime CurrentTime { get; private set; }
        private int _simulationSpeed = 1;

        public Airport() : this(new Schedule())
        {
        }   

        public Airport(Schedule schedule)
        {
            Schedule = schedule;
            ScheduleUpdated += delegate { };
            SpeedChanged += delegate { };
            TimeUpdated += delegate { };
            ScheduleRecord firstFlight;
            if (Schedule.TryGetNextFlight(out firstFlight))
            {
                CurrentTime = firstFlight.Time;
            }
            else
            {
                CurrentTime = DateTime.Now;
            }
        }

        public async Task StartWorking()
        {
            await Task.Run(() => Working()); 
        }

        public void AddFlight(ScheduleRecord record)
        {   
            Schedule.AddFlight(record);
        }

        public void FlightDone()
        {
            var flight = Schedule.RemoveLastRecord();
            var rnd = new Random();
            CompletedFlight completedFlight;
            switch (flight.FlightType)
            {
                case FlightType.Arrival:
                    completedFlight = new CompletedFlight(flight, rnd.Next(1, flight.Airplane.Capacity));
                    ArrivalsStatistics.FlightCompleted(completedFlight, CurrentTime.Ticks);
                    break;
                case FlightType.Departure:
                    completedFlight = new CompletedFlight(flight, rnd.Next(1, flight.Airplane.Capacity));
                    DeparturesStatistics.FlightCompleted(completedFlight, CurrentTime.Ticks);
                    break;
                default:
                    return;
            }
            LastFlight = completedFlight;
            ScheduleUpdated(this, this);
        }

        public void ChangeSpeed(int speed)
        {
            _simulationSpeed = speed;
            SpeedChanged(this, speed);
        }

        private void Working()
        {
            ScheduleUpdated(this, this);
            SpeedChanged(this, _simulationSpeed);
            ScheduleRecord nextFlight;
            while (true)
            {
                // Симулируем течение времени с указанной скоростью.
                for (int i = 0; i < 10; i++)
                {
                    int time = 1000 / _simulationSpeed;
                    Thread.Sleep(time);
                    CurrentTime = CurrentTime.AddSeconds(1);
                    TimeUpdated(this, CurrentTime);
                }
                // Проверяем, есть ли рейсы, которые должны быть закончены.
                while (Schedule.TryGetNextFlight(out nextFlight))
                {
                    if (CurrentTime >= nextFlight.Time)
                    {
                        FlightDone();
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    }
}
