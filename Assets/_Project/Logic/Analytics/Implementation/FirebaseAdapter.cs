using Asteroids.Logic.Analytics.Core;
using System.Collections.Generic;
using System.Linq;

namespace Asteroids.Logic.Analytics.Implementation
{
    public class FirebaseAdapter : IAnalytic
    {
        public void SendEvent(string eventName, IEnumerable<AnalyticParameter> parameters)
        {
            if (parameters.Count() == 0)
            {
                Firebase.Analytics.FirebaseAnalytics.LogEvent(eventName);
            }
            else
            {
                var parametersList = new List<Firebase.Analytics.Parameter>();

                foreach (var parameter in parameters)
                {
                    switch (parameter.Value)
                    {
                        case int:
                            parametersList.Add(new Firebase.Analytics.Parameter(parameter.Name, (int)parameter.Value));
                            break;
                        case double:
                            parametersList.Add(new Firebase.Analytics.Parameter(parameter.Name, (double)parameter.Value));
                            break;
                        case long:
                            parametersList.Add(new Firebase.Analytics.Parameter(parameter.Name, (long)parameter.Value));
                            break;
                        default:
                            parametersList.Add(new Firebase.Analytics.Parameter(parameter.Name, parameter.Value.ToString()));
                            break;
                    }
                }

                Firebase.Analytics.FirebaseAnalytics.LogEvent(eventName, parametersList.ToArray());
            }
        }
    }
}