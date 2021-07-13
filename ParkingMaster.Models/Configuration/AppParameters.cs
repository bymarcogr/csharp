using ParkingMaster.Models.Configuration.Type;

namespace ParkingMaster.Models.Configuration
{
    public class AppParameters
    {
        public string ConfigurationPath { get; set; }
        public string Text { get; set; }
        public string FilePath { get; set; }
        public string JsonObject { get; set; }

        public DatasourceType GetDataSourceType()
        {
            if (!string.IsNullOrEmpty(this.FilePath))
            {
                return DatasourceType.FILE;
            }
            if (!string.IsNullOrEmpty(this.Text))
            {
                return DatasourceType.TEXT;
            }
            if (!string.IsNullOrEmpty(this.JsonObject))
            {
                return DatasourceType.JSON;
            }
            return DatasourceType.UNKNOWN;
        }
    }
}
