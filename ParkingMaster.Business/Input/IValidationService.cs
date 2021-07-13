using ParkingMaster.Models.Input;
using System.Collections.Generic;

namespace ParkingMaster.Business.Input
{
    public interface IValidationService
    {
        public IEnumerable<InputEvent> Process(IEnumerable<InputEvent> inputs);
    }
}
