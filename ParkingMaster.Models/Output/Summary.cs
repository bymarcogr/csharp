using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace ParkingMaster.Models.Output
{
    public class Summary
    {
        public int MaxCars { get; set; }
        public uint[] Plates { get; set; }
        public string PeakPeriod { get; set; }
        public uint PeakDuration { get; set; }

        public string ToJson()
        {
            return  JsonSerializer.Serialize(this);
        }
        public string[] ToArray()
        {
            return new string[] {
                $"Max cars: {this.MaxCars}",
                $"License plates at max: {string.Join(",", this.Plates) }",
                $"Peak period: {this.PeakPeriod}",
                $"Peak duration: {this.PeakDuration}"};
        }
        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach(string line in this.ToArray())
            {
                sb.Append(line);
            }
            return sb.ToString();
        }
    }
}
