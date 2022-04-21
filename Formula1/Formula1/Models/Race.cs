using Formula1.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Formula1.Models
{
    public class Race : IRace
    {
        private string raceName;
        private int numberOfLaps;
        private List<IPilot> pilots;
        public string RaceName
        {
            get { return this.raceName; }
            private set
            {
                if (String.IsNullOrWhiteSpace(value) || value.Length < 5)
                {
                    throw new ArgumentException($"Invalid race name: {value}.");
                }

                this.raceName = value;
            }
        }

        public int NumberOfLaps
        {
            get { return this.numberOfLaps; }
            private set
            {
                if (value  < 1)
                {
                    throw new ArgumentException($"Invalid lap numbers: {value}.");
                }

                this.numberOfLaps = value;
            }
        }

        public Race(string raceName, int numberOfLaps)
        {
            this.RaceName = raceName;
            this.NumberOfLaps = numberOfLaps;
            TookPlace = false;
            this.pilots = new List<IPilot>();
        }

        public bool TookPlace { get; set; }

        public ICollection<IPilot> Pilots => pilots;

        public void AddPilot(IPilot pilot)
        {
            this.pilots.Add(pilot);
        }

        public string RaceInfo()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"The {this.RaceName} race has:");
            sb.AppendLine($"Participants: {this.Pilots.Count}");
            sb.AppendLine($"Number of laps: {this.NumberOfLaps}");
            if (this.TookPlace)
            {
                sb.AppendLine($"Took place: Yes");
            } else
            {
                sb.AppendLine($"Took place: No");
            }

            return sb.ToString().TrimEnd();
            
        }
    }
}
