using Asteroids.Logic.Analytics.Core;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Asteroids.Logic.Analytics.Implementation
{
    public class TestAnalytic : IAnalytic
    {
        public void SendEvent(string eventName, IEnumerable<AnalyticParameter> args)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(eventName);
            foreach (var arg in args)
                builder.AppendLine(arg.ToString());
            Debug.Log(builder.ToString());
        }
    }
}