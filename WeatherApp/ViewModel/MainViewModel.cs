using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WeatherApp.Model;
using WeatherApp.Services;
using CommunityToolkit.Mvvm.Input;

namespace WeatherApp.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly WeatherService _WeatherService;
        private string _city;
        private WeatherData _weather;

        public string City
        {
            get => _city;
            set
            {
                _city = value;
                OnPropertyChanged();
            }
        }

        public WeatherData Weather
        {
            get => _weather;
            set
            {
                _weather = value;
                OnPropertyChanged();
            }
        }

        public ICommand GetWeatherCommand { get; }

        public MainViewModel()
        {
            _WeatherService = new WeatherService();
            GetWeatherCommand = new RelayCommand(async () => await LoadWeather());
        }

        private async Task LoadWeather()
        {
            if (!string.IsNullOrWhiteSpace(City))
                Weather = await _WeatherService.GetWeatherAsync(City);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
