
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;

namespace MyBehaviors
{
    public class MyCustomBindingCollectionElement : BindingCollectionElement
    {
        public override Type BindingType
        {
            get { return typeof(MyCustomBinding); }
        }

        public override ReadOnlyCollection<IBindingConfigurationElement> ConfiguredBindings
        {
            get
            {
                return new ReadOnlyCollection<IBindingConfigurationElement>(
                new List<IBindingConfigurationElement>());
            }
        }

        public override bool ContainsKey(string name)
        {
            throw new System.NotImplementedException();
        }

        protected override Binding GetDefault()
        {
            return new MyCustomBinding();
        }

        protected override bool TryAdd(string name, Binding binding, Configuration config)
        {
            throw new System.NotImplementedException();
        }
    }
}
