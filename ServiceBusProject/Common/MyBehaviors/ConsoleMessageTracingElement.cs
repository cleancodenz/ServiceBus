

using System;
using System.ServiceModel.Configuration;

namespace MyBehaviors
{
    public class ConsoleMessageTracingElement : BehaviorExtensionElement
    {
        public override Type BehaviorType
        {
            get
            {
                return typeof(ConsoleMessageTracing);
                
            }
        }

        protected override object CreateBehavior()
        {
            return new ConsoleMessageTracing();
        }
    }
}
