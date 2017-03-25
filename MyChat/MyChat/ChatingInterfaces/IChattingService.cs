using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace ChatingInterfaces //ServiceLibrary fajne oddzielenie interfejsów od reszty
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IChattingService" in both code and config file together.
    [ServiceContract(CallbackContract =typeof(IClient))]
    public interface IChattingService
    {
        [OperationContract]
         int Login(string userName);
        [OperationContract]
         void SendMessageToALL(string message, string userName);
            
     }
}
