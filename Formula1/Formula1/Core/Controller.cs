using Formula1.Core.Contracts;
using Formula1.Models;
using Formula1.Models.Contracts;
using Formula1.Repositories;
using Formula1.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Formula1.Core
{
    public class Controller : IController
    {
        private IRepository<IPilot> pilots;
        private IRepository<IRace> races;
        private IRepository<IFormulaOneCar> cars;

        public Controller()
        {
            this.pilots = new PilotRepository();
            this.races = new RaceRepository();
            this.cars = new FormulaOneCarRepository();
        }
        public string AddCarToPilot(string pilotName, string carModel)
        {
            IPilot pilot = this.pilots.FindByName(pilotName);
            if (pilot == null || pilot.Car != null)
            {
                throw new InvalidOperationException($"Pilot {pilotName} does not exist or has a car.");
            }

            IFormulaOneCar car = this.cars.FindByName(carModel);
            if (car == null)
            {
                throw new NullReferenceException($"Car {carModel} does not exist.");
            }

            pilot.AddCar(car);
            this.cars.Remove(car);

            return $"Pilot {pilotName} will drive a {car.GetType().Name} {carModel} car.";
        }

        public string AddPilotToRace(string raceName, string pilotFullName)
        {
            IRace race = this.races.FindByName(raceName);
            if (race == null)
            {
                throw new NullReferenceException($"Race {raceName} does not exist.");
            }
            
            IPilot pilot = this.pilots.FindByName(pilotFullName);
            if (pilot == null || !pilot.CanRace || race.Pilots.Contains(pilot))
            {
                throw new InvalidOperationException($"Can not add pilot {pilotFullName} to the race.");
            }

            race.AddPilot(pilot);
            return $"Pilot {pilotFullName} is added to the {raceName} race.";
        }

        public string CreateCar(string type, string model, int horsepower, double engineDisplacement)
        {
            IFormulaOneCar car = this.cars.FindByName(model);
            if (car != null)
            {
                throw new InvalidOperationException($"Formula one car {model} is already created.");
            }

            if (type == nameof(Ferrari))
            {
                car = new Ferrari(model, horsepower, engineDisplacement);
            } else if (type == nameof(Williams))
            {
                car = new Williams(model, horsepower, engineDisplacement);
            } else
            {
                throw new InvalidOperationException($"Formula one car type {type} is not valid.");
            }

            this.cars.Add(car);
            return $"Car {type}, model {model} is created.";
        }

        public string CreatePilot(string fullName)
        {
            IPilot pilot = this.pilots.FindByName(fullName);
            if (pilot != null)
            {
                throw new InvalidOperationException($"Pilot {fullName} is already created.");
            }

            pilot = new Pilot(fullName);
            this.pilots.Add(pilot);

            return $"Pilot {fullName} is created.";
        }

        public string CreateRace(string raceName, int numberOfLaps)
        {
            IRace race = this.races.FindByName(raceName);
            if (race != null)
            {
                throw new InvalidOperationException($"Race {raceName} is already created.");
            }

            race = new Race(raceName, numberOfLaps);
            this.races.Add(race);
            return $"Race {raceName} is created.";
        }

        public string PilotReport()
        {
            StringBuilder sb = new StringBuilder();
            List<IPilot> pilotsToPrint = new List<IPilot>();

            foreach (IPilot pilot in this.pilots.Models)
            {
                pilotsToPrint.Add(pilot);
            }

            pilotsToPrint = pilotsToPrint.OrderByDescending(p => p.NumberOfWins).ToList();
            foreach (IPilot pilot in pilotsToPrint)
            {
                sb.AppendLine($"Pilot {pilot.FullName} has {pilot.NumberOfWins} wins.");
            }

            return sb.ToString().TrimEnd();
        }

        public string RaceReport()
        {
            StringBuilder sb = new StringBuilder();
            foreach (IRace race in this.races.Models)
            {
                if (race.TookPlace)
                {
                    sb.AppendLine(race.RaceInfo());
                }
            }

            return sb.ToString().TrimEnd();
        }

        public string StartRace(string raceName)
        {
            IRace race = this.races.FindByName(raceName);
            if (race == null)
            {
                throw new NullReferenceException ($"Race {raceName} does not exist.");
            }

            if (race.Pilots == null)
            {
                throw new InvalidOperationException($"Race {raceName} cannot start with less than three participants.");
            }
            if (race.Pilots.Count < 3)
            {
                throw new InvalidOperationException($"Race {raceName} cannot start with less than three participants.");
            }

            if (race.TookPlace)
            {
                throw new InvalidOperationException($"Can not execute race {raceName}.");
            }
            race.TookPlace = true;
            List<IPilot> sortingPilots = new List<IPilot>();
            foreach (IPilot pilot in race.Pilots)
            {
                sortingPilots.Add(pilot);
            }

            sortingPilots = sortingPilots.OrderByDescending(p => p.Car.RaceScoreCalculator(race.NumberOfLaps)).ToList();

            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Pilot {sortingPilots[0].FullName} wins the {race.RaceName} race.");
            sb.AppendLine($"Pilot {sortingPilots[1].FullName} is second in the {race.RaceName} race.");
            sb.AppendLine($"Pilot {sortingPilots[2].FullName} is third in the {race.RaceName} race.");

            sortingPilots[0].WinRace();           

            return sb.ToString().TrimEnd();
        }
    }
}
