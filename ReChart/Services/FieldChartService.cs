using MoMMusicAnalysis;
using ReChart.ViewModels.ReChart;
using ReChart.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using ReChart.Interfaces;

namespace ReChart.Services
{
    public class FieldChartService : IFieldChartService
    {
        //public FieldBattleChartManager FieldBattleChartManager;
        //public FieldBattleSubChartManager FieldBattleSubChartManager;

        public MusicFile MusicFile { get; set; }

        public void Initialize(MusicFile musicFile)
        {
            this.MusicFile = musicFile;

            //this.FieldBattleChartManager = new FieldBattleChartManager(this.MusicFile);
            //this.FieldBattleSubChartManager = new FieldBattleSubChartManager(this.FieldBattleChartManager);
        }

        public void Deinitialize()
        {
            this.MusicFile = null;

            //this.FieldBattleChartManager = null;
            //this.FieldBattleSubChartManager = null;
        }

        public MusicFile GetMusicFile()
        {
            return this.MusicFile;
        }

        public Dictionary<string, List<FieldNoteDisplay>> GetDisplayNotes(Difficulty difficulty)
        {
            var displayNotes = new Dictionary<string, List<FieldNoteDisplay>>
            {
                { "FieldNote", new List<FieldNoteDisplay>() },
                { "FieldAsset", new List<FieldNoteDisplay>() },
                { "PerformerNote", new List<FieldNoteDisplay>() },
                { "Tempo", new List<FieldNoteDisplay>() }
            };
            
            var fieldBattleSong = (FieldBattleSong)this.MusicFile.SongPositions.FirstOrDefault(x => x.Value.Difficulty == difficulty).Value;

            displayNotes["FieldNote"] = fieldBattleSong.Notes.OrderBy(x => x.HitTime)
                .Select(x => new FieldNoteDisplay { NoteIndex = fieldBattleSong.Notes.IndexOf(x), HitTime = x.HitTime, Lane = x.Lane, Image = Utilities.GetImage(x.ModelType, x.NoteType, x.Unk3) }).ToList();

            displayNotes["FieldAsset"] = fieldBattleSong.FieldAssets.OrderBy(x => x.HitTime)
                .Select(x => new FieldNoteDisplay { NoteIndex = fieldBattleSong.FieldAssets.IndexOf(x), HitTime = x.HitTime, Lane = x.Lane, Image = Utilities.GetImage(x.ModelType) }).ToList();

            displayNotes["PerformerNote"] = fieldBattleSong.PerformerNotes.OrderBy(x => x.HitTime)
                .Select(x => new FieldNoteDisplay { NoteIndex = fieldBattleSong.PerformerNotes.IndexOf(x), HitTime = x.HitTime, Lane = x.Lane, Image = Utilities.GetImage(x.PerformerType) }).ToList();

            displayNotes["Tempo"] = fieldBattleSong.TimeShifts.OrderBy(x => x.HitTime)
                .Select(x => new FieldNoteDisplay { NoteIndex = fieldBattleSong.TimeShifts.IndexOf(x), HitTime = x.HitTime, Lane = x.Lane, Image = Utilities.GetImage("FieldBattle") }).ToList();

            return displayNotes;
        }

        public List<FieldNote> GetFieldNotes(Difficulty difficulty)
        {
            var fieldBattleSong = (FieldBattleSong)this.MusicFile.SongPositions.FirstOrDefault(x => x.Value.Difficulty == difficulty).Value;

            return fieldBattleSong.Notes;
        }

        public int GetNoteIndex(Difficulty difficulty, FieldNote fieldNote)
        {
            var fieldBattleSong = (FieldBattleSong)this.MusicFile.SongPositions.FirstOrDefault(x => x.Value.Difficulty == difficulty).Value;

            return fieldBattleSong.Notes.ToList().IndexOf(fieldNote);
        }

