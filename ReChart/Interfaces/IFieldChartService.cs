using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using System;
using MoMMusicAnalysis;
using ReChart.Logic;
using System.Collections.Generic;
using ReChart.ViewModels.ReChart;
using MoMMusicAnalysis.Song;
using MoMMusicAnalysis.Song.FieldBattle;

namespace ReChart.Interfaces
{
    public interface IFieldChartService
    {
        public void Initialize(MusicFile musicFile);

        public void Deinitialize();

        public MusicFile GetMusicFile();

        public Dictionary<string, List<FieldNoteDisplay>> GetDisplayNotes(Difficulty difficulty);

        public List<FieldNote> GetFieldNotes(Difficulty difficulty);

        public void AddFieldNote(Difficulty difficulty, int time, FieldLane lane, string modelType, List<FieldAnimation> assetAnims);

        public FieldNote LoadFieldNote(Difficulty difficulty, int id);

        public void SaveFieldNote(Difficulty difficulty, ref FieldNote fieldNote, FieldNote fieldNoteCopy);

        public void RemoveFieldNote(Difficulty difficulty, int id);

        public List<FieldAsset> GetFieldAssets(Difficulty difficulty);

        public FieldNoteDisplay GetClosestFieldAsset(Difficulty difficulty, int fieldNoteIndex, string fieldModelType);

        public void AddFieldAsset(Difficulty difficulty, int time, FieldLane lane, string parentFieldModelType, List<FieldAnimation> assetAnims);

        public FieldAsset LoadFieldAsset(Difficulty difficulty, int id);

        public void SaveFieldAsset(Difficulty difficulty, FieldNoteDisplay noteDisplay, string parentFieldModelType, List<FieldAnimation> assetAnims);

        public void RemoveFieldAsset(Difficulty difficulty, int id);

        public void AddPerformerNote(Difficulty difficulty, int time, FieldLane lane, PerformerType modelType);

        public PerformerNote<FieldLane> LoadFieldPerformerNote(Difficulty difficulty, int id);

        public void SavePerformerNote(ref PerformerNote<FieldLane> performerNote, PerformerNote<FieldLane> performerNoteCopy);

        public void RemovePerformerNote(Difficulty difficulty, int id);

        public List<TimeShift<FieldLane>> GetTempos(Difficulty difficulty);

        public void AddFieldTempo(Difficulty difficulty, int time, int speed);

        public TimeShift<FieldLane> LoadFieldTempo(Difficulty difficulty, int id);

        public void SaveFieldTempo(ref TimeShift<FieldLane> tempo, TimeShift<FieldLane> tempoCopy);

        public void RemoveFieldTempo(Difficulty difficulty, int id);

        public int GetNoteIndex(Difficulty difficulty, FieldNote fieldNote);

        public void AddFieldAnimation(Difficulty difficulty, FieldNote note);

        public void RemoveFieldAnimation(Difficulty difficulty, FieldNote note, int id);

        public void AddFieldAnimation(Difficulty difficulty, FieldAsset asset);

        public void RemoveFieldAnimation(Difficulty difficulty, FieldNoteDisplay assetDisplay, int id);

        public List<byte> RecompileFieldBattleSongs();
    }
}