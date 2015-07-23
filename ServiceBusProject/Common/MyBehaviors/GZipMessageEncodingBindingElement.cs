
using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Xml;

namespace MyBehaviors
{
    public class GZipMessageEncodingBindingElement 
          : MessageEncodingBindingElement //BindingElement
          , IPolicyExportExtension
    {

        //We will use an inner binding element to store information required for the inner encoder
        MessageEncodingBindingElement innerBindingElement;

        //By default, use the default text encoder as the inner encoder
        public GZipMessageEncodingBindingElement()
            : this(new TextMessageEncodingBindingElement()) { }

        public GZipMessageEncodingBindingElement(MessageEncodingBindingElement messageEncoderBindingElement)
        {
            this.innerBindingElement = messageEncoderBindingElement;
        }

        public MessageEncodingBindingElement InnerMessageEncodingBindingElement
        {
            get { return innerBindingElement; }
            set { innerBindingElement = value; }
        }

     

        //Main entry point into the encoder binding element. Called by WCF to get the factory that will create the
        //message encoder
        public override MessageEncoderFactory CreateMessageEncoderFactory()
        {
            return new GZipMessageEncoderFactory(innerBindingElement.CreateMessageEncoderFactory());
        }

        public override MessageVersion MessageVersion
        {
            get { return innerBindingElement.MessageVersion; }
            set { innerBindingElement.MessageVersion = value; }
        }

        public override BindingElement Clone()
        {
            return new GZipMessageEncodingBindingElement(this.innerBindingElement);
        }


        void IPolicyExportExtension.ExportPolicy(MetadataExporter exporter, PolicyConversionContext policyContext)
        {
            if (policyContext == null)
            {
                throw new ArgumentNullException("policyContext");
            }
            XmlDocument document = new XmlDocument();
            policyContext.GetBindingAssertions().Add(document.CreateElement(
                GZipMessageEncodingPolicyConstants.GZipEncodingPrefix,
                GZipMessageEncodingPolicyConstants.GZipEncodingName,
                GZipMessageEncodingPolicyConstants.GZipEncodingNamespace));
        }

        #region Is this useful? yes, otehrwise CreateMessageEncoderFactory will not be called
        public override T GetProperty<T>(BindingContext context)
        {
            //if XmlDictionaryReaderQuotas, direct it to innerBindingElement
            if (typeof(T) == typeof(System.Xml.XmlDictionaryReaderQuotas))
            {
                return innerBindingElement.GetProperty<T>(context);
            }
            else
            {
                return base.GetProperty<T>(context);
            }
        }

        // Must implement this, otherwise current binding element is not added, even though you added to custombinding
        public override IChannelFactory<TChannel> BuildChannelFactory<TChannel>(BindingContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            context.BindingParameters.Add(this);
            return context.BuildInnerChannelFactory<TChannel>();
        }

        // Must implement this, otherwise current binding element is not added, even though you added to custombinding
        public override IChannelListener<TChannel> BuildChannelListener<TChannel>(BindingContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            context.BindingParameters.Add(this);
            return context.BuildInnerChannelListener<TChannel>();
        }

        // Must implement this, otherwise current binding element is not added, even though you added to custombinding
        public override bool CanBuildChannelListener<TChannel>(BindingContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            context.BindingParameters.Add(this);
            return context.CanBuildInnerChannelListener<TChannel>();
        }
        #endregion
    }
}
