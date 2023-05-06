using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;

namespace WindowsFormsApp1
{
    public partial class Web_Page_Form : Form
    {
        private readonly Home _home;
        public Web_Page_Form(Home home)
        {
            InitializeComponent();
            _home = home;
        }

        private void Web_Page_Form_Load(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to exit?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Application.Exit(); // Terminate the entire application
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            _home.Show();
            this.Hide();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != null)
            {
                textBox2.Clear();
                IPAddress ipAddress = IPAddress.Parse("192.168.1.7");
                Uri uri = new Uri(textBox1.Text);
                using (TcpClient client = new TcpClient())
                {
                    client.Connect(ipAddress, 80);
                    textBox2.Text += $"Server has started on {textBox1.Text}.{0}Waiting for a connection…";
                    string request = $"GET {uri.PathAndQuery} HTTP/1.1\r\nHost: {uri.Host} Accept: text/html, charset=utf-8 Connection: close\r\n\r\n";
                    byte[] requestBytes = Encoding.UTF8.GetBytes(request);
                    client.GetStream().Write(requestBytes, 0, requestBytes.Length);
                    byte[] responseBytes = new byte[1024];
                    int bytesRead;
                    while ((bytesRead = client.GetStream().Read(responseBytes, 0, responseBytes.Length)) > 0)
                    {
                        textBox2.Text+= Encoding.UTF8.GetString(responseBytes, 0, bytesRead);
                    }
                }
            }
            else
                MessageBox.Show("Are you sure you want to exit?", "Waring", MessageBoxButtons.OK);



        }
    }
}
