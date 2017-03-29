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
            updateHelper(0, userName);//0 że zalogowane
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Client login: {0} at {1}", newClient.UserName, System.DateTime.Now);
            Console.ResetColor();
            return 0;
        }

        //wylogowywanie się
        public void LogOut()
        {
            ConnectedClient client = GetMyClient();
            if(client !=null)
            {
                ConnectedClient removedClient;

                _connectedClients.TryRemove(client.UserName, out removedClient);
                updateHelper(1, removedClient.UserName);//1 że wylogowane
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Client logoff: {0} at {1}", removedClient.UserName, System.DateTime.Now);
                Console.ResetColor();
            }
        }
        // sprawdza czy klient połaczany w celu uzycia w logOut i wylogowania
        public ConnectedClient GetMyClient()
        {
            var establishedUserConnection = OperationContext.Current.GetCallbackChannel<IClient>();//wywyołuje callback czyli wiemy gdzie wysyłac
            foreach(var client in _connectedClients)
            {
                if(client.Value.connection == establishedUserConnection)//sprawdza czy są w kolekcji ja nie null mozna zrobic linq
                {
                    return client.Value;
                }
            }
            return null;
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

        private void updateHelper(int value, string userName)
        {
            foreach (var client in _connectedClients)
            {
                if (client.Value.UserName.ToLower() != userName.ToLower())
                {
                    client.Value.connection.GetUpdate(value, userName);
                }
            }
        }

        public List<string> GetCurrentUsers()
        {
            List<string> listOfUsers = new List<string>();
            foreach(var client in _connectedClients)
            {
                listOfUsers.Add(client.Value.UserName);
            }
            return listOfUsers;
        }
    }
}
