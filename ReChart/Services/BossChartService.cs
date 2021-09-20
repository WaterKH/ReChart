using MoMMusicAnalysis;
using ReChart.ViewModels.ReChart;
using ReChart.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using ReChart.Interfaces;

namespace ReChart.Services
{
    public class BossChartService : IBossChartService
    {
        public MusicFile MusicFile { get; set; }

        public void Initialize(MusicFile musicFile)
        {
            this.MusicFile = musicFile;
        }

        public void Deinitialize()
        {
            this.MusicFile = null;
        }

        public MusicFile GetMusicFile()
        {
            return this.MusicFile;
        }

        public Dictionary<string, List<BossNoteDisplay>> GetDisplayNotes(Difficulty difficulty)
        {
            var displayNotes = new Dictionary<string, List<BossNoteDisplay>>
            {
                { "BossNote", new List<BossNoteDisplay>() },
                { "PerformerNote", new List<BossNoteDisplay>() },
                { "Tempo", new List<BossNoteDisplay>() },
                { "DarkZone", new List<BossNoteDisplay>() }
            };
            
            var bossBattleSong = (BossBattleSong)this.MusicFile.SongPositions.FirstOrDefault(x => x.Value.Difficulty == difficulty).Value;

            displayNotes["BossNote"] = bossBattleSong.Notes.OrderBy(x => x.HitTime)
                .Select(x => new BossNoteDisplay { NoteIndex = bossBattleSong.Notes.IndexOf(x), HitTime = x.HitTime, Lane = x.Lane, Image = Utilities.GetImage(x.BossNoteType) }).ToList();

            displayNotes["PerformerNote"] = bossBattleSong.PerformerNotes.OrderBy(x => x.HitTime)
                .Select(x => new BossNoteDisplay { NoteIndex = bossBattleSong.PerformerNotes.IndexOf(x), HitTime = x.HitTime, Lane = x.Lane, Image = Utilities.GetImage(x.PerformerType) }).ToList();

            displayNotes["Tempo"] = bossBattleSong.TimeShifts.OrderBy(x => x.HitTime)
                .Select(x => new BossNoteDisplay { NoteIndex = bossBattleSong.TimeShifts.IndexOf(x), HitTime = x.HitTime, Lane = x.Lane, Image = Utilities.GetImage("BossBattle") }).ToList();

            displayNotes["DarkZone"] = bossBattleSong.DarkZones.OrderBy(x => x.HitTime)
                .Select(x => new BossNoteDisplay { NoteIndex = bossBattleSong.DarkZones.IndexOf(x), HitTime = x.HitTime, Lane = x.Lane, Image = Utilities.GetImage("DarkZone") }).ToList();

            return displayNotes;
        }

        public List<BossNote> GetBossNotes(Difficulty difficulty)
        {
            var bossBattleSong = (BossBattleSong)this.MusicFile.SongPositions.FirstOrDefault(x => x.Value.Difficulty == difficulty).Value;

            return bossBattleSong.Notes;
        }

        public int GetNoteIndex(Difficulty difficulty, BossNote bossNote)
        {
            var bossBattleSong = (BossBattleSong)this.MusicFile.SongPositions.FirstOrDefault(x => x.Value.Difficulty == difficulty).Value;

            return bossBattleSong.Notes.ToList().IndexOf(bossNote);
        }

        public void AddBossNote(Difficulty difficulty, int time, BossLane lane, string modelType)
        {
            var bossNote = new BossNote
            {
                HitTime = time,
                Lane = lane,
                BossNoteType = Enum.Parse<BossNoteType>(modelType),
            };

            var bossBattleSong = (BossBattleSong)this.MusicFile.SongPositions.FirstOrDefault(x => x.Value.Difficulty == difficulty).Value;
            bossBattleSong.Notes.Add(bossNote);
        }

        public BossNote LoadBossNote(Difficulty difficulty, int id)
        {
            var bossBattleSong = (BossBattleSong)this.MusicFile.SongPositions.FirstOrDefault(x => x.Value.Difficulty == difficulty).Value;

            return bossBattleSong.Notes.ToList()[id];
        }

