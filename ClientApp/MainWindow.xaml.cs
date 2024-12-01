using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;

namespace UDPClientApp
{
    public partial class MainWindow : Window
    {
        private UdpClient udpClient;
        private IPEndPoint serverEndPoint;

        public MainWindow()
        {
            InitializeComponent();

            // Ініціалізація UDP клієнта
            udpClient = new UdpClient();
            serverEndPoint = new IPEndPoint(IPAddress.Loopback, 3300); // Використовуємо локальний сервер (127.0.0.1) та порт 3300

            // Запуск отримання повідомлень у окремому потоці
            Thread receiveThread = new Thread(ReceiveMessages);
            receiveThread.Start();
        }

        // Метод для відправки повідомлення на сервер
        private void SendMessage(string message)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            udpClient.Send(data, data.Length, serverEndPoint); // Відправка повідомлення на сервер
        }

        // Метод для отримання повідомлень від сервера
        private void ReceiveMessages()
        {
            while (true)
            {
                try
                {
                    byte[] receivedData = udpClient.Receive(ref serverEndPoint);
                    string receivedMessage = Encoding.UTF8.GetString(receivedData);
                    Dispatcher.Invoke(() =>
                    {
                        // Оновлюємо інтерфейс користувача з отриманим повідомленням
                        DialogBox.Text += "Server: " + receivedMessage + "\n";
                    });
                }
                catch (Exception ex)
                {
                    // Логування помилки
                    Console.WriteLine("Error receiving message: " + ex.Message);
                    break;
                }
            }
        }

        // Обробник натискання кнопки Send
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string message = MessageTextBox.Text;
            SendMessage(message);

            // Додаємо повідомлення клієнта в діалог
            DialogBox.Text += "Client: " + message + "\n";
            MessageTextBox.Clear(); // Очищаємо поле вводу
        }
    }
}
