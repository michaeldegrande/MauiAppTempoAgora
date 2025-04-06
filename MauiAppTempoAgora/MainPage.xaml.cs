using Microsoft.Maui.Networking;
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
        // Verifica conexão
        if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
        {
            await DisplayAlert("Sem conexão", "Você está sem acesso à internet. Verifique sua conexão e tente novamente.", "OK");
            return;
        }

        // Verifica campo vazio
        if (string.IsNullOrWhiteSpace(txt_cidade.Text))
        {
            await DisplayAlert("Erro", "Digite uma cidade válida.", "OK");
            return;
        }

        var resultado = await DataService.GetPrevisao(txt_cidade.Text.Trim());

        if (resultado == null)
        {
            await DisplayAlert("Erro", "Cidade não encontrada ou erro ao buscar os dados.", "OK");
        }
        else
        {
            lbl_res.Text =
                $"Clima: {resultado.main}\n" +
                $"Descrição: {resultado.description}\n" +
                $"Temperatura: {resultado.temp_min}°C a {resultado.temp_max}°C\n" +
                $"Vento: {resultado.speed} m/s\n" +
                $"Visibilidade: {resultado.visibility} metros\n" +
                $"Nascer do sol: {resultado.sunrise}\n" +
                $"Pôr do sol: {resultado.sunset}";
        }
    }
}