        public void SaveBossNote(Difficulty difficulty, ref BossNote bossNote, BossNote bossNoteCopy)
        {
            var bossBattleSong = (BossBattleSong)this.MusicFile.SongPositions.FirstOrDefault(x => x.Value.Difficulty == difficulty).Value;

            bossNote.AerialFlag = bossNoteCopy.AerialFlag;
            bossNote.BossNoteType = bossNoteCopy.BossNoteType;
            bossNote.EndHoldNote = bossNoteCopy.EndHoldNote;
            bossNote.EndHoldNoteIndex = bossNoteCopy.EndHoldNoteIndex;
            bossNote.HitTime = bossNoteCopy.HitTime;
            bossNote.Lane = bossNoteCopy.Lane;
            bossNote.NoteType = bossNoteCopy.NoteType;
            bossNote.StartHoldNote = bossNoteCopy.StartHoldNote;
            bossNote.StartHoldNoteIndex = bossNoteCopy.StartHoldNoteIndex;
            bossNote.SwipeDirection = bossNoteCopy.SwipeDirection;
            bossNote.Unk1 = bossNoteCopy.Unk1;
            bossNote.Unk2 = bossNoteCopy.Unk2;
            bossNote.Unk3 = bossNoteCopy.Unk3;
            bossNote.Unk4 = bossNoteCopy.Unk4;
            bossNote.Unk5 = bossNoteCopy.Unk5;
            bossNote.Unk6 = bossNoteCopy.Unk6;
            bossNote.Unk7 = bossNoteCopy.Unk7;
            bossNote.Unk8 = bossNoteCopy.Unk8;
            bossNote.UnkFF = bossNoteCopy.UnkFF;

            if (bossNoteCopy.EndHoldNoteIndex != -1)
            {
                bossNote.EndHoldNote = bossBattleSong.Notes.ElementAt(bossNoteCopy.EndHoldNoteIndex);

                bossNote.EndHoldNote.StartHoldNote = bossNote;
                bossNote.EndHoldNote.StartHoldNoteIndex = bossBattleSong.Notes.IndexOf(bossNote);
            }

            if (bossNoteCopy.StartHoldNoteIndex != -1)
            {
                bossNote.StartHoldNote = bossBattleSong.Notes.ElementAt(bossNoteCopy.StartHoldNoteIndex);

                bossNote.StartHoldNote.EndHoldNote = bossNote;
                bossNote.StartHoldNote.EndHoldNoteIndex = bossBattleSong.Notes.IndexOf(bossNote);
            }
        }

        public void RemoveBossNote(Difficulty difficulty, int id)
        {
            var bossBattleSong = (BossBattleSong)this.MusicFile.SongPositions.FirstOrDefault(x => x.Value.Difficulty == difficulty).Value;

            bossBattleSong.Notes.RemoveAt(id);
        }

        public void AddPerformerNote(Difficulty difficulty, int time, BossLane lane, PerformerType modelType)
        {
            var performerNote = new PerformerNote<BossLane>
            {
                HitTime = time,
                Lane = lane,
                PerformerType = modelType,
                DuplicateType = modelType
            };

            var bossBattleSong = (BossBattleSong)this.MusicFile.SongPositions.FirstOrDefault(x => x.Value.Difficulty == difficulty).Value;
            bossBattleSong.PerformerNotes.Add(performerNote);
        }

        public PerformerNote<BossLane> LoadBossPerformerNote(Difficulty difficulty, int id)
        {
            var bossBattleSong = (BossBattleSong)this.MusicFile.SongPositions.FirstOrDefault(x => x.Value.Difficulty == difficulty).Value;

            return bossBattleSong.PerformerNotes.ToList()[id];
        }

