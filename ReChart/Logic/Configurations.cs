using System.Collections.Generic;

namespace ReChart.Logic
{
    public class Configurations
    {
        public string MelodyOfMemoryRootFolder { get; set; }
        public KeyConfigs KeyConfigs { get; set; }
        public Songs Songs { get; set; }
    }

    public class KeyConfigs
    {
        public Dictionary<string, string> Beats { get; set; }
        public Chart FieldBattle { get; set; }
        public Chart MemoryDive { get; set; }
        public Chart BossBattle { get; set; }
    }

    public class Chart
    {
        public Dictionary<string, string> Notes { get; set; }
        public Dictionary<string, string> Displays { get; set; }
    }

    public class Songs
    {
        public Dictionary<string, string> FieldBattle { get; set; }
        public Dictionary<string, string> MemoryDive { get; set; }
        public Dictionary<string, string> BossBattle { get; set; }
        public Dictionary<string, string> CoOp { get; set; }

    }
}