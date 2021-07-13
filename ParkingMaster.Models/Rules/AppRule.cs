using ParkingMaster.Models.Rules.Type;

namespace ParkingMaster.Models.Rules
{
    public class AppRule
    {
        public RuleType Type { get; set; }
        public string RegexValidation { get; set; }
        public bool Status { get; set; }

        public AppRule()
        {
            this.Status = false;
        }

    }
}
