using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Images_Transfer : Form
    {
        private readonly Home _home;
        private TcpListener listener;
        public Images_Transfer(Home home)
        {
            InitializeComponent();
            _home = home;
        }

        private void Images_Transfer_Load(object sender, EventArgs e)
        {
            // Specify the port number to listen on
            int port = 48484;

            // Create a TcpListener object to listen for incoming connections
            listener = new TcpListener(IPAddress.Any, port);

            // Start listening for incoming connections asynchronously
            listener.Start();


            // Start accepting incoming connections asynchronously
            AcceptConnectionsAsync();
        }

        private async Task AcceptConnectionsAsync()
        {
            while (true)
            {
                // Accept an incoming client connection asynchronously
                TcpClient client = await listener.AcceptTcpClientAsync();

                // Handle the client connection asynchronously
                HandleClientAsync(client);
            }
        }
        private async Task HandleClientAsync(TcpClient client)
        {
            // Get a stream object for reading and writing
            NetworkStream stream = client.GetStream();

            byte[] sizeBytes = new byte[4];
            stream.Read(sizeBytes, 0, 4);
            int imageSize = BitConverter.ToInt32(sizeBytes, 0);

            // Receive the image data in chunks
            int bufferSize = 4096; // Use a larger buffer size
            byte[] imageData = new byte[imageSize];
            int bytesReceived = 0;
            while (bytesReceived < imageSize)
            {
                int bytesToReceive = Math.Min(bufferSize, imageSize - bytesReceived);
                int bytesRead = stream.Read(imageData, bytesReceived, bytesToReceive);

                if (bytesRead == 0)
                {
                    // End of stream reached prematurely, handle the error
                    throw new Exception("End of stream reached prematurely.");
                }

                bytesReceived += bytesRead;
            }

            // Convert the received data back into an image file and save it to a desired location
            using (MemoryStream ms = new MemoryStream(imageData))
            {
                Image image = Image.FromStream(ms);
                image.Save("E:/Tasks_Mosh/Socket Task/Socket/ImageSaved/test.jpg"); // Replace with desired location to save the received image
            }
            // Clean up
            stream.Close();
            client.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to exit?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Application.Exit(); // Terminate the entire application
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listener.Stop();
            _home.Show();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                openFileDialog.Filter = "Image Files (*.bmp, *.jpg, *.png)|*.bmp;*.jpg;*.png";
                // Connect to the remote host
                TcpClient client = new TcpClient("192.168.1.7", 48484);

                // Get a stream object for writing
                NetworkStream stream = client.GetStream();
                byte[] imageData = File.ReadAllBytes(openFileDialog.FileName);

                // Send the total size of the image data first
                byte[] sizeBytes = BitConverter.GetBytes(imageData.Length);
                stream.Write(sizeBytes, 0, sizeBytes.Length);

                // Send the image data in chunks
                int bufferSize = 4096; // Use a larger buffer size
                int bytesSent = 0;
                while (bytesSent < imageData.Length)
                {
                    int bytesToSend = Math.Min(bufferSize, imageData.Length - bytesSent);
                    stream.Write(imageData, bytesSent, bytesToSend);
                    bytesSent += bytesToSend;
                }
            }
        }
    }
}
