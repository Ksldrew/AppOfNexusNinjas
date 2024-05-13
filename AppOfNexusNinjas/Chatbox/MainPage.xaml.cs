using Microsoft.Maui.Controls;
using System;
using AppOfNexusNinjas.Services;
using AppOfNexusNinjas.Chatbox;
using static AppOfNexusNinjas.Services.UTSInfo; // Add the correct using directive for the namespace containing ApiService

namespace AppOfNexusNinjas
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void SendMessage_Clicked(object sender, EventArgs e)
        {
            string message = MessageEntry.Text;
            if (!string.IsNullOrEmpty(message))
            {
                DisplayMessage(message, Colors.Blue);
                // Provide the required parameters for ApiService constructor
                var service = new ApiService("Your_API_Key_Here", "foundationJsonPath", "utsJsonPath");
                var result = await service.GenerateResponse(message);
                DisplayMessage(result, Colors.Green);
                MessageEntry.Text = "";
            }
        }

        private void DisplayMessage(string message, Color color)
        {
            var messageBubble = new MessageBubble
            {
                BindingContext = new MessageViewModel { MessageText = message, BubbleColor = color, HorizontalAlignment = LayoutOptions.End }
            };
            MessageContainer.Children.Add(messageBubble);
        }

        // Event handler for the Completed event of the MessageEntry Editor
        private void OnMessageEntryCompleted(object sender, EventArgs e)
        {
            SendMessage_Clicked(sender, e);  // Call send message when enter is pressed
        }
    }
}



