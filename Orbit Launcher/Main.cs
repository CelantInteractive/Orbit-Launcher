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

        public Main(String[] args)
        {
            Console.SetOut(logger);
            Console.WriteLine("Initializing launcher");
            if (args.Length != 0)
            {
                Console.WriteLine(args[0]);
                Console.WriteLine(args[1]);
                Console.WriteLine(args[2]);
                Console.WriteLine(args[3]);
            }

            InitializeComponent();

            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            RestClient client = new RestClient();
            Lib.CARDINAL_SERVER = client.GetRequest(Lib.API_LOADBALANCE, Lib.CARDINAL_BALANCER);

            CheckForUpdates();

            AuthManager = new AuthManager(this);

            Dictionary<string,Profile> Profiles = AuthManager.GetProfiles(); // Retrieve profiles from authentication file
            RefreshProfileList();

            Starting = false;
            Console.WriteLine("Initialised");
        }

        public string ScrubInput(string s)
        {
            //TODO: Scrub input of nasties
            return s;
        }

        private void CheckForUpdates()
        {
            RestClient client = new RestClient();

            String versionFileLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Lib.VERSION_FILE_LOCATION);

            String tempLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Lib.TEMP_LOCATION);

            String version = "";

            if (File.Exists(tempLocation + ".update"))
            {
                String FileContents = File.ReadAllText(tempLocation + ".update");
                Dictionary<string, string> UpdateFileContents = JsonConvert.DeserializeObject<Dictionary<string, string>>(FileContents);
                if (UpdateFileContents.Count > 0)
                {
                    UpdateFileContents.TryGetValue("version", out version);
                    File.WriteAllText(versionFileLocation, JsonConvert.SerializeObject(UpdateFileContents, Formatting.Indented));
                }
                File.Delete(tempLocation + ".update");
            }

            String remoteVersion = client.GetRemoteVersionNumber();
            String localVersion = "0";
            Dictionary<string, string> RemoteVersionDict = new Dictionary<string, string>();
            RemoteVersionDict.Add("version", remoteVersion);

            if (!File.Exists(versionFileLocation))
            {
                File.WriteAllText(versionFileLocation, JsonConvert.SerializeObject(new Dictionary<string, string>() { { "version", "1431371514" } }, Formatting.Indented));
            }
            else
            {
                Dictionary<string, string> contents = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(versionFileLocation));
                if (contents.Count > 0)
                {
                   contents.TryGetValue("version", out localVersion);
                }
            }

            if (Int64.Parse(localVersion) < Int64.Parse(remoteVersion))
            {
                RetrieveUpdates(RemoteVersionDict);
            }
            
        }

        private async void RetrieveUpdates(Dictionary<string,string> ver)
        {
            MessageBox.Show("The application will now prepare to update", "Update Required");
            RestClient client = new RestClient();
            this.Enabled = false;
            await client.DownloadInstaller();


            Process process = new Process();
            // Configure the process using the StartInfo properties.
            process.StartInfo.FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Lib.TEMP_INSTALLER_LOCATION);
            process.StartInfo.Arguments = "/SILENT /NOCANCEL /CLOSEAPPLICATIONS /RESTARTAPPLICATIONS";
            process.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;

            String tempLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Lib.TEMP_LOCATION);

            File.WriteAllText(tempLocation + ".update", JsonConvert.SerializeObject(ver, Formatting.Indented));

            process.Start();
            process.WaitForExit();
            if (process.ExitCode != 0)
            {
                File.Delete(tempLocation + ".update");
            }
            Application.Exit();

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
            MessageBox.Show("I got an update!");
        }
    }
}