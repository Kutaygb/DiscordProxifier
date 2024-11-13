using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.IO.Compression;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Security.AccessControl;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using Newtonsoft.Json;
using Octokit;

namespace DiscordUnblocker
{
    public partial class MAIN : Form
    {
        public Logger logger = new Logger(); // Initialize the logger
        public List<string> ReStart = new List<string>();

        public void ReStartList(List<string> list)
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
            string ShortcutToString(Shortcut shortcut)
            {
                if (shortcut != null)
                {
                    string DiscordName = "";
                    string[] parts = shortcut.WorkingDirectory.Split('\\');

                    int localIndex = Array.IndexOf(parts, "Local");

                    if (localIndex != -1 && localIndex + 1 < parts.Length)
                    {
                        string result = parts[localIndex + 1];
                        switch (result.ToLower())
                        {
                            case "discord":
                                DiscordName = "Discord";
                                break;
                            case "discordptb":
                                DiscordName = "DiscordPTB";

                                break;
                            case "discordcanary":
                                DiscordName = "DiscordCanary";

                                break;
                            case "discorddevelopment":
                                DiscordName = "DiscordDevelopment";

                                break;
                        }
                        return DiscordName;
                    }
                }
                else
                {
                    logger.Log(
                        $"No shortcut found matching selected item {shortcut.LBName}",
                        LogType.WARNING
                    );
                }
                return null;
            }
            List<string> AlrStarted = new List<string>();
            foreach (string s in list)
            {
                Shortcut shortcut = Shortcuts
                    .Where(
                        (x) =>
                        {
                            return ShortcutToString(x).ToLower() == s.ToLower();
                        }
                    )
                    .FirstOrDefault();
                if (shortcut != null && !AlrStarted.Contains(s))
                {
                    AlrStarted.Add(s);
                    Process.Start(shortcut.ShortcutPath);
                }
            }
        }

        public MAIN()
        {
            this.DoubleBuffered = true;
            logger.Log("Initializing Form1");
            int a = 0;
            foreach (Process p in Process.GetProcesses())
            {
                if (
                    p.ProcessName.ToLower() == "discordproxifier"
                    && p.Id != Process.GetCurrentProcess().Id
                )
                {
                    a += 1;
                }
            }
            if (a != 0)
            {
                DialogResult res = MessageBox.Show(
                    "An Process Already Running! Did you want to close?",
                    "Discord Proxifier",
                    MessageBoxButtons.YesNo
                );
                if (res == DialogResult.Yes)
                {
                    foreach (Process p in Process.GetProcesses())
                    {
                        if (
                            p.ProcessName.ToLower() == "discordproxifier"
                            && p.Id != Process.GetCurrentProcess().Id
                        )
                        {
                            p.Kill();
                        }
                    }
                }
                else
                {
                    Environment.Exit(0);
                }
            }
            InitializeComponent();
        }

        public async Task<bool> PrepareData()
        {
            void UnZip(string Inp, string Out)
            {
                ZipFile.ExtractToDirectory(Inp, Out);
            }
            void DownloadFile(string Url, string Out)
            {
                new WebClient().DownloadFile(Url, Out);
            }
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
                else
                {
                    DownloadFile(
                        "https://raw.githubusercontent.com/s0rp/DiscordProxifier/refs/heads/main/Data/Tor.zip",
                        Directory.GetCurrentDirectory() + "\\Data\\Tor.zip"
                    );
                    UnZip(
                        Directory.GetCurrentDirectory() + "\\Data\\Tor.zip",
                        Directory.GetCurrentDirectory() + "\\Data\\Tor"
                    );
                    File.Delete(Directory.GetCurrentDirectory() + "\\Data\\Tor.zip");
                    Res = File.Exists(Directory.GetCurrentDirectory() + "\\Data\\Tor\\tor.exe");
                }
                return Res;
            }
            bool DExe(string ExeLoc, string ExeDLink)
            { //Directory.GetCurrentDirectory() + "\\Data\\gost.exe")
                bool Res = false;
                if (File.Exists(ExeLoc))
                {
                    Res = true;
                }
                else
                {
                    DownloadFile(
                        //"https://raw.githubusercontent.com/s0rp/DiscordProxifier/refs/heads/main/Data/gost.exe",
                        ExeDLink,
                        ExeLoc
                    );
                    Res = File.Exists(ExeLoc);
                }
                return Res;
            }

