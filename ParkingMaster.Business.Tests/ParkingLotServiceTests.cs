using ParkingMaster.Models.Input;
using System;
using System.Collections.Generic;
using Xunit;
using System.Linq;

namespace ParkingMaster.Business.Tests
{
    public class ParkingLotServiceTests
    {

        private IParkingLotService parkingLotService;
        private List<LogRow> logRows;
        public ParkingLotServiceTests()
        {
            this.parkingLotService = new ParkingLotService();
            logRows = new List<LogRow>()
            {
                new LogRow(){ PlateNumber= 1, Time= 111, Move = true},
                new LogRow(){ PlateNumber= 4, Time= 112, Move = true},
                new LogRow(){ PlateNumber= 1, Time= 200, Move = false},
            };
        }
        [Fact]
        public void GetParkingSummary_SendLogRowsCorrect_ShouldSuccess()
        {
            var summary = this.parkingLotService.GetParkingSummary(this.logRows);
            Assert.Equal(2, summary.MaxCars);
            Assert.Equal($"112 to 200", summary.PeakPeriod);
            Assert.Equal((uint)88, summary.PeakDuration);
            Assert.Equal(2, summary.Plates.Length);
        }

        [Fact]
        public void GetParkingSummary_SendLogRowsNotGoingInYet_ShouldThrowException()
        {
            this.logRows.Add(new LogRow() { PlateNumber = 1, Time = 100, Move = false });

            var exception = Assert.Throws<InvalidProgramException>(() => this.parkingLotService.GetParkingSummary(this.logRows));

            Assert.Equal($"Invalid datasource vehicle 1 was not going in", exception.Message);
         
        }

        [Fact]
        public void GetParkingSummary_SendLogRowsNotGoingOutYet_ShouldThrowException()
        {
            this.logRows.Add(new LogRow() { PlateNumber = 1, Time = 115, Move = true });

            var exception = Assert.Throws<InvalidProgramException>(() => this.parkingLotService.GetParkingSummary(this.logRows));

            Assert.Equal($"Invalid datasource vehicle 1 was not going out", exception.Message);

        }
    }
}
