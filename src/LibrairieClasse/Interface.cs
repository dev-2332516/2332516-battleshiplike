using System.Numerics;
using System.Threading.Tasks;
using System.Xml;

namespace LibrairieClasse
{
    public class Interface
    {
        public static string[,] FullMap;

        public static void DrawGame(string[,] hitMap, string[,] playerMap)
        {
            // Montre la map hit et la map miss
            Console.WriteLine("Carte des hit et des miss");
            DrawMap(hitMap, 4, 4, 2, 2);
            Console.WriteLine("\n");
            Console.WriteLine("Votre carte:");
            DrawMap(playerMap, 4, 4, 2, 10);
        }

        /// <summary>
        /// Crée la map et choisi la position du bateau
        /// </summary>
        /// <param name="longueur"></param>
        /// <param name="hauteur"></param>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        /// <returns></returns>
        public static string[,] CreateInitialMap(int longueur, int hauteur, int posX, int posY)
        {
            string[,] fullBaseMap = new string[longueur, hauteur];
            int tempPosY = posY + 1;
            // Definir toutes les lignes
            Console.SetCursorPosition(posX, posY - 1);
            for (int i = 1; i <= longueur; i++)
            {
                if (i < 10)
                {
                    Console.Write(i + " ");
                }
                else Console.Write(i);
            }
            Console.SetCursorPosition(posX, posY);
            for (int y = 0; y < hauteur; y++)
            {
                for (int x = 0; x < longueur; x++)
                {
                    fullBaseMap[x, y] = "-";
                }
            }
            for (int y = 0; y < hauteur; y++)
            {
                for (int x = 0; x < longueur; x++)
                {
                    DrawBlockWithColor(fullBaseMap[x, y]);
                }
                Console.SetCursorPosition(posX, tempPosY - 2);
                Console.Write("\n" + NumberToLetter(y));
                Console.SetCursorPosition(posX, tempPosY);
                tempPosY++;
            }
            FullMap = fullBaseMap;
            return fullBaseMap;
        }

        public static void DrawMap(string[,] mapToDraw, int longueur, int hauteur, int posX, int posY)
        {
            int tempPosY = posY + 1;
            Console.SetCursorPosition(posX, posY - 1);
            for (int i = 1; i <= longueur; i++)
            {
                if (i < 10)
                {
                    Console.Write(i + " ");
                }
                else Console.Write(i);
            }
            Console.SetCursorPosition(posX, posY);
            for (int y = 0; y < hauteur; y++)
            {

                for (int x = 0; x < longueur; x++)
                { 
                    DrawBlockWithColor(mapToDraw[x, y]);
                }
                Console.SetCursorPosition(posX, tempPosY - 2);
                Console.Write("\n" + NumberToLetter(y));
                Console.SetCursorPosition(posX, tempPosY);
                tempPosY++;
            }
        }
        
        /// <summary>
        /// Dessine un carré avec une couleur selon le charactere dans l'input
        /// </summary>
        /// <param name="c"></param>
        public static void DrawBlockWithColor(string c)
        {
            //gives out a console color for wathever char it gets, might make this a switch or an enum in the future idk
            if (c == "-") Console.BackgroundColor = ConsoleColor.White;
            if (c == "B") Console.BackgroundColor = ConsoleColor.DarkYellow;
            if (c == "M") Console.BackgroundColor = ConsoleColor.Gray;
            if (c == "H") Console.BackgroundColor = ConsoleColor.Green;
            if (c == "S") Console.BackgroundColor = ConsoleColor.Red;

            Console.Write("  ");
            Console.ResetColor();
        }

