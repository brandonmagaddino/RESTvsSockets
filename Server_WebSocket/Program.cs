using Shared_DataModels.Model;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server_WebSocket
{
    class Program
    {
        public static List<Message> messages = new List<Message>();
        public static List<Socket> clients = new List<Socket>();


        static void Main(string[] args)
        {
            try
            {
                
                IPAddress ipAd = IPAddress.Parse("127.0.0.1");
                // use local m/c IP address, and 
                // use the same in the client

                /* Initializes the Listener */
                TcpListener myList = new TcpListener(ipAd, 8001);

                /* Start Listeneting at the specified port */
                myList.Start();

                Console.WriteLine("The server is running at port 8001...");
                Console.WriteLine("The local End point is  :" +
                                  myList.LocalEndpoint);
                Console.WriteLine("Waiting for a connection.....");

                while (true)
                {
                    Socket socket = myList.AcceptSocket();
                    Console.WriteLine("Connection accepted from " + socket.RemoteEndPoint);

                    StartListeningToClient(socket);
                }
                /* clean up */
                //socket.Close();
                //myList.Stop();

            }
            catch (Exception e)
            {
                Console.WriteLine("Error..... " + e.StackTrace);
            }

            while(true)
            {

            }
        }

        public static void StartListeningToClient(Socket client)
        {
            clients.Add(client);
            UpdateAllClients();

            new Task(() => {
                while (client.Connected)
                {
                    try
                    {
                        byte[] bytes = new byte[1026];
                        int byteArrayLength = client.Receive(bytes);

                        var result = System.Text.Encoding.Default.GetString(bytes);

                        var newMessage = Newtonsoft.Json.JsonConvert.DeserializeObject<Message>(result);
                        messages.Add(newMessage);
                        UpdateAllClients();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
                clients.Remove(client);
            }).Start();
        }

        public static void UpdateAllClients()
        {
            new Task(() => {
                foreach (var client in clients)
                {
                    if (!client.Connected)
                        continue;

                    ASCIIEncoding asen = new ASCIIEncoding();
                    client.Send(asen.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(messages)));
                    Console.WriteLine("\nSent Message Update to all clients");
                }
            }).Start();
        }
    }
}
