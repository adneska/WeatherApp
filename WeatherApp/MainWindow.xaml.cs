﻿using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;

namespace WeatherApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        string APIKey = "fbc651e37c2f6a34113a44b53d6a3574";
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            getWeather();
        }
        public async Task getWeather() 
        {
            using (HttpClient web = new HttpClient())
            {
                var url = string.Format("https://api.openweathermap.org/data/2.5/weather?q={0}&Appid={1}", CityName.Text, APIKey);
                var json = await web.GetStringAsync(url);
                WeatherInfo.root Info = JsonConvert.DeserializeObject<WeatherInfo.root>(json);

                BitmapImage bitmapImage = new BitmapImage();                
                bitmapImage.UriSource = new Uri("https://openweathermap.org/img/w/" + Info.weather[0].icon + ".png");                
                WeatherPicture.Source = bitmapImage;

                Condition.Text = Info.weather[0].main;
                Details.Text = Info.weather[0].description;
                Sunset.Text = GetCorrectTime(Info.sys.sunset);
                Sunrise.Text = GetCorrectTime(Info.sys.sunrise);
                WindSpeed.Text = Info.wind.speed.ToString();
                Preasure.Text = Info.main.pressure.ToString();

            }
        }
        public static string GetCorrectTime(long time)
        {
            DateTime day = DateTime.UnixEpoch.AddSeconds(time);
             
            return day.AddMilliseconds(time).ToLocalTime().ToShortTimeString();
        }
    }
}