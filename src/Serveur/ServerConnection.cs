using LibrairieClasse;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Serveur
{
    internal class ServerConnection
    {
        public static void StartListening()
        {
            byte[] bytes = new byte[256];
            byte[] buffer = new byte[1024];

            string[,] playerMap;
            string[,] hitMap;
            int[] positionEnnemie;

            IPAddress ipAddress = IPAddress.Any;
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 22222);
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {

                bool endCondition = false;
                bool winCondition = false;

                listener.Bind(localEndPoint);
                listener.Listen(10);

                Console.WriteLine("...ATTENTE...");
                Socket handler = listener.Accept();
                Console.WriteLine("Connection Établie. Début de la partie.");
                Console.Clear();

                while (!endCondition)
                {
                    // Attend pour la reponse du Client
                    Console.WriteLine("Waiting client to place first boat...");
                    int bytesRec = handler.Receive(buffer);
                    string data = Encoding.ASCII.GetString(buffer, 0, bytesRec);
                    int[] boatPositions = data.Split(',').Select(int.Parse).ToArray();
                    positionEnnemie = boatPositions;
                    Connection.InitEnemyBoat(positionEnnemie);

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
                    Connection.Sender(handler, BoatPostion.ToArray());
                    Connection.InitBoat(BoatPostion.ToArray());

                    Console.WriteLine("Waiting for server to start the game...");
                    Connection.SendMessage(handler, "OK");

                    Console.Clear();



                    while (!winCondition)
                    {
                        bool maybeWin = false;
                        bool touched;
                        int[] missilePosition = new int[2];

                        // Montre la map hit et la map miss
                        Interface.DrawGame(hitMap, playerMap);
                        Console.WriteLine("C'est au tour du client...");
                        //Server received
                        missilePosition = Connection.Receiver(handler);
                        touched = Connection.MissOrTouched(missilePosition, 'E');
                        if (touched)
                        {
                            //change color for red
                            playerMap[missilePosition[0], missilePosition[1]] = "S";
                            Console.Clear();
                            Interface.DrawGame(hitMap, playerMap);
                            maybeWin = Connection.ReceiveGameStatus(handler);
                            if (maybeWin)
                            {
                                if (Connection.LoseCheck())
                                {
                                    winCondition = true;
                                    break;
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

                        //Server playing
                        bool valide = false;
                        missilePosition = new int[2];
                        while (!valide)
                        {
                            Console.Clear();
                            Interface.DrawGame(hitMap, playerMap);

                            //Client start playing
                            Console.Write("Entrez la position de votre action: ");
                            string playedMove = Console.ReadLine() ?? "";
                            valide = Interface.PositionValide(out missilePosition, playedMove, 4, 4);
                        }

                        Connection.Sender(handler, missilePosition);
                        touched = Connection.MissOrTouched(missilePosition, 'M');
                        if (touched)
                        {
                            hitMap[missilePosition[0], missilePosition[1]] = "H";
                            Console.Clear();
                            Interface.DrawGame(hitMap, playerMap);
                            //change color for green or sum
                            maybeWin = Connection.WinCheck();
                            Connection.SendGameStatus(handler, maybeWin);

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
                    }
                    Console.Clear();
                    Console.WriteLine("La Partie est Fini \n Attende de la décision du client. ");
                    string awnser = Connection.ReceiveMessage(handler);
                    if (awnser == "RESTART")
                    {
                        winCondition = false;
                    }
                    else
                    {
                        endCondition = true;
                        Console.WriteLine("Partie Fini, Fin de la connection");
                    }
                }
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected exception: {0}", ex.ToString());
            }
        }
    }
}

