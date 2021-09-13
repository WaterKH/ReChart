// Code from https://github.dev/ipax77/ElectronUpdateTest
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ElectronNET.API;
using ElectronNET.API.Entities;

namespace ReChart.Services
{
    public class ElectronService : IDisposable
    {
        public static string VERSION { get; } = "0.1.1";
        public string UpdateInfo { get; set; } = VERSION;
        public bool Resized { get; set; } = false;
        Task<UpdateCheckResult> resultTask;

        public ElectronService()
        {
            Electron.App.SetAppUserModelId("com.ReChart.app");
        }

        public void Dispose()
        {
        }

        public async Task CheckUpdate()
        {
            Electron.Notification.Show(new NotificationOptions("Hello", "World"));

            if (HybridSupport.IsElectronActive)
            {
                Console.WriteLine("HybridSupport Electron is active.");
                Electron.AutoUpdater.OnError += (message) => Electron.Dialog.ShowErrorBox("Error", message);

                Electron.AutoUpdater.OnDownloadProgress += (info) =>
                {
                    var message1 = "Download speed: " + info.BytesPerSecond + "\n<br/>";
                    var message2 = "Downloaded " + info.Percent + "%" + "\n<br/>";
                    var message3 = $"({info.Transferred}/{info.Total})" + "\n<br/>";
                    var message4 = "Progress: " + info.Progress + "\n<br/>";
                    var information = message1 + message2 + message3 + message4;
                    Console.WriteLine(information);
                };

                var currentVersion = new Version(await Electron.App.GetVersionAsync());

                Electron.AutoUpdater.AutoDownload = false;
                var updateResult = await Electron.AutoUpdater.CheckForUpdatesAsync();

                // var updateCheckResult = await Electron.AutoUpdater.CheckForUpdatesAndNotifyAsync();

                var availableVersion = new Version(updateResult.UpdateInfo.Version);
                string information = $"Current version: {currentVersion} - available version: {availableVersion}";
                Console.WriteLine(information);
                Electron.Notification.Show(new NotificationOptions("Update", information));

                if (availableVersion > currentVersion)
                {
                    await Update();
                }

            }
            else
            {
                Console.WriteLine("HybridSupport Electron is not active.");
            }
        }

        public async Task Update()
        {
            Console.WriteLine("Restart and Install new update");

            Electron.AutoUpdater.OnUpdateDownloaded += (info) =>
            {
                Electron.AutoUpdater.QuitAndInstall(true, true);
            };

            var result = await Electron.AutoUpdater.DownloadUpdateAsync();

            Console.WriteLine(result);
        }
    }
}