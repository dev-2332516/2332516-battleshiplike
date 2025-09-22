using CsvHelper;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LibrairieClasse
{
    public static class Themer
    {
        public static int[] Theme = new int[5];
        private static ConsoleColor[] Colors = (ConsoleColor[])ConsoleColor.GetValues(typeof(ConsoleColor));
        private static List<ConsoleColor> ColorList = new List<ConsoleColor>() {
        ConsoleColor.Red,
        ConsoleColor.DarkBlue,
        ConsoleColor.Green,
        ConsoleColor.DarkYellow,
        ConsoleColor.DarkGray,
        ConsoleColor.Gray,
        ConsoleColor.White,
        ConsoleColor.Cyan};
        public static bool InitializeTheme()
        {
            // Check if file exists and deserializes it
            if (File.Exists("theme.json"))
            {
                string fileName = "theme.json";
                string jsonString = File.ReadAllText(fileName);
                Theme = JsonSerializer.Deserialize<int[]>(jsonString);
            }
            else AskForTheme();
            return true;
        }

        private static void AskForTheme()
        {
            // Demande de couleur pour chaque élément
            int choice = 0;
            while (choice >= ColorList.Count() || choice == 0)
            {
                Console.Clear();
                ShowColors();
                ResetStyle();
                Console.Write("\n\nChoix du board: ");
                choice = Interface.AskForNumber();
            }
            Theme[0] = Colors.ToList().IndexOf(ColorList[choice - 1]);
            ColorList.Remove(ColorList[choice - 1]);
            choice = 0;
            while (choice >= ColorList.Count() || choice == 0)
            {
                Console.Clear();
                ShowColors();
                ResetStyle();
                Console.Write("\n\nChoix du bateau: ");
                choice = Interface.AskForNumber();
            }
            Theme[1] = Colors.ToList().IndexOf(ColorList[choice - 1]);
            ColorList.Remove(ColorList[choice - 1]);
            choice = 0;
            while (choice >= ColorList.Count() || choice == 0)
            {
                Console.Clear();
                ShowColors();
                ResetStyle();
                Console.Write("\n\nChoix d'un coup maqué: ");
                choice = Interface.AskForNumber();
            }
            Theme[2] = Colors.ToList().IndexOf(ColorList[choice - 1]);
            ColorList.Remove(ColorList[choice - 1]);
            choice = 0;
            while (choice >= ColorList.Count() || choice == 0)
            {
                Console.Clear();
                ShowColors();
                ResetStyle();
                Console.Write("\n\nTouché sur bateau ennemi: ");
                choice = Interface.AskForNumber();
            }
            Theme[3] = Colors.ToList().IndexOf(ColorList[choice - 1]);
            ColorList.Remove(ColorList[choice - 1]);
            choice = 0;
            while (choice >= ColorList.Count() || choice == 0)
            {
                Console.Clear();
                ShowColors();
                ResetStyle();
                Console.Write("\n\nTouché sur votre bateau: ");
                choice = Interface.AskForNumber();
            }
            Theme[4] = Colors.ToList().IndexOf(ColorList[choice - 1]);
            ColorList.Remove(ColorList[choice - 1]);
            choice = 0;

            string fileName = "theme.json";
            string jsonString = JsonSerializer.Serialize(Theme);
            File.WriteAllText(fileName, jsonString);
        }

        public static ConsoleColor GetColor(int i)
        {
            return Colors[Theme[i]];
        }

        private static void ShowColors()
        {
            Console.ForegroundColor = ConsoleColor.Black;
            foreach (ConsoleColor color in ColorList)
            {
                Console.BackgroundColor = color;
                Console.WriteLine(ColorList.IndexOf(color) + 1);
            }
            Console.BackgroundColor = ConsoleColor.Black;
        }

        public static void ResetStyle()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;
        }
    }
}
