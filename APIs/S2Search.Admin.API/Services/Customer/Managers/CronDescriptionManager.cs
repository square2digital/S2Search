using Services.Interfaces.Managers;
using CronExpressionDescriptor;

namespace Services.Managers
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
