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
            //int longueur, hauteur;
            //bool valid = false;
            //while (!valid)
            //{
            //    // Affiche les instructions et demande la taille
            //    Console.WriteLine("La taille de la maquette doit être entre 4x4 et 12x12\n");
            //    Console.Write("Longueur de la maquette: ");
            //    longueur = Interface.AskForNumber();
            //    Console.Write("\nHauteur de la maquette: ");
            //    hauteur = Interface.AskForNumber();

            //    // Verifier si la taille entrée est valide 
            //    if (longueur >= 4 && longueur <= 12 && hauteur >= 4 && hauteur <= 12) valid = true;
            //    else
            //    {
            //        // Affiche le message d'erreur
            //        Console.Clear();
            //        Console.ForegroundColor = ConsoleColor.Red;
            //        Console.WriteLine("Taille entrée invalide");
            //        Console.ForegroundColor = ConsoleColor.White;
            //    }
            //    Interface.Longueur = longueur;
            //    Interface.Hauteur = hauteur;
            //}
            //while (true)
            //{
            //    Interface.Longueur = 6;
            //    Interface.Hauteur = 5;
            //    Console.Clear();
            //    Interface.SelectGrid(2, 2);
            //    Console.ReadLine();
            //}
        }
    }

}
