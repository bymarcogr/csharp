
using System;

namespace ParkingMaster.Models.Input
{
    public class LogRow
    {
        public uint PlateNumber { get; set; }
        public uint Time { get; set; }
        public bool Move { get; set; }

        public LogRow()
        {

        }
        public LogRow(InputEvent input)
        {
            this.PlateNumber = Convert.ToUInt32(input.PlateNumber);
            this.Time = Convert.ToUInt32(input.Time);
            this.Move = input.Move == "0";
        }
    }
}
