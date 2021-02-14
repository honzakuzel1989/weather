using System;
using System.Collections.Generic;
using System.Text;

namespace weather.Core.Entities
{
    public class WeatherData
    {
        public WeatherData(Location address, CurrentWeather current, DailyWeather[] daily)
        {
            Location = address;
            Current = current;
            Daily = daily;
        }

        public Location Location { get; set; }
        public CurrentWeather Current { get; }
        public DailyWeather[] Daily { get; }
    }

    public class Location
    {
        public Location(string title)
        {
            Title = title;
        }
        public string Title { get; }

    }

    public class CurrentWeather
    {
        public CurrentWeather(SunTime sunrise, SunTime sunset, float realTemp, float feelsTemp, int pressure, int humidity, float windSpeed, WeatherText text)
        {
            Sunrise = sunrise;
            Sunset = sunset;
            RealTemp = realTemp;
            FeelsTemp = feelsTemp;
            Pressure = pressure;
            Humidity = humidity;
            WindSpeed = windSpeed;
            Text = text;
        }

        public SunTime Sunrise { get; }
        public SunTime Sunset { get; }
        public Temperature RealTemp { get; }
        public Temperature FeelsTemp { get; }
        public int Pressure { get; }
        public int Humidity { get; }
        public float WindSpeed { get; }
        public WeatherText Text { get; }
    }

    public class WeatherText
    {
        public WeatherText(string title, string description)
        {
            Title = title;
            Description = description;
        }

        public string Title { get; }
        public string Description { get; }

        public override string ToString()
        {
            return $"{Title} ({Description})";
        }
    }

    public class DailyWeather
    {
        public DailyWeather(DateTime date, SunTime sunrise, SunTime sunset, RealTemp realTemp, FeelsTemp feelsTemp, int pressure, int humidity, float windSpeed, float snow, float rain, WeatherText text, float probability)
        {
            Date = date;
            Sunrise = sunrise;
            Sunset = sunset;
            RealTemp = realTemp;
            FeelsTemp = feelsTemp;
            Pressure = pressure;
            Humidity = humidity;
            WindSpeed = windSpeed;
            Snow = snow;
            Rain = rain;
            Text = text;
            Probability = probability;
        }

        public DateTime Date { get; }
        public SunTime Sunrise { get; }
        public SunTime Sunset { get; }
        public RealTemp RealTemp { get; }
        public FeelsTemp FeelsTemp { get; }
        public int Pressure { get; }
        public int Humidity { get; }
        public float WindSpeed { get; }
        public float Snow { get; }
        public float Rain { get; }
        public WeatherText Text { get; }
        public float Probability { get; }
    }

    public class RealTemp
    {
        public RealTemp(float day, float min, float max, float night, float eve, float morn)
        {
            Day = day;
            Min = min;
            Max = max;
            Night = night;
            Eve = eve;
            Morn = morn;
        }

        public Temperature Day { get; }
        public Temperature Min { get; }
        public Temperature Max { get; }
        public Temperature Night { get; }
        public Temperature Eve { get; }
        public Temperature Morn { get; }
    }

    public class FeelsTemp
    {
        public FeelsTemp(float day, float night, float eve, float morn)
        {
            Day = day;
            Night = night;
            Eve = eve;
            Morn = morn;
        }

        public Temperature Day { get; }
        public Temperature Night { get; }
        public Temperature Eve { get; }
        public Temperature Morn { get; }
    }

    public class SunTime
    {
        public SunTime(int hour, int minute)
        {
            Hour = hour;
            Minute = minute;
        }

        public int Hour { get; }
        public int Minute { get; }

        public override string ToString()
        {
            return $"{Hour.ToString("00")}:{Minute.ToString("00")}";
        }
    }

    public class Temperature
    {
        public Temperature(float value)
        {
            Value = value;
        }

        public float Value { get; }

        public override string ToString()
        {
            return Value.ToString("+0.0;-0.0;0");
        }

        public static implicit operator Temperature(float temp)
        {
            return new Temperature(temp);
        }
    }
}
