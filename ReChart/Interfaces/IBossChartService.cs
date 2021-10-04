using MoMMusicAnalysis;
using System.Collections.Generic;
using ReChart.ViewModels.ReChart;
using MoMMusicAnalysis.Song;
using MoMMusicAnalysis.Song.BossBattle;

namespace ReChart.Interfaces
{
    public interface IBossChartService
    {
        public void Initialize(MusicFile musicFile);

        public void Deinitialize();

        public MusicFile GetMusicFile();

        public Dictionary<string, List<BossNoteDisplay>> GetDisplayNotes(Difficulty difficulty);

        public List<BossNote> GetBossNotes(Difficulty difficulty);

        public void AddBossNote(Difficulty difficulty, int time, BossLane lane, string modelType);

        public BossNote LoadBossNote(Difficulty difficulty, int id);

        public void SaveBossNote(Difficulty difficulty, ref BossNote bossNote, BossNote bossNoteCopy);

        public void RemoveBossNote(Difficulty difficulty, int id);

        public void AddPerformerNote(Difficulty difficulty, int time, BossLane lane, PerformerType modelType);

        public PerformerNote<BossLane> LoadBossPerformerNote(Difficulty difficulty, int id);

        public void SavePerformerNote(ref PerformerNote<BossLane> performerNote, PerformerNote<BossLane> performerNoteCopy);

        public void RemovePerformerNote(Difficulty difficulty, int id);

        public List<TimeShift<BossLane>> GetTempos(Difficulty difficulty);

        public void AddBossTempo(Difficulty difficulty, int time, int speed);

        public TimeShift<BossLane> LoadBossTempo(Difficulty difficulty, int id);

        public void SaveBossTempo(ref TimeShift<BossLane> tempo, TimeShift<BossLane> tempoCopy);

        public void RemoveBossTempo(Difficulty difficulty, int id);

        public int GetNoteIndex(Difficulty difficulty, BossNote bossNote);

        public List<BossDarkZone> GetDarkZones(Difficulty difficulty);

        public void AddBossDarkZone(Difficulty difficulty, int time, int endTime, int endAttackTime);

        public BossDarkZone LoadBossDarkZone(Difficulty difficulty, int id);

        public void RemoveBossDarkZone(Difficulty difficulty, int id);

        public void SaveBossDarkZone(ref BossDarkZone darkZone, BossDarkZone darkZoneCopy);

        public List<byte> RecompileBossBattleSongs();
    }
}