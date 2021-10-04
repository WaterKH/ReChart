using MoMMusicAnalysis;
using System.Collections.Generic;
using ReChart.ViewModels.ReChart;
using MoMMusicAnalysis.Song;
using MoMMusicAnalysis.Song.MemoryDive;
using System.IO;
using System.Threading.Tasks;

namespace ReChart.Interfaces
{
    public interface IMemoryChartService
    {
        public void Initialize(MusicFile musicFile);

        public void Deinitialize();

        public MusicFile GetMusicFile();

        public Dictionary<string, List<MemoryNoteDisplay>> GetDisplayNotes(Difficulty difficulty);

        public List<MemoryNote> GetMemoryNotes(Difficulty difficulty);

        public void AddMemoryNote(Difficulty difficulty, int time, MemoryLane lane, string modelType);

        public MemoryNote LoadMemoryNote(Difficulty difficulty, int id);

        public void SaveMemoryNote(Difficulty difficulty, ref MemoryNote memoryNote, MemoryNote memoryNoteCopy);

        public void RemoveMemoryNote(Difficulty difficulty, int id);

        public void AddPerformerNote(Difficulty difficulty, int time, MemoryLane lane, PerformerType modelType);

        public PerformerNote<MemoryLane> LoadMemoryPerformerNote(Difficulty difficulty, int id);

        public void SavePerformerNote(ref PerformerNote<MemoryLane> performerNote, PerformerNote<MemoryLane> performerNoteCopy);

        public void RemovePerformerNote(Difficulty difficulty, int id);

        public List<TimeShift<MemoryLane>> GetTempos(Difficulty difficulty);

        public void AddMemoryTempo(Difficulty difficulty, int time, int speed);

        public TimeShift<MemoryLane> LoadMemoryTempo(Difficulty difficulty, int id);

        public void SaveMemoryTempo(ref TimeShift<MemoryLane> tempo, TimeShift<MemoryLane> tempoCopy);

        public void RemoveMemoryTempo(Difficulty difficulty, int id);

        public int GetNoteIndex(Difficulty difficulty, MemoryNote memoryNote);

        public List<byte> RecompileMemoryDiveSongs();

        public Task<List<byte>> ReplaceMemoryDiveMovie(string fileName, Stream data);
    }
}