using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AirportBoard.Model;
using AirportBoard.View;
using AirportBoard.Controllers;
using System.Threading;
using System.Windows.Threading;
using Newtonsoft.Json;

namespace AirportBoard
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class AirportBoard : Window, IAirportView
    {
        private readonly string _scheduleFileName = "Schedule.txt";
        public event EventHandler<int> SpeedChanged;
        private AirportController controller;

        public AirportBoard()
        {
            InitializeComponent();
            SpeedChanged += delegate { };
            Schedule schedule;
            var fileSchedule = FileOperations.Load(AppContext.BaseDirectory, _scheduleFileName);
            if (string.IsNullOrEmpty(fileSchedule))
            {
                schedule = GenerateFlights(10000);
                FileOperations.Save(AppContext.BaseDirectory, _scheduleFileName,
                    JsonConvert.SerializeObject(schedule, Formatting.Indented));
            }
            else
            {
                schedule = JsonConvert.DeserializeObject<Schedule>(fileSchedule);
            }
            controller = new AirportController(new Airport(schedule), this);
            controller.StartSimulationAsync();
        }

        private Schedule GenerateFlights(int count)
        {
            Schedule schedule = new Schedule();
            var rnd = new Random();
            var nextFlightTime = DateTime.Now;
            for (int i = 0; i < count; i++)
            {
                nextFlightTime = nextFlightTime.AddMinutes(rnd.Next(2, 50));
                schedule.AddFlight(new ScheduleRecord(
                    new Airplane("Airplane" + i.ToString(), 100),
                    (FlightType)rnd.Next(2),
                    nextFlightTime,
                    "City" + i.ToString()
                    ));
            }
            return schedule;
        }

        public void UpdateStats(Airport airport)
        {
            Dispatcher.Invoke(() =>
            {
                UpdateLastFlight(airport.LastFlight);
                UpdateArrivalsInfo(airport.ArrivalsStatistics);
                UpdateDeparturesInfo(airport.DeparturesStatistics);
            },
            DispatcherPriority.Send);
        }

        private void UpdateLastFlight(CompletedFlight lastFlight)
        {
            LastFlight.Content = $"Тип: {lastFlight?.FlightType}\r\nВремя: {lastFlight?.Time}\r\n" +
                $"Город: {lastFlight?.City}\r\nБорт: {lastFlight?.Airplane.Name}\r\nПасс.: {lastFlight?.PassengersCount}";
        }

        private void UpdateArrivalsInfo(AirportStatistics statistics)
        {
            ArrLastDayPassengers.Content = "За последние сутки: " + statistics.LastDayPassengers;
            ArrLastFlightPassengers.Content = "В последнем полете: " + statistics.LastFlightPassengers;
            ArrTotalPassengers.Content = "За всё время: " + statistics.TotalPassengers;
        }

        private void UpdateDeparturesInfo(AirportStatistics statistics)
        {
            DepLastDayPassengers.Content = "За последние сутки: " + statistics.LastDayPassengers;
            DepLastFlightPassengers.Content = "В последнем полете: " + statistics.LastFlightPassengers;
            DepTotalPassengers.Content = "За всё время: " + statistics.TotalPassengers;
        }

        public void UpdateSpeed(int speed)
        {
            Dispatcher.Invoke(() =>
            {
                SpeedTextBox.Text = speed.ToString();
            }, 
            DispatcherPriority.Send);
        }

        public void UpdateTime(DateTime currentTime)
        {
            Dispatcher.Invoke(() =>
            {
                CurrentTimeLabel.Content = $"Текущее время: {currentTime.ToString()}";
            },
            DispatcherPriority.Send);
        }

        private void SpeedTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int speed;
            if (int.TryParse(SpeedTextBox.Text, out speed))
            {
                SpeedChanged?.Invoke(this, speed);
            }
        }
    }
}
