using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using LibrairieClasse;
using System.Diagnostics.CodeAnalysis;

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


            byte[] bytes = new byte[256];
            byte[] buffer = new byte[1024];

            try
            {
                Console.Write("Entrez l'adresse IP du serveur:");
                string IP = Console.ReadLine() ?? "";
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
                    Console.Clear();

                    // Loop du jeu
                    while (!endCondition)
                    {
                        //Choose first boat position
                        //Send it to Server
                        Console.Clear();
                        playerMap = Interface.SelectGrid(4, 4, 2, 1);
                        List<int> BoatPostion = new List<int>();
                        for (int y = 0; y < 4; y++)
                        {
                            for (int x = 0; x < 4; x++)
                            {
                                if (playerMap[x, y] == "B")
                                {
                                    BoatPostion.Add(x);
                                    BoatPostion.Add(y);
                                }
                            }
                        }
                        // Initialise la hit map de base
                        hitMap = new string[,] { { "-", "-", "-", "-" }, { "-", "-", "-", "-" }, { "-", "-", "-", "-" }, { "-", "-", "-", "-" } };
                        Connection.Sender(sender, BoatPostion.ToArray());
                        Connection.InitBoat(BoatPostion.ToArray());
                        // Attend pour la reponse du serveur
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
                                valide = Interface.PositionValide(out missilePosition, playedMove, 4, 4);
                            }

                            Connection.Sender(sender, missilePosition);
                            bool touched = Connection.MissOrTouched(missilePosition, 'M'); // player M => me
                            if (touched)
                            {
                                // Sauvegarde la position Touché et redessine la map
                                hitMap[missilePosition[0], missilePosition[1]] = "H";
                                Console.Clear();
                                Interface.DrawGame(hitMap, playerMap);
                                //change color for green or sum
                                // TODO : send win and wait for server check
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
                                hitMap[missilePosition[0], missilePosition[1]] = "M";
                                Console.Clear();
                                Interface.DrawGame(hitMap, playerMap);
                            }

                            //Server playing

                            missilePosition = Connection.Receiver(sender);
                            touched = Connection.MissOrTouched(missilePosition, 'E'); // player E => enemy
                            if (touched)
                            {
                                //change color for red
                                playerMap[missilePosition[0], missilePosition[1]] = "S";
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
                                playerMap[missilePosition[0], missilePosition[1]] = "M";
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
    }
}
