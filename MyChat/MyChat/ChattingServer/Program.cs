using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ChattingServer
{
    class Program
    {
        public static ChattingService _server;//static bo w Chating server w atrybucie jest InstanceContextMode =InstanceContextMode.Single
        static void Main(string[] args)
        {
            _server = new ChattingService();
            using (ServiceHost host = new ServiceHost(_server))
            {
                host.Open();
                Console.WriteLine("Server is running...");
                Console.ReadLine();

            }
        }
    }
}
