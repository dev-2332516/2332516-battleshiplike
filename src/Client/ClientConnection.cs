using LibrairieClasse;
using System.Net;
using System.Net.Sockets;

namespace Client
{
    internal class ClientConnection
    {
        public static void StartClient()
        {
            // Map joueur, map hit/miss, et position joueurs
            string[,] playerMap;
            string[,] hitMap;
            int[] positionEnnemie;

            // Variables temporaires pour la hauteur et la longueur
            int longueur, hauteur;

            byte[] bytes = new byte[256];
            byte[] buffer = new byte[1024];

            try
            {
                // Demande la taille du grid
                AskForSize(out longueur, out hauteur);
                Interface.Longueur = longueur;
                Interface.Hauteur = hauteur;

                // Demande l'ip du serveur
                Console.Write("Entrez l'adresse IP du serveur:");
                string IP = Console.ReadLine() ?? "";

                //string IP = "10.99.60.148";
                // Cree les sockets
                IPAddress ipAdress = IPAddress.Parse(IP);

                IPHostEntry hostEntry = new IPHostEntry
                {
                    HostName = IP, // Ou un nom d'hôte fictif si nécessaire
                    AddressList = new[] { ipAdress },
                    Aliases = new string[0] // Les alias peuvent rester vides
                };

                IPAddress ipAddress = hostEntry.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 22222);
                Socket listner = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    Console.Clear();
                    // Attend la connection
                    byte[] msg = new byte[256];
                    int bytesSent = 0;
                    bool endCondition = false;

                    Socket sender = listner;

                    sender.Connect(remoteEP);
                    Console.WriteLine("Connection Établie. Début de la partie.");

                    // Envoie la taille du grid au serveur et setup quelques variables
                    int[] taille = new int[2];
                    taille[0] = Interface.Longueur;
                    taille[1] = Interface.Hauteur;
                    Connection.Sender(sender, taille);
                    hitMap = new string[Interface.Longueur, Interface.Hauteur];
                    Console.Clear();

                    // Loop du jeu
                    while (!endCondition)
                    {
                        //Choose boat position
                        Console.Clear();
                        playerMap = Interface.SelectGrid(2, 1);
                        List<int> BoatPostion = new List<int>();
                        for (int y = 0; y < Interface.Hauteur; y++)
                        {
                            for (int x = 0; x < Interface.Longueur; x++)
                            {
                                if (playerMap[x, y] == "B")
                                {
                                    BoatPostion.Add(x);
                                    BoatPostion.Add(y);
                                }
                            }
                        }
                        // Initialise la hit map de base
                        for (int y = 0; y < Interface.Hauteur; y++)
                        {
                            for (int x = 0; x < Interface.Longueur; x++)
                            {
                                hitMap[x, y] = "-";
                            }
                        }

                        // Set les board dans le validator
                        Validator._actualEnemyBoard = hitMap;
                        Validator._myActualBoard = playerMap;

                        // Envoi la position du bateau joueur au serveur
                        Connection.Sender(sender, BoatPostion.ToArray());
                        Connection.InitBoat(BoatPostion.ToArray());

                        // Attend la position du bateau du serveur
                        Console.Clear();
                        Console.WriteLine("Waiting server to place first boat...");
                        positionEnnemie = Connection.Receiver(sender);
                        Connection.InitEnemyBoat(positionEnnemie);

                        Console.WriteLine("Waiting for server to start the game...");
                        Connection.ReceiveMessage(sender);
                        bool winCondition = false;

                        // Logique primaire du jeu
                        while (!winCondition)
                        {
                            bool maybeWin = false;
                            bool valide = false;
                            int[] missilePosition = new int[2];
                            while (!valide)
                            {
                                Console.Clear();
                                Interface.DrawGame(hitMap,playerMap);

                                //Client start playing
                                Console.Write("Entrez la position de votre action: ");
                                string playedMove = Console.ReadLine() ?? "";
                                valide = Interface.PositionValide(out missilePosition, playedMove);
                            }

                            Connection.Sender(sender, missilePosition);
                            bool touched = Connection.MissOrTouched(missilePosition, 'M'); // player M => me
                            if (touched)
                            {
                                // Sauvegarde la position Touché et redessine la map
                                hitMap[missilePosition[1], missilePosition[0]] = "H";
                                Console.Clear();
                                Interface.DrawGame(hitMap, playerMap);

                                maybeWin = Connection.WinCheck();
                                Connection.SendGameStatus(sender, maybeWin);

                                if (maybeWin)
                                {
                                    winCondition = true;
                                    break;
                                }
                                else
                                {
                                    maybeWin = false;
                                }
                            }
                            else
                            {
                                hitMap[missilePosition[1], missilePosition[0]] = "M";
                                Console.Clear();
                                Interface.DrawGame(hitMap, playerMap);
                            }

                            //Server playing

                            missilePosition = Connection.Receiver(sender);
                            touched = Connection.MissOrTouched(missilePosition, 'E'); // player E => enemy
                            if (touched)
                            {
                                //change color for red
                                playerMap[missilePosition[1], missilePosition[0]] = "S";
                                Console.Clear();
                                Interface.DrawGame(hitMap, playerMap);
                                maybeWin = Connection.ReceiveGameStatus(sender);
                                if (maybeWin)
                                {
                                    if (Connection.LoseCheck())
                                    {
                                        winCondition = true;
                                    }
                                    else
                                    {
                                        maybeWin = false;
                                    }
                                }
                            }
                            else
                            {
                                playerMap[missilePosition[1], missilePosition[0]] = "M";
                                Console.Clear();
                                Interface.DrawGame(hitMap, playerMap);
                            }
                        }

                        ConsoleKeyInfo keyPress = new ConsoleKeyInfo();

                        Console.Clear();
                        Console.WriteLine("Voulez-vous recommencer la partie ? \n Appuyer sur R pour recommencer.");
                        keyPress = Console.ReadKey();
                        if(keyPress.Key == ConsoleKey.R) 
                        {
                            winCondition = false;
                            Connection.SendMessage(sender, "RESTART");
                        }
                        else
                        {
                            endCondition = true;
                            Connection.SendMessage(sender, "END");
                            Console.WriteLine("Partie Fini, Fin de la connection");
                        }
                    }

                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception: {0}", e.ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static void AskForSize(out int longueur, out int hauteur)
        {
            // Initialise les variables
            longueur = 0;
            hauteur = 0;
            bool valid = false;
            
            while (!valid)
            {
                // Affiche les instructions et demande la taille
                Console.WriteLine("La taille de la maquette doit être entre 4x4 et 12x12\n");
                Console.Write("Longueur de la maquette: ");
                longueur = Interface.AskForNumber();
                Console.Write("\nHauteur de la maquette: ");
                hauteur = Interface.AskForNumber();

                // Verifier si la taille entrée est valide 
                if (longueur >= 4 && longueur <= 12 && hauteur >= 4 && hauteur <= 12) valid = true;
                else
                {
                    // Affiche le message d'erreur
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Taille entrée invalide");
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }
    }
}
