using LibrairieClasse;

namespace Serveur
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Themer.ResetStyle();
            Console.Clear();
            Themer.InitializeTheme();
            Console.Clear();
            ServerConnection.StartListening();
        }
    }
}
