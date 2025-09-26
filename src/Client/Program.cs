using LibrairieClasse;

namespace Client
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Reset la console au vide 
            Themer.ResetStyle();
            Console.Clear();
            Themer.InitializeTheme();
            Console.Clear();
            ClientConnection.StartClient();
        }
    }

}
