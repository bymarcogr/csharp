using ParkingMaster.Management;
using ParkingMaster.Models.Configuration;
using System;

namespace ParkingMaster.ConsoleApp
{

    public class Program
    {
        static void Main(string[] args)
        {
            IManagerAPIService management;
            args = new string[] { "-config", @"C:\Users\user\source\repos\ParkingMaster\ParkingMaster.Console\bin\Debug\netcoreapp3.1\appsettings.json", "-file", @"C:\Temp\file.txt" };
            //args = new string[] { "-?" };

            if (ValidateErrors(args))
            {
                Console.WriteLine($"An unexpected error occurred while invalid parameters specified");
                Environment.Exit(1);
            }
            try
            {
                var parameters = new AppParameters();
                bool executeService = true;
                for (int i = 0; i < args.Length; i += 2)
                {
                    string command = args[i] ?? "";                   
                    switch (command)
                    {
                        case "-config":
                            parameters.ConfigurationPath = args[i + 1];
                            break;
                        case "-json":
                            parameters.JsonObject = args[i + 1];
                            break;
                        case "-file":
                            parameters.FilePath = args[i + 1];
                            break;
                        case "-string":
                            parameters.Text = args[i + 1];
                            break;
                        case "-help":
                        case "-?":
                            Program.ProcessHelp();
                            executeService = false;
                            break;
                        default:
                            Console.WriteLine($"Unsupported command '{command}'.");
                            executeService = false;
                            break;
                    }
                }
                if (executeService)
                {
                    management = new ManagerAPIService(parameters);
                    var result = management.ProcessParkingLotInformation();
                    Console.WriteLine(result);
                }
            }
            catch (InvalidProgramException ex)
            {
                Console.WriteLine($"An unexpected error occurred. Error: {ex.Message}");
            }
            
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred while attempt to process the specified command '{args[0]}'");
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private static bool ValidateErrors(string[] args)
        {
            return (args.Length == 0 || args.Length % 2 > 0) && args.Length > 1;
        }

        public static void ProcessHelp()
        {
            Console.WriteLine("");
            Console.WriteLine("Usage: ParkingMasterApp.exe [[-config] [filepath]] [[-json] | [-file] | [-string]] [datasource]");
            Console.WriteLine("");
            Console.WriteLine("Options:");
            Console.WriteLine("    -config          Json configuration file path. Exclude argument to use default file appsettings.json");
            Console.WriteLine("    -json            Datasource - String in JSON format with array of standars input objects.");
            Console.WriteLine("    -file            Datasource - Plain text file path in standard format.");
            Console.WriteLine("    -string          Datasource - String in special format, each field must be separated by space character");
            Console.WriteLine("                     and each row must be separated by | caracter");
            Console.WriteLine("");
            Console.WriteLine("Output:");
            Console.WriteLine("                     Output will take the same dalivery option as input datasource");
            Console.WriteLine("");
            Console.WriteLine("Examples:");
            Console.WriteLine("");
            Console.WriteLine(" -config");
            Console.WriteLine("  ParkingMasterApp.exe -config  'C:\\appsettings.json'");
            Console.WriteLine("");
            Console.WriteLine(" -json");
            Console.WriteLine($@"  ParkingMasterApp.exe -json  '[{{""number"":99999, ""time"":123, ""move"":1}},{{""number"":88888, ""time"":123, ""move"":0}}]'");
            Console.WriteLine($@"              Output Console  '{{""MaxCars"":99999, ""Plates"":123, ""PeakPeriod"":1, ""PeakDuration"":""3333""}}'");
            Console.WriteLine("");
            Console.WriteLine(" -file");
            Console.WriteLine("  ParkingMasterApp.exe -file  'C:\\datasource.txt'");
            Console.WriteLine($@"              Output File   'C:\\datasource_result.txt'");
            Console.WriteLine("");
            Console.WriteLine(" -string");
            Console.WriteLine("  ParkingMasterApp.exe -string  '999999 666 1|88888 987 0'");
            Console.WriteLine($@"             Output Console   'MaxCars:99999|Plates:123|PeakPeriod:1|PeakDuration:3333'");
        }
    }
}
