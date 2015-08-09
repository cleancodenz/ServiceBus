

using System.ServiceModel;
namespace MyContract
{
   [ServiceContract]
   public interface IEchoRestContract
    {
       [OperationContract]
       string Echo(string text);

       [OperationContract]
       string SimpleText();
    }
}
