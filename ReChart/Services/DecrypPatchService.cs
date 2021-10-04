using MoMMusicAnalysis;
using ReChart.Enums;
using ReChart.Logic;
using ReChart.ViewModels.ReChart;
using Reloaded.Injector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;

namespace ReChart.Services
{
    public class DecryptPatchService
    {
        public string PatchGameAssembly(int position, List<byte> replacementBytes)
        {
            try
            {
                var fileName = Settings.Configurations.MelodyOfMemoryRootFolder + "\\GameAssembly.dll";
                var originalFileName = Settings.Configurations.MelodyOfMemoryRootFolder + "\\GameAssembly_original.dll";

                if (!File.Exists(originalFileName))
                    File.Copy(fileName, originalFileName);

                using var gameAssembly = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite);

                gameAssembly.Position = position;

                gameAssembly.Write(replacementBytes.ToArray(), 0, replacementBytes.Count);

                return "GameAssembly.dll has been patched to support decrypted AssetBundles!";
            }
            catch (FileNotFoundException)
            {
                return "GameAssembly.dll Not Found. Check the Melody of Memory Root Folder.";
            }
            catch (Exception)
            {
                return "Unknown Exception Occurred. Report this on GitHub or Discord.";
            }
        }

        public string DecryptAssetBundles()
        {
            try
            {
                var pathToAssetBundles = $"{Settings.Configurations.MelodyOfMemoryRootFolder}\\KINGDOM HEARTS Melody of Memory_Data\\StreamingAssets\\AssetBundles";

                using var reader = new StreamReader($"{Path.GetFullPath("wwwroot")}\\configurations\\NoPath_AssetBundleNames.txt");
                //using var writer = new StreamWriter($"{Settings.Configurations.MelodyOfMemoryRootFolder}\\AssetBundleNames.txt", false);
                using var writer2 = new StreamWriter($"{Path.GetFullPath("wwwroot")}\\AssetBundleNames.txt", false);

                var line = string.Empty;
                while ((line = reader.ReadLine()) != null)
                {
                    //writer.WriteLine($"{pathToAssetBundles}\\{line}");
                    writer2.WriteLine($"{pathToAssetBundles}\\{line}");
                }

                //writer.Close();
                writer2.Close();

                //writer.Dispose();
                writer2.Dispose();

                var process = Process.GetProcessesByName("KINGDOM HEARTS Melody of Memory")[0];
                var injector = new Injector(process);

                injector.Inject($"{Path.GetFullPath("wwwroot")}\\DecryptAssetBundles.dll");

                injector.Dispose();

                process.WaitForExit();

                // The files are saved as hex text, so convert them to hex binary files
                // Also, replaces the original files with the decrypted ones and makes copies of the originals
                Utilities.ConvertAndCopyFiles();

                return "Decryption Complete! You should have full access to the AssetBundles now!";
            }
            catch (IndexOutOfRangeException)
            {
                return "Please start Melody of Memory to Decrypt the AssetBundles.";
            }
            catch (Exception ex)
            {
                return "Unknown Exception Occurred. Report this on GitHub or Discord.";
            }
        }
    }
}