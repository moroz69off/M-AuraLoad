using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AuraThree
{
    public partial class AForm : Form
    {
        public AForm()
        {
            InitializeComponent();
        }

        private void AForm_Load(object sender, EventArgs e)
        {
            AsyncHttpServer server = new AsyncHttpServer();
            server.Start("http://localhost:8088/");
            webBrowser.AllowNavigation = true;
            //var path = Path.Combine("file:///", AppDomain.CurrentDomain.BaseDirectory, "data", "index.html");
            //webBrowser.Navigate(path);
            webBrowser.Navigate("http://localhost:8088/");
        }
    }
}
