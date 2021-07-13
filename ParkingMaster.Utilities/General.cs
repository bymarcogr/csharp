using Newtonsoft.Json;
using ParkingMaster.Models.Configuration;
using System.IO;
using System.Text;

namespace ParkingMaster.Utilities
{
    public class General
    {
        public static string ReadFile(string path) => File.ReadAllText(path);

        public static string[] ReadLinesFile(string path) => File.ReadAllLines(path);

        public static AppSettings ReadConfig(string path) => JsonConvert.DeserializeObject<AppSettings>(ReadFile(path));

        public static void WriteFile(string path, string[] contents) => System.IO.File.WriteAllLines(path, contents,Encoding.UTF8);

    }
}
