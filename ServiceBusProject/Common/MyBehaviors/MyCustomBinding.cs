

using System.ServiceModel.Channels;


namespace MyBehaviors
{
    public class MyCustomBinding : Binding
    {
        private HttpTransportBindingElement transport;
        private BinaryMessageEncodingBindingElement encoding;

        public MyCustomBinding()
            : base()
        {
            this.transport = new HttpTransportBindingElement();
            this.encoding = new BinaryMessageEncodingBindingElement();
        }

        public override BindingElementCollection CreateBindingElements()
        {
            BindingElementCollection elements = new BindingElementCollection();
            elements.Add(this.encoding);
            elements.Add(this.transport);
            return elements;
        }

        public override string Scheme
        {
            get { return this.transport.Scheme; }
        }

        
    }
}
