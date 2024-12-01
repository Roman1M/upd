using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UDPServerApp
{
    class Program
    {
        private static UdpClient udpServer;
        private static IPEndPoint clientEndPoint;

        static void Main(string[] args)
        {
            udpServer = new UdpClient(3300); // Порт сервера
            clientEndPoint = new IPEndPoint(IPAddress.Any, 0); // Слухаємо на всіх інтерфейсах

            Console.WriteLine("Server is listening...");

            while (true)
            {
                byte[] receivedData = udpServer.Receive(ref clientEndPoint); // Отримуємо дані від клієнта
                string receivedMessage = Encoding.UTF8.GetString(receivedData);
                Console.WriteLine($"Received: {receivedMessage}");

                // Відправка відповіді
                string response = GetResponse(receivedMessage);
                byte[] responseData = Encoding.UTF8.GetBytes(response);
                udpServer.Send(responseData, responseData.Length, clientEndPoint); // Відправляємо відповідь
                Console.WriteLine($"Sent: {response}");
            }
        }

        // Метод для отримання відповіді
        private static string GetResponse(string message)
        {
            if (message.Contains("hi", StringComparison.OrdinalIgnoreCase)) // Порівняння без врахування регістру
            {
                return "Hello, client!";
            }
            return "I don't understand.";
        }
    }
}
