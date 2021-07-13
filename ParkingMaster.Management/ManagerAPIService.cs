using ParkingMaster.Business;
using ParkingMaster.Business.Datasource;
using ParkingMaster.Business.File;
using ParkingMaster.Business.Input;
using ParkingMaster.Models.Configuration;
using ParkingMaster.Models.Configuration.Type;
using ParkingMaster.Models.Rules;
using ParkingMaster.Utilities;
using Serilog;
using System;
using System.IO;
using System.Linq;
using Unity;

namespace ParkingMaster.Management
{
    public class ManagerAPIService: IManagerAPIService
    {
        private ILogger logger;
        private UnityContainer container = new UnityContainer();
        private AppParameters parameters;
        private IParkingLotService parkingLotService;
        private string appConfig { get; set; }    
              
        public ManagerAPIService(AppParameters parameters)
        {
            this.parameters = parameters;
            this.SetUpServices();          
        }
        internal void SetUpServices()
        {
            this.appConfig = this.parameters.ConfigurationPath ?? $@"{Directory.GetCurrentDirectory()}\appsettings.json";
            var config = General.ReadConfig(this.appConfig);
            container.RegisterFactory<ILogger>(c => new Serilog.LoggerConfiguration()
                        .WriteTo.File(path: $"{config.Logger.Path}{config.Logger.Name}.log",
                                      rollingInterval: RollingInterval.Day,
                                      rollOnFileSizeLimit: true,
                                      shared: true)
                        .CreateLogger()
                );

            container.RegisterType<IValidationService, ValidationService>();
            container.RegisterInstance<AppRule[]>(config.Rules);
            container.RegisterType<IDatasourceService, FileDatasourceService>("FileDataSource");
            container.RegisterType<IDatasourceService, JsonDatasourceService>("JsonDataSource");
            container.RegisterType<IDatasourceService, TextDatasourceService>("TextDataSource");
            container.RegisterType<IParkingLotService, ParkingLotService>();
            container.RegisterType<IFileService, FileService>();
            this.logger = container.Resolve<ILogger>();
            this.logger.Information($"Starting ManagerService - Configuration: { this.appConfig}");
            this.logger.Information("Services configuration completed");
        }
        public object ProcessParkingLotInformation()
        {
            var resolve = this.ResolveDataSource();
            IDatasourceService datasource = resolve.Item1;
            string args = resolve.Item2;

            var inputs = datasource.GetValidInputRows(args);
            if(inputs.Count() > 0)
            {
                parkingLotService = container.Resolve<IParkingLotService>();
                var summary = parkingLotService.GetParkingSummary(inputs);
                this.logger.Information("Completed!");
                return datasource.GetSummaryOutput(summary);
            }
            else
            {
                var error = "There are no valid log information, check datatypes and format before continue";
                this.logger.Error(error);
                throw new InvalidProgramException(error);
            }
        }
        private Tuple<IDatasourceService, string> ResolveDataSource()
        {
            IDatasourceService datasource;
            string args;
            switch (this.parameters.GetDataSourceType())
            {
                case DatasourceType.FILE:
                    datasource = container.Resolve<IDatasourceService>("FileDataSource");
                    args = this.parameters.FilePath;
                    break;
                case DatasourceType.JSON:
                    datasource = container.Resolve<IDatasourceService>("JsonDataSource");
                    args = this.parameters.JsonObject;
                    break;
                case DatasourceType.TEXT:
                    datasource = container.Resolve<IDatasourceService>("TextDataSource");
                    args = this.parameters.Text;
                    break;
                default:
                    this.logger.Information("Datasource type unknown");
                    throw new InvalidProgramException("Datasource selected is not valid, please try again.");
            }

            return new Tuple<IDatasourceService, string>(datasource, args);
        } 
    }
}