        public void AddFieldNote(Difficulty difficulty, int time, FieldLane lane, string modelType, List<FieldAnimation> assetAnimations)
        {
            var fieldBattleSong = (FieldBattleSong)this.MusicFile.SongPositions.FirstOrDefault(x => x.Value.Difficulty == difficulty).Value;
            var noteType = modelType.Contains("Crystal") ? 1 : modelType.Contains("Projectile") ? 2: modelType == "GlideNote" ? 3 : modelType == "Barrel" || modelType == "Crate" ? 4 : 0;
            var altModel = modelType == "Crate" ? 1 : 0;

            var fieldNote = new FieldNote
            {
                NoteType = noteType,
                HitTime = time,
                Lane = lane,
                ModelType = Enum.Parse<FieldModelType>(modelType),
                Unk3 = altModel
            };

            if (modelType != "GlideNote" && (modelType.Contains("Projectile") || ((modelType.Contains("Aerial") && !modelType.Contains("Hittable"))) || modelType.Contains("Crystal")))
            {
                var assetTime = time;
                var assetLane = lane;

                if (modelType.Contains("Projectile"))
                {
                    // Nothing to do
                }
                else if ((modelType.Contains("Aerial") && !modelType.Contains("Hittable")))
                {
                    var tempo = this.GetTempoForHitTime(difficulty, time);

                    assetTime = time - tempo.Speed;
                }
                else if (modelType == "CrystalEnemyLeftRight")
                {
                    assetTime = time - 1650;
                    assetLane = FieldLane.OutOfMapLeft;

                }
                else if (modelType == "CrystalEnemyCenter")
                {
                    assetTime = time - 1800;
                    assetLane = FieldLane.PlayerCenter;
                }

                
                this.AddFieldAsset(difficulty, assetTime, assetLane, modelType, assetAnimations);
            }

            fieldBattleSong.Notes.Add(fieldNote);

            this.AddFieldAnimation(difficulty, fieldNote);

            fieldBattleSong.NoteCount++;
        }

        public FieldNote LoadFieldNote(Difficulty difficulty, int id)
        {
            var fieldBattleSong = (FieldBattleSong)this.MusicFile.SongPositions.FirstOrDefault(x => x.Value.Difficulty == difficulty).Value;

            return fieldBattleSong.Notes.ToList()[id];
        }

