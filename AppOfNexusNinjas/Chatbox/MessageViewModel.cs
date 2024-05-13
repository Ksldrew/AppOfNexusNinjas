
using Microsoft.Maui.Controls;

namespace AppOfNexusNinjas.Chatbox;

    public class MessageViewModel
{
    public required string MessageText { get; set; }
    public required Color BubbleColor { get; set; }
    public LayoutOptions HorizontalAlignment { get; set; }
}