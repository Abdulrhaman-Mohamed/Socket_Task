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
    public partial class File_Transfer : Form
    {

        // variables
        string File_Name;
        bool Click = false;
        string destinationPath;
        private TcpListener listener;
        private readonly Home _home;
        public File_Transfer(Home home)
        {
            InitializeComponent();
            _home = home;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to exit?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Application.Exit(); // Terminate the entire application
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            listener.Stop();
            MessageBox.Show("Socket closed ","Information", MessageBoxButtons.OK);
            _home.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.ShowDialog();
            File_Name = openFileDialog1.FileName;


        }

        private void button4_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {

                destinationPath = saveFileDialog.FileName;
                saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";

                // Connect to the remote host
                TcpClient client = new TcpClient("192.168.1.7", 48484);

                // Get a stream object for writing
                NetworkStream stream = client.GetStream();

                // Convert the file to a byte array and send it
                byte[] fileBytes = File.ReadAllBytes(File_Name);
                stream.Write(fileBytes, 0, fileBytes.Length);

                //// Save the file to the destination path
                //File.WriteAllBytes(destinationPath, fileBytes);


                // Clean up
                stream.Close();
                client.Close();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            


        }

   

        private void button6_Click(object sender, EventArgs e)
        {
            listener.Stop();
            MessageBox.Show("Socket closed ", "Information", MessageBoxButtons.OK);
        }

        private void File_Transfer_Load(object sender, EventArgs e)
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

            // Read the data sent by the client asynchronously
            byte[] data = new byte[1024];
            int bytesRead = await stream.ReadAsync(data, 0, data.Length);
            string message = Encoding.ASCII.GetString(data, 0, bytesRead);

            File.WriteAllText(destinationPath, message);
            MessageBox.Show("Writting Done ", "Information", MessageBoxButtons.OK);



            // Clean up
            stream.Close();
            client.Close();
        }
    }
}
