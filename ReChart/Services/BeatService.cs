using MoMMusicAnalysis;
using ReChart.Enums;
using ReChart.Logic;
using ReChart.ViewModels.ReChart;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ReChart.Services
{
    public class BeatService<TLane>
    {
        public TimeShift<TLane> CurrentTempo { get; set; }
        public int CurrentTime { get; set; } = -1;
        public int StartPosition { get; set; }

        public List<Line> CurrentBeats { get; set; } = new List<Line>();

        public List<Line> WholeBeats { get; set; } = new List<Line> { null };
        public List<Line> HalfBeats { get; set; } = new List<Line> { null, null };
        public List<Line> QuarterBeats { get; set; } = new List<Line> { null, null, null, null };
        public List<Line> EighthBeats { get; set; } = new List<Line> { null, null, null, null, null, null, null, null };
        public List<Line> SixteenthBeats { get; set; } = new List<Line> { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null };
        public List<Line> ThirtySecondBeats { get; set; } = new List<Line> { null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null };

        public void CalculateOffset(List<FieldNote> notes, List<FieldAsset> assets)
        {
            this.StartPosition = notes.Select(x => x.HitTime).Concat(assets.OrderBy(x => x.HitTime).Select(x => x.HitTime)).OrderBy(x => x).FirstOrDefault();
        }

        public void CalculateOffset(List<BossNote> notes)
        {
            this.StartPosition = notes.Select(x => x.HitTime).OrderBy(x => x).FirstOrDefault();
        }

        public void CalculateOffset(List<MemoryNote> notes)
        {
            this.StartPosition = notes.Select(x => x.HitTime).OrderBy(x => x).FirstOrDefault();
        }

        public Line SnapToBeat(int positionX)
        {
            Line closestTime = null;

            switch (Settings.CurrentBeat)
            {
                case Beat.None:
                    break;
                case Beat.Whole:
                    closestTime = this.WholeBeats.FirstOrDefault();
                    break;
                case Beat.Half:
                    closestTime = this.FindClosestBeat(this.HalfBeats, positionX);
                    break;
                case Beat.Quarter:
                    closestTime = this.FindClosestBeat(this.QuarterBeats, positionX);
                    break;
                case Beat.Eighth:
                    closestTime = this.FindClosestBeat(this.EighthBeats, positionX);
                    break;
                case Beat.Sixteenth:
                    closestTime = this.FindClosestBeat(this.SixteenthBeats, positionX);
                    break;
                case Beat.ThirtySecond:
                    closestTime = this.FindClosestBeat(this.ThirtySecondBeats, positionX);
                    break;
                default:
                    break;
            }

            return closestTime;
        }

        public void UpdateCurrentBeats(string key)
        {
            if (key == Settings.Configurations.KeyConfigs.Beats["Whole"])
            {
                this.CurrentBeats = this.WholeBeats;
                Settings.CurrentBeat = Beat.Whole;
            }
            else if (key == Settings.Configurations.KeyConfigs.Beats["Half"])
            { 
                this.CurrentBeats = this.HalfBeats;
                Settings.CurrentBeat = Beat.Half;
            }
            else if (key == Settings.Configurations.KeyConfigs.Beats["Quarter"])
            { 
                this.CurrentBeats = this.QuarterBeats;
                Settings.CurrentBeat = Beat.Quarter;
            }
            else if (key == Settings.Configurations.KeyConfigs.Beats["Eighth"])
            { 
                this.CurrentBeats = this.EighthBeats;
                Settings.CurrentBeat = Beat.Eighth;
            }
            else if (key == Settings.Configurations.KeyConfigs.Beats["Sixteenth"])
            { 
                this.CurrentBeats = this.SixteenthBeats;
                Settings.CurrentBeat = Beat.Sixteenth;
            }
            else if (key == Settings.Configurations.KeyConfigs.Beats["ThirtySecond"])
            { 
                this.CurrentBeats = this.ThirtySecondBeats;
                Settings.CurrentBeat = Beat.ThirtySecond;
            }

            else if (key == Settings.Configurations.KeyConfigs.Beats["None"])
            { 
                this.CurrentBeats = new List<Line>();
                Settings.CurrentBeat = Beat.None;
            }
        }

        public bool DisplayChartBeats(List<TimeShift<TLane>> times, int positionX)
        {
            TimeShift<TLane> currTempo = null;

            if (times.Count == 1)
            {
                currTempo = times.FirstOrDefault();
            }
            else
            {
                for (int i = 0; i < times.Count; ++i)
                {
                    var time = times[i];

                    if (i + 1 < times.Count)
                    {
                        var nextTime = times[i + 1];

                        if (positionX > (time.HitTime + this.StartPosition) && positionX <= (nextTime.HitTime + this.StartPosition))
                        {
                            currTempo = time;

                            break;
                        }
                    }
                    else
                    {
                        currTempo = time;
                    }
                }
            }

            var tempoWhole = (currTempo.Speed * 4);

            var currentTime = ((positionX / tempoWhole) * tempoWhole);

            if (currentTime != this.CurrentTime)
            {
                this.CurrentTempo = currTempo;
                this.CurrentTime = currentTime;

                this.CreateChartBeats();

                return true;
            }

            return false;
        }

        public void RemoveChartBeats()
        {
            this.CurrentBeats = new List<Line>();
        }

        private void CreateChartBeats()
        {
            var wholeNote = (this.CurrentTempo.Speed * 4);
            for (int i = 0; i < 1; ++i)
                this.WholeBeats[i] = new Line { Color = Color.Purple, Location = new Point((wholeNote * i) + this.CurrentTime, 0) };

            var halfNote = (this.CurrentTempo.Speed * 2);
            for (int i = 0; i < 2; ++i)
                this.HalfBeats[i] = new Line { Color = Color.MediumPurple, Location = new Point((halfNote * i) + this.CurrentTime, 0) };

            var quarterNote = (this.CurrentTempo.Speed);
            for (int i = 0; i < 4; ++i)
                this.QuarterBeats[i] = new Line { Color = Color.MediumPurple, Location = new Point((quarterNote * i) + this.CurrentTime, 0) };

            var eighthNote = (this.CurrentTempo.Speed / 2);
            for (int i = 0; i < 8; ++i)
                this.EighthBeats[i] = new Line { Color = Color.MediumPurple, Location = new Point((eighthNote * i) + this.CurrentTime, 0) };

            var sixteenthNote = (this.CurrentTempo.Speed / 4);
            for (int i = 0; i < 16; ++i)
                this.SixteenthBeats[i] = new Line { Color = Color.MediumPurple, Location = new Point((sixteenthNote * i) + this.CurrentTime, 0) };

            var thirtySecondNote = (this.CurrentTempo.Speed / 8);
            for (int i = 0; i < 32; ++i)
                this.ThirtySecondBeats[i] = new Line { Color = Color.MediumPurple, Location = new Point((thirtySecondNote * i) + this.CurrentTime, 0) };
        }

        private Line FindClosestBeat(List<Line> beats, int positionX)
        {
            var closestTime = beats.Aggregate((x, y) => Math.Abs(x.Location.X - positionX) < Math.Abs(y.Location.X - positionX) ? x : y);

            beats.ForEach(x =>
            {
                if (x.Location.X == closestTime.Location.X)
                    x.Color = Color.MediumPurple;
                else
                    x.Color = Color.Purple;

            });

            return closestTime;
        }
    }
}