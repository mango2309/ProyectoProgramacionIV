using ProyectoProgramacionIV.Models;
using ProyectoProgramacionIV.Services;
using System.Collections.ObjectModel;

namespace ProyectoProgramacionIV.Views;

public partial class ReservacionesPage : ContentPage
{
    private FileService _fileService;
    private List<Reservacion> _reservaciones;
    private List<Cliente> _clientes;
    private List<Carro> _carros;

    public ReservacionesPage()
    {
        InitializeComponent();
        _fileService = new FileService("reservaciones.json");
        _reservaciones = new List<Reservacion>();
        _clientes = new List<Cliente>();
        _carros = new List<Carro>();

        LoadClientes();
        LoadCarros();
        LoadReservaciones();
    }

    // Cargar clientes
    private async void LoadClientes()
    {
        var clienteService = new FileService("clientes.json");
        _clientes = await clienteService.LoadDataAsync<Cliente>();
        ClientePicker.ItemsSource = _clientes;
        ClientePicker.ItemDisplayBinding = new Binding("Nombre");
    }

    // Cargar carros
    private async void LoadCarros()
    {
        var carroService = new FileService("carros.json");
        _carros = await carroService.LoadDataAsync<Carro>();
        CarroPicker.ItemsSource = _carros;
        CarroPicker.ItemDisplayBinding = new Binding("Marca");
    }

    // Cargar reservaciones
    private async void LoadReservaciones()
    {
        _reservaciones = await _fileService.LoadDataAsync<Reservacion>();

        foreach (var reservacion in _reservaciones)
        {
            var cliente = _clientes.FirstOrDefault(c => c.IdCliente == reservacion.IdCliente);
            var carro = _carros.FirstOrDefault(c => c.IdCarro == reservacion.IdCarro);

            reservacion.NombreCliente = cliente?.Nombre;
            reservacion.NombreCarro = carro?.Marca;
        }

        ReservacionesListView.ItemsSource = _reservaciones;
    }


    // Guardar reservaciones
    private async void SaveReservaciones()
    {
        await _fileService.SaveDataAsync(_reservaciones);
    }

    // Agregar nueva reservación
    private void AddReservacionButton_Clicked(object sender, EventArgs e)
    {
        if (ClientePicker.SelectedItem == null || CarroPicker.SelectedItem == null)
        {
            DisplayAlert("Error", "Debe seleccionar un cliente y un carro", "OK");
            return;
        }

        var selectedCliente = (Cliente)ClientePicker.SelectedItem;
        var selectedCarro = (Carro)CarroPicker.SelectedItem;

        var newReservacion = new Reservacion
        {
            IdReservacion = _reservaciones.Count + 1,
            IdCliente = selectedCliente.IdCliente,
            IdCarro = selectedCarro.IdCarro,
            FechaReserva = FechaReservaPicker.Date,
            NombreCliente = selectedCliente.Nombre, // Asignar el nombre del cliente
            NombreCarro = selectedCarro.Marca      // Asignar la marca del carro
        };

        _reservaciones.Add(newReservacion);
        ReservacionesListView.ItemsSource = null;
        ReservacionesListView.ItemsSource = _reservaciones;

        SaveReservaciones();
    }

    private async void DeleteAllReservacionesButton_Clicked(object sender, EventArgs e)
    {
        if (await DisplayAlert("Confirmación", "¿Deseas eliminar todas las reservaciones?", "Sí", "No"))
        {
            _reservaciones.Clear();
            ReservacionesListView.ItemsSource = null;
            await _fileService.SaveDataAsync(_reservaciones);
        }
    }
}