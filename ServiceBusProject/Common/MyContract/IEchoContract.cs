

using System.ServiceModel;

namespace MyContract
{
    [ServiceContract]
    public interface IEchoContract
    {
        [OperationContract]
        string Echo(string text);
    }
}
