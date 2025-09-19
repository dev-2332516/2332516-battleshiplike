using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrairieClasse
{
    public static class Themer
    {
        public static ConsoleColor[] Theme = new ConsoleColor[5];
        public static bool InitializeTheme()
        {
            // Check if file exists and deserializes it
            if (File.Exists("theme.csv")) return true;
            else AskForTheme();
            return true;
        }

        private static void AskForTheme()
        {
            // Theme de base 
            #region theme de base
            //Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;

            Console.WriteLine("Theme de base:");
            Console.ForegroundColor = ConsoleColor.Black;

            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("Maquette\t\n");

            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.Write("Bateau\t\n");

            Console.BackgroundColor = ConsoleColor.Green;
            Console.Write("Touché bateau Ennemi\t\n");

            Console.BackgroundColor = ConsoleColor.Red;
            Console.Write("Touché bateau joueur\t\n");

            Console.BackgroundColor = ConsoleColor.Gray;
            Console.Write("Manqué\t\n\n");

            #endregion theme de base

            #region theme maritime
            // Theme maritime
            //Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine("Theme maritime:");
            Console.ForegroundColor = ConsoleColor.Black;

            Console.BackgroundColor = ConsoleColor.Blue;
            Console.Write("Maquette\t\n");

            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.Write("Bateau\t\n");

            Console.BackgroundColor = ConsoleColor.Green;
            Console.Write("Touché bateau Ennemi\t\n"); 

            Console.BackgroundColor = ConsoleColor.Red;
            Console.Write("Touché bateau joueur\t\n");

            Console.BackgroundColor = ConsoleColor.Gray;
            Console.Write("Manqué\t\n\n");

            #endregion theme maritime
            Console.ReadKey();

            Console.BackgroundColor = ConsoleColor.Black;


            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write("Choix 1");
            //Console.BackgroundColor = ConsoleColor.
        }   
    }
}
