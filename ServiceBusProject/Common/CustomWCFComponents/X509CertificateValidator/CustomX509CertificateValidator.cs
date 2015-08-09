using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace CustomWCFComponents.X509CertificateValidator
{
    public class CustomX509CertificateValidator : System.IdentityModel.Selectors.X509CertificateValidator
    {
        private List<string> _selfSignedCertificates; 
        public CustomX509CertificateValidator()
        {
            _selfSignedCertificates = new List<string>
            {
                "CN=Johnson.Test.Service",
                "CN=Johnson.Test.Client",
                "CN=JohnsonSTS.Test.Service",
                "CN=JohnsonSTS.Test.Service1",
                "CN=JohnsonSTS.Test.FP",
            };
        }

        public override void Validate(X509Certificate2 certificate)
        {
            // Check that there is a certificate. 
            if (certificate == null)
            {
                throw new ArgumentNullException("certificate");
            }

            // create chain and set validation options
            X509Chain chain = new X509Chain();
            SetValidationSettings(chain);

            // check if cert is valid and chains up to a trusted CA
            if (!chain.Build(certificate))
            {
                foreach (X509ChainStatus status in chain.ChainStatus)
                {
                    if ((certificate.Subject == certificate.Issuer) &&
                        (status.Status == X509ChainStatusFlags.UntrustedRoot))
                    {
                        // Self-signed certificates with an untrusted root are valid. 
                        // You could do further check
                        if (_selfSignedCertificates.Contains(certificate.Subject))
                        {
                            continue;
                        }
                        else
                        {
                            throw new SecurityTokenValidationException(
                                "Client certificate is not valid"); 
                        }
                    }
                    
                    // CN=idsrv3test has a different issuer aka not self signed
                    // And status is also not as untrusted root
                    else if ((certificate.Subject == "CN=idsrv3test"))
                    {
                        // Self-signed certificates with an untrusted root are valid. 
                        // You could do further check
                        continue;
                    }
                    else
                    {
                        throw new SecurityTokenValidationException(
                        "Client certificate is not valid"); 
                    }
                }
           
            }

           
        }

        private void SetValidationSettings(X509Chain chain)
        {
            chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
            chain.ChainPolicy.RevocationFlag = X509RevocationFlag.ExcludeRoot;
            chain.ChainPolicy.VerificationFlags = X509VerificationFlags.NoFlag;
            chain.ChainPolicy.VerificationTime = DateTime.Now;
            chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 0, 0);
        }
    }
}
