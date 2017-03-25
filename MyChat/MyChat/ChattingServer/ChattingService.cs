using ChatingInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Collections.Concurrent;
namespace ChattingServer
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ChattingService" in both code and config file together.
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single)]//InstanceContexMode po to żeby się tylko jeden serwer tworzył
    public class ChattingService : IChattingService                                                           //ConcurrencyMode=ConcurrencyMode.Multiple żeby serwer dzieli pomiędzy wątki różnych ludzi wysyłających wiadomości
    {
        public ConcurrentDictionary<string, ConnectedClient> _connectedClients = new ConcurrentDictionary<string, ConnectedClient>();

        public int Login(string userName)
        {
            foreach(var client in _connectedClients)
            {
                if(client.Key.ToLower()==userName.ToLower())
                {
                    return 1;
                }
            }
            var establishedUserConnection = OperationContext.Current.GetCallbackChannel<IClient>();//wywyołuje callback czyli wiemy gdzie wysyłac
            ConnectedClient newClient = new ConnectedClient();//tworze nowa osobe
            newClient.connection = establishedUserConnection;//connection to IClient interface 
            newClient.UserName = userName;//ustawiamy imie

            _connectedClients.TryAdd(userName, newClient); //próbujemy dodać do słownika
            return 0;
        }

        public void SendMessageToALL(string message, string userName)
        {
          foreach(var client in _connectedClients)
            {
                if(client.Key.ToLower() !=userName.ToLower())//żeby smameu sobie nie wysyłąc
                {
                    client.Value.connection.GetMessage(message, userName);
                }
            }
        }
    }
}
