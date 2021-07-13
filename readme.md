# Parking Master Solution
You are a software consultant for Parking Masters, a company that sells parking lot management software.  Parking Masters would like you to write a routine that takes a log of car movements and returns information about the state of a parking lot. Pay attention to all function and non-function requirements and treat this as if you were coding as a member of the Parking Master team.

## Requirements
- Net Core 3.0.1 LTS

## Dependencies
- Serilog
- Serilog.Sinks.File
- Unity
- Newtonsoft.Json


## Projects
Below you will find al the projects contained in this solution.

### ParkingMaster.Business
- Services incharge of core bussiness and main services

### ParkingMaster.ConsoleApp
- Main console app 

### ParkingMaster.Management
- Project in charge of dependency injection, where all the services are declares, if we move this app into an Web API, this is the main services

- **appsettings.json** main configuration, it manages log name and location and load field rules, it uses regular expression to validate some of the rules automatically.

### ParkingMaster.Models
-  General models using by all the projects

### ParkingMaster.Utilities
- General utilities