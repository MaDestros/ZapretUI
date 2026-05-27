using System.Diagnostics;
using System.IO.Compression;
using System.Net;

namespace ZapretUI
{
    public partial class Main : Form
    {
        internal static readonly string _Directory = @$"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\MaDestro Projects\";
        internal static readonly string _Zapret = @$"{_Directory}Zapret\";
        internal static readonly string _ZapretURL = "https://github.com/bol-van/zapret2/releases/download/v0.9.5.2/zapret2-v0.9.5.2.zip";
        internal static readonly string _Boosty = "https://boosty.to/madestro";

        internal static readonly string _ProcessFile = @$"{_Zapret}zapret2-v0.9.5.2\binaries\windows-x86_64\winws2.exe";

        internal static string _SystemPath = @$"{_Directory}Revolt Launcher.ini";

        Process? process;

        public Main()
        {
            InitializeComponent();
        }

        private void bt_github_Click(object sender, EventArgs e) { Process.Start(new ProcessStartInfo("https://github.com/MaDestros/ZapretUI") { UseShellExecute = true }); }

        private void bt_gitea_Click(object sender, EventArgs e) { Process.Start(new ProcessStartInfo("https://git.net-state.ru/MaDestro/ZapretUI") { UseShellExecute = true }); }

        private void Form1_Load(object sender, EventArgs e) { CheckDirectory(); }

        private void _appClose_Click(object sender, EventArgs e) { Application.Exit(); }

        private void _Minimize_Click(object sender, EventArgs e) { WindowState = FormWindowState.Minimized; }

        private void bt_boosty_Click(object sender, EventArgs e) { Process.Start(new ProcessStartInfo(_Boosty) { UseShellExecute = true }); }

        private void bt_installer_Click(object sender, EventArgs e)
        {
            Directory.CreateDirectory(_Zapret);
            DownloadRelease();
            CheckDirectory();
        }

        private void bt_start_Click(object sender, EventArgs e)
        {
            try
            {
                process = new Process();
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.FileName = _ProcessFile;
                process.StartInfo.Arguments = "--wf-tcp-out=80,443 --wf-udp-out=443 --lua-desync=antidpi:typ=ip"; // Ключевые параметры!
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error"); }
        }

        private void bt_stop_Click(object sender, EventArgs e)
        {
            try
            {
                process.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error"); }
        }

        void CheckDirectory()
        {
            if (!Directory.Exists(_Zapret))
            {
                bt_installer.Enabled = true;
                bt_installer.Text = "Установить";
                bt_start.Enabled = false;
                bt_stop.Enabled = false;
            }
            else
            {
                bt_installer.Enabled = false;
                bt_installer.Text = "Установлено";
                bt_start.Enabled = true;
                bt_stop.Enabled = true;
            }
        }

        async void DownloadRelease()
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.DownloadProgressChanged += (s, e) => bunifuProgressBar1.Value = e.ProgressPercentage;
                    client.DownloadFileCompleted += Client_DownloadFileCompleted;
                    client.DownloadFileAsync(new Uri(_ZapretURL), @$"{_Zapret}tools.zip");
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error Part 1"); }
        }

        private void Client_DownloadFileCompleted(object? sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            bunifuProgressBar1.Value = 0;
            try
            {
                if (File.Exists(@$"{_Zapret}tools.zip")) // Распаковка инструмента
                {
                    ZipFile.ExtractToDirectory($@"{_Zapret}tools.zip", $@"{_Zapret}");
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error Part 2"); }

            bt_start.Enabled = true;
            bt_stop.Enabled = true;
        }
    }
}