using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Runtime.InteropServices;
using OrbitLauncher.Authentication;
using Newtonsoft.Json;

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

            CheckForUpdates();

            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            InitializeComponent();

            AuthManager = new AuthManager(this);

            Dictionary<string,Profile> Profiles = AuthManager.GetProfiles(); // Retrieve profiles from authentication file
            RefreshProfileList();

            Starting = false;
            Console.WriteLine("Initialised");
        }

        private async void CheckForUpdates()
        {
            RestClient client = new RestClient();
            String remoteVersion = await client.GetRemoteVersionNumber();
            String localVersion = "";

            if (!File.Exists(Lib.VERSION_FILE_LOCATION))
            {
                File.WriteAllText(Lib.VERSION_FILE_LOCATION, JsonConvert.SerializeObject(new Dictionary<string, string>(), Formatting.Indented));
            }
            else
            {
                Dictionary<string, string> contents = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(Lib.VERSION_FILE_LOCATION));
                if (contents.Count > 0)
                {
                   contents.TryGetValue("version", out localVersion);
                }
            }

            if (localVersion.Length > 5 && remoteVersion.Length > 5)
            {

            }
            
        }

        private async void RetrieveUpdates()
        {
            RestClient client = new RestClient();

        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine(@"------------------------------------------------------------");
            Console.WriteLine(@"THE APPLICATION HAS CRASHED! CRASH REPORT BELOW!");
            Console.WriteLine(@"------------------------------------------------------------");
            Console.WriteLine(e.ExceptionObject);
            MessageBox.Show("The launcher has unexpectedly crashed! A crash report has been placed in the log", "Unhandled Error");
            Process.GetCurrentProcess().Kill();
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
            SelectedProfile.SelectedIndex = 0;
            if (SelectedProfile.SelectedItem.Equals("Add new profile"))
            {
                launchButton.Enabled = false;
            }
            else
            {
                launchButton.Enabled = true;
            }
        }

        private void selectedProfile_SelectionChangeCommited(object sender, EventArgs e)
        {
            if (SelectedProfile.SelectedItem.Equals("Add new profile"))
            {
                AddProfileWindow addProfileWindow = new AddProfileWindow(this);
                addProfileWindow.ShowDialog();
                launchButton.Enabled = false;
            }
            else
            {
                launchButton.Enabled = true;
            }
        }

        private void launchButton_Click(object sender, EventArgs e)
        {

        }
    }
}