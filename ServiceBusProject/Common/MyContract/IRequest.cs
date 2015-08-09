

using System.ServiceModel;

namespace MyContract
{
    [ServiceContract]
    public interface IRequest
    {
        [OperationContract(IsOneWay = true)]
        void SendMessage(string message);

        [OperationContract(IsOneWay = true)]
        [ReceiveContextEnabled(ManualControl = true)] 
        void SendMessageWithContext(string message);

    }


}
