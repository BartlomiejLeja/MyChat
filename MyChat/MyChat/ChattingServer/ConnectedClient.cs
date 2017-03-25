using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatingInterfaces;
namespace ChattingServer
{
   public class ConnectedClient
    {
        public IClient connection; //callback kontrak
        public string UserName { get; set; }
    }
}
