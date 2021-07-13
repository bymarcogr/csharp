using ParkingMaster.Models.Input;
using ParkingMaster.Models.Output;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ParkingMaster.Business
{
    public class ParkingLotService: IParkingLotService
    {
        private HashSet<uint> SlotsAvailable { get; set; }
        private Dictionary<int,Summary> Summaries { get; set; }

        public ParkingLotService()
        {  
            this.SlotsAvailable = new HashSet<uint>();
            this.Summaries = new Dictionary<int, Summary>();
        }

        /// <summary>
        /// Getting summary object needed for the output object
        /// </summary>
        /// <param name="logRows">IEnumerable<LogRow></param>
        /// <returns>Summary</returns>
        public Summary GetParkingSummary(IEnumerable<LogRow> logRows)
        {
            int index= 0;
         
            foreach (var item in logRows.OrderBy(o => o.Time))
            {             
                bool exists = SlotsAvailable.Contains(item.PlateNumber);
                if (item.Move)
                {
                    if (exists)
                    {
                        throw new InvalidProgramException($"Invalid datasource vehicle {item.PlateNumber} was not going out");
                    }
                    SlotsAvailable.Add(item.PlateNumber);
                }
                else
                {
                    if (!exists)
                    {
                        throw new InvalidProgramException($"Invalid datasource vehicle {item.PlateNumber} was not going in");
                    }
                    SlotsAvailable.Remove(item.PlateNumber);
                }

                // Get step by step summary results
                this.GetStepSummary(index,item);
                index++;
            }
            return this.GetSummary();
        }

        /// <summary>
        /// Step by step validation
        /// </summary>
        /// <param name="index">current index item</param>
        /// <param name="current">current logrow object</param>
        private void GetStepSummary(int index,LogRow current)
        {
            this.Summaries.Add(index,new Summary() { MaxCars = SlotsAvailable.Count, Plates = SlotsAvailable.ToArray(), PeakDuration = current.Time });
        }

        /// <summary>
        ///  Get output object needed for the final report
        /// </summary>
        /// <returns>Summary</returns>
        private Summary GetSummary()
        {
            var item = this.Summaries.First(o => o.Value.MaxCars == this.Summaries.Max(o => o.Value.MaxCars));
            var next = this.Summaries.ElementAt(item.Key + 1);

            return new Summary() { 
                MaxCars = item.Value.MaxCars, 
                Plates = item.Value.Plates, 
                PeakPeriod = $"{item.Value.PeakDuration.ToString()} to {next.Value.PeakDuration.ToString()}", 
                PeakDuration = next.Value.PeakDuration - item.Value.PeakDuration
            };
        }
    }
}
