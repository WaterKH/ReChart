using ReChart.Enums;
using System.IO;
using System.Text.Json;

namespace ReChart.Logic
{
    public static class Settings
    {
        //public static int ZoomVariable { get; set; } = 10;
        public static Beat CurrentBeat { get; set; } = Beat.Quarter;

        public static Configurations Configurations { get; set; }

        public static void LoadSettings()
        {
            using var reader = new StreamReader(Path.GetFullPath("wwwroot") + "/configurations/configurations.json");
            var fileContents = reader.ReadToEnd();

            Configurations = JsonSerializer.Deserialize<Configurations>(fileContents);
        }

        public static void SaveSettings()
        {
            var contentsToWriteToFile = JsonSerializer.Serialize(Configurations);

            using var writer = new StreamWriter(Path.GetFullPath("wwwroot") + "/configurations/configurations.json", false);
            writer.Write(contentsToWriteToFile);
        }
    }
}