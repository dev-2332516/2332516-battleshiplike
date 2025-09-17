using Microsoft.VisualBasic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using LibrairieClasse;
using System.Security.Cryptography.X509Certificates;
namespace Client
{
    public class Program
    {
        static void Main(string[] args)
        {
            ClientConnection.StartClient();
        }
    }

}
