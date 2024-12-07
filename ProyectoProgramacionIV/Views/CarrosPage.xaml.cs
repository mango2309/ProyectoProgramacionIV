using ProyectoProgramacionIV.Models;
using ProyectoProgramacionIV.Services;
using System.Collections.ObjectModel;

namespace ProyectoProgramacionIV.Views;

public partial class CarrosPage : ContentPage
{
    private FileService _fileService;
    private List<Carro> _carros;

    public CarrosPage()
    {
        InitializeComponent();
        _fileService = new FileService("carros.json");
        _carros = new List<Carro>();
        LoadCarros();
    }

    private async void LoadCarros()
    {
        _carros = await _fileService.LoadDataAsync<Carro>();
        CarrosListView.ItemsSource = _carros;
    }

    private void AddCarroButton_Clicked(object sender, EventArgs e)
    {
        if (CarroPicker.SelectedIndex == -1) // Asegurarse de que el usuario haya seleccionado un auto
        {
            DisplayAlert("Error", "Por favor selecciona un modelo de auto.", "OK");
            return;
        }

        var seleccion = CarroPicker.SelectedItem.ToString();
        var datos = seleccion.Split('-'); // Divide la cadena "Marca - Modelo"

        var newCarro = new Carro
        {
            IdCarro = _carros.Count + 1,  // Asignar un ID único
            Marca = datos[0].Trim(),      // Extraer la marca
            Modelo = datos[1].Trim()     // Extraer el modelo
        };

        _carros.Add(newCarro);
        CarrosListView.ItemsSource = null;  // Refrescar la lista
        CarrosListView.ItemsSource = _carros;

        SaveCarros();
    }

    private async void SaveCarros()
    {
        await _fileService.SaveDataAsync(_carros);
    }

    private void CarrosListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem != null)
        {
            var selectedCarro = (Carro)e.SelectedItem;
            DisplayAlert("Carro Seleccionado", $"{selectedCarro.Marca} {selectedCarro.Modelo}", "OK");
        }
    }
    private async void DeleteAllCarrosButton_Clicked(object sender, EventArgs e)
    {
        if (await DisplayAlert("Confirmación", "¿Deseas eliminar todos los carros?", "Sí", "No"))
        {
            _carros.Clear();
            CarrosListView.ItemsSource = null;
            await _fileService.SaveDataAsync(_carros);
        }
    }
}