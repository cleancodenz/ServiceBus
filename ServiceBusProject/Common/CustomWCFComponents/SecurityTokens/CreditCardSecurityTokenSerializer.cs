

using System;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;
using System.ServiceModel.Security;
using System.Xml;

namespace CustomWCFComponents.SecurityTokens
{
    /// <summary>
    /// Derived keys are enabled by default. 
    /// If you create a custom security token and use it as the primary token, 
    /// WCF derives a key from it. 
    /// While doing so, it calls the custom security token serializer to write the SecurityKeyIdentifierClause for the custom security token 
    /// while serializing the DerivedKeyToken to the wire. 
    /// On the receiving end, when deserializing the token off the wire, 
    /// the DerivedKeyToken serializer expects a SecurityTokenReference element as the top-level child under itself.
    /// If the custom security token serializer did not add a SecurityTokenReference element while serializing its clause type, 
    /// an exception is thrown.
    /// </summary>
    public class CreditCardSecurityTokenSerializer : WSSecurityTokenSerializer
    {
        public CreditCardSecurityTokenSerializer(SecurityTokenVersion version) : base() { }

        /// <summary>
        /// Override the CanReadTokenCore(XmlReader) method, 
        /// which relies on an XmlReader to read the XML stream. 
        /// The method returns true if the serializer implementation can deserialize the security token based given its current element. 
        /// In this example, this method checks whether the XML reader's current XML element has the correct element name and namespace. 
        /// If it does not, it calls the base class implementation of this method to handle the XML element.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected override bool CanReadTokenCore(XmlReader reader)
        {
            XmlDictionaryReader localReader = XmlDictionaryReader.CreateDictionaryReader(reader);
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }
            if (reader.IsStartElement(CreditCardTokenConstants.CreditCardTokenName,
                CreditCardTokenConstants.CreditCardTokenNamespace))
            {
                return true;
            }
            return base.CanReadTokenCore(reader);
        }

        /// <summary>
        /// Override the ReadTokenCore(XmlReader, SecurityTokenResolver) method. 
        /// This method reads the XML content of the security token and constructs the appropriate in-memory representation for it. 
        /// If it does not recognize the XML element on which the passed-in XML reader is standing, 
        /// it calls the base class implementation to process the system-provided token types.
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="tokenResolver"></param>
        /// <returns></returns>
        protected override SecurityToken ReadTokenCore(XmlReader reader, SecurityTokenResolver tokenResolver)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("reader");
            }
            if (reader.IsStartElement(CreditCardTokenConstants.CreditCardTokenName, CreditCardTokenConstants.CreditCardTokenNamespace))
            {
                string id = reader.GetAttribute(CreditCardTokenConstants.Id, CreditCardTokenConstants.WsUtilityNamespace);

                reader.ReadStartElement();

                // Read the credit card number.
                string creditCardNumber = reader.ReadElementString(CreditCardTokenConstants.CreditCardNumberElementName, CreditCardTokenConstants.CreditCardTokenNamespace);

                // Read the expiration date.
                string expirationTimeString = reader.ReadElementString(CreditCardTokenConstants.CreditCardExpirationElementName, CreditCardTokenConstants.CreditCardTokenNamespace);
                DateTime expirationTime = XmlConvert.ToDateTime(expirationTimeString, XmlDateTimeSerializationMode.Utc);

                // Read the issuer of the credit card.
                string creditCardIssuer = reader.ReadElementString(CreditCardTokenConstants.CreditCardIssuerElementName, CreditCardTokenConstants.CreditCardTokenNamespace);
                reader.ReadEndElement();

                CreditCardInfo cardInfo = new CreditCardInfo(creditCardNumber, creditCardIssuer, expirationTime);

                return new CreditCardToken(cardInfo, id);
            }
            else
            {
                return WSSecurityTokenSerializer.DefaultInstance.ReadToken(reader, tokenResolver);
            }
        }

        /// <summary>
        /// Override the CanWriteTokenCore(SecurityToken) method. 
        /// This method returns true if it can convert the in-memory token representation (passed in as an argument) to the XML representation. 
        /// If it cannot convert, it calls the base class implementation.
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        protected override bool CanWriteTokenCore(SecurityToken token)
        {
            if (token is CreditCardToken)
            {
                return true;
            }
            else
            {
                return base.CanWriteTokenCore(token);
            }
        }

        /// <summary>
        /// Override the WriteTokenCore(XmlWriter, SecurityToken) method. 
        /// This method converts an in-memory security token representation into an XML representation. 
        /// If the method cannot convert, it calls the base class implementation
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="token"></param>
        protected override void WriteTokenCore(XmlWriter writer, SecurityToken token)
        {
            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }
            if (token == null)
            {
                throw new ArgumentNullException("token");
            }

            CreditCardToken c = token as CreditCardToken;
            if (c != null)
            {
                writer.WriteStartElement(CreditCardTokenConstants.CreditCardTokenPrefix, CreditCardTokenConstants.CreditCardTokenName, CreditCardTokenConstants.CreditCardTokenNamespace);
                writer.WriteAttributeString(CreditCardTokenConstants.WsUtilityPrefix, CreditCardTokenConstants.Id, CreditCardTokenConstants.WsUtilityNamespace, token.Id);
                writer.WriteElementString(CreditCardTokenConstants.CreditCardNumberElementName, CreditCardTokenConstants.CreditCardTokenNamespace, c.CardInfo.CardNumber);
                writer.WriteElementString(CreditCardTokenConstants.CreditCardExpirationElementName, CreditCardTokenConstants.CreditCardTokenNamespace, XmlConvert.ToString(c.CardInfo.ExpirationDate, XmlDateTimeSerializationMode.Utc));
                writer.WriteElementString(CreditCardTokenConstants.CreditCardIssuerElementName, CreditCardTokenConstants.CreditCardTokenNamespace, c.CardInfo.CardIssuer);
                writer.WriteEndElement();
                writer.Flush();
            }
            else
            {
                base.WriteTokenCore(writer, token);
            }
        }
    }
}
