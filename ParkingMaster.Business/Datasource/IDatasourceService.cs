using ParkingMaster.Models.Input;
using ParkingMaster.Models.Output;
using System.Collections.Generic;

namespace ParkingMaster.Business.Datasource
{
    public interface IDatasourceService
    {
        public IEnumerable<LogRow> GetValidInputRows(string datasource);

        public string GetSummaryOutput(Summary info);
    }
}
