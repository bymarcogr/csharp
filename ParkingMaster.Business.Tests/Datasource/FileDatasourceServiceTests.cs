using Moq;
using ParkingMaster.Business.Datasource;
using ParkingMaster.Business.File;
using ParkingMaster.Business.Input;
using ParkingMaster.Models.Input;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ParkingMaster.Business.Tests.Datasource
{
    public class FileDatasourceServiceTests
    {
        private Mock<ILogger> loggerMock = new Mock<ILogger>();

        private Mock<IValidationService> validationService;

        private Mock<IFileService> fileService;
        private IDatasourceService fileDatasourceService { get; set; }
        public FileDatasourceServiceTests()
        {
            this.loggerMock = new Mock<ILogger>();
            this.validationService = new Mock<IValidationService>();
            this.fileService = new Mock<IFileService>();
        }

        [Fact]
        public void GetValidInputRows_DataSource_ShouldSuccess()
        {
            string[] rawData = new string[] { "99000 10 0", "99001 20 0", "9902 30 1" };

            fileService.Setup(o => o.ReadLinesFile(It.IsAny<string>())).Returns(rawData);
            validationService.Setup(o => o.Process(It.IsAny<List<InputEvent>>())).Returns(new List<InputEvent>() {
            new InputEvent(){ PlateNumber="99000", Move= "0", Time= "10", Status = true    },
            new InputEvent(){ PlateNumber="99001", Move= "0", Time= "20", Status = true    },
            new InputEvent(){ PlateNumber="9902", Move= "1", Time= "30", Status = true    }
            });


            this.fileDatasourceService = new FileDatasourceService(this.loggerMock.Object, this.validationService.Object, fileService.Object);
            var list = this.fileDatasourceService.GetValidInputRows("File");
            Assert.Equal(3, list.Count());

        }

        [Fact]
        public void GetValidInputRows_DataSourceBadInput_ShouldThrownException()
        {
            string[] rawData = new string[] { "99000 10 0", "99001 20 0", "9902 30 1", "9902A 30 1" };

            fileService.Setup(o => o.ReadLinesFile(It.IsAny<string>())).Returns(rawData);
            validationService.Setup(o => o.Process(It.IsAny<List<InputEvent>>())).Returns(new List<InputEvent>() {
            new InputEvent(){ PlateNumber="99000", Move= "0", Time= "10", Status = true},
            new InputEvent(){ PlateNumber="99001", Move= "0", Time= "20", Status = true},
            new InputEvent(){ PlateNumber="9902", Move= "1", Time= "30", Status = true },
             new InputEvent(){ PlateNumber="9902A", Move= "1", Time= "30", Status = false}
            });

            this.fileDatasourceService = new FileDatasourceService(this.loggerMock.Object, this.validationService.Object, fileService.Object);
            var exception = Assert.Throws<InvalidProgramException>(() => this.fileDatasourceService.GetValidInputRows("File"));

            Assert.Equal("Invalid datasource, there are invalid entries in the input data", exception.Message);
        }

        [Fact]
        public void GetValidInputRows_DataSourceDuplicatedInput_ShouldThrownException()
        {
            string[] rawData = new string[] { "99000 10 0", "99001 20 0", "9902 30 1", "99000 10 1" };

            fileService.Setup(o => o.ReadLinesFile(It.IsAny<string>())).Returns(rawData);
            validationService.Setup(o => o.Process(It.IsAny<List<InputEvent>>())).Returns(new List<InputEvent>() {
            new InputEvent(){ PlateNumber="99000", Move= "0", Time= "10", Status = true},
            new InputEvent(){ PlateNumber="99001", Move= "0", Time= "20", Status = true},
            new InputEvent(){ PlateNumber="9902", Move= "1", Time= "30", Status = true },
             new InputEvent(){ PlateNumber="99000", Move= "1", Time= "10", Status = true}
            });

            this.fileDatasourceService = new FileDatasourceService(this.loggerMock.Object, this.validationService.Object, fileService.Object);
            var exception = Assert.Throws<InvalidProgramException>(() => this.fileDatasourceService.GetValidInputRows("File"));

            Assert.Equal("A same car wouldn't go in and out at the same time", exception.Message);
        }
    }
}
