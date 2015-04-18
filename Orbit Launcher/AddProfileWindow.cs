using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OrbitLauncher
{
    public partial class AddProfileWindow : Form
    {
        private Main ParentClass;
        public AddProfileWindow(Main parent)
        {
            InitializeComponent();
            this.ParentClass = parent;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void addProfileButton_Click(object sender, EventArgs e)
        {
            addProfileButton.Enabled = false;
            ParentClass.AddProfile(emailBox.Text, passwordBox.Text);
            this.Close();
        }
    }
}