        public void SaveFieldNote(Difficulty difficulty, ref FieldNote fieldNote, FieldNote fieldNoteCopy)
        {
            var fieldBattleSong = (FieldBattleSong)this.MusicFile.SongPositions.FirstOrDefault(x => x.Value.Difficulty == difficulty).Value;

            fieldNote.AerialAndCrystalCounter = fieldNoteCopy.AerialAndCrystalCounter;
            fieldNote.AerialFlag = fieldNoteCopy.AerialFlag;
            fieldNote.AnimationReference = fieldNoteCopy.AnimationReference;
            fieldNote.Animations = fieldNoteCopy.Animations;
            fieldNote.HitTime = fieldNoteCopy.HitTime;
            fieldNote.Lane = fieldNoteCopy.Lane;
            fieldNote.ModelType = fieldNoteCopy.ModelType;
            fieldNote.NextEnemyNote = fieldNoteCopy.NextEnemyNote;
            fieldNote.NextEnemyNoteIndex = fieldNoteCopy.NextEnemyNoteIndex;
            fieldNote.NoteType = fieldNoteCopy.NoteType;
            fieldNote.PartyFlag = fieldNoteCopy.PartyFlag;
            fieldNote.PreviousEnemyNote = fieldNoteCopy.PreviousEnemyNote;
            fieldNote.PreviousEnemyNoteIndex = fieldNoteCopy.PreviousEnemyNoteIndex;
            fieldNote.ProjectileOriginNote = fieldNoteCopy.ProjectileOriginNote;
            fieldNote.ProjectileOriginNoteIndex = fieldNoteCopy.ProjectileOriginNoteIndex;
            fieldNote.StarFlag = fieldNoteCopy.StarFlag;
            fieldNote.Unk1 = fieldNoteCopy.Unk1;
            fieldNote.Unk2 = fieldNoteCopy.Unk2;
            fieldNote.Unk3 = fieldNoteCopy.Unk3;
            fieldNote.Unk4 = fieldNoteCopy.Unk4;
            fieldNote.Unk5 = fieldNoteCopy.Unk5;
            fieldNote.Unk6 = fieldNoteCopy.Unk6;

            if (fieldNoteCopy.NextEnemyNoteIndex != -1)
            {
                fieldNote.NextEnemyNote = fieldBattleSong.Notes.ElementAt(fieldNoteCopy.NextEnemyNoteIndex);

                fieldNote.NextEnemyNote.PreviousEnemyNote = fieldNote;
                fieldNote.NextEnemyNote.PreviousEnemyNoteIndex = fieldBattleSong.Notes.IndexOf(fieldNote);
            }

            if (fieldNoteCopy.PreviousEnemyNoteIndex != -1)
            {
                fieldNote.PreviousEnemyNote = fieldBattleSong.Notes.ElementAt(fieldNoteCopy.PreviousEnemyNoteIndex);

                fieldNote.PreviousEnemyNote.NextEnemyNote = fieldNote;
                fieldNote.PreviousEnemyNote.NextEnemyNoteIndex = fieldBattleSong.Notes.IndexOf(fieldNote);
            }

            if (fieldNoteCopy.ProjectileOriginNoteIndex != -1)
                fieldNote.ProjectileOriginNote = fieldBattleSong.Notes.ElementAt(fieldNoteCopy.ProjectileOriginNoteIndex);


            var anims = fieldBattleSong.Notes.SelectMany(x => x.Animations).Concat(fieldBattleSong.FieldAssets.SelectMany(x => x.Animations));

            fieldNote.AnimationReference = anims.OrderBy(x => x.AnimationEndTime).ThenBy(x => x.AnimationStartTime).ToList().IndexOf(fieldNote.Animations.LastOrDefault());
        }

        public void RemoveFieldNote(Difficulty difficulty, int id)
        {
            var fieldBattleSong = (FieldBattleSong)this.MusicFile.SongPositions.FirstOrDefault(x => x.Value.Difficulty == difficulty).Value;

            fieldBattleSong.Notes.RemoveAt(id);

            fieldBattleSong.NoteCount--;
        }

        public List<FieldAsset> GetFieldAssets(Difficulty difficulty)
        {
            var fieldBattleSong = (FieldBattleSong)this.MusicFile.SongPositions.FirstOrDefault(x => x.Value.Difficulty == difficulty).Value;

            return fieldBattleSong.FieldAssets;
        }

