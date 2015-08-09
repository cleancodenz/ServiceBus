

using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace TestCertificates
{
    public class MyCertificates
    {
        public static X509Certificate2 GetTestServiceCertificate()
        {
            return GetEmbededCertificateWithPrivateKey("TestCertificates.Johnson.Test.Service.pfx", "1234");
      
        }
        public static X509Certificate2 GetTestServiceCertificatePublicKeyOnly()
        {
            return GetEmbededCertificateWithPublicKey("TestCertificates.Johnson.Test.Service.cer");

        }


        public static X509Certificate2 GetTestClientCertificate()
        {
            return GetEmbededCertificateWithPrivateKey("TestCertificates.Johnson.Test.Client.pfx", "1234");
      
        }

        public static X509Certificate2 GetTestClientCertificatePublicKeyOnly()
        {
            return GetEmbededCertificateWithPublicKey("TestCertificates.Johnson.Test.Client.cer");

        }

        public static X509Certificate2 GetSTSSecurityTokenServiceCertificate()
        {
            return GetEmbededCertificateWithPrivateKey("TestCertificates.JohnsonSTS.Test.Service.pfx", "1234");
        }

        public static X509Certificate2 GetSTSSecurityTokenServiceCertificatePublicKeyOnly()
        {
            return GetEmbededCertificateWithPublicKey("TestCertificates.JohnsonSTS.Test.Service.cer");
        }

         public static X509Certificate2 GetSTSServerCertificate()
         {
             return GetEmbededCertificateWithPrivateKey("TestCertificates.JohnsonSTS.Test.Service1.pfx", "1234");
         }

         public static X509Certificate2 GetSTSServerCertificatePublicKeyOnly()
         {
             return GetEmbededCertificateWithPublicKey("TestCertificates.JohnsonSTS.Test.Service1.cer");
         }

         public static X509Certificate2 GetSTSServerFPCertificate()
         {
             return GetEmbededCertificateWithPrivateKey("TestCertificates.JohnsonSTS.Test.FP.pfx", "1234");
         }

         public static X509Certificate2 GetSTSServerFPCertificatePublicKeyOnly()
         {
             return GetEmbededCertificateWithPublicKey("TestCertificates.JohnsonSTS.Test.FP.cer");
         }

        private static X509Certificate2 GetEmbededCertificateWithPrivateKey(string name, string password)
        {
            X509Certificate2 certificate;

            // Note: in a real world app, you'd probably prefer storing the X.509 certificate
            // in the user or machine store. To keep this sample easy to use, the certificate
            // is extracted from the Certificate.pfx file embedded in this assembly.
            using (
                var stream =
                    typeof(MyCertificates).Assembly.GetManifestResourceStream(
                        name))
            using (var buffer = new MemoryStream())
            {
                stream.CopyTo(buffer);
                buffer.Flush();

                certificate = new X509Certificate2(
                    rawData: buffer.GetBuffer(),
                    password: password);
            }

            return certificate;
        }


        private static X509Certificate2 GetEmbededCertificateWithPublicKey(string name)
        {
            X509Certificate2 certificate;

            // Note: in a real world app, you'd probably prefer storing the X.509 certificate
            // in the user or machine store. To keep this sample easy to use, the certificate
            // is extracted from the Certificate.pfx file embedded in this assembly.
            using (
                var stream =
                    typeof(MyCertificates).Assembly.GetManifestResourceStream(
                        name))
            using (var buffer = new MemoryStream())
            {
                stream.CopyTo(buffer);
                buffer.Flush();

                certificate = new X509Certificate2(
                    rawData: buffer.GetBuffer());
            }

            return certificate;
        }


    }
}