            bool Tor = PrepareTor();
            bool Gost = DExe(
                Directory.GetCurrentDirectory() + "\\Data\\gost.exe",
                "https://raw.githubusercontent.com/s0rp/DiscordProxifier/refs/heads/main/Data/gost.exe"
            );
            return Tor && Gost;
        }

        public async Task<bool> PrepareDll()
        {
            logger.Log("Preparing Dll");

            logger.Log("Checking Data Directory");
            if (!Directory.Exists("Data"))
            {
                logger.Log("Creating Data Directory");
                Directory.CreateDirectory("Data");
            }

            string dllver = "";
            if (File.Exists(Directory.GetCurrentDirectory() + "\\Data\\DllVer"))
            {
                logger.Log("Reading DLL version from file");
                dllver = File.ReadAllText(Directory.GetCurrentDirectory() + "\\Data\\DllVer");
            }

            logger.Log("Creating GitHub client");
            GitHubClient client = new GitHubClient(new ProductHeaderValue("DiscordUnblocker"));
            logger.Log("Fetching releases from GitHub");
            IReadOnlyList<Release> releases = null;
            try
            {
                releases = await client.Repository.Release.GetAll("aiqinxuancai", "discord-proxy");
            }
            catch (Exception ex)
            {
                logger.Log($"Error fetching releases: {ex.Message}", LogType.ERROR);
                return false; // Indicate failure
            }

            if (releases == null || releases.Count == 0)
            {
                logger.Log("No releases found on GitHub.", LogType.ERROR);
                return false; // Indicate failure
            }

            Version latestGitHubVersion = new Version(releases[0].TagName.Substring(1));
            Version localVersion;
            string TRdllver = dllver.TrimStart('V', 'v');
            if (Regex.IsMatch(TRdllver, @"^\d+\.\d+\.\d+$"))
            {
                localVersion = new Version(TRdllver);
                logger.Log($"Local DLL version: {localVersion}");
            }
            else
            {
                localVersion = new Version("0.0.0");
                logger.Log("No valid local DLL version found. Using 0.0.0");
            }
            int versionComparison = localVersion.CompareTo(latestGitHubVersion);
            logger.Log(
                $"Comparing local version ({localVersion}) with latest GitHub version ({latestGitHubVersion})"
            );

            if (versionComparison < 0)
            {
                logger.Log("Newer DLL version found on GitHub");
                bool downloaded = false; // Flag to track if any DLL was downloaded

                foreach (ReleaseAsset a in releases[0].Assets)
                {
                    if (a.Name.ToLower().Contains("x86"))
                    {
                        logger.Log($"Downloading {a.Name}");

                        if (File.Exists(Directory.GetCurrentDirectory() + "\\Data\\version.zip"))
                        {
                            logger.Log("Deleting existing version.zip");
                            File.Delete(Directory.GetCurrentDirectory() + "\\Data\\version.zip");
                        }

                        try
                        {
                            new WebClient().DownloadFile(
                                a.BrowserDownloadUrl,
                                Directory.GetCurrentDirectory() + "\\Data\\version.zip"
                            );
                            downloaded = true; // Mark as downloaded successfully
                        }
                        catch (Exception ex)
                        {
                            logger.Log($"Error downloading DLL: {ex.Message}", LogType.ERROR);
                            continue; // Try the next asset
                        }

                        logger.Log("Extracting downloaded zip");

                        if (Directory.Exists(Directory.GetCurrentDirectory() + "\\Data\\x86"))
                        {
                            logger.Log("Deleting existing x86 version.dll");
                            Directory.Delete(Directory.GetCurrentDirectory() + "\\Data\\x86", true);
                        }
                        Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\Data\\x86");
                        try
                        {
                            ZipFile.ExtractToDirectory(
                                Directory.GetCurrentDirectory() + "\\Data\\version.zip",
                                Directory.GetCurrentDirectory() + "\\Data\\x86"
                            );
                        }
                        catch (Exception ex)
                        {
                            logger.Log($"Error extracting DLL: {ex.Message}", LogType.ERROR);
                            continue;
                        }

                        if (File.Exists(Directory.GetCurrentDirectory() + "\\Data\\version.zip"))
                        {
                            logger.Log("Deleting version.zip");
                            File.Delete(Directory.GetCurrentDirectory() + "\\Data\\version.zip");
                        }

                        File.WriteAllText(
                            Directory.GetCurrentDirectory() + "\\Data\\DllVer",
                            releases[0].TagName
                        );

                        logger.Log("DLL update complete");
                        // return true; // Indicate success
                    }
                    if (a.Name.ToLower().Contains("x64"))
                    {
                        logger.Log($"Downloading {a.Name}");

                        if (File.Exists(Directory.GetCurrentDirectory() + "\\Data\\version.zip"))
                        {
                            logger.Log("Deleting existing version.zip");
                            File.Delete(Directory.GetCurrentDirectory() + "\\Data\\version.zip");
                        }

                        try
                        {
                            new WebClient().DownloadFile(
                                a.BrowserDownloadUrl,
                                Directory.GetCurrentDirectory() + "\\Data\\version.zip"
                            );
                            downloaded = true; // Mark as downloaded successfully
                        }
                        catch (Exception ex)
                        {
                            logger.Log($"Error downloading DLL: {ex.Message}", LogType.ERROR);
                            continue; // Try the next asset
                        }

                        logger.Log("Extracting downloaded zip");

                        if (Directory.Exists(Directory.GetCurrentDirectory() + "\\Data\\x64"))
                        {
                            logger.Log("Deleting existing x64 version.dll");
                            Directory.Delete(Directory.GetCurrentDirectory() + "\\Data\\x64", true);
                        }
                        Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\Data\\x64");
                        try
                        {
                            ZipFile.ExtractToDirectory(
                                Directory.GetCurrentDirectory() + "\\Data\\version.zip",
                                Directory.GetCurrentDirectory() + "\\Data\\x64"
                            );
                        }
                        catch (Exception ex)
                        {
                            logger.Log($"Error extracting DLL: {ex.Message}", LogType.ERROR);
                            continue;
                        }

                        if (File.Exists(Directory.GetCurrentDirectory() + "\\Data\\version.zip"))
                        {
                            logger.Log("Deleting version.zip");
                            File.Delete(Directory.GetCurrentDirectory() + "\\Data\\version.zip");
                        }

                        File.WriteAllText(
                            Directory.GetCurrentDirectory() + "\\Data\\DllVer",
                            releases[0].TagName
                        );

                        logger.Log("DLL update complete");
                        //return true; // Indicate success
                    }
                }
                if (!downloaded) // no x86 asset and hence dll downloaded
                {
                    logger.Log("no x64/X86 DLL found", LogType.ERROR);
                    return false;
                }
            }
            else if (versionComparison > 0)
            {
                logger.Log("Local DLL version is newer than GitHub version. Skipping update.");
            }
            else
            {
                logger.Log("Local DLL version is up to date. Skipping update.");
            }
            return true;
        }

        private Process Gost;
        private Process Tor;
        private Process Warp;
        public string GostPort = "9129";
        public string MainProxyPort = "29812";
        public string mode = "Tor";
        public bool ServerRuning = false;
        public bool OpenAtStartup = false;
        public bool AutoUpdateProxified = false;

        public bool StopServer()
        {
            logger.Log("Stopping server");

            try
            {
                if (Tor != null)
                {
                    logger.Log("Closing Tor process");
                    Tor.CloseMainWindow(); // Try a graceful close first
                    if (!Tor.WaitForExit(5000)) // Wait for 5 seconds
                    {
                        logger.Log("Tor did not exit gracefully, killing process", LogType.WARNING);
                        Tor.Kill(); // Forcefully terminate if necessary
                    }
                    logger.Log("Tor process closed");
                    Tor.Dispose();
                    Tor = null;
                }
            }
            catch (Exception ex)
            {
                logger.Log($"Error stopping Tor: {ex.Message}", LogType.ERROR);
            }

            try
            {
                if (Gost != null)
                {
                    logger.Log("Closing Gost process");

                    Gost.CloseMainWindow();
                    if (!Gost.WaitForExit(5000))
                    {
                        logger.Log(
                            "Gost did not exit gracefully, killing process",
                            LogType.WARNING
                        );

                        Gost.Kill();
                    }
                    logger.Log("Gost process closed");
                    Gost.Dispose();
                    Gost = null;
                }
            }
            catch (Exception ex)
            {
                logger.Log($"Error stopping Gost: {ex.Message}", LogType.ERROR);
            }
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
                if (enable && key.GetValue(appName) != $"\"{exePath}\" /high")
                {
                    try
                    {
                        key.SetValue(appName, exePath);
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        throw new UnauthorizedAccessException(
                            "Failed to set registry key. Please run the application as an administrator."
                        );
                    }
                }
                else
                {
                    key.DeleteValue(appName, false);
                }
            }
        }

        public bool PrepareServer()
        {
            void PrepGost()
            {
                logger.Log("Preparing Gost");
                try
                {
                    if (Gost != null)
                    {
                        logger.Log("Killing existing Gost process");

                        Gost.Kill();
                    }
                }
                catch (Exception ex)
                {
                    logger.Log($"Error killing Gost process: {ex.Message}", LogType.ERROR);
                }
                Gost = new Process();
                Gost.StartInfo.FileName = Directory.GetCurrentDirectory() + "\\Data\\gost.exe";
                Gost.StartInfo.CreateNoWindow = true;
                Gost.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                Gost.StartInfo.Arguments =
                    $" -L=:{GostPort}  -F=socks5://localhost:{MainProxyPort}";
                logger.Log($"Starting Gost with arguments: {Gost.StartInfo.Arguments}");
                try
                {
                    Gost.Start();
                }
                catch (Exception ex)
                {
                    logger.Log($"Error starting Gost: {ex.Message}", LogType.ERROR);
                }
            }
            void PrepTor()
            {
                logger.Log("Preparing Tor");
                try
                {
                    if (Tor != null)
                    {
                        logger.Log("Killing existing Tor process");
                        Tor.Kill();
                    }
                }
                catch (Exception ex)
                {
                    logger.Log($"Error killing Tor process: {ex.Message}", LogType.ERROR);
                }

                File.WriteAllText(
                    Directory.GetCurrentDirectory() + "\\Data\\Tor\\torrc",
                    $"SocksPort {MainProxyPort}"
                );

                Tor = new Process();
                Tor.StartInfo.FileName = Directory.GetCurrentDirectory() + "\\Data\\Tor\\tor.exe";
                Tor.StartInfo.CreateNoWindow = true;
                Tor.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                Tor.StartInfo.Arguments =
                    $" -f \"{Directory.GetCurrentDirectory()}\\Data\\Tor\\torrc\"";
                Tor.StartInfo.CreateNoWindow = true;
                logger.Log($"Starting Tor with arguments: {Tor.StartInfo.Arguments}");

                try
                {
                    Tor.Start();
                }
                catch (Exception ex)
                {
                    logger.Log($"Error starting Tor: {ex.Message}", LogType.ERROR);
                }
            }
            void PrepWarp()
            {
                logger.Log("Preparing Warp");
                if (File.Exists(Directory.GetCurrentDirectory() + "\\WG\\config.conf"))
                {
                    try
                    {
                        if (Tor != null)
                        {
                            logger.Log("Killing existing Tor process");
                            Tor.Kill();
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Log($"Error killing Tor process: {ex.Message}", LogType.ERROR);
                    }

                    File.WriteAllText(
                        Directory.GetCurrentDirectory() + "\\Data\\Tor\\torrc",
                        $"SocksPort {MainProxyPort}"
                    );

                    Warp = new Process();
                    Warp.StartInfo.FileName =
                        Directory.GetCurrentDirectory() + "\\Data\\Tor\\tor.exe";
                    Warp.StartInfo.CreateNoWindow = true;
                    Warp.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    Warp.StartInfo.Arguments =
                        $" -f \"{Directory.GetCurrentDirectory()}\\Data\\Tor\\torrc\"";
                    Warp.StartInfo.CreateNoWindow = true;
                    logger.Log($"Starting Tor with arguments: {Warp.StartInfo.Arguments}");

                    try
                    {
                        Tor.Start();
                    }
                    catch (Exception ex)
                    {
                        logger.Log($"Error starting Tor: {ex.Message}", LogType.ERROR);
                    }
                }
                else
                {
                    try
                    {
                        if (Tor != null)
                        {
                            logger.Log("Killing existing Warp process");
                            Tor.Kill();
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Log($"Error killing Warp process: {ex.Message}", LogType.ERROR);
                    }

                    /*File.WriteAllText(
                       ,
                        $"SocksPort {MainProxyPort}"
                    );*/

                    Warp = new Process();
                    Warp.StartInfo.FileName =
                        Directory.GetCurrentDirectory() + "\\Data\\Tor\\tor.exe";
                    Warp.StartInfo.CreateNoWindow = true;
                    Warp.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    Warp.StartInfo.Arguments =
                        $" -f \"{Directory.GetCurrentDirectory()}\\Data\\Tor\\torrc\"";
                    Warp.StartInfo.CreateNoWindow = true;
                    logger.Log($"Starting Tor with arguments: {Warp.StartInfo.Arguments}");

                    try
                    {
                        Tor.Start();
                    }
                    catch (Exception ex)
                    {
                        logger.Log($"Error starting Tor: {ex.Message}", LogType.ERROR);
                    }
                }
            }

            logger.Log("Setting up Tor and Gost");
            PrepTor();
            PrepGost();
            logger.Log("Tor and Gost setup complete");
            ServerRuning = true;
            return true;
        }

        public async Task<bool> StopServerAsync()
        {
            logger.Log("Stopping server");
            bool success = true;
            List<Task> Waitme = new List<Task>();
            try
            {
                if (Tor != null)
                {
                    Task t = new Task(async () =>
                    {
                        await StopProcessAsync(Tor, "Tor");
                    });
                    t.Start();
                    Waitme.Add(t);
                }
            }
            catch (Exception ex)
            {
                logger.Log($"Error stopping Tor: {ex.Message}", LogType.ERROR);
                success = false;
            }

            try
            {
                if (Gost != null)
                {
                    Task t = new Task(async () =>
                    {
                        await StopProcessAsync(Gost, "Gost");
                    });
                    t.Start();
                    Waitme.Add(t);
                }
            }
            catch (Exception ex)
            {
                logger.Log($"Error stopping Gost: {ex.Message}", LogType.ERROR);
                success = false;
            }
            Task.WaitAll(Waitme.ToArray());
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
            //logger.Log("Loading shortcuts");

            int id = 0;
            listBox1.Items.Clear();
            Shortcuts = new List<Shortcut>();
            string roamingPath = Environment.GetFolderPath(
                Environment.SpecialFolder.ApplicationData
            );
            string discordPath = Path.Combine(
                roamingPath,
                @"Microsoft\Windows\Start Menu\Programs\Discord Inc"
            );
            if (Directory.Exists(discordPath))
            {
                //logger.Log($"Found Discord shortcuts directory: {discordPath}");
                foreach (string file in Directory.GetFiles(discordPath, "*.lnk"))
                {
                    // logger.Log($"Processing shortcut: {file}");
                    try
                    {
                        listBox1.Items.Add(
                            $"{file.Replace(".lnk", "").Replace(discordPath, "").Replace("\\", "")}  ({id})"
                        );
                        IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();

                        IWshRuntimeLibrary.IWshShortcut link = (IWshRuntimeLibrary.IWshShortcut)
                            shell.CreateShortcut(file);

                        Shortcut cut = new Shortcut();
                        cut.TargetPath = link.TargetPath;
                        cut.Arguments = link.Arguments;
                        cut.WorkingDirectory = link.WorkingDirectory;
                        cut.LBName =
                            $"{file.Replace(".lnk", "").Replace(discordPath, "").Replace("\\", "")}  ({id})";
                        cut.ShortcutPath = file;
                        Shortcuts.Add(cut);
                        id++;
                        //logger.Log($"Shortcut '{cut.LBName}' added");
                    }
                    catch (Exception ex)
                    {
                        /*logger.Log(
                            $"Error processing shortcut '{file}': {ex.Message}",
                            LogType.ERROR
                        );**/
                    }
                }
            }
            else
            {
                /*  logger.Log(
                      $"Discord shortcuts directory not found: {discordPath}",
                      LogType.WARNING
                  );*/
            }
            //logger.Log("Shortcuts loaded");
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
                IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();

                IWshRuntimeLibrary.IWshShortcut link = (IWshRuntimeLibrary.IWshShortcut)
                    shell.CreateShortcut(Shopath);

                string DiscordName = "";
                string[] parts = DcPath.Split('\\');

                int localIndex = Array.IndexOf(parts, "Local");

                if (localIndex != -1 && localIndex + 1 < parts.Length)
                {
                    string result = parts[localIndex + 1];
                    switch (result.ToLower())
                    {
                        case "discord":
                            DiscordName = "Discord";
                            break;
                        case "discordptb":
                            DiscordName = "DiscordPTB";

                            break;
                        case "discordcanary":
                            DiscordName = "DiscordCanary";

                            break;
                        case "discorddevelopment":
                            DiscordName = "DiscordDevelopment";

                            break;
                    }
                }
                else
                {
                    logger.Log(
                        "Discord directory not found within working directory",
                        LogType.ERROR
                    );
                }
                link.Arguments = $"--processStart {DiscordName}.exe {ExtraArguments}";
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
            string DiscordName = "";
            string[] parts = dir.Split('\\');

            int localIndex = Array.IndexOf(parts, "Local");

            if (localIndex != -1 && localIndex + 1 < parts.Length)
            {
                string result = parts[localIndex + 1];
                switch (result.ToLower())
                {
                    case "discord":
                        DiscordName = "Discord";
                        break;
                    case "discordptb":
                        DiscordName = "DiscordPTB";

                        break;
                    case "discordcanary":
                        DiscordName = "DiscordCanary";

                        break;
                    case "discorddevelopment":
                        DiscordName = "DiscordDevelopment";

                        break;
                }
            }
            return DiscordName;
        }

        private void CheckProxifiedDiscords(bool Override = false)
        {
            if (AutoUpdateProxified || Override)
            {
                List<Shortcut> shortcuts = Shortcuts
                    .Where(
                        (x) =>
                        {
                            return x.Arguments.Contains("proxy");
                        }
                    )
                    .ToList();
                foreach (Shortcut shortcut in shortcuts)
                {
                    if (
                        shortcut != null
                        && (
                            (
                                new Regex(
                                    $"--a=--proxy-server=http://127.0.0.1:{GostPort}$"
                                ).IsMatch(shortcut.Arguments.Split(' ').Last().ToLower()) == false
                            )
                            && (
                                new Regex($"http://127.0.0.1:{GostPort}$").IsMatch(
                                    shortcut.Arguments.ToLower()
                                ) == false
                            )
                            && (
                                new Regex($"{GostPort}$").IsMatch(shortcut.Arguments.ToLower())
                                == false
                            )
                        )
                    )
                    {
                        UpdateShortcut(
                            shortcut.ShortcutPath,
                            shortcut.WorkingDirectory,
                            $"--a=--proxy-server=http://127.0.0.1:{GostPort}"
                        );
                    }
                }
            }
        }

        string currentver = "01A";

        public bool CheckVersion()
        {
            bool result = false;
            try
            {
                string s = new WebClient().DownloadString(
                    "https://raw.githubusercontent.com/s0rp/DiscordProxifier/refs/heads/main/ver.verme"
                );
                Versionlbl.Text = $"Version : {currentver.Trim()}";
                this.Text = $"Discord Proxifier (V:{currentver.Trim()})";
                if (s != null && s.Trim() == currentver.Trim())
                {
                    result = true;
                }
                else
                {
                    Versionlbl.Text = $"Version : {currentver.Trim()} (Newest : {s.Trim()})";
                }
            }
            catch (Exception ex)
            {
                logger.Log(ex.Message, LogType.ERROR);
                result = false;
            }
            return result;
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            Versionlbl.Text = "Version : " + currentver.Trim();
            this.Text = $"Discord Proxifier (V:{currentver.Trim()})";
            bool isuptodate = CheckVersion();
            if (!isuptodate)
            {
                MessageBox.Show("New Version Available!");
                Process.Start("https://github.com/s0rp/DiscordProxifier/");
            }
            if (File.Exists(Directory.GetCurrentDirectory() + "\\Config.json"))
                configuration = JsonConvert.DeserializeObject<Configuration>(
                    File.ReadAllText(Directory.GetCurrentDirectory() + "\\Config.json")
                );
            LoadConfig(configuration);
            if (OpenAtStartup)
            {
                foreach (Process p in Process.GetProcesses())
                {
                    if (
                        p.ProcessName.ToLower() == "discord"
                        || p.ProcessName.ToLower() == "discordptb"
                        || p.ProcessName.ToLower() == "discordcanary"
                        || p.ProcessName.ToLower() == "discorddevelopment"
                    )
                    {
                        ReStart.Add(p.ProcessName);
                        p.Kill();
                    }
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
                return ":p";
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

            if (listBox1.SelectedItem != null)
            {
                string selectedItem = listBox1.SelectedItem.ToString();

                Shortcut shortcut = Shortcuts
                    .Where(
                        (x) =>
                        {
                            return LPT(x.LBName) == LPT(selectedItem);
                        }
                    )
                    .FirstOrDefault();
                if (shortcut != null)
                {
                    if (shortcut != null)
                    {
                        string DiscordName = "";
                        string[] parts = shortcut.WorkingDirectory.Split('\\');

                        int localIndex = Array.IndexOf(parts, "Local");

                        if (localIndex != -1 && localIndex + 1 < parts.Length)
                        {
                            string result = parts[localIndex + 1];
                            switch (result.ToLower())
                            {
                                case "discord":
                                    DiscordName = "Discord";
                                    break;
                                case "discordptb":
                                    DiscordName = "DiscordPTB";

                                    break;
                                case "discordcanary":
                                    DiscordName = "DiscordCanary";

                                    break;
                                case "discorddevelopment":
                                    DiscordName = "DiscordDevelopment";

                                    break;
                            }

                            foreach (Process p in Process.GetProcesses())
                            {
                                if (p.ProcessName.ToLower() == DiscordName.ToLower())
                                {
                                    p.Kill();
                                }
                            }
                            System.Threading.Thread.Sleep(2000);
                            if (File.Exists(shortcut.WorkingDirectory + "\\version.dll"))
                                File.Delete(shortcut.WorkingDirectory + "\\version.dll");
                            if (Directory.Exists(shortcut.WorkingDirectory))
                                File.Copy(
                                    GetDll(shortcut.WorkingDirectory + "\\" + DiscordName + ".exe"),
                                    shortcut.WorkingDirectory + "\\version.dll",
                                    true
                                );
                            return true;
                        }
                        else
                        {
                            logger.Log(
                                "Discord directory not found within working directory.",
                                LogType.ERROR
                            );
                        }
                    }
                }
            }
            return false;
        }

        private bool RemoveVDLL()
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

            if (listBox1.SelectedItem != null)
            {
                string selectedItem = listBox1.SelectedItem.ToString();

                Shortcut shortcut = Shortcuts
                    .Where(
                        (x) =>
                        {
                            return LPT(x.LBName) == LPT(selectedItem);
                        }
                    )
                    .FirstOrDefault();
                if (shortcut != null)
                {
                    if (shortcut != null)
                    {
                        string DiscordName = "";
                        string[] parts = shortcut.WorkingDirectory.Split('\\');

                        int localIndex = Array.IndexOf(parts, "Local");

                        if (localIndex != -1 && localIndex + 1 < parts.Length)
                        {
                            string result = parts[localIndex + 1];
                            switch (result.ToLower())
                            {
                                case "discord":
                                    DiscordName = "Discord";
                                    break;
                                case "discordptb":
                                    DiscordName = "DiscordPTB";

                                    break;
                                case "discordcanary":
                                    DiscordName = "DiscordCanary";

                                    break;
                                case "discorddevelopment":
                                    DiscordName = "DiscordDevelopment";

                                    break;
                            }

                            foreach (Process p in Process.GetProcesses())
                            {
                                if (p.ProcessName.ToLower() == DiscordName.ToLower())
                                {
                                    p.Kill();
                                }
                            }
                            System.Threading.Thread.Sleep(2000);
                            if (File.Exists(shortcut.WorkingDirectory + "\\version.dll"))
                                File.Delete(shortcut.WorkingDirectory + "\\version.dll");

                            return true;
                        }
                        else
                        {
                            logger.Log(
                                "Discord directory not found within working directory.",
                                LogType.ERROR
                            );
                        }
                    }
                }
            }
            return false;
        }

        private void StartWProxy(string or = null)
        {
            CopyVDLL();
            logger.Log("Start with proxy button clicked");
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

            if (listBox1.SelectedItem != null || or != null)
            {
                string selectedItem = or == null ? listBox1.SelectedItem.ToString() : or;

                Shortcut shortcut = Shortcuts
                    .Where(
                        (x) =>
                        {
                            return LPT(x.LBName) == LPT(selectedItem);
                        }
                    )
                    .FirstOrDefault();

                if (shortcut != null)
                {
                    string DiscordName = "";
                    string[] parts = shortcut.WorkingDirectory.Split('\\');

                    int localIndex = Array.IndexOf(parts, "Local");

                    if (localIndex != -1 && localIndex + 1 < parts.Length)
                    {
                        string result = parts[localIndex + 1];
                        switch (result.ToLower())
                        {
                            case "discord":
                                DiscordName = "Discord";
                                break;
                            case "discordptb":
                                DiscordName = "DiscordPTB";

                                break;
                            case "discordcanary":
                                DiscordName = "DiscordCanary";

                                break;
                            case "discorddevelopment":
                                DiscordName = "DiscordDevelopment";

                                break;
                        }
                    }
                    else
                    {
                        logger.Log(
                            "Discord directory not found within working directory.",
                            LogType.ERROR
                        );
                    }
                    var process = new Process();
                    process.StartInfo.FileName = shortcut.TargetPath;
                    process.StartInfo.Arguments =
                        $"--processStart {DiscordName}.exe --a=--proxy-server=http://127.0.0.1:{GostPort}";
                    process.StartInfo.WorkingDirectory = shortcut.WorkingDirectory;
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
                else
                {
                    logger.Log(
                        $"No shortcut found matching selected item {selectedItem}",
                        LogType.WARNING
                    );
                }
            }
            else
            {
                logger.Log("No client selected", LogType.WARNING);
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
            if (listBox1.SelectedItem == null)
            {
                logger.Log("No client selected", LogType.WARNING);
                return;
            }
            Shortcut shortcut = Shortcuts
                .Where(
                    (x) =>
                    {
                        return LPT(x.LBName) == LPT(listBox1.SelectedItem.ToString());
                    }
                )
                .First();

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
            try
            {
                listBox1.SetSelected(val, true);
            }
            catch (Exception ex) { }
        }

        private void ServerT_Click(object sender, EventArgs e)
        {
            logger.Log("Server toggle button clicked");

            if (ServerRuning == false)
            {
                logger.Log("Starting server");

                ServerStatus.Text = "Server Status : Up";
                TRAY_SERVER_STATUS_C.Text = "Server Status : Up";
                StartServer.Text = "Stop Server";
                TRAY_SERVER_TOGGLE_C.Text = "Stop Server";
                PrepareServer();
            }
            else
            {
                logger.Log("Stopping server");

                ServerStatus.Text = "Server Status : Down";
                TRAY_SERVER_STATUS_C.Text = "Server Status : Down";
                TRAY_SERVER_TOGGLE_C.Text = "Start Server";
                StartServer.Text = "Start Server";
                StopServer();
            }
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
            List<Shortcut> old = Shortcuts;
            int val = listBox1.SelectedIndex;
            LoadShortcuts();
            try
            {
                listBox1.SetSelected(val, true);
            }
            catch (Exception ex) { }
            UpdateChecker(old, Shortcuts);
            try
            {
                bool x1 = false;
                bool x2 = false;
                foreach (Process p in Process.GetProcesses())
                {
                    if (p.ProcessName.ToLower() == "tor")
                    {
                        x1 = true;
                    }
                    if (p.ProcessName.ToLower() == "gost")
                    {
                        x2 = true;
                    }
                }
                if ((x1 && x2) == false && ServerRuning == true)
                {
                    string xy = "";
                    if (!x1)
                        xy += "P";
                    if (!x2)
                        xy += "G";
                    ServerStatus.Text = $"Server Status : E1_{xy}";
                    TRAY_SERVER_STATUS_C.Text = $"Server Status : E1_{xy}";
                }
                else
                {
                    if (ServerRuning == true)
                    {
                        ServerStatus.Text = "Server Status : Up";
                        TRAY_SERVER_STATUS_C.Text = "Server Status : Up";
                        StartServer.Text = "Stop Server";
                        TRAY_SERVER_TOGGLE_C.Text = "Stop Server";
                    }
                    else
                    {
                        ServerStatus.Text = "Server Status : Down";
                        TRAY_SERVER_STATUS_C.Text = "Server Status : Down";
                        StartServer.Text = "Start Server";
                        TRAY_SERVER_TOGGLE_C.Text = "Start Server";
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Log(ex.Message, LogType.ERROR);
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
            Configuration old = configuration;
            Settings settingsForm = new Settings();
            settingsForm.ShowDialog();
            Configuration conf = settingsForm.Configuration;
            configuration = conf;
            LoadConfig(configuration);
            CheckProxifiedDiscords();
            bool am =
                Process
                    .GetProcesses()
                    .Where(
                        (p) =>
                        {
                            if (
                                p.ProcessName.ToLower() == "discord"
                                || p.ProcessName.ToLower() == "discordptb"
                                || p.ProcessName.ToLower() == "discordcanary"
                                || p.ProcessName.ToLower() == "discorddevelopment"
                            )
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    )
                    .ToArray()
                    .Length > 0;
            if ((configuration.GostPort != old.GostPort) && am)
            {
                DialogResult dres = MessageBox.Show(
                    "Restart Discord(s) to use newer Gost port?",
                    "DiscordUnblocker",
                    MessageBoxButtons.YesNo
                );
                if (dres == DialogResult.Yes)
                {
                    foreach (Process p in Process.GetProcesses())
                    {
                        if (
                            p.ProcessName.ToLower() == "discord"
                            || p.ProcessName.ToLower() == "discordptb"
                            || p.ProcessName.ToLower() == "discordcanary"
                            || p.ProcessName.ToLower() == "discorddevelopment"
                        )
                        {
                            ReStart.Add(p.ProcessName);
                            p.Kill();
                        }
                    }

                    ReStartList(ReStart);
                    ReStart.Clear();
                }
            }

            if (ServerRuning)
            {
                ServerStatus.Text = "Server Status : Down";
                TRAY_SERVER_STATUS_C.Text = "Server Status : Down";
                ServerRuning = false;
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