        public FieldNoteDisplay GetClosestFieldAsset(Difficulty difficulty, int fieldNoteIndex, string fieldModelType)
        {
            var fieldBattleSong = (FieldBattleSong)this.MusicFile.SongPositions.FirstOrDefault(x => x.Value.Difficulty == difficulty).Value;
            var fieldNote = fieldBattleSong.Notes[fieldNoteIndex];
            var fieldNoteHitTime = fieldNote.HitTime;

            var currentTempo = this.GetTempoForHitTime(difficulty, fieldNoteHitTime);
            FieldNoteDisplay asset = null;

            if ((fieldModelType.Contains("Aerial") && (!fieldModelType.Contains("Hittable") && !fieldModelType.Contains("Projectile"))) || fieldModelType.Contains("Glide"))
            {
                var tempAsset = fieldBattleSong.FieldAssets.LastOrDefault(x => (fieldNoteHitTime - 650) <= x.HitTime && fieldNoteHitTime >= x.HitTime);

                if (tempAsset != null)
                    asset = new FieldNoteDisplay() { NoteIndex = fieldBattleSong.FieldAssets.OrderBy(x => x.HitTime).ToList().IndexOf(tempAsset), HitTime = tempAsset.HitTime, Lane = tempAsset.Lane, Image = Utilities.GetImage(tempAsset.ModelType) };
                else
                    asset = new FieldNoteDisplay { NoteIndex = -1, HitTime = fieldNoteHitTime - currentTempo.Speed, Lane = fieldNote.Lane, Image = fieldModelType.Contains("Glide") ? Utilities.GetImage(FieldAssetType.GlideArrow) : Utilities.GetImage(FieldAssetType.AerialCommonArrow) };
            }
            else if (fieldModelType.Contains("Projectile"))
            {
                var tempAsset = fieldBattleSong.FieldAssets.LastOrDefault(x => (fieldNoteHitTime - 650) <= x.HitTime && fieldNoteHitTime >= x.HitTime);

                if (tempAsset != null)
                    asset = new FieldNoteDisplay() { NoteIndex = fieldBattleSong.FieldAssets.OrderBy(x => x.HitTime).ToList().IndexOf(tempAsset), HitTime = tempAsset.HitTime, Lane = tempAsset.Lane, Image = Utilities.GetImage(tempAsset.ModelType) };
                else
                    asset = new FieldNoteDisplay() { NoteIndex = -1, HitTime = fieldNoteHitTime, Lane = fieldNote.Lane, Image = Utilities.GetImage(FieldAssetType.ShooterProjectileArrow) };
            }
            else if (fieldModelType.Contains("Crystal"))
            {
                var tempAsset = fieldBattleSong.FieldAssets.LastOrDefault(x => (fieldNoteHitTime + 1000) <= x.HitTime && (fieldNoteHitTime + 2500) >= x.HitTime);

                if (tempAsset != null)
                    asset = new FieldNoteDisplay() { NoteIndex = fieldBattleSong.FieldAssets.OrderBy(x => x.HitTime).ToList().IndexOf(tempAsset), HitTime = tempAsset.HitTime, Lane = tempAsset.Lane, Image = Utilities.GetImage(tempAsset.ModelType) };
                else if (fieldModelType.Contains("Center"))
                    asset = new FieldNoteDisplay() { NoteIndex = -1, HitTime = fieldNoteHitTime + 1800, Lane = fieldNote.Lane, Image = Utilities.GetImage(FieldAssetType.CrystalCenter) };
                else if (fieldModelType.Contains("Right"))
                    asset = new FieldNoteDisplay() { NoteIndex = -1, HitTime = fieldNoteHitTime + 1650, Lane = fieldNote.Lane, Image = Utilities.GetImage(FieldAssetType.CrystalRightLeft) };
            }
            
            return asset;
        }

        public void AddFieldAsset(Difficulty difficulty, int time, FieldLane lane, string parentFieldModelType, List<FieldAnimation> assetAnimations)
        {
            var modelType = Utilities.GetFieldAssetEnumForFieldNote(parentFieldModelType);

            var fieldAsset = new FieldAsset
            {
                HitTime = time,
                Lane = lane,
                ModelType = modelType,
                JumpFlag = modelType == FieldAssetType.CrystalRightLeft || modelType == FieldAssetType.CrystalCenter ? false : true,
                Unk1 = modelType == FieldAssetType.CrystalRightLeft ? 1 : 0,
                Animations = assetAnimations
            };
            
            var fieldBattleSong = (FieldBattleSong)this.MusicFile.SongPositions.FirstOrDefault(x => x.Value.Difficulty == difficulty).Value;
            fieldBattleSong.FieldAssets.Add(fieldAsset);

            this.AddFieldAnimation(difficulty, fieldAsset);

            fieldBattleSong.AssetCount++;
        }

        public FieldAsset LoadFieldAsset(Difficulty difficulty, int id)
        {
            var fieldBattleSong = (FieldBattleSong)this.MusicFile.SongPositions.FirstOrDefault(x => x.Value.Difficulty == difficulty).Value;

            return fieldBattleSong.FieldAssets.ToList()[id];
        }

