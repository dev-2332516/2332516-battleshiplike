using LibrairieClasse;

namespace Serveur
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
            Themer.InitializeTheme();
            ServerConnection.StartListening();
        }
    }
}
