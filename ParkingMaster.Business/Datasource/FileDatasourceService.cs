using ParkingMaster.Business.File;
using ParkingMaster.Business.Input;
using ParkingMaster.Models.Input;
using ParkingMaster.Models.Output;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ParkingMaster.Business.Datasource
{
    public class FileDatasourceService : IDatasourceService
    {
        private readonly ILogger logger;
        private readonly IValidationService validationService;
        private readonly IFileService fileService;
        public string filePath { get; set; }
        public FileDatasourceService(ILogger logger, IValidationService validationService, IFileService fileService)
        {
            this.logger = logger;
            this.validationService = validationService;
            this.fileService = fileService;
        }   

        /// <summary>
        /// Return the result file path
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public string GetSummaryOutput(Summary info)
        {
            var name = $"{filePath.Substring(0, filePath.Length - 4)}_result.txt";
            this.fileService.WriteFile(name, info.ToArray());         
            return name;
        }

        /// <summary>
        /// Receive the data source which is the path of the file to load 
        /// </summary>
        /// <param name="datasource">Datasource file path</param>
        /// <returns>IEnumerable<LogRow></returns>
        public IEnumerable<LogRow> GetValidInputRows(string datasource)
        {
            this.logger.Information($"Getting rows from file {datasource}");
            var inputs = new List<InputEvent>();
            this.filePath = datasource;
            var lines = this.ReadFileLines();

            foreach(string line in lines)
            {
                inputs.Add(GetInput(line));
            }
            this.logger.Information($"Rows {inputs.Count}");

            inputs = (List<InputEvent>)this.validationService.Process(inputs);

            var validInputs = from input in inputs
                              where input.Status == true
                              select new LogRow(input);     
            
            this.logger.Information($"Valid Rows {validInputs.Count()}");

            if(inputs.Count != validInputs.Count())
            {
                var error = "Invalid datasource, there are invalid entries in the input data";
                this.logger.Error(error);
                throw new InvalidProgramException(error);
            }
            
            
            if(validInputs.GroupBy(o => new { o.PlateNumber, o.Time }).Count() != validInputs.Count())
            {
                var error = "A same car wouldn't go in and out at the same time";             
                this.logger.Error(error);
                throw new InvalidProgramException(error);
            }
                      
            return validInputs;
        }

        /// <summary>
        /// Converts from string to InputEvent record
        /// </summary>
        /// <param name="line">String</param>
        /// <returns></returns>
        private InputEvent GetInput(string line)
        {
            string[] fields = line.Split(' ');
            if (fields.Length > 0)
            {
                return new InputEvent() { PlateNumber = fields[0], Time = fields[1], Move = fields[2] };
            }
            return null;
        }

        private string[] ReadFileLines()
        {
            return this.fileService.ReadLinesFile(this.filePath);
        }
    }
}