        public void SaveFieldAsset(Difficulty difficulty, FieldNoteDisplay noteDisplay, string parentFieldModelType, List<FieldAnimation> assetAnims)
        {
            var fieldBattleSong = (FieldBattleSong)this.MusicFile.SongPositions.FirstOrDefault(x => x.Value.Difficulty == difficulty).Value;
            var fieldAsset = fieldBattleSong.FieldAssets.FirstOrDefault(x => x.HitTime == noteDisplay.HitTime && x.Lane == noteDisplay.Lane);

            var modelType = Utilities.GetFieldAssetEnumForFieldNote(parentFieldModelType);

            fieldAsset.HitTime = noteDisplay.HitTime;
            fieldAsset.Lane = noteDisplay.Lane;
            fieldAsset.ModelType = modelType;
            fieldAsset.Animations = assetAnims;
            fieldAsset.JumpFlag = modelType == FieldAssetType.CrystalRightLeft || modelType == FieldAssetType.CrystalCenter ? false : true;
            fieldAsset.Unk1 = modelType == FieldAssetType.CrystalRightLeft ? 1 : 0;


            var anims = fieldBattleSong.Notes.SelectMany(x => x.Animations).Concat(fieldBattleSong.FieldAssets.SelectMany(x => x.Animations));

            fieldAsset.AnimationReference = anims.OrderBy(x => x.AnimationEndTime).ThenBy(x => x.AnimationStartTime).ToList().IndexOf(fieldAsset.Animations.LastOrDefault());
        }

        public void RemoveFieldAsset(Difficulty difficulty, int id)
        {
            var fieldBattleSong = (FieldBattleSong)this.MusicFile.SongPositions.FirstOrDefault(x => x.Value.Difficulty == difficulty).Value;

            fieldBattleSong.FieldAssets.OrderBy(x => x.HitTime).ToList().RemoveAt(id);

            fieldBattleSong.AssetCount--;
        }

        public void AddPerformerNote(Difficulty difficulty, int time, FieldLane lane, PerformerType modelType)
        {
            var performerNote = new PerformerNote<FieldLane>
            {
                HitTime = time,
                Lane = lane,
                PerformerType = modelType,
                DuplicateType = modelType
            };

            var fieldBattleSong = (FieldBattleSong)this.MusicFile.SongPositions.FirstOrDefault(x => x.Value.Difficulty == difficulty).Value;
            fieldBattleSong.PerformerNotes.Add(performerNote);

            fieldBattleSong.PerformerCount++;
        }

        public PerformerNote<FieldLane> LoadFieldPerformerNote(Difficulty difficulty, int id)
        {
            var fieldBattleSong = (FieldBattleSong)this.MusicFile.SongPositions.FirstOrDefault(x => x.Value.Difficulty == difficulty).Value;

            return fieldBattleSong.PerformerNotes.ToList()[id];
        }

        public void SavePerformerNote(ref PerformerNote<FieldLane> performerNote, PerformerNote<FieldLane> performerNoteCopy)
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
            var fieldBattleSong = (FieldBattleSong)this.MusicFile.SongPositions.FirstOrDefault(x => x.Value.Difficulty == difficulty).Value;

            fieldBattleSong.PerformerNotes.RemoveAt(id);

            fieldBattleSong.PerformerCount--;
        }

        public List<TimeShift<FieldLane>> GetTempos(Difficulty difficulty)
        {
            var fieldBattleSong = (FieldBattleSong)this.MusicFile.SongPositions.FirstOrDefault(x => x.Value.Difficulty == difficulty).Value;

            return fieldBattleSong.TimeShifts;
        }

