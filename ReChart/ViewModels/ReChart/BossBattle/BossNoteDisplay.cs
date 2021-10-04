using MoMMusicAnalysis.Song.BossBattle;

namespace ReChart.ViewModels.ReChart
{
    public class BossNoteDisplay
    {
        public int NoteIndex { get; set; }
        public int HitTime { get; set; }
        public BossLane Lane { get; set; }
        public string Image { get; set; }
    }
}