using MoMMusicAnalysis.Song.MemoryDive;

namespace ReChart.ViewModels.ReChart
{
    public class MemoryNoteDisplay
    {
        public int NoteIndex { get; set; }
        public int HitTime { get; set; }
        public MemoryLane Lane { get; set; }
        public string Image { get; set; }
    }
}