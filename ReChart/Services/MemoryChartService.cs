using MoMMusicAnalysis;
using ReChart.ViewModels.ReChart;
using ReChart.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using ReChart.Interfaces;

namespace ReChart.Services
{
    public class MemoryChartService : IMemoryChartService
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

        public Dictionary<string, List<MemoryNoteDisplay>> GetDisplayNotes(Difficulty difficulty)
        {
            var displayNotes = new Dictionary<string, List<MemoryNoteDisplay>>
            {
                { "MemoryNote", new List<MemoryNoteDisplay>() },
                { "PerformerNote", new List<MemoryNoteDisplay>() },
                { "Tempo", new List<MemoryNoteDisplay>() }
            };
            
            var memoryBattleSong = (MemoryDiveSong)this.MusicFile.SongPositions.FirstOrDefault(x => x.Value.Difficulty == difficulty).Value;

            displayNotes["MemoryNote"] = memoryBattleSong.Notes.OrderBy(x => x.HitTime)
                .Select(x => new MemoryNoteDisplay { NoteIndex = memoryBattleSong.Notes.IndexOf(x), HitTime = x.HitTime, Lane = x.Lane, Image = Utilities.GetImage(x.MemoryNoteType) }).ToList();

            displayNotes["PerformerNote"] = memoryBattleSong.PerformerNotes.OrderBy(x => x.HitTime)
                .Select(x => new MemoryNoteDisplay { NoteIndex = memoryBattleSong.PerformerNotes.IndexOf(x), HitTime = x.HitTime, Lane = x.Lane, Image = Utilities.GetImage(x.PerformerType) }).ToList();

            displayNotes["Tempo"] = memoryBattleSong.TimeShifts.OrderBy(x => x.HitTime)
                .Select(x => new MemoryNoteDisplay { NoteIndex = memoryBattleSong.TimeShifts.IndexOf(x), HitTime = x.HitTime, Lane = x.Lane, Image = Utilities.GetImage("MemoryDive") }).ToList();

            return displayNotes;
        }

        public List<MemoryNote> GetMemoryNotes(Difficulty difficulty)
        {
            var memoryBattleSong = (MemoryDiveSong)this.MusicFile.SongPositions.FirstOrDefault(x => x.Value.Difficulty == difficulty).Value;

            return memoryBattleSong.Notes;
        }

        public int GetNoteIndex(Difficulty difficulty, MemoryNote memoryNote)
        {
            var memoryBattleSong = (MemoryDiveSong)this.MusicFile.SongPositions.FirstOrDefault(x => x.Value.Difficulty == difficulty).Value;

            return memoryBattleSong.Notes.ToList().IndexOf(memoryNote);
        }

        public void AddMemoryNote(Difficulty difficulty, int time, MemoryLane lane, string modelType)
        {
            var memoryNote = new MemoryNote
            {
                HitTime = time,
                Lane = lane,
                MemoryNoteType = Enum.Parse<MemoryNoteType>(modelType),
            };

            var memoryBattleSong = (MemoryDiveSong)this.MusicFile.SongPositions.FirstOrDefault(x => x.Value.Difficulty == difficulty).Value;
            memoryBattleSong.Notes.Add(memoryNote);
        }

        public MemoryNote LoadMemoryNote(Difficulty difficulty, int id)
        {
            var memoryBattleSong = (MemoryDiveSong)this.MusicFile.SongPositions.FirstOrDefault(x => x.Value.Difficulty == difficulty).Value;

            return memoryBattleSong.Notes.ToList()[id];
        }

