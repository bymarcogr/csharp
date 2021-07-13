using ParkingMaster.Models.Input;
using ParkingMaster.Models.Output;
using System;
using System.Collections.Generic;

namespace ParkingMaster.Business.Datasource
{
    public class TextDatasourceService : IDatasourceService
    {
        public string GetSummaryOutput(Summary info)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<LogRow> GetValidInputRows(string datasource)
        {
            throw new NotImplementedException();
        }
    }
}
