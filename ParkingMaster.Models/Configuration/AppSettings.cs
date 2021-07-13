using ParkingMaster.Models.Rules;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParkingMaster.Models.Configuration
{
    /// <summary>
    /// This is used by ManagerService on object initialization
    /// </summary>
    public class AppSettings
    {
        public AppConfiguration App { get; set; }

        public AppRule[] Rules { get; set; }

        public LoggerConfiguration Logger { get; set; }
    }
}
