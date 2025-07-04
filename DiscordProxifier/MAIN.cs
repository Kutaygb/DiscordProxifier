using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using Newtonsoft.Json;


namespace DiscordUnblocker
{
    public partial class MAIN : Form
    {
        public Logger logger = new Logger(); // Initialize the logger
        public List<string> ReStart = new List<string>();

        public void ReStartList(List<string> list)
        {
            var discordNames = new[] { "discord", "discordptb", "discordcanary", "discorddevelopment" };
            var alreadyStarted = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (string name in list)
            {
                var shortcut = Shortcuts.FirstOrDefault(sc =>
                {
                    var parts = sc.WorkingDirectory.Split('\\');
                    int idx = Array.IndexOf(parts, "Local");
                    if (idx != -1 && idx + 1 < parts.Length)
                    {
                        string sname = parts[idx + 1];
                        return discordNames.Contains(sname.ToLower()) && sname.Equals(name, StringComparison.OrdinalIgnoreCase);
                    }
                    return false;
                });

                if (shortcut != null && alreadyStarted.Add(name))
                {
                    Process.Start(shortcut.ShortcutPath);
                }
            }
        }

        public MAIN()
        {
            this.DoubleBuffered = true;
            logger.Log("Initializing Form1");

            int currentId = Process.GetCurrentProcess().Id;
            var others = Process.GetProcesses()
                .Where(p => p.ProcessName.Equals("discordproxifier", StringComparison.OrdinalIgnoreCase) && p.Id != currentId)
                .ToList();

            if (others.Any())
            {
                var res = MessageBox.Show(
                    "A process is already running! Do you want to close it?",
                    "Discord Proxifier",
                    MessageBoxButtons.YesNo
                );

                if (res == DialogResult.Yes)
                {
                    foreach (var p in others)
                        p.Kill();
                }
                else
                {
                    Environment.Exit(0);
                }
            }

            InitializeComponent();
        }


        public bool PrepareData()
        {
            bool PrepareTor()
            {
                bool Res = false;
                if (
                    Directory.Exists(Directory.GetCurrentDirectory() + "\\Data\\Tor")
                    && File.Exists(Directory.GetCurrentDirectory() + "\\Data\\Tor\\tor.exe")
                )
                {
                    Res = true;
                }

                return Res;
            }
            bool DExe(string ExeLoc)
            {
                if (File.Exists(ExeLoc))
                {
                    return true;
                }
                else
                {
                    logger.Log($"File not found: {ExeLoc}. Please make sure it exists.", LogType.ERROR);
                    return false;
                }
            }

            bool Tor = PrepareTor();
            bool Gost = DExe(Directory.GetCurrentDirectory() + "\\Data\\gost.exe");
            return Tor && Gost;
        }


        public bool PrepareDll()
        {
            logger.Log("Preparing Dll");

            logger.Log("Checking Data Directory");
            if (!Directory.Exists("Data"))
            {
                logger.Log("Creating Data Directory");
                Directory.CreateDirectory("Data");
            }

            string x86Dll = Directory.GetCurrentDirectory() + "\\Data\\x86\\version.dll";
            string x64Dll = Directory.GetCurrentDirectory() + "\\Data\\x64\\version.dll";

            bool x86Exists = File.Exists(x86Dll);
            bool x64Exists = File.Exists(x64Dll);

            if (!x86Exists || !x64Exists)
            {
                logger.Log("Required DLL files are missing Please place version.dll in both Data\\x64 folders.", LogType.ERROR);
                return false;
            }

            logger.Log("dll files are present");
            return true;
        }


        private Process Gost;
        private Process Tor;
        public string GostPort = "9129";
        public string MainProxyPort = "29812";
        public string mode = "Tor";
        public bool ServerRuning = false;
        public bool OpenAtStartup = false;
        public bool AutoUpdateProxified = false;