        public void SavePerformerNote(ref PerformerNote<BossLane> performerNote, PerformerNote<BossLane> performerNoteCopy)
        {
            performerNote.DuplicateType = performerNoteCopy.DuplicateType;
            performerNote.HitTime = performerNoteCopy.HitTime;
            performerNote.Lane = performerNoteCopy.Lane;
            performerNote.NoteType = performerNoteCopy.NoteType;
            performerNote.PerformerType = performerNoteCopy.PerformerType;
            performerNote.Unk1 = performerNoteCopy.Unk1;
            performerNote.Unk2 = performerNoteCopy.Unk2;
            performerNote.Unk3 = performerNoteCopy.Unk3;
            performerNote.Unk4 = performerNoteCopy.Unk4;
            performerNote.Unk5 = performerNoteCopy.Unk5;
            performerNote.Unk6 = performerNoteCopy.Unk6;
            performerNote.Unk7 = performerNoteCopy.Unk7;
            performerNote.Unk8 = performerNoteCopy.Unk8;
        }

        public void RemovePerformerNote(Difficulty difficulty, int id)
        {
            var bossBattleSong = (BossBattleSong)this.MusicFile.SongPositions.FirstOrDefault(x => x.Value.Difficulty == difficulty).Value;

            bossBattleSong.PerformerNotes.RemoveAt(id);
        }

        public List<TimeShift<BossLane>> GetTempos(Difficulty difficulty)
        {
            var bossBattleSong = (BossBattleSong)this.MusicFile.SongPositions.FirstOrDefault(x => x.Value.Difficulty == difficulty).Value;

            return bossBattleSong.TimeShifts;
        }

        public TimeShift<BossLane> GetTempoForHitTime(Difficulty difficulty, int bossNoteHitTime)
        {
            var bossBattleSong = (BossBattleSong)this.MusicFile.SongPositions.FirstOrDefault(x => x.Value.Difficulty == difficulty).Value;
            TimeShift<BossLane> tempo = bossBattleSong.TimeShifts.FirstOrDefault();

            for (int i = 0; i < bossBattleSong.TimeShiftCount; ++i)
            {
                if (i + 1 < bossBattleSong.TimeShiftCount)
                {
                    if (bossNoteHitTime >= bossBattleSong.TimeShifts[i].HitTime && bossNoteHitTime < bossBattleSong.TimeShifts[i + 1].HitTime)
                    {
                        tempo = bossBattleSong.TimeShifts[i];
                    }
                }
                else
                {
                    tempo = bossBattleSong.TimeShifts[i];
                }
            }

            return tempo;
        }

        public void AddBossTempo(Difficulty difficulty, int time, int speed)
        {
            var tempo = new TimeShift<BossLane>
            {
                HitTime = time,
                Lane = BossLane.PlayerTop,
                Speed = speed
            };

            var bossBattleSong = (BossBattleSong)this.MusicFile.SongPositions.FirstOrDefault(x => x.Value.Difficulty == difficulty).Value;
            bossBattleSong.TimeShifts.Add(tempo);
        }

        public TimeShift<BossLane> LoadBossTempo(Difficulty difficulty, int id)
        {
            var bossBattleSong = (BossBattleSong)this.MusicFile.SongPositions.FirstOrDefault(x => x.Value.Difficulty == difficulty).Value;

            return bossBattleSong.TimeShifts.ToList()[id];
        }

        public void RemoveBossTempo(Difficulty difficulty, int id)
        {
            var bossBattleSong = (BossBattleSong)this.MusicFile.SongPositions.FirstOrDefault(x => x.Value.Difficulty == difficulty).Value;

            bossBattleSong.TimeShifts.RemoveAt(id);
        }

        public void SaveBossTempo(ref TimeShift<BossLane> tempo, TimeShift<BossLane> tempoCopy)
        {
            tempo.HitTime = tempoCopy.HitTime;
            tempo.Lane = tempoCopy.Lane;
            tempo.NoteType = tempoCopy.NoteType;
            tempo.Speed = tempoCopy.Speed;
        }

        public List<BossDarkZone> GetDarkZones(Difficulty difficulty)
        {
            var bossBattleSong = (BossBattleSong)this.MusicFile.SongPositions.FirstOrDefault(x => x.Value.Difficulty == difficulty).Value;

            return bossBattleSong.DarkZones;
        }

        public void AddBossDarkZone(Difficulty difficulty, int time, int endTime, int endAttackTime)
        {
            var darkZone = new BossDarkZone
            {
                HitTime = time,
                Lane = BossLane.PlayerTop,
                EndTime = endTime,
                EndAttackTime = endAttackTime
            };

            var bossBattleSong = (BossBattleSong)this.MusicFile.SongPositions.FirstOrDefault(x => x.Value.Difficulty == difficulty).Value;
            bossBattleSong.DarkZones.Add(darkZone);
        }