        /// <summary>
        /// Methode principale pour decider la position et la rotation du bateau
        /// Retourne la FullMap
        /// </summary>
        /// <param name="longueur"></param>
        /// <param name="hauteur"></param>
        /// <param name="posX"></param>
        /// <param name="posY"></param>
        public static string[,] SelectGrid(int longueur, int hauteur, int posX, int posY)
        {
            //const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            //const string digits = "0123456789";
            bool isSelected = false;
            do
            {
                // Choisir la position du bateau et sa rotation

                // Demande la position initiale
                bool canContinue = false;
                string position = "";
                int[] finalPos = new int[2];

                while (!canContinue)
                {
                    canContinue = true;
                    Console.Clear();
                    CreateInitialMap(longueur, hauteur, posX, posY);
                    Console.Write("Position (example de format: A1): ");
                    position = Console.ReadLine() ?? "";

                    canContinue = PositionValide(out finalPos, position, hauteur, longueur);
                }
                // Dessiner la map
                FullMap[finalPos[1], finalPos[0]] = "B";
                DrawMap(FullMap, longueur, hauteur, posX, posY);

                // Choisir la rotation
                bool rotationValide = false;
                bool showErrorMsg = false;
                do
                {
                    Console.Clear();
                    DrawMap(FullMap, longueur, hauteur, posX, posY);
                    // Demander la rotation désirer
                    Console.SetCursorPosition(posX, posY + hauteur + 2);
                    Console.WriteLine("Choisir l'orientation:\n1 = Nord\n2 = Sud\n3 = Ouest\n4 = Est");
                    Console.Write("Choix : ");
                    if (showErrorMsg)
                    {
                        int tempLeft = Console.GetCursorPosition().Left;
                        int tempTop = Console.GetCursorPosition().Top;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\nEntrée invalide");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.SetCursorPosition(tempLeft, tempTop);

                    }
                    string orientation = Console.ReadLine() ?? "";
                    switch (orientation)
                    {
                        // Verifie si la posisition demandé est valide, si oui alors dessiner la map avec le bateau complet
                        case "1":
                            if (finalPos[0] < 1)
                            {
                                showErrorMsg = true;
                                break;
                            }
                            else FullMap[finalPos[1], finalPos[0] - 1] = "B";
                            DrawMap(FullMap, longueur, hauteur, posX, posY);
                            rotationValide = true;
                            isSelected = true;
                            break;
                        case "2":
                            if (finalPos[0] + 1 == hauteur)
                            {
                                showErrorMsg = true;
                                break;
                            }
                            else FullMap[finalPos[1], finalPos[0] + 1] = "B";
                            DrawMap(FullMap, longueur, hauteur, posX, posY);
                            rotationValide = true;
                            isSelected = true;
                            break;
                        case "3":
                            if (finalPos[1] < 1)
                            {
                                showErrorMsg = true;
                                break;
                            }
                            else FullMap[finalPos[1] - 1, finalPos[0]] = "B";
                            DrawMap(FullMap, longueur, hauteur, posX, posY);
                            rotationValide = true;
                            isSelected = true;
                            break;
                        case "4":
                            if (finalPos[1] + 1 == longueur)
                            {
                                showErrorMsg = true;
                                break;
                            }
                            else FullMap[finalPos[1] + 1, finalPos[0]] = "B";
                            DrawMap(FullMap, longueur, hauteur, posX, posY);
                            rotationValide = true;
                            isSelected = true;
                            break;
                        default:
                            showErrorMsg = true;
                            break;
                    }
                } while (!rotationValide);
            } while (!isSelected);
            return FullMap;
        }


        /// <summary>
        /// Verifie la validité d'un entrée d'une position
        /// </summary>
        /// <param name="positionEntree"></param>
        /// <param name="hauteur"></param>
        /// <param name="longueur"></param>
        /// <param name="finalPos"></param>
        /// <returns></returns>
        public static bool PositionValide(out int[] finalPos, string positionEntree, int hauteur, int longueur)
        {
            const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string digits = "0123456789";
            finalPos = new int[2];
            // Verification si la positionEntree est valide
            if (positionEntree.Length != 2) return false;
            if (!letters.Contains(positionEntree.ToArray()[0].ToString().ToUpper())) return false;
            if (letters.IndexOf(positionEntree.ToArray()[0].ToString().ToUpper()) > hauteur) return false;
            if (!digits.Contains(positionEntree.ToArray()[1])) return false;
            if ((int)Char.GetNumericValue(positionEntree.ToArray()[1]) - 1 >= hauteur || (int)Char.GetNumericValue(positionEntree.ToArray()[1]) - 1 < 0) return false;

            // Transformer la position A1 en { 0, 0 }
            finalPos[0] = LetterToNumber(positionEntree.ToArray()[0]); // Position Y
            finalPos[1] = (int)Char.GetNumericValue(positionEntree.ToArray()[1]) - 1; // Position X

            // Verfie si la position est dans le grid de jeu
            if (finalPos[0] < 0) return false;
            if (finalPos[1] < 0) return false;
            if (finalPos[0] + 1 > hauteur) return false;
            if (finalPos[1] + 1 > longueur) return false;

            return true;
        }

        // Trouver sur https://stackoverflow.com/questions/10373561/convert-a-number-to-a-letter-in-c-sharp-for-use-in-microsoft-excel
        public static string NumberToLetter(int index)
        {
            const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            var value = "";

            if (index >= letters.Length)
                value += letters[index / letters.Length - 1];

            value += letters[index % letters.Length];

            return value;
        }
        public static int LetterToNumber(char letter)
        {

            const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            return letters.IndexOf((string)letter.ToString().ToUpper());
        }
    }
}