        public TimeShift<FieldLane> GetTempoForHitTime(Difficulty difficulty, int fieldNoteHitTime)
        {
            var fieldBattleSong = (FieldBattleSong)this.MusicFile.SongPositions.FirstOrDefault(x => x.Value.Difficulty == difficulty).Value;
            TimeShift<FieldLane> tempo = fieldBattleSong.TimeShifts.FirstOrDefault();

            for (int i = 0; i < fieldBattleSong.TimeShiftCount; ++i)
            {
                if (i + 1 < fieldBattleSong.TimeShiftCount)
                {
                    if (fieldNoteHitTime >= fieldBattleSong.TimeShifts[i].HitTime && fieldNoteHitTime < fieldBattleSong.TimeShifts[i + 1].HitTime)
                    {
                        tempo = fieldBattleSong.TimeShifts[i];
                    }
                }
                else
                {
                    tempo = fieldBattleSong.TimeShifts[i];
                }
            }

            return tempo;
        }

        public void AddFieldTempo(Difficulty difficulty, int time, int speed)
        {
            var tempo = new TimeShift<FieldLane>
            {
                HitTime = time,
                Lane = FieldLane.OutOfMapLeft,
                Speed = speed
            };

            var fieldBattleSong = (FieldBattleSong)this.MusicFile.SongPositions.FirstOrDefault(x => x.Value.Difficulty == difficulty).Value;
            fieldBattleSong.TimeShifts.Add(tempo);

            fieldBattleSong.TimeShiftCount++;
        }

        public TimeShift<FieldLane> LoadFieldTempo(Difficulty difficulty, int id)
        {
            var fieldBattleSong = (FieldBattleSong)this.MusicFile.SongPositions.FirstOrDefault(x => x.Value.Difficulty == difficulty).Value;

            return fieldBattleSong.TimeShifts.ToList()[id];
        }

        public void RemoveFieldTempo(Difficulty difficulty, int id)
        {
            var fieldBattleSong = (FieldBattleSong)this.MusicFile.SongPositions.FirstOrDefault(x => x.Value.Difficulty == difficulty).Value;

            fieldBattleSong.TimeShifts.RemoveAt(id);

            fieldBattleSong.TimeShiftCount--;
        }

        public void SaveFieldTempo(ref TimeShift<FieldLane> tempo, TimeShift<FieldLane> tempoCopy)
        {
            tempo.HitTime = tempoCopy.HitTime;
            tempo.Lane = tempoCopy.Lane;
            tempo.NoteType = tempoCopy.NoteType;
            tempo.Speed = tempoCopy.Speed;
        }


        public void AddFieldAnimation(Difficulty difficulty, FieldNote note)
        {
            var fieldBattleSong = (FieldBattleSong)this.MusicFile.SongPositions.FirstOrDefault(x => x.Value.Difficulty == difficulty).Value;
            fieldBattleSong.Notes.FirstOrDefault(x => x.HitTime == note.HitTime && x.Lane == note.Lane).Animations
                .Add(new FieldAnimation
                    {
                        AerialFlag = note.AerialFlag,
                        AnimationEndTime = note.HitTime,
                        AnimationStartTime = note.HitTime - 3000 > 0 ? note.HitTime - 3000 : 0,
                        Lane = note.Lane
                    });

            fieldBattleSong.AnimationCount++;
        }

        public void RemoveFieldAnimation(Difficulty difficulty, FieldNote note, int id)
        {
            var fieldBattleSong = (FieldBattleSong)this.MusicFile.SongPositions.FirstOrDefault(x => x.Value.Difficulty == difficulty).Value;
            fieldBattleSong.Notes.FirstOrDefault(x => x.HitTime == note.HitTime && x.Lane == note.Lane).Animations.RemoveAt(id);

            fieldBattleSong.AnimationCount--;
        }

        public void AddFieldAnimation(Difficulty difficulty, FieldAsset asset)
        {
            var fieldBattleSong = (FieldBattleSong)this.MusicFile.SongPositions.FirstOrDefault(x => x.Value.Difficulty == difficulty).Value;
            fieldBattleSong.FieldAssets.FirstOrDefault(x => x.HitTime == asset.HitTime && x.Lane == asset.Lane).Animations
                .Add(new FieldAnimation
                    {
                        AerialFlag = !asset.JumpFlag,
                        AnimationEndTime = asset.HitTime,
                        AnimationStartTime = asset.HitTime - 3000 > 0 ? asset.HitTime - 3000 : 0,
                        Lane = asset.Lane
                    });

            fieldBattleSong.AnimationCount++;
        }

