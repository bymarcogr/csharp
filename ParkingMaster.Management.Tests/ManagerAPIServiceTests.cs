using ParkingMaster.Models.Configuration;
using System;
using Xunit;

namespace ParkingMaster.Management.Tests
{
    public class ManagerAPIServiceTests
    {

        private IManagerAPIService manageAPIService { get; set; }
        public ManagerAPIServiceTests()
        {
            this.manageAPIService = new ManagerAPIService(new AppParameters() { FilePath = $"{System.IO.Directory.GetCurrentDirectory()}\\file.txt" });
        }
        [Fact]
        public void ProcessParkingLotInformation_CorrectParams_ShouldSuccess()
        {
            var response = this.manageAPIService.ProcessParkingLotInformation();
            Assert.NotNull(response);
        }
    }
}
