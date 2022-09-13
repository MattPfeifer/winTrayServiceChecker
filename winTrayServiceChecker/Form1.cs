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

            CheckServices();
            timerCheckServices.Interval = settings.Interval * 60* 1000 ;
            timerCheckServices.Tick += TimerCheckServices_Tick;
            timerCheckServices.Start();
        }

        async private void TimerCheckServices_Tick(object? sender, EventArgs e)
        {
            await CheckServices();
        }

        async private Task CheckServices()
        {
            bool serviceDown = false;
            contextMenu.Items.Clear();
            foreach(ServiceEndpoint endpoint in settings.ServiceEndpoints)
            {
                HttpResponseMessage httpResponseMessage = await CallEndpoint(endpoint);

                var rand = new Random();
                var num = rand.Next(0, 100);
                if (num > 90) httpResponseMessage.StatusCode = System.Net.HttpStatusCode.NotFound;

                if (!httpResponseMessage.IsSuccessStatusCode)
                {
                    serviceDown = true;
                    ShowToast($"Unable to access {endpoint.Name}", $"Statue Code: {httpResponseMessage.StatusCode}", endpoint.Url);
                }

                //update icon in log and menu
                ReloadEndpoint(endpoint, httpResponseMessage);
                serviceStatusIcon.Icon = serviceDown ? Resources.iconInvalid : Resources.iconOK;
            }

            contextMenu.Items.Add(new ToolStripSeparator());
            ToolStripMenuItem menuLogs = new ToolStripMenuItem("View Logs");
            menuLogs.Click += MenuLogs_Click;
            contextMenu.Items.Add(menuLogs);
            ToolStripMenuItem menuExit = new ToolStripMenuItem("Exit");
            menuExit.Click += MenuExit_Click;
            contextMenu.Items.Add(menuExit);

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

        private void ReloadEndpoint(ServiceEndpoint endpoint, HttpResponseMessage httpResponseMessage)
        {
            int imageIndex = httpResponseMessage.IsSuccessStatusCode ? 0 : 1;
            ListViewItem item = new ListViewItem(string.Empty, imageIndex) {  Name = endpoint.Name };
            item.SubItems.Add(httpResponseMessage.StatusCode.ToString());
            item.SubItems.Add(DateTime.Now.ToString());
            item.SubItems.Add(endpoint.Name);
            lvwLog.Items.Add(item);
            //lvwLog.Items.Insert(0, item);

            Image imageStatus = httpResponseMessage.IsSuccessStatusCode ? Resources.StatusOK : Resources.StatusInvalid;
            ToolStripMenuItem menuItem = new ToolStripMenuItem() { Text = endpoint.Name, ToolTipText = endpoint.Url, Image = imageStatus };
            menuItem.Tag = endpoint;
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

        private static void ShowToast(string title, string message, string message2 = "")
        {
            ToastContentBuilder toast = new ToastContentBuilder()
                .AddText(title)
                .AddText(message)
                .AddText(message2);
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
    }
}