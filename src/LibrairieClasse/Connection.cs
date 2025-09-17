using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LibrairieClasse
{
    public static class Connection
    {   
        private static readonly Validator validator = new(4);

        public static void Sender(Socket socket,int[] boatPosition ) 
        {
            string data = string.Join(",", boatPosition);
            byte[] msg = Encoding.ASCII.GetBytes(data);
            socket.Send(msg);
        }

        public static int[] Receiver(Socket socket) 
        {
            byte[] buffer = new byte[100];
            int bytesRec = socket.Receive(buffer);
            string data = Encoding.ASCII.GetString(buffer, 0, bytesRec);
            int[] boatPositions = data.Split(',')
                                      .Select(int.Parse)
                                      .ToArray();
            return boatPositions;
        }

        public static void SendGameStatus (Socket socket, bool win) 
        {
            string msg =  win ? "1" :"0";
            byte[] data = Encoding.ASCII.GetBytes(msg);
            socket.Send(data);
        }

        public static bool ReceiveGameStatus(Socket socket)
        {
            byte[] buffer = new byte[1];
            int bytesReceived = socket.Receive(buffer);
            string msg = Encoding.ASCII.GetString(buffer, 0, bytesReceived);
            return msg == "1";
        }

        public static string ReceiveMessage(Socket socket)
        {
            byte[] buffer = new byte[1024];
            int bytesRec = socket.Receive(buffer);
            string data = Encoding.ASCII.GetString(buffer, 0, bytesRec);

            return data;
        }

        public static void SendMessage(Socket socket, string msg)
        {

            byte[] data = Encoding.ASCII.GetBytes(msg);
            socket.Send(data);
        }


        public static bool MissOrTouched(int[] missilePosition, char player) 
        {
            return validator.GetPlayResult(missilePosition, player);
        }

        public static bool LoseCheck() 
        {
            return validator.WinCheck();
        }

        public static bool WinCheck()
        {
            return validator.EnemyWinCheck();
        }

        public static void InitBoat(int[] boatPostion)
        {
            validator.InitMyBoat(boatPostion);
        }

        public static void InitEnemyBoat(int[] serverBoatPositons)
        {
            validator.InitEnemyBoat(serverBoatPositons);
        }
    }
}