        public BossDarkZone LoadBossDarkZone(Difficulty difficulty, int id)
        {
            var bossBattleSong = (BossBattleSong)this.MusicFile.SongPositions.FirstOrDefault(x => x.Value.Difficulty == difficulty).Value;

            return bossBattleSong.DarkZones.ToList()[id];
        }

        public void RemoveBossDarkZone(Difficulty difficulty, int id)
        {
            var bossBattleSong = (BossBattleSong)this.MusicFile.SongPositions.FirstOrDefault(x => x.Value.Difficulty == difficulty).Value;

            bossBattleSong.TimeShifts.RemoveAt(id);
        }

        public void SaveBossDarkZone(ref BossDarkZone darkZone, BossDarkZone darkZoneCopy)
        {
            darkZone.HitTime = darkZoneCopy.HitTime;
            darkZone.Lane = darkZoneCopy.Lane;
            darkZone.NoteType = darkZoneCopy.NoteType;
            darkZone.EndTime = darkZoneCopy.EndTime;
            darkZone.EndAttackTime = darkZoneCopy.EndAttackTime;
        }


        #region Recompile Boss Battle

        public List<byte> RecompileBossBattleSongs()
        {
            var musicFile = new MusicFile
            {
                FileName = MusicFile.FileName,
                Header = MusicFile.Header,
                SongPositions = new Dictionary<int, ISong>(),
                AssetPositions = new Dictionary<int, List<byte>>()
            };

            // Recompile Sections (Assets + Songs)
            var offset = this.RecompileSections(ref musicFile);

            // Recompile Header
            this.RecompileHeader(ref musicFile, offset);

            // Recompile Into MusicFile
            return musicFile.RecompileMusicFile();
        }

        private void RecompileHeader(ref MusicFile musicFile, int offset)
        {
            var fileSize = 0x1000 + offset;

            musicFile.Header.FileSizeCount = fileSize < 0x1F400 ? 1 : fileSize < 0x3E800 ? 2 : 3; // Do we need to plan for higher?

            if (musicFile.Header.FileSizeCount == 1)
            {
                musicFile.Header.NextSize1 = musicFile.Header.NextSize2 = 0x5B;
                musicFile.Header.FileSizes = new List<FileSize> {
                    new FileSize { MainFileSize1 = fileSize, MainFileSize2 = fileSize }
                };
                musicFile.Header.EntireFileSize = fileSize + 0x8C;
            }
            else if (musicFile.Header.FileSizeCount == 2)
            {
                var fileSizes = BitConverter.GetBytes(fileSize);
                var fileSize1 = BitConverter.ToInt32(new byte[4] { 0x0, 0x0, 0x0, fileSizes[2] });
                var fileSize2 = BitConverter.ToInt32(new byte[4] { 0x0, fileSizes[0], fileSizes[1], 0x0 });

                musicFile.Header.NextSize1 = musicFile.Header.NextSize2 = 0x65;
                musicFile.Header.FileSizes = new List<FileSize> {
                    new FileSize { MainFileSize1 = fileSize1, MainFileSize2 = fileSize1 },
                    new FileSize { MainFileSize1 = fileSize2, MainFileSize2 = fileSize2 },
                };
                musicFile.Header.EntireFileSize = fileSize + 0x96;
            }
            else if (musicFile.Header.FileSizeCount == 3)
            {
                // TODO Not sure how to handle this
                //var fileSizes = BitConverter.GetBytes(fileSize);
                //var fileSize1 = BitConverter.ToInt32(new byte[4] { 0x0, 0x0, 0x0, fileSizes[3]});
                //var fileSize2 = BitConverter.ToInt32(new byte[4] { 0x0, 0x0, fileSizes[2], 0x0 });
                //var fileSize3 = BitConverter.ToInt32(new byte[4] { fileSizes[0], fileSizes[1], 0x0, 0x0 });

                //musicFile.Header.FileSizes = new List<FileSize> {
                //    new FileSize { MainFileSize1 = fileSize1, MainFileSize2 = fileSize1 },
                //    new FileSize { MainFileSize1 = fileSize2, MainFileSize2 = fileSize2 },
                //    new FileSize { MainFileSize1 = fileSize3, MainFileSize2 = fileSize3 },
                //};
                //musicFile.Header.EntireFileSize = fileSize + 0xA0;
            }

            musicFile.Header.CompleteFileSize = fileSize;
            musicFile.Header.FinalFileSize = fileSize;
        }

