using ProyectoProgramacionIV.Models;
using ProyectoProgramacionIV.Services;
using System.Collections.ObjectModel;

namespace ProyectoProgramacionIV.Views;

public partial class ClientesPage : ContentPage
{
    private FileService _fileService;
    private List<Cliente> _clientes;

    public ClientesPage()
    {
        InitializeComponent();
        _fileService = new FileService("clientes.json");
        _clientes = new List<Cliente>();
        LoadClientes();
    }

    private async void LoadClientes()
    {
        _clientes = await _fileService.LoadDataAsync<Cliente>();
        ClientesListView.ItemsSource = _clientes;
    }

    private async void SaveClientes()
    {
        await _fileService.SaveDataAsync(_clientes);
    }

    private void AddClienteButton_Clicked(object sender, EventArgs e)
    {
        // Verificar que los campos no est�n vac�os
        if (string.IsNullOrEmpty(NombreEntry.Text) || string.IsNullOrEmpty(EmailEntry.Text))
        {
            DisplayAlert("Error", "Todos los campos deben ser llenados", "OK");
            return;
        }

        var newCliente = new Cliente
        {
            IdCliente = _clientes.Count + 1,
            Nombre = NombreEntry.Text,
            Email = EmailEntry.Text
        };

        _clientes.Add(newCliente);
        ClientesListView.ItemsSource = null;
        ClientesListView.ItemsSource = _clientes;

        // Limpiar los campos despu�s de agregar
        NombreEntry.Text = string.Empty;
        EmailEntry.Text = string.Empty;

        SaveClientes();
    }

    private async void DeleteAllClientesButton_Clicked(object sender, EventArgs e)
    {
        if (await DisplayAlert("Confirmaci�n", "�Deseas eliminar todos los clientes?", "S�", "No"))
        {
            _clientes.Clear();
            ClientesListView.ItemsSource = null;
            await _fileService.SaveDataAsync(_clientes);
        }
    }
}