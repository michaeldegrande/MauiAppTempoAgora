using MauiAppTempoAgora.Models;
using Newtonsoft.Json.Linq;

namespace MauiAppTempoAgora.Services
{
    public class DataService
    {
        public static async Task<Tempo?> GetPrevisao(string cidade)
        {
            Tempo? t = null;

            string chave = "aca17d0d27a5eb68543af4978b64ac59";
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={cidade}&units=metric&appid={chave}&lang=pt_br";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage resp = await client.GetAsync(url);

                    if (resp.IsSuccessStatusCode)
                    {
                        string json = await resp.Content.ReadAsStringAsync();
                        var rascunho = JObject.Parse(json);

                        // Verificação de existência dos dados principais
                        if (rascunho["weather"] == null || rascunho["main"] == null || rascunho["sys"] == null)
                        {
                            await App.Current.MainPage.DisplayAlert("Erro", "Dados incompletos recebidos da API.", "OK");
                            return null;
                        }

                        DateTime time = new();
                        DateTime sunrise = time.AddSeconds((double?)rascunho["sys"]?["sunrise"] ?? 0).ToLocalTime();
                        DateTime sunset = time.AddSeconds((double?)rascunho["sys"]?["sunset"] ?? 0).ToLocalTime();

                        t = new Tempo
                        {
                            lat = (double?)rascunho["coord"]?["lat"] ?? 0,
                            lon = (double?)rascunho["coord"]?["lon"] ?? 0,
                            description = (string?)rascunho["weather"]?[0]?["description"] ?? "Sem descrição",
                            main = (string?)rascunho["weather"]?[0]?["main"] ?? "Sem dados",
                            temp_min = (double?)rascunho["main"]?["temp_min"] ?? 0,
                            temp_max = (double?)rascunho["main"]?["temp_max"] ?? 0,
                            speed = (double?)rascunho["wind"]?["speed"] ?? 0,
                            visibility = (int?)rascunho["visibility"] ?? 0,
                            sunrise = sunrise.ToString("HH:mm"),
                            sunset = sunset.ToString("HH:mm")
                        };
                    }
                    else
                    {
                        await App.Current.MainPage.DisplayAlert("Erro", "Cidade não encontrada ou erro ao buscar dados.", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Erro", $"Ocorreu um erro: {ex.Message}", "OK");
            }

            return t;
        }
    }
}
