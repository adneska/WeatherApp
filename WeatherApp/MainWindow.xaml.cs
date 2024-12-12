using System.Text;
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
using System.Configuration;
using Microsoft.Extensions.Configuration;

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
        private static string GetApiKey()
        {
            var builder = new ConfigurationBuilder()
                .AddUserSecrets<MainWindow>(); 

            IConfiguration configuration = builder.Build();

            return configuration["ApiSettings:ApiKey"];
            
        }
        
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            getWeather();
        }
        public async Task getWeather() 
        {
            using (HttpClient web = new HttpClient())
            {
                
                var url = string.Format("https://api.openweathermap.org/data/2.5/weather?q={0}&Appid={1}", CityName.Text, GetApiKey());
                var json = await web.GetStringAsync(url);
                WeatherInfo.root Info = JsonConvert.DeserializeObject<WeatherInfo.root>(json);

                

                SetData(Info);
            }
        }

        private void SetData(WeatherInfo.root Info)
        {
            Condition.Text = Info.weather[0].main;
            Details.Text = Info.weather[0].description;
            Sunset.Text = GetCorrectTime(Info.sys.sunset);
            Sunrise.Text = GetCorrectTime(Info.sys.sunrise);
            WindSpeed.Text = Info.wind.speed.ToString();
            Preasure.Text = Info.main.pressure.ToString();

            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.UriSource = new Uri("https://openweathermap.org/img/w/" + Info.weather[0].icon + ".png");
            WeatherPicture.Source = bitmapImage;
        }

        public static string GetCorrectTime(long time)
        {
            DateTime day = DateTime.UnixEpoch.AddSeconds(time);
             
            return day.AddMilliseconds(time).ToLocalTime().ToShortTimeString();
        }
    }
}