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

        AuthManager AuthManager = new AuthManager();

        public Main()
        {
            Console.SetOut(new Logger());
            Console.WriteLine("Initializing launcher");

            InitializeComponent();

            List<Profile> Profiles = AuthManager.GetProfiles(); // Retrieve profiles from authentication file
            foreach (Profile Profile in Profiles)
            {
                Console.WriteLine("Found {0}", Profile.Username);
                SelectedProfile.Items.Add(Profile.Username);
            }
            SelectedProfile.Items.Add("Add new profile");
        }

        public void AddProfile(String username, String password)
        {
            AuthManager.AuthenticateNewProfile(username, password);
        }

        private void selectedProfile_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedProfile.SelectedItem.Equals("Add new profile"))
            {
                AddProfileWindow addProfileWindow = new AddProfileWindow(this);
                addProfileWindow.ShowDialog();
                SelectedProfile.Items.Clear();
                foreach (Profile profile in AuthManager.AuthenticationDb.Profiles)
                {
                    SelectedProfile.Items.Add(profile.Username);
                }
                SelectedProfile.Items.Add("Add new profile");
            }
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
    }
}