        public bool StopServer()
        {
            logger.Log("Stopping server");

            void StopProcess(ref Process proc, string name)
            {
                if (proc == null) return;
                try
                {
                    logger.Log($"Closing {name} process");
                    proc.CloseMainWindow();
                    if (!proc.WaitForExit(5000))
                    {
                        logger.Log($"{name} did not exit gracefully, killing process", LogType.WARNING);
                        proc.Kill();
                    }
                    logger.Log($"{name} process closed");
                    proc.Dispose();
                    proc = null;
                }
                catch (Exception ex)
                {
                    logger.Log($"Error stopping {name}: {ex.Message}", LogType.ERROR);
                }
            }

            StopProcess(ref Tor, "Tor");
            StopProcess(ref Gost, "Gost");

            ServerRuning = false;
            logger.Log("Server stopped");

            return false;
        }


        static void SetAutoStart(bool enable)
        {
            string appName = "Discord Unblocker Made By Srp";
            string exePath = Process.GetCurrentProcess().MainModule.FileName;

            using (
                RegistryKey key = Registry.CurrentUser.OpenSubKey(
                    @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run",
                    true
                )
            )
            {
                string expectedValue = $"\"{exePath}\" /normal";
                string currentValue = key.GetValue(appName) as string;

                if (enable && currentValue != expectedValue)
                {
                    try
                    {
                        key.SetValue(appName, expectedValue);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        throw new UnauthorizedAccessException(
                            "Failed to set registry key. Please run the application as an administrator."
                        );
                    }
                }
                else if (!enable && currentValue != null)
                {
                    key.DeleteValue(appName, false);
                }
            }
        }

        public bool PrepareServer()
        {
            void PrepProcess(ref Process proc, string name, string exe, string args, Action beforeStart = null)
            {
                logger.Log($"Preparing {name}");
                try
                {
                    if (proc != null)
                    {
                        logger.Log($"Killing existing {name} process");
                        proc.Kill();
                    }
                }
                catch (Exception ex)
                {
                    logger.Log($"Error killing {name} process: {ex.Message}", LogType.ERROR);
                }

                beforeStart?.Invoke();

                proc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = exe,
                        Arguments = args,
                        CreateNoWindow = true,
                        WindowStyle = ProcessWindowStyle.Hidden
                    }
                };
                logger.Log($"Starting {name} with arguments: {args}");
                try
                {
                    proc.Start();
                }
                catch (Exception ex)
                {
                    logger.Log($"Error starting {name}: {ex.Message}", LogType.ERROR);
                }
            }

            logger.Log("Setting up Tor and Gost");

            string torExe = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Tor", "tor.exe");
            string torrc = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Tor", "torrc");
            PrepProcess(
                ref Tor,
                "Tor",
                torExe,
                $"-f \"{torrc}\"",
                () => File.WriteAllText(torrc, $"SocksPort {MainProxyPort}")
            );

            string gostExe = Path.Combine(Directory.GetCurrentDirectory(), "Data", "gost.exe");
            PrepProcess(
                ref Gost,
                "Gost",
                gostExe,
                $"-L=:{GostPort} -F=socks5://localhost:{MainProxyPort}"
            );