        private int RecompileSections(ref MusicFile musicFile)
        {
            int offset = 0;

            for (int i = 0; i < musicFile.Header.Sections.Length; ++i)
            {
                if (this.MusicFile.AssetPositions.ContainsKey(i))
                {
                    musicFile.Header.Sections[i].Offset = offset;

                    musicFile.AssetPositions.Add(i, new List<byte>());
                    musicFile.AssetPositions[i] = MusicFile.AssetPositions[i];
                    musicFile.AssetHasEmptyData = MusicFile.AssetHasEmptyData;

                    // initial length + asset length
                    var assetLength = 4 + musicFile.AssetPositions[i].Count;
                    assetLength += MusicFile.AssetHasEmptyData ? 4 : 0;

                    musicFile.Header.Sections[i].Size = assetLength;
                    offset += assetLength;
                }
                else if (this.MusicFile.SongPositions.ContainsKey(i))
                {
                    musicFile.Header.Sections[i].Offset = offset;

                    var song = this.MusicFile.SongPositions[i];
                    var newSong = new BossBattleSong(song.Difficulty, 0, SongType.BossBattle)
                    {
                        HasEmptyData = ((BossBattleSong)song).HasEmptyData
                    };

                    this.RecompileBossSong(ref newSong, (BossBattleSong)this.MusicFile.SongPositions.FirstOrDefault(x => x.Value.Difficulty == song.Difficulty).Value);

                    musicFile.SongPositions.Add(i, newSong);

                    // method name length + method name + length size + Song length
                    var entireSongLength = (4 + 16 + 4 + newSong.Length);

                    musicFile.Header.Sections[i].Size = entireSongLength;
                    offset += entireSongLength;
                    offset += newSong.HasEmptyData ? 4 : 0;
                }
            }

            return offset;
        }

        private void RecompileBossSong(ref BossBattleSong newSong, BossBattleSong chart)
        {
            newSong.NoteCount = chart.Notes.Count;
            newSong.PerformerCount = chart.PerformerNotes.Count;
            newSong.TimeShiftCount = chart.TimeShifts.Count;
            newSong.DarkZoneCount = chart.DarkZones.Count;

            newSong.Unk1 = 1;

            var newNotes = new List<Note<BossLane>>();

            foreach (BossNote note in chart.Notes.OrderBy(x => x.HitTime))
            {
                if (note.StartHoldNote != null)
                    note.StartHoldNoteIndex = chart.Notes.OrderBy(x => x.HitTime).ToList().IndexOf(note.StartHoldNote);

                if (note.EndHoldNote != null)
                    note.EndHoldNoteIndex = chart.Notes.OrderBy(x => x.HitTime).ToList().IndexOf(note.EndHoldNote);

                newSong.Notes.Add(note);
            }

            foreach (PerformerNote<BossLane> note in chart.PerformerNotes.OrderBy(x => x.HitTime))
            {
                newSong.PerformerNotes.Add(note);
            }


            foreach (TimeShift<BossLane> time in chart.TimeShifts.OrderBy(x => x.HitTime))
            {
                newSong.TimeShifts.Add(time);
            }

            foreach (BossDarkZone darkZone in chart.DarkZones.OrderBy(x => x.HitTime))
            {
                newSong.DarkZones.Add(darkZone);
            }

            // Add Note + Header Lengths
            newSong.Length = (newSong.NoteCount * 0x40) + (newSong.PerformerCount * 0x30) + (newSong.TimeShiftCount * 0x08) + (newSong.DarkZoneCount * 0x10) + 0x28;
        }

        #endregion Recompile Boss Battle Music File

    }
}