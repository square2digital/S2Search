using CronExpressionDescriptor;
using S2Search.Backend.Services.Admin.Customer.Interfaces.Managers;

namespace S2Search.Backend.Services.Admin.Customer.Managers
{
    public class CronDescriptionManager : ICronDescriptionManager
    {
        public string Get(string cronExpression)
        {
            var cronDescriptorOptions = new Options()
            {
                DayOfWeekStartIndexZero = false,
                Use24HourTimeFormat = true,
                Locale = "en"
            };

            string friendlyDescription = ExpressionDescriptor.GetDescription(cronExpression, cronDescriptorOptions);

            return friendlyDescription;

        }
    }
}