            logger.Log("Tor and Gost setup complete");
            ServerRuning = true;
            return true;
        }


        public async Task<bool> StopServerAsync()
        {
            logger.Log("Stopping server");
            bool success = true;
            var waitme = new List<Task>();

            if (Tor != null)
                waitme.Add(StopProcessAsync(Tor, "Tor"));
            if (Gost != null)
                waitme.Add(StopProcessAsync(Gost, "Gost"));

            try
            {
                await Task.WhenAll(waitme);
            }
            catch (Exception ex)
            {
                logger.Log($"Error stopping servers: {ex.Message}", LogType.ERROR);
                success = false;
            }

            ServerRuning = false;
            logger.Log("Server stopped");
            return success;
        }

        private async Task<bool> StopProcessAsync(Process process, string processName)
        {
            logger.Log($"Closing {processName} process");
            process.CloseMainWindow();

            var completed = Task.Run(() => process.WaitForExit(1000));
            if (await completed)
            {
                logger.Log($"{processName} process closed gracefully.");
                process.Dispose();
                return true;
            }
            else
            {
                logger.Log(
                    $"{processName} did not exit gracefully, killing process",
                    LogType.WARNING
                );
                try
                {
                    process.Kill();
                }
                catch (Exception ex)
                {
                    logger.Log($"Error killing {processName} process: {ex.Message}", LogType.ERROR);
                    return false;
                }
                finally
                {
                    process.Dispose();
                }
                return false;
            }
        }

        public List<Shortcut> Shortcuts = new List<Shortcut>();

        private void LoadShortcuts()
        {
            int id = 0;
            listBox1.Items.Clear();
            Shortcuts = new List<Shortcut>();
            string roamingPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string discordPath = Path.Combine(roamingPath, @"Microsoft\Windows\Start Menu\Programs\Discord Inc");

            if (!Directory.Exists(discordPath))
            {
                // logger.Log($"Discord shortcuts directory not found: {discordPath}", LogType.WARNING);
                return;
            }

            foreach (var file in Directory.GetFiles(discordPath, "*.lnk"))
            {
                try
                {
                    string name = $"{Path.GetFileNameWithoutExtension(file)}  ({id})";
                    listBox1.Items.Add(name);

                    var shell = new IWshRuntimeLibrary.WshShell();
                    var link = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(file);

                    Shortcuts.Add(new Shortcut
                    {
                        TargetPath = link.TargetPath,
                        Arguments = link.Arguments,
                        WorkingDirectory = link.WorkingDirectory,
                        LBName = name,
                        ShortcutPath = file
                    });
                    id++;
                }
                catch (Exception ex)
                {
                    logger.Log($"Error processing shortcut '{file}': {ex.Message}", LogType.ERROR);
                }
            }
        }

        public Configuration configuration = new Configuration();

        private void LoadConfig(Configuration configuration)
        {
            //NConfig = configuration;
            configuration.OpenAtStartup = configuration.OpenAtStartup || false;
            configuration.AutoUpdateProxified = configuration.AutoUpdateProxified || false;
            configuration.Mode =
                configuration.Mode != null && configuration.Mode.Length > 0
                    ? configuration.Mode
                    : "Tor";
            configuration.MainProxyPort =
                configuration.MainProxyPort != null
                && configuration.MainProxyPort.ToString().Length > 0
                    ? configuration.MainProxyPort.ToString()
                    : "42213";
            configuration.GostPort =
                configuration.GostPort != null && configuration.GostPort.ToString().Length > 0
                    ? configuration.GostPort.ToString()
                    : "42212";
            OpenAtStartup = configuration.OpenAtStartup;
            SetAutoStart(OpenAtStartup);
            AutoUpdateProxified = configuration.AutoUpdateProxified;

            mode = configuration.Mode;
            GostPort = configuration.GostPort;
            MainProxyPort = configuration.MainProxyPort;
        }

        private void UpdateShortcut(string Shopath, string DcPath, string ExtraArguments = "")
        {
            try
            {
                var shell = new IWshRuntimeLibrary.WshShell();
                var link = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(Shopath);

                // DiscordName'i kısaca bul
                string discordName = DcPath.Split('\\')
                    .SkipWhile(part => !part.Equals("Local", StringComparison.OrdinalIgnoreCase))
                    .Skip(1)
                    .FirstOrDefault();

                if (string.IsNullOrEmpty(discordName))
                {
                    logger.Log("Discord directory not found within working directory", LogType.ERROR);
                    return;
                }

                link.Arguments = $"--processStart {discordName}.exe {ExtraArguments}";
                link.Save();
                logger.Log($"Shortcut '{Shopath}' updated with arguments: {link.Arguments}");
            }
            catch (Exception ex)
            {
                logger.Log($"Error updating shortcut '{Shopath}': {ex.Message}", LogType.ERROR);
            }
        }


        private string DiscordName(string dir)
        {
            var names = new[] { "discord", "discordptb", "discordcanary", "discorddevelopment" };
            var parts = dir.Split('\\');
            int idx = Array.IndexOf(parts, "Local");
            if (idx != -1 && idx + 1 < parts.Length)
            {
                var result = parts[idx + 1].ToLower();
                return names.Contains(result)
                    ? char.ToUpper(result[0]) + result.Substring(1)
                    : "";
            }
            return "";
        }


        private void CheckProxifiedDiscords(bool Override = false)
        {
            if (!AutoUpdateProxified && !Override)
                return;

            foreach (var shortcut in Shortcuts.Where(x => x.Arguments.Contains("proxy")))
            {
                if (shortcut == null)
                    continue;

                string args = shortcut.Arguments.ToLower();
                string expected = $"--a=--proxy-server=http://127.0.0.1:{GostPort}";

                if (!args.Contains($"proxy-server=http://127.0.0.1:{GostPort}"))
                {
                    UpdateShortcut(
                        shortcut.ShortcutPath,
                        shortcut.WorkingDirectory,
                        expected
                    );
                }
            }
        }


        string currentver = "01A";

        private void Form1_Load(object sender, EventArgs e)
        {
            Versionlbl.Text = $"Version : {currentver}";
            this.Text = $"Discord Proxifier (V: {currentver})";

            string configPath = Path.Combine(Directory.GetCurrentDirectory(), "Config.json");
            if (File.Exists(configPath))
                configuration = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(configPath));
            LoadConfig(configuration);

            string[] discordNames = { "discord", "discordptb", "discordcanary", "discorddevelopment" };

            if (OpenAtStartup)
            {
                foreach (var p in Process.GetProcesses().Where(x => discordNames.Contains(x.ProcessName.ToLower())))
                {
                    ReStart.Add(p.ProcessName);
                    p.Kill();
                }
            }

            logger.Log("Form1 loading");
            LoadShortcuts();
            PrepareDll();
            PrepareData();
            ShortcutUpdater.Start();
            CheckProxifiedDiscords();

            if (OpenAtStartup)
            {
                PrepareServer();
                ReStartList(ReStart);
                ReStart.Clear();
            }

            logger.Log("Form1 loaded");
        }


        public enum MachineType
        {
            Native = 0,
            I386 = 0x014c,
            Itanium = 0x0200,
            x64 = 0x8664
        }

        public static MachineType GetMachineType(string fileName)
        {
            const int PE_POINTER_OFFSET = 60;
            const int MACHINE_OFFSET = 4;
            byte[] data = new byte[4096];
            using (Stream s = new FileStream(fileName, System.IO.FileMode.Open, FileAccess.Read))
            {
                s.Read(data, 0, 4096);
            }
            int PE_HEADER_ADDR = BitConverter.ToInt32(data, PE_POINTER_OFFSET);
            int machineUint = BitConverter.ToUInt16(data, PE_HEADER_ADDR + MACHINE_OFFSET);
            return (MachineType)machineUint;
        }

        private string GetDll(string file)
        {
            try
            {
                MachineType mt = GetMachineType(file);
                if (mt == MachineType.x64)
                {
                    return Directory.GetCurrentDirectory() + "\\Data\\X64\\version.dll";
                }
                else
                {
                    return Directory.GetCurrentDirectory() + "\\Data\\X86\\version.dll";
                }
            }
            catch (Exception ex)
            {
                logger.Log("Got Error On Getting Dll (PE) " + ex.Message, LogType.ERROR);
                return Directory.GetCurrentDirectory() + "\\Data\\X64\\version.dll";
            }
        }

        private async void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            logger.Log("Form1 closing");

            bool serverStoppedSuccessfully = await StopServerAsync();

            if (!serverStoppedSuccessfully)
            {
                e.Cancel = true;
                MessageBox.Show(
                    "Error stopping server.  Please try again or close the application manually.",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                logger.Log("Server shutdown failed, but allowing form closure.");
            }
        }

        private async void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            logger.Log("Form1 closed :p");

            bool serverStoppedSuccessfully = await StopServerAsync();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //logger.Log("Client selection changed");
            if (listBox1.SelectedItem != null)
            {
                string selecteditem = listBox1.SelectedItem.ToString();
                Shortcut shortcut = Shortcuts
                    .Where(
                        (x) =>
                        {
                            return x.LBName == selecteditem;
                        }
                    )
                    .FirstOrDefault();

                if (shortcut != null)
                {
                    name_label.Text = "Client Name : " + selecteditem;
                    if (shortcut.Arguments.Contains("proxy"))
                    {
                        proxified_label.Text = "Is Proxified : Yes";
                        proxify.Text = "Deproxify The Shortcut";
                    }
                    else
                    {
                        proxified_label.Text = "Is Proxified : No";
                        proxify.Text = "Proxify The Shortcut";
                    }
                    //logger.Log($"Selected client: {selecteditem}");
                }
                else
                {
                    /*logger.Log(
                        $"Shortcut not found for selected item: {selecteditem}",
                        LogType.WARNING
                    );*/
                }
            }
            else
            {
                //logger.Log("No item selected in ListBox", LogType.WARNING);
            }
        }

        private bool CopyVDLL()
        {
            if (listBox1.SelectedItem == null)
                return false;

            string GetLpt(string input)
            {
                var match = Regex.Match(input ?? "", @"\(([^)]*)\)$");
                return match.Success ? match.Groups[1].Value : null;
            }

            string selectedLpt = GetLpt(listBox1.SelectedItem.ToString());
            var shortcut = Shortcuts.FirstOrDefault(x => GetLpt(x.LBName) == selectedLpt);
            if (shortcut == null)
                return false;

            string discordName = shortcut.WorkingDirectory
                .Split('\\')
                .SkipWhile(s => !s.Equals("Local", StringComparison.OrdinalIgnoreCase))
                .Skip(1)
                .FirstOrDefault();

            if (string.IsNullOrEmpty(discordName))
            {
                logger.Log("Discord directory not found within working directory.", LogType.ERROR);
                return false;
            }

            foreach (var p in Process.GetProcesses().Where(p => p.ProcessName.Equals(discordName, StringComparison.OrdinalIgnoreCase)))
                p.Kill();

            System.Threading.Thread.Sleep(2000);

            string vDllPath = Path.Combine(shortcut.WorkingDirectory, "version.dll");
            if (File.Exists(vDllPath))
                File.Delete(vDllPath);

            if (Directory.Exists(shortcut.WorkingDirectory))
            {
                string sourceDll = GetDll(Path.Combine(shortcut.WorkingDirectory, $"{discordName}.exe"));
                File.Copy(sourceDll, vDllPath, true);
                return true;
            }

            return false;
        }


        private bool RemoveVDLL()
        {
            if (listBox1.SelectedItem == null)
                return false;

            string GetLpt(string input)
            {
                var match = Regex.Match(input ?? "", @"\(([^)]*)\)$");
                return match.Success ? match.Groups[1].Value : null;
            }

            string selectedLpt = GetLpt(listBox1.SelectedItem.ToString());
            var shortcut = Shortcuts.FirstOrDefault(x => GetLpt(x.LBName) == selectedLpt);
            if (shortcut == null)
                return false;

            string discordName = shortcut.WorkingDirectory
                .Split('\\')
                .SkipWhile(s => !s.Equals("Local", StringComparison.OrdinalIgnoreCase))
                .Skip(1)
                .FirstOrDefault();

            if (string.IsNullOrEmpty(discordName))
            {
                logger.Log("Discord directory not found within working directory.", LogType.ERROR);
                return false;
            }

            foreach (var p in Process.GetProcesses().Where(p => p.ProcessName.Equals(discordName, StringComparison.OrdinalIgnoreCase)))
                p.Kill();

            System.Threading.Thread.Sleep(2000);

            string vDllPath = Path.Combine(shortcut.WorkingDirectory, "version.dll");
            if (File.Exists(vDllPath))
                File.Delete(vDllPath);

            return true;
        }


        private void StartWProxy(string or = null)
        {
            CopyVDLL();
            logger.Log("Start with proxy button clicked");

            string GetLpt(string input)
            {
                var match = Regex.Match(input ?? "", @"\(([^)]*)\)$");
                return match.Success ? match.Groups[1].Value : null;
            }

            string selectedItem = or ?? listBox1.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedItem))
            {
                logger.Log("No client selected", LogType.WARNING);
                return;
            }

            var shortcut = Shortcuts.FirstOrDefault(x => GetLpt(x.LBName) == GetLpt(selectedItem));
            if (shortcut == null)
            {
                logger.Log($"No shortcut found matching selected item {selectedItem}", LogType.WARNING);
                return;
            }

            string discordName = shortcut.WorkingDirectory
                .Split('\\')
                .SkipWhile(s => !s.Equals("Local", StringComparison.OrdinalIgnoreCase))
                .Skip(1)
                .FirstOrDefault();

            if (string.IsNullOrEmpty(discordName))
            {
                logger.Log("Discord directory not found within working directory.", LogType.ERROR);
                return;
            }

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = shortcut.TargetPath,
                    Arguments = $"--processStart {discordName}.exe --a=--proxy-server=http://127.0.0.1:{GostPort}",
                    WorkingDirectory = shortcut.WorkingDirectory
                }
            };

            logger.Log($"Starting Discord with arguments: {process.StartInfo.Arguments}");

            try
            {
                process.Start();
                logger.Log("Discord started successfully");
            }
            catch (Exception ex)
            {
                logger.Log($"Error starting Discord: {ex.Message}", LogType.ERROR);
            }
        }

        private void startwproxy_Click(object sender, EventArgs e)
        {
            StartWProxy();
            CheckProxifiedDiscords();
        }

        private void proxify_Click(object sender, EventArgs e)
        {
            CopyVDLL();
            logger.Log("Proxify/Deproxify button clicked");

            string GetLpt(string input)
            {
                var match = Regex.Match(input ?? "", @"\(([^)]*)\)$");
                return match.Success ? match.Groups[1].Value : null;
            }

            if (listBox1.SelectedItem == null)
            {
                logger.Log("No client selected", LogType.WARNING);
                return;
            }

            string selectedLpt = GetLpt(listBox1.SelectedItem.ToString());
            var shortcut = Shortcuts.FirstOrDefault(x => GetLpt(x.LBName) == selectedLpt);

            if (shortcut == null)
            {
                logger.Log("No shortcut found for selected client", LogType.WARNING);
                return;
            }

            if (shortcut.Arguments.Contains("proxy"))
            {
                logger.Log("Deproxifying shortcut");
                UpdateShortcut(shortcut.ShortcutPath, shortcut.WorkingDirectory);
            }
            else
            {
                logger.Log("Proxifying shortcut");
                UpdateShortcut(
                    shortcut.ShortcutPath,
                    shortcut.WorkingDirectory,
                    $"--a=--proxy-server=http://127.0.0.1:{GostPort}"
                );
            }

            int val = listBox1.SelectedIndex;
            LoadShortcuts();
            if (val >= 0 && val < listBox1.Items.Count)
            {
                try { listBox1.SetSelected(val, true); }
                catch (Exception ex)
                {
                    logger.Log(
                        $"Error reselecting item in listBox1 after shortcut update: {ex.Message}",
                        LogType.ERROR
                    );
                }
            }
        }


        private void ServerT_Click(object sender, EventArgs e)
        {
            logger.Log("Server toggle button clicked");

            bool starting = !ServerRuning;
            logger.Log(starting ? "Starting server" : "Stopping server");

            string status = starting ? "Up" : "Down";
            string btnText = starting ? "Stop Server" : "Start Server";

            ServerStatus.Text = TRAY_SERVER_STATUS_C.Text = $"Server Status : {status}";
            StartServer.Text = TRAY_SERVER_TOGGLE_C.Text = btnText;

            if (starting)
                PrepareServer();
            else
                StopServer();
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            logger.Log("Exit button clicked");

            StopServer();
            Environment.Exit(0);
        }

        private void Minimize_Click(object sender, EventArgs e)
        {
            logger.Log("Minimize button clicked");

            this.WindowState = FormWindowState.Minimized;
        }

        private Point mouseOffset;
        private bool isDragging = false;

        private void DragPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                mouseOffset = new Point(e.X, e.Y);
            }
        }

        private void DragPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point newLocation = this.PointToScreen(new Point(e.X, e.Y));
                newLocation.Offset(-mouseOffset.X, -mouseOffset.Y);
                this.Location = newLocation;
            }
        }

        private void DragPanel_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = false;
            }
        }

        private void ShowLogs_Click(object sender, EventArgs e)
        {
            Logs l = new Logs(logger);
            l.Show();
        }

        private void UpdateChecker(List<Shortcut> old, List<Shortcut> nw)
        {
            string LPT(string input)
            {
                if (string.IsNullOrEmpty(input))
                {
                    return null;
                }

                Match match = Regex.Match(input, @"\(([^)]*)\)$");

                if (match.Success)
                {
                    return match.Groups[1].Value;
                }

                return null;
            }
            foreach (Shortcut sc in nw)
            {
                Shortcut shortcut = old.Where(
                        (x) =>
                        {
                            return LPT(sc.LBName) == LPT(x.LBName);
                        }
                    )
                    .First();
                if (shortcut != null)
                {
                    if (sc.WorkingDirectory != shortcut.WorkingDirectory)
                    {
                        //StartWProxy(sc.LBName);
                        CopyVDLL();
                    }
                }
            }
        }

        private void ShortcutUpdater_Tick(object sender, EventArgs e)
        {
            var old = Shortcuts;
            int val = listBox1.SelectedIndex;
            LoadShortcuts();
            try
            {
                if (val >= 0 && val < listBox1.Items.Count)
                {
                    listBox1.SetSelected(val, true);
                }
            }
            catch (Exception ex)
            {
                logger.Log($"Error reselecting item in listBox1 after LoadShortcuts: {ex.Message}\n{ex.StackTrace}", LogType.ERROR);
            }

            UpdateChecker(old, Shortcuts);

            try
            {
                var procs = Process.GetProcesses().Select(p => p.ProcessName.ToLower()).ToList();
                bool torRunning = procs.Contains("tor");
                bool gostRunning = procs.Contains("gost");

                if (!(torRunning && gostRunning) && ServerRuning)
                {
                    string xy = (!torRunning ? "P" : "") + (!gostRunning ? "G" : "");
                    ServerStatus.Text = TRAY_SERVER_STATUS_C.Text = $"Server Status : E1_{xy}";
                }
                else if (ServerRuning)
                {
                    ServerStatus.Text = TRAY_SERVER_STATUS_C.Text = "Server Status : Up";
                    StartServer.Text = TRAY_SERVER_TOGGLE_C.Text = "Stop Server";
                }
                else
                {
                    ServerStatus.Text = TRAY_SERVER_STATUS_C.Text = "Server Status : Down";
                    StartServer.Text = TRAY_SERVER_TOGGLE_C.Text = "Start Server";
                }
            }
            catch (Exception ex)
            {
                logger.Log($"Error in server status update: {ex.Message}\n{ex.StackTrace}", LogType.ERROR);
            }
        }

        private void FStop_Click(object sender, EventArgs e)
        {
            foreach (Process p in Process.GetProcesses())
            {
                if (p.ProcessName.ToLower() == "tor")
                {
                    p.Kill();
                }
                if (p.ProcessName.ToLower() == "gost")
                {
                    p.Kill();
                }
            }
            ServerRuning = false;
        }

        private void settingsbtn_Click(object sender, EventArgs e)
        {
            var old = configuration;
            var settingsForm = new Settings();
            settingsForm.ShowDialog();
            configuration = settingsForm.Configuration;
            LoadConfig(configuration);
            CheckProxifiedDiscords();

            string[] discordNames = { "discord", "discordptb", "discordcanary", "discorddevelopment" };
            var discordProcs = Process.GetProcesses()
                .Where(p => discordNames.Contains(p.ProcessName.ToLower()))
                .ToList();

            // Eğer GostPort değiştiyse ve Discord açıksa, restart sor
            if (configuration.GostPort != old.GostPort && discordProcs.Any())
            {
                var dres = MessageBox.Show(
                    "Restart Discord(s) to use newer Gost port?",
                    "DiscordUnblocker",
                    MessageBoxButtons.YesNo
                );
                if (dres == DialogResult.Yes)
                {
                    foreach (var p in discordProcs)
                    {
                        ReStart.Add(p.ProcessName);
                        p.Kill();
                    }
                    ReStartList(ReStart);
                    ReStart.Clear();
                }
            }

            if (ServerRuning)
            {
                ServerStatus.Text = TRAY_SERVER_STATUS_C.Text = "Server Status : Down";
                foreach (var p in Process.GetProcesses())
                {
                    var name = p.ProcessName.ToLower();
                    if (name == "tor" || name == "gost")
                        p.Kill();
                }
                ServerRuning = false;
                PrepareServer();
            }
        }


        private void TRAY_SERVER_TOGGLE_C_Click(object sender, EventArgs e)
        {
            if (ServerRuning == false)
            {
                logger.Log("Starting server (TRAY)");

                TRAY_SERVER_TOGGLE_C.Text = "Server Status : Up";
                TRAY_SERVER_TOGGLE_C.Text = "Stop Server";
                PrepareServer();
            }
            else
            {
                logger.Log("Stopping server (TRAY)");

                TRAY_SERVER_TOGGLE_C.Text = "Server Status : Down";
                TRAY_SERVER_TOGGLE_C.Text = "Start Server";
                StopServer();
            }
        }

        private void TRAY_SHOW_APP_C_Click(object sender, EventArgs e)
        {
            this.Show();
            TRAY.Visible = false;
        }

        private void TRAY_EXIT_C_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void Traybtn_Click(object sender, EventArgs e)
        {
            TRAY.Visible = true;
            TRAY.ShowBalloonTip(500);
            this.Hide();
        }
    }

    public class Shortcut
    {
        public string TargetPath { get; set; }
        public string Arguments { get; set; }
        public string WorkingDirectory { get; set; }
        public string LBName { get; set; }
        public string ShortcutPath { get; set; }
    }

    public enum LogType
    {
        NORMAL,
        WARNING,
        ERROR
    }

    public class Log
    {
        public LogType Type { get; set; }
        public string Text { get; set; }
        public DateTime Time { get; set; }
    }

    public class Logger
    {
        List<Log> Logs = new List<Log>();

        public Logger() { }

        public FlowLayoutPanel CurrentPanel;

        public void Log(string text, LogType type = LogType.NORMAL)
        {
            Log nl = new Log();
            nl.Text = text;
            nl.Type = type;
            nl.Time = DateTime.UtcNow;
            Logs.Add(nl);
            if (CurrentPanel != null)
                AddLog(nl, CurrentPanel);
        }

        public void AddLog(Log l, FlowLayoutPanel panel)
        {
            Color GetLogColor(LogType type)
            {
                switch (type)
                {
                    case LogType.NORMAL:
                        return Color.White;
                    case LogType.WARNING:
                        return Color.Yellow;
                    case LogType.ERROR:
                        return Color.Red;
                    default:
                        return Color.White;
                }
            }
            System.Windows.Forms.Label CreateLabel()
            {
                System.Windows.Forms.Label label = new System.Windows.Forms.Label();

                label.ForeColor = System.Drawing.Color.White;
                label.Font = new Font("Arial", 11, FontStyle.Regular);
                label.Name = "LOG";

                label.AutoSize = true;
                label.MaximumSize = new System.Drawing.Size(panel.Width - 10, 400);
                label.Text = "";
                label.Margin = new Padding(1);
                return label;
            }

            string formattedLog = $"\n[{l.Time:HH:mm:ss}] : {l.Text}";
            Color logColor = GetLogColor(l.Type);
            System.Windows.Forms.Label box = CreateLabel();
            box.ForeColor = logColor;
            box.Text = formattedLog;
            panel.Controls.Add(box);
            panel.ScrollControlIntoView(box);
        }

        public void AddLogs(FlowLayoutPanel panel)
        {
            Color GetLogColor(LogType type)
            {
                switch (type)
                {
                    case LogType.NORMAL:
                        return Color.White;
                    case LogType.WARNING:
                        return Color.Yellow;
                    case LogType.ERROR:
                        return Color.Red;
                    default:
                        return Color.White;
                }
            }
            System.Windows.Forms.Label CreateLabel()
            {
                System.Windows.Forms.Label label = new System.Windows.Forms.Label();

                label.ForeColor = System.Drawing.Color.White;

                label.Name = "LOG";
                label.AutoSize = true;
                label.Font = new Font("Arial", 11, FontStyle.Regular);

                label.MaximumSize = new System.Drawing.Size(panel.Width - 10, 400);
                label.Text = "";
                label.Margin = new Padding(1);
                return label;
            }
            foreach (Log l in Logs)
            {
                string formattedLog = $"[{l.Time:HH:mm:ss}][{l.Type}]: {l.Text}";
                Color logColor = GetLogColor(l.Type);
                System.Windows.Forms.Label box = CreateLabel();
                box.ForeColor = logColor;
                box.Text = formattedLog;
                panel.Controls.Add(box);
                panel.ScrollControlIntoView(box);
            }
        }
    }
}
