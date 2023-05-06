using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Web_Page_Form web_Page_Form = new Web_Page_Form(this);
            this.Hide();
            web_Page_Form.Show();
            
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            File_Transfer file_Transfer = new File_Transfer(this);
            this.Hide();
            file_Transfer.Show();
        }
    }
}
