using ParkingMaster.Models.Input;
using ParkingMaster.Models.Rules;
using ParkingMaster.Models.Rules.Type;
using Serilog;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ParkingMaster.Business.Input
{
    public class ValidationService : IValidationService
    {
        private readonly ILogger logger;
        private readonly AppRule PlateRule;
        private readonly AppRule TimeRule;
        private readonly AppRule MoveRule;
        private delegate void ValidateInput(InputEvent input);

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger">Unity logger resolved</param>
        /// <param name="rules">Configuration rules</param>
        public ValidationService(ILogger logger, AppRule[] rules)
        {
            this.logger = logger;
            this.PlateRule = rules.ToList().FirstOrDefault(o => o.Type == RuleType.PLATE);
            this.TimeRule = rules.ToList().FirstOrDefault(o => o.Type == RuleType.TIME);
            this.MoveRule = rules.ToList().FirstOrDefault(o => o.Type == RuleType.MOVE);

        }
        /// <summary>
        /// Validate input data using the rules contained in the appsettings
        /// </summary>
        /// <param name="inputs">Input rows</param>
        public IEnumerable<InputEvent> Process(IEnumerable<InputEvent> inputs)
        {
            this.logger.Information("Validating input data rules");

            ValidateInput delvalid = ValidateAllRules;
            
            inputs.ToList().ForEach(o => delvalid(o));
            
            this.logger.Information("Input data rules validation completed");
            return inputs;
        }

        /// <summary>
        /// Validate vehicle status depending on the input datasource
        /// </summary>
        /// <param name="input"></param>
        private void ValidateAllRules(InputEvent input)
        {
            input.Status =
                (this.PlateRule.Status && new Regex(this.PlateRule.RegexValidation).IsMatch(input.PlateNumber))
                && (this.TimeRule.Status && new Regex(this.TimeRule.RegexValidation).IsMatch(input.Time))
                && (this.MoveRule.Status && new Regex(this.MoveRule.RegexValidation).IsMatch(input.Move));
        }
    }
}