        public void SaveMemoryNote(ref MemoryNote memoryNote, MemoryNote memoryNoteCopy)
        {
            memoryNote.AerialFlag = memoryNoteCopy.AerialFlag;
            memoryNote.MemoryNoteType = memoryNoteCopy.MemoryNoteType;
            memoryNote.EndHoldNote = memoryNoteCopy.EndHoldNote;
            memoryNote.EndHoldNoteIndex = memoryNoteCopy.EndHoldNoteIndex;
            memoryNote.HitTime = memoryNoteCopy.HitTime;
            memoryNote.Lane = memoryNoteCopy.Lane;
            memoryNote.NoteType = memoryNoteCopy.NoteType;
            memoryNote.StartHoldNote = memoryNoteCopy.StartHoldNote;
            memoryNote.StartHoldNoteIndex = memoryNoteCopy.StartHoldNoteIndex;
            memoryNote.SwipeDirection = memoryNoteCopy.SwipeDirection;
            memoryNote.Unk1 = memoryNoteCopy.Unk1;
            memoryNote.Unk2 = memoryNoteCopy.Unk2;
            memoryNote.Unk3 = memoryNoteCopy.Unk3;
            memoryNote.Unk4 = memoryNoteCopy.Unk4;
            memoryNote.Unk5 = memoryNoteCopy.Unk5;
            memoryNote.Unk6 = memoryNoteCopy.Unk6;
            memoryNote.Unk7 = memoryNoteCopy.Unk7;
            memoryNote.Unk8 = memoryNoteCopy.Unk8;
            memoryNote.UnkFF = memoryNoteCopy.UnkFF;
        }

        public void RemoveMemoryNote(Difficulty difficulty, int id)
        {
            var memoryBattleSong = (MemoryDiveSong)this.MusicFile.SongPositions.FirstOrDefault(x => x.Value.Difficulty == difficulty).Value;

            memoryBattleSong.Notes.RemoveAt(id);
        }

        public void AddPerformerNote(Difficulty difficulty, int time, MemoryLane lane, PerformerType modelType)
        {
            var performerNote = new PerformerNote<MemoryLane>
            {
                HitTime = time,
                Lane = lane,
                PerformerType = modelType,
                DuplicateType = modelType
            };

            var memoryBattleSong = (MemoryDiveSong)this.MusicFile.SongPositions.FirstOrDefault(x => x.Value.Difficulty == difficulty).Value;
            memoryBattleSong.PerformerNotes.Add(performerNote);
        }

        public PerformerNote<MemoryLane> LoadMemoryPerformerNote(Difficulty difficulty, int id)
        {
            var memoryBattleSong = (MemoryDiveSong)this.MusicFile.SongPositions.FirstOrDefault(x => x.Value.Difficulty == difficulty).Value;

            return memoryBattleSong.PerformerNotes.ToList()[id];
        }

        public void SavePerformerNote(ref PerformerNote<MemoryLane> performerNote, PerformerNote<MemoryLane> performerNoteCopy)
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
            var memoryBattleSong = (MemoryDiveSong)this.MusicFile.SongPositions.FirstOrDefault(x => x.Value.Difficulty == difficulty).Value;

