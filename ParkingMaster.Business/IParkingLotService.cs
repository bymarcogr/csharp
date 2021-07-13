using ParkingMaster.Models.Input;
using ParkingMaster.Models.Output;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParkingMaster.Business
{
    public interface IParkingLotService
    {
        public Summary GetParkingSummary(IEnumerable<LogRow> logRows);
    }
}
