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
            webBrowser.DocumentText = "Document text";
            webBrowser.AllowNavigation = true;
            webBrowser.Navigate("http://moroz69off.coolpage.biz/");
        }
    }
}
