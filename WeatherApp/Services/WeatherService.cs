using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Model;

namespace WeatherApp.Services
{
    internal class WeatherService
    {
        private static string GetApiKey()
        {
            var builder = new ConfigurationBuilder()
                .AddUserSecrets<MainWindow>();

            IConfiguration configuration = builder.Build();

            return configuration["ApiSettings:ApiKey"];

        }
        
        public async Task<WeatherData> GetWeatherAsync(string CityName)
        {
            using HttpClient client = new HttpClient();

            var url = string.Format("https://api.openweathermap.org/data/2.5/weather?q={0}&Appid={1}&units=metric", CityName, GetApiKey());
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);

                response.EnsureSuccessStatusCode();

                string responseData = await response.Content.ReadAsStringAsync();
                var weatherInfo = JsonConvert.DeserializeObject<WeatherInfo.Root>(responseData);

                return new WeatherData
                {
                    Temperature = weatherInfo.main.temp,
                    Details = weatherInfo.weather[0].description,
                    Sunrise = GetCorrectTime(weatherInfo.sys.sunrise),
                    Sunset = GetCorrectTime(weatherInfo.sys.sunset),
                    WindSpeed = weatherInfo.wind.speed,
                    Pressure = weatherInfo.main.pressure
                };
            }
            catch (HttpRequestException httpEx)
            {
                throw new Exception("Ошибка HTTP-запроса. Проверьте соединение или корректность введенных данных.", httpEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Произошла непредвиденная ошибка.", ex);
            }

        }
        private static DateTime GetCorrectTime(long time)
        {
            DateTime day = DateTime.UnixEpoch.AddSeconds(time);

            return day.AddMilliseconds(time).ToLocalTime();
        }
    }
}
