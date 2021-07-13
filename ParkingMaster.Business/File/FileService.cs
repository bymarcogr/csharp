using ParkingMaster.Models.Configuration;
using ParkingMaster.Utilities;

namespace ParkingMaster.Business.File
{
    public class FileService : IFileService
    {
        public AppSettings ReadConfig(string path)
        {
            return General.ReadConfig(path);
        }

        public string ReadFile(string path)
        {
            return General.ReadFile(path);
        }

        public string[] ReadLinesFile(string path)
        {
            return General.ReadLinesFile(path);
        }

        public void WriteFile(string path, string[] contents)
        {
            General.WriteFile(path, contents);
        }
    }
}
