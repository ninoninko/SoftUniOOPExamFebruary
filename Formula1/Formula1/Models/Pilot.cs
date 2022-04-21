using Formula1.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Formula1.Models
{
    public class Pilot : IPilot
    {
        private string fullName;
        private IFormulaOneCar car;
        public string FullName
        { 
            get { return this.fullName; }
            private set
            { 
                if (String.IsNullOrWhiteSpace(value) || value.Length < 5)
                {
                    throw new ArgumentException($"Invalid pilot name: {value}.");
                }

                this.fullName = value; 
            }
        }

        public IFormulaOneCar Car
        {
            get { return this.car; }
            private set
            {
                if (value == null)
                {
                    throw new NullReferenceException("Pilot car can not be null.");
                }

                car = value;
            }
        }

        public int NumberOfWins { get; private set; }

        public bool CanRace { get; private set; }

        public Pilot(string fullName)
        {
            this.FullName = fullName;
            this.CanRace = false;
        }

        public void AddCar(IFormulaOneCar car)
        {
            this.Car = car;
            this.CanRace = true;
        }

        public void WinRace()
        {
            this.NumberOfWins = this.NumberOfWins + 1;
        }

        public override string ToString()
        {
            return $"Pilot {this.FullName} has {this.NumberOfWins} wins.";
        }
    }
}
