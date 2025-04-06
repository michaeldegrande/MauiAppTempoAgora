using MauiAppTempoAgora.Models;
using MauiAppTempoAgora.Services;

namespace MauiAppTempoAgora;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txt_cidade.Text))
        {
            await DisplayAlert("Ops", "Digite o nome de uma cidade.", "OK");
            return;
        }

        try
        {
            var resultado = await DataService.GetPrevisao(txt_cidade.Text);

            if (resultado == null)
            {
                await DisplayAlert("Erro", "Não foi possível obter a previsão do tempo.", "OK");
                return;
            }

            lbl_res.Text =
                $"Clima: {resultado.main}\n" +
                $"Descrição: {resultado.description}\n" +
                $"Temperatura: {resultado.temp_min}°C a {resultado.temp_max}°C\n" +
                $"Vento: {resultado.speed} m/s\n" +
                $"Visibilidade: {resultado.visibility} metros\n" +
                $"Nascer do sol: {resultado.sunrise}\n" +
                $"Pôr do sol: {resultado.sunset}";
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erro inesperado", ex.Message, "OK");
        }
    }
}
