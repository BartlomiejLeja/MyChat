using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using ChatingInterfaces;
using System.Windows;

namespace MyChat
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class ClientCallback : IClient
    {
        public void GetMessage(string message, string userName)
        {
            ((MainWindow)Application.Current.MainWindow).TakeMessage(message, userName);//dostanie sie do okna aplikacji nieźły casting
        }

        public void GetUpdate(int value, string userName)
        {
            switch (value)
            { 
                case 0:
                {
                    ((MainWindow)Application.Current.MainWindow).AddUserToList(userName);
                    break;
                }
            case 1:
                {
                    ((MainWindow)Application.Current.MainWindow).RemoveUserToList(userName);
                    break;
                }
            }
        }

    }
}