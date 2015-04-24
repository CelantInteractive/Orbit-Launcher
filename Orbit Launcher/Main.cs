using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Runtime.InteropServices;
using OrbitLauncher.Authentication;

namespace OrbitLauncher
{
    public partial class Main : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public static bool Starting = true;

        public static Logger logger = new Logger();

        AuthManager AuthManager;

        public Main()
        {
            Console.SetOut(logger);
            Console.WriteLine("Initializing launcher");

            InitializeComponent();

            AuthManager = new AuthManager(this);

            Dictionary<string,Profile> Profiles = AuthManager.GetProfiles(); // Retrieve profiles from authentication file
            RefreshProfileList();

            Starting = false;
            Console.WriteLine("Initialised");
        }

        public void AddProfile(String username, String password)
        {
            AuthManager.AuthenticateNewProfile(username, password);
        }

        private void TestButton_Click(object sender, EventArgs e)
        {
            AuthManager.AuthenticateNewProfile("test@test.com", "testpassword123");
        }

        private void Main_MouseDown(object sender,
        System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        public void RefreshProfileList()
        {
            SelectedProfile.Items.Clear();
            foreach (KeyValuePair<string, Profile> Profile in AuthManager.AuthenticationDb.Profiles)
            {
                Console.WriteLine("Loading profile: {0}", Profile.Value.Email);
                SelectedProfile.Items.Add(Profile.Value.Email);
            }
            SelectedProfile.Items.Add("Add new profile");

            SelectedProfile.Text = "Add new profile";
        }

        private void selectedProfile_SelectionChangeCommited(object sender, EventArgs e)
        {
            if (SelectedProfile.SelectedItem.Equals("Add new profile"))
            {
                AddProfileWindow addProfileWindow = new AddProfileWindow(this);
                addProfileWindow.ShowDialog();

            }
        }

        private void launchButton_Click(object sender, EventArgs e)
        {

        }
    }
}