        public void RemoveFieldAnimation(Difficulty difficulty, FieldNoteDisplay assetDisplay, int id)
        {
            var fieldBattleSong = (FieldBattleSong)this.MusicFile.SongPositions.FirstOrDefault(x => x.Value.Difficulty == difficulty).Value;
            fieldBattleSong.FieldAssets.FirstOrDefault(x => x.HitTime == assetDisplay.HitTime && x.Lane == assetDisplay.Lane).Animations.RemoveAt(id);

            fieldBattleSong.AnimationCount--;
        }

        #region Recompile Field Battle

        public List<byte> RecompileFieldBattleSongs()
        {
            var musicFile = new MusicFile
            {
                FileName = this.MusicFile.FileName,
                Header = this.MusicFile.Header,
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
                var fileSize1 = BitConverter.ToInt32(new byte[4] { 0x0, 0x0, fileSizes[2], 0x0 });
                var fileSize2 = BitConverter.ToInt32(new byte[4] { fileSizes[0], fileSizes[1], 0x0, 0x0 });

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
                    var newSong = new FieldBattleSong(song.Difficulty, 0, SongType.FieldBattle)
                    {
                        HasEmptyData = ((FieldBattleSong)song).HasEmptyData
                    };

                    this.RecompileFieldSong(ref newSong, (FieldBattleSong)this.MusicFile.SongPositions.FirstOrDefault(x => x.Value.Difficulty == song.Difficulty).Value);

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

        private void RecompileFieldSong(ref FieldBattleSong newSong, FieldBattleSong chart)
        {
            var animations = chart.Notes.SelectMany(x => x.Animations).Concat(chart.FieldAssets.OrderBy(x => x.HitTime).SelectMany(x => x.Animations)).OrderBy(x => x.AnimationEndTime).ThenBy(x => x.AnimationStartTime).ToList();

            newSong.NoteCount = chart.Notes.Count;
            newSong.AnimationCount = animations.Count;
            newSong.AssetCount = chart.FieldAssets.Count;
            newSong.PerformerCount = chart.PerformerNotes.Count;
            newSong.TimeShiftCount = chart.TimeShifts.Count;

            newSong.Unk1 = 1; // TODO Why? Is this the Identifier for FieldBattle?

            //var newNotes = new List<Note<FieldLane>>();
            //int count = 0;
            int aerialCrystalCount = 0;

            //foreach (var anim in animations.OrderBy(x => x.AnimationEndTime).ThenBy(x => x.AnimationStartTime))
            //{
            //    anim.Id = count++;
            //}

            foreach (FieldNote note in chart.Notes.OrderBy(x => x.HitTime))
            {
                var modelString = "";
                if (note.ModelType == FieldModelType.EnemyShooterProjectile && note.NoteType == 0)
                {
                    note.ModelType = FieldModelType.EnemyShooter;
                    modelString = "EnemyShooter";
                }
                else if (note.ModelType == FieldModelType.AerialEnemyShooterProjectile && note.NoteType == 0)
                {
                    note.ModelType = FieldModelType.AerialEnemyShooter;
                    modelString = "AerialEnemyShooter";
                }
                else if (note.ModelType == FieldModelType.Barrel && note.Unk3 == 1)
                {
                    note.ModelType = FieldModelType.Crate;
                    modelString = "Crate";
                }
                else
                    modelString = note.ModelType.ToString();

                var aerialFlag = (modelString.Contains("Aerial") && !(modelString == "AerialEnemyShooterProjectile" || modelString == "HittableAerialUncommonEnemy")) ||
                    (modelString == "GlideNote" && note.PreviousEnemyNote == null) ||
                    (note.ModelType == FieldModelType.EnemyShooter && note.NoteType == 2);

                if (aerialFlag || (note.ModelType == FieldModelType.CrystalEnemyCenter || note.ModelType == FieldModelType.CrystalEnemyLeftRight))
                    note.AerialAndCrystalCounter = aerialCrystalCount++;
                else
                    note.AerialAndCrystalCounter = -1;

                if (note.PreviousEnemyNote != null)
                    note.PreviousEnemyNoteIndex = chart.Notes.OrderBy(x => x.HitTime).ToList().IndexOf(note.PreviousEnemyNote);
                else
                    note.PreviousEnemyNoteIndex = -1;

                if (note.NextEnemyNote != null)
                    note.NextEnemyNoteIndex = chart.Notes.OrderBy(x => x.HitTime).ToList().IndexOf(note.NextEnemyNote);
                else
                    note.NextEnemyNoteIndex = -1;

                if (note.ProjectileOriginNote != null)
                    note.ProjectileOriginNoteIndex = chart.Notes.OrderBy(x => x.HitTime).ToList().IndexOf(note.ProjectileOriginNote);
                else
                    note.ProjectileOriginNoteIndex = -1;

                for (int i = 0; i < note.Animations.Count; ++i)
                {
                    var anim = note.Animations[i];

                    if (i > 0)
                    {
                        animations.ElementAt(animations.IndexOf(anim)).Previous = animations.IndexOf(note.Animations[i - 1]);
                    }

                    if (i < note.Animations.Count - 1)
                    {
                        animations.ElementAt(animations.IndexOf(anim)).Next = animations.IndexOf(note.Animations[i + 1]);
                    }
                }

                try
                {
                    note.AnimationReference = animations.IndexOf(note.Animations[0]);
                }
                catch (ArgumentOutOfRangeException)
                {
                    throw new Exception($"Every Note needs an animation. Note Error at Time: {note.HitTime}");

                    return;
                }

                newSong.Notes.Add(note);
            }

            foreach (FieldAsset asset in chart.FieldAssets.OrderBy(x => x.HitTime))
            {
                for (int i = 0; i < asset.Animations.Count; ++i)
                {
                    var anim = asset.Animations[i];

                    if (i > 0)
                    {
                        animations.ElementAt(animations.IndexOf(anim)).Previous = animations.IndexOf(asset.Animations[i - 1]);
                    }

                    if (i < asset.Animations.Count - 1)
                    {
                        animations.ElementAt(animations.IndexOf(anim)).Next = animations.IndexOf(asset.Animations[i + 1]);
                    }
                }

                try
                {
                    asset.AnimationReference = animations.IndexOf(asset.Animations[0]);
                }
                catch (ArgumentOutOfRangeException)
                {
                    throw new Exception($"Every Asset needs an animation. Asset Error at Time: {asset.HitTime}");

                    return;
                }

                newSong.FieldAssets.Add(asset);
            }

            foreach (var anim in animations.OrderBy(x => x.AnimationEndTime).ThenBy(x => x.AnimationStartTime))
            {
                newSong.FieldAnimations.Add(anim);
            }

            foreach (PerformerNote<FieldLane> note in chart.PerformerNotes.OrderBy(x => x.HitTime))
            {
                newSong.PerformerNotes.Add(note);
            }


            foreach (TimeShift<FieldLane> time in chart.TimeShifts.OrderBy(x => x.HitTime))
            {
                newSong.TimeShifts.Add(time);
            }

            // Add Note + Header Lengths
            newSong.Length = (newSong.NoteCount * 0x48) + (newSong.AnimationCount * 0x3C) + (newSong.AssetCount * 0x44) + (newSong.PerformerCount * 0x30) + (newSong.TimeShiftCount * 0x08) + 0x28;
        }

        #endregion Recompile Field Battle Music File
    }
}