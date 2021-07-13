using Moq;
using ParkingMaster.Business.Input;
using ParkingMaster.Models.Input;
using ParkingMaster.Models.Rules;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq; 

namespace ParkingMaster.Business.Tests.Input
{
    public class ValidationServiceTests
    {
        private Mock<ILogger> loggerMock = new Mock<ILogger>();

        private  IValidationService validationService;
        private AppRule[] rules;
        private List<InputEvent> InputValues;

        public ValidationServiceTests()
        {
            this.loggerMock = new Mock<ILogger>();
            this.rules = new AppRule[]
            {
                new AppRule(){ Type = Models.Rules.Type.RuleType.PLATE, RegexValidation = "^([1-9][0-9]{4})$", Status = true },
                new AppRule(){ Type = Models.Rules.Type.RuleType.TIME, RegexValidation = "^((86400)|(86[0-3][0-9]{2})|(8[0-5][0-9]{3})|([0-7]?[0-9]{1,4}))$", Status = true },
                new AppRule(){ Type = Models.Rules.Type.RuleType.MOVE, RegexValidation = "^(0|1)$", Status = true }
            };

            this.InputValues = new List<InputEvent>()
            {
                new InputEvent()
                {
                    PlateNumber = "87845",
                    Time = "57845",
                    Status = true,
                    Move = "0"
                },
                 new InputEvent()
                {
                    PlateNumber = "97845",
                    Time = "27845",
                    Status = true,
                    Move = "0"
                }
            };
           
        }
        [Fact]
        public void Process_CorrectInput_ShouldSuccess()
        {
            this.validationService = new ValidationService(loggerMock.Object, this.rules);
            this.validationService.Process(this.InputValues);

            Assert.Equal(3, this.InputValues.Count);
            Assert.False(this.InputValues.Where(o => o.Status == false).Any());         
        }

        [Fact]
        public void Process_IncorrectInput_ContainThreeDeactivate_ShouldSuccess()
        {
            this.InputValues.AddRange(new List<InputEvent>()
            {
                   new InputEvent()
                   {
                       PlateNumber = "3",
                       Time = "200A",
                       Status = true,
                       Move = "0"
                   },
                     new InputEvent()
                     {
                         PlateNumber = "A",
                         Time = "200",
                         Status = true,
                         Move = "0"
                     },
                new InputEvent()
                {
                    PlateNumber = "2",
                    Time = "200",
                    Status = true,
                    Move = "3"
                } 
            });

            this.validationService = new ValidationService(loggerMock.Object, this.rules);
            this.validationService.Process(this.InputValues);

            Assert.Equal(5, this.InputValues.Count);
            Assert.True(this.InputValues.Where(o => o.Status == false).Any());
            Assert.Equal(3, this.InputValues.Where(o => o.Status == false).Count());
        }
    }
}
