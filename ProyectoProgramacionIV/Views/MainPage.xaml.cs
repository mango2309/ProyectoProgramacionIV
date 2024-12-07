using ProyectoProgramacionIV.Views;

namespace ProyectoProgramacionIV.Views
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }


        private async void OnClientesButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ClientesPage());
        }
    }

}
