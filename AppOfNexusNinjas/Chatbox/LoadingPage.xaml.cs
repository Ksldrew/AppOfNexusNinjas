using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;

namespace AppOfNexusNinjas.Chatbox
{
    public partial class LoadingPage : ContentPage
    {
        public LoadingPage()
        {
            InitializeComponent();  // Make sure this method is correctly linked to the XAML
            StartTimer();
        }

        private async void StartTimer()
        {
            for (int i = 0; i <= 100; i++)
            {
                PercentageLabel.Text = $"{i}%";  // Ensure this Label is defined in XAML
                await Task.Delay(50);
            }

            await Navigation.PushAsync(new MainPage());  // Assumes MainPage is defined
        }
    }
}
