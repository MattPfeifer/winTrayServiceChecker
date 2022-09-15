using Microsoft.Toolkit.Uwp.Notifications;
using Newtonsoft.Json;
using System.Diagnostics;
using winTrayServiceChecker.Classes;
using winTrayServiceChecker.Properties;

namespace winTrayServiceChecker
{
    public partial class Form1 : Form
    {
        ServiceSettings settings = new ServiceSettings();
        bool serviceDown = false;

        int checkIntervalNormal = 30 * 60 * 1000;   //default to 30 minutes
        int checkIntervalError = 5 * 60 * 1000;   //default to 5 minutes

        public Form1()
        {
            InitializeComponent();
            this.Hide();
            LoadServices();
        }

        private void LoadServices()
        {
            using (StreamReader r = new StreamReader("appSettings.json"))
            {
                string json = r.ReadToEnd();
                settings = JsonConvert.DeserializeObject<ServiceSettings>(json);
            }

            if (settings?.Interval != null && settings.Interval > 0)
            { 
                checkIntervalNormal = settings.Interval * 60 * 1000;
            }
            CheckServices();
            //Task.Run(async () => await CheckServices());

            StartChecker();
        }

        private void StartChecker()
        {
            timerCheckServices.Interval = checkIntervalNormal;
            timerCheckServices.Tick += TimerCheckServices_Tick;
            timerCheckServices.Start();
        }

        async private void TimerCheckServices_Tick(object? sender, EventArgs e)
        {
            await CheckServices();
        }

        async private Task CheckServices()
        {
            bool serviceFailedThisTimeAround = false;   //keep track of the current loop through the services,
                                                        //if they are all up, change the service mode back
            contextMenu.Items.Clear();
            foreach(ServiceEndpoint endpoint in settings.ServiceEndpoints)
            {
                string imageToastUri; 

                HttpResponseMessage httpResponseMessage = null;
                try
                {
                    httpResponseMessage = await CallEndpoint(endpoint);
                    if (!httpResponseMessage.IsSuccessStatusCode)
                    {
                        if (!serviceDown)
                        {
                            imageToastUri = Path.GetFullPath(@"Images\StatusInvalid.png");
                            serviceDown = true;
                            serviceFailedThisTimeAround = true;
                            ChangeServiceMode($"Unable to access a configured service", $"Status Code: {httpResponseMessage.StatusCode}", endpoint.Url, imageToastUri);
                        }
                    }
                }
                catch(Exception ex)
                {
                    httpResponseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.RequestTimeout);
                    if (!serviceDown)
                    {
                        imageToastUri = Path.GetFullPath(@"Images\StatusWarning.png");
                        serviceDown = true;
                        serviceFailedThisTimeAround = true;
                        ChangeServiceMode($"Unable to access a configured service", $"{ex.Message}", endpoint.Url, imageToastUri);
                    }
                }
                    
                //update icon in log and menu
                UpdateUI(endpoint, httpResponseMessage);
                serviceStatusIcon.Icon = serviceDown ? Resources.iconInvalid : Resources.iconOK;

                //Trim to 1000 entries, just in case
                int itemCount = lvwLog.Items.Count;
                for (int i = itemCount - 1; i >= 1000; i--)
                {
                    lvwLog.Items.RemoveAt(i);
                }
            }

            if (serviceFailedThisTimeAround)
            {
                serviceDown = false;
                string imageToastUri = Path.GetFullPath(@"Images\StatusOK.png");
                ChangeServiceMode("Services restored", "All configured services are available.", "", imageToastUri);
            }
            contextMenu.Items.Add(new ToolStripSeparator());
            ToolStripMenuItem menuLogs = new ToolStripMenuItem("View Logs");
            menuLogs.Click += MenuLogs_Click;
            contextMenu.Items.Add(menuLogs);
            ToolStripMenuItem menuExit = new ToolStripMenuItem("Exit");
            menuExit.Click += MenuExit_Click;
            contextMenu.Items.Add(menuExit);

        }

        private void ChangeServiceMode(string title, string message1, string serviceUrl, string toastImagePath)
        {
            if (serviceDown)
            {
                ShowToast(title, message1, serviceUrl, toastImagePath);
                timerCheckServices.Stop();
                timerCheckServices.Interval = checkIntervalError;
                timerCheckServices.Start();
            }
            else
            {
                timerCheckServices.Stop();
                timerCheckServices.Interval = checkIntervalNormal;
                timerCheckServices.Start();
            }
        }

        private void MenuExit_Click(object? sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MenuLogs_Click(object? sender, EventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
        }

        async private Task<HttpResponseMessage> CallEndpoint(ServiceEndpoint endpoint)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(endpoint.Url);

            HttpResponseMessage response = await client.SendAsync(new HttpRequestMessage());
            return response;
        }

        private void UpdateUI(ServiceEndpoint endpoint, HttpResponseMessage httpResponseMessage)
        {
            int imageIndex = httpResponseMessage.IsSuccessStatusCode ? 0 :
                (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.RequestTimeout) ? 2 : 1;
            ListViewItem item = new ListViewItem(string.Empty, imageIndex) {  Name = endpoint.Name };
            item.SubItems.Add(httpResponseMessage.StatusCode.ToString());
            item.SubItems.Add(DateTime.Now.ToString());
            item.SubItems.Add(endpoint.Name);
            lvwLog.Items.Insert(0, item);
            Image imageStatus;
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                imageStatus = Resources.StatusOK;
            }
            else if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.RequestTimeout)
            {
                imageStatus = Resources.StatusWarning;
            }
            else
            {
                imageStatus = Resources.StatusInvalid;
            }
            ToolStripMenuItem menuItem = new ToolStripMenuItem() { Text = endpoint.Name, ToolTipText = endpoint.Url, Image = imageStatus };
            menuItem.Tag = endpoint;
            menuItem.ToolTipText = httpResponseMessage.ReasonPhrase;
            menuItem.Click += MenuItem_Click;
            contextMenu.Items.Insert(0, menuItem);
        }

        private void MenuItem_Click(object? sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            if (menuItem != null)
            {
                ServiceEndpoint endpoint = menuItem.Tag as ServiceEndpoint;
                if (endpoint != null)
                {
                    System.Diagnostics.Process.Start(new ProcessStartInfo
                    {
                        FileName = endpoint.Url,
                        UseShellExecute = true
                    });
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            lvwLog.Items.Clear();
        }

        private static void ShowToast(string title, string message, string serviceUrl, string toastImagePath)
        {
            ToastContentBuilder toast = new ToastContentBuilder()
                .AddText(title)
                .AddText(message)
                .AddText(serviceUrl)
                .AddAppLogoOverride(new Uri(toastImagePath))
                .SetToastDuration(ToastDuration.Short);
            toast.Show();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                serviceStatusIcon.Visible = true;
                this.Hide();
                e.Cancel = true;
            }
            else
            {
                serviceStatusIcon.Visible = false;
                serviceStatusIcon.Icon.Dispose();
                serviceStatusIcon.Dispose(); 
            }
        }

        private void serviceStatusIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            serviceStatusIcon.ShowBalloonTip(5);
        }
    }
}