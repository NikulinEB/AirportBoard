using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AirportBoard.Model;
using AirportBoard.View;

namespace AirportBoard.Controllers
{
    class AirportController
    {
        private IAirport _airport;
        private IAirportView _view;

        public AirportController(IAirport airport, IAirportView scheduleView)
        {
            _airport = airport;
            _airport.ScheduleUpdated += UpdateView;
            _airport.SpeedChanged += ShowSpeed;
            _airport.TimeUpdated += UpdateTimeInView;
            _view = scheduleView;
            _view.SpeedChanged += ChangeSpeed;
        }

        public async void StartSimulationAsync()
        {
            await _airport.StartWorking();
        }

        private void UpdateView(object sender, Airport airport)
        {
            _view.UpdateStats(airport);
        }

        private void ChangeSpeed(object sender, int speed)
        {
            _airport.ChangeSpeed(speed);
        }

        private void ShowSpeed(object sender, int speed)
        {
            _view.UpdateSpeed(speed);
        }

        private void UpdateTimeInView(object sender, DateTime currentTime)
        {
            _view.UpdateTime(currentTime);
        }
    }
}