            memoryBattleSong.PerformerNotes.RemoveAt(id);
        }

        public List<TimeShift<MemoryLane>> GetTempos(Difficulty difficulty)
        {
            var memoryBattleSong = (MemoryDiveSong)this.MusicFile.SongPositions.FirstOrDefault(x => x.Value.Difficulty == difficulty).Value;

            return memoryBattleSong.TimeShifts;
        }

        public TimeShift<MemoryLane> GetTempoForHitTime(Difficulty difficulty, int memoryNoteHitTime)
        {
            var memoryBattleSong = (MemoryDiveSong)this.MusicFile.SongPositions.FirstOrDefault(x => x.Value.Difficulty == difficulty).Value;
            TimeShift<MemoryLane> tempo = memoryBattleSong.TimeShifts.FirstOrDefault();

            for (int i = 0; i < memoryBattleSong.TimeShiftCount; ++i)
            {
                if (i + 1 < memoryBattleSong.TimeShiftCount)
                {
                    if (memoryNoteHitTime >= memoryBattleSong.TimeShifts[i].HitTime && memoryNoteHitTime < memoryBattleSong.TimeShifts[i + 1].HitTime)
                    {
                        tempo = memoryBattleSong.TimeShifts[i];
                    }
                }
                else
                {
                    tempo = memoryBattleSong.TimeShifts[i];
                }
            }

            return tempo;
        }

        public void AddMemoryTempo(Difficulty difficulty, int time, int speed)
        {
            var tempo = new TimeShift<MemoryLane>
            {
                HitTime = time,
                Lane = MemoryLane.PlayerLeft,
                Speed = speed
            };

            var memoryBattleSong = (MemoryDiveSong)this.MusicFile.SongPositions.FirstOrDefault(x => x.Value.Difficulty == difficulty).Value;
            memoryBattleSong.TimeShifts.Add(tempo);
        }

        public TimeShift<MemoryLane> LoadMemoryTempo(Difficulty difficulty, int id)
        {
            var memoryBattleSong = (MemoryDiveSong)this.MusicFile.SongPositions.FirstOrDefault(x => x.Value.Difficulty == difficulty).Value;

            return memoryBattleSong.TimeShifts.ToList()[id];
        }

        public void RemoveMemoryTempo(Difficulty difficulty, int id)
        {
            var memoryBattleSong = (MemoryDiveSong)this.MusicFile.SongPositions.FirstOrDefault(x => x.Value.Difficulty == difficulty).Value;

            memoryBattleSong.TimeShifts.RemoveAt(id);
        }

        public void SaveMemoryTempo(ref TimeShift<MemoryLane> tempo, TimeShift<MemoryLane> tempoCopy)
        {
            tempo.HitTime = tempoCopy.HitTime;
            tempo.Lane = tempoCopy.Lane;
            tempo.NoteType = tempoCopy.NoteType;
            tempo.Speed = tempoCopy.Speed;
        }


        #region Recompile Memory Dive

        public List<byte> RecompileMemoryDiveSongs()
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
                    var newSong = new MemoryDiveSong(song.Difficulty, 0, SongType.MemoryDive)
                    {
                        HasEmptyData = ((MemoryDiveSong)song).HasEmptyData
                    };

                    this.RecompileMemorySong(ref newSong, (MemoryDiveSong)this.MusicFile.SongPositions.FirstOrDefault(x => x.Value.Difficulty == song.Difficulty).Value);

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

        private void RecompileMemorySong(ref MemoryDiveSong newSong, MemoryDiveSong chart)
        {
            newSong.NoteCount = chart.Notes.Count;
            newSong.PerformerCount = chart.PerformerNotes.Count;
            newSong.TimeShiftCount = chart.TimeShifts.Count;

            newSong.Unk1 = 1;

            var newNotes = new List<Note<MemoryLane>>();

            foreach (MemoryNote note in chart.Notes.OrderBy(x => x.HitTime))
            {
                if (note.StartHoldNote != null)
                    note.StartHoldNoteIndex = chart.Notes.OrderBy(x => x.HitTime).ToList().IndexOf(note.StartHoldNote);

                if (note.EndHoldNote != null)
                    note.EndHoldNoteIndex = chart.Notes.OrderBy(x => x.HitTime).ToList().IndexOf(note.EndHoldNote);

                newSong.Notes.Add(note);
            }

            foreach (PerformerNote<MemoryLane> note in chart.PerformerNotes.OrderBy(x => x.HitTime))
            {
                newSong.PerformerNotes.Add(note);
            }

            foreach (TimeShift<MemoryLane> time in chart.TimeShifts.OrderBy(x => x.HitTime))
            {
                newSong.TimeShifts.Add(time);
            }

            // Add Note + Header Lengths
            newSong.Length = (newSong.NoteCount * 0x40) + (newSong.PerformerCount * 0x30) + (newSong.TimeShiftCount * 0x08) + 0x24;
        }

        #endregion Recompile Memory Dive Music File
    }
}