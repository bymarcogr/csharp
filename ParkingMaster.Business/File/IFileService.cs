using ParkingMaster.Models.Configuration;

namespace ParkingMaster.Business.File
{
    public interface IFileService
    {
        string ReadFile(string path);

        string[] ReadLinesFile(string path);

        AppSettings ReadConfig(string path);

        void WriteFile(string path, string[] contents);
    }
}
