using MauiAppTempoAgora.Models;
using Newtonsoft.Json.Linq;

namespace MauiAppTempoAgora.Services;

public class DataService
{
    public static async Task<Tempo?> GetPrevisao(string cidade)
    {
        if (string.IsNullOrWhiteSpace(cidade))
            return null;

        Tempo? t = null;
        string chave = "aca17d0d27a5eb68543af4978b64ac59";
        string url = $"https://api.openweathermap.org/data/2.5/weather?q={cidade}&units=metric&appid={chave}&lang=pt";

        try
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage resp = await client.GetAsync(url);

                if (!resp.IsSuccessStatusCode)
                    return null;

                string json = await resp.Content.ReadAsStringAsync();
                var rascunho = JObject.Parse(json);

                DateTime time = new();
                DateTime sunrise = time.AddSeconds((double)rascunho["sys"]["sunrise"]).ToLocalTime();
                DateTime sunset = time.AddSeconds((double)rascunho["sys"]["sunset"]).ToLocalTime();

                t = new Tempo
                {
                    lat = (double)rascunho["coord"]["lat"],
                    lon = (double)rascunho["coord"]["lon"],
                    description = (string)rascunho["weather"][0]["description"],
                    main = (string)rascunho["weather"][0]["main"],
                    temp_min = (double)rascunho["main"]["temp_min"],
                    temp_max = (double)rascunho["main"]["temp_max"],
                    speed = (double)rascunho["wind"]["speed"],
                    visibility = (int)rascunho["visibility"],
                    sunrise = sunrise.ToString("HH:mm"),
                    sunset = sunset.ToString("HH:mm")
                };
            }
        }
        catch
        {
            return null;
        }

        return t;
    }
}
