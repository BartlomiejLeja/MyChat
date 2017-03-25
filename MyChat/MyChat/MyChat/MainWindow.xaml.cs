using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ChatingInterfaces;
using System.ServiceModel;

namespace MyChat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static IChattingService Server;
        private static DuplexChannelFactory<IChattingService> _channelFactory;
        public MainWindow()
        {
            InitializeComponent();
            _channelFactory = new DuplexChannelFactory<IChattingService>(new ClientCallback(), "ChattingServiceEndPoint");
            Server = _channelFactory.CreateChannel();
        }

        public void TakeMessage(string message, string userName)
        {
           TextDispalyTexBox.Text +=userName + ": " + message+ "\n";
            TextDispalyTexBox.ScrollToEnd();
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if(MessageTextBox.Text.Length==0)
            {
                return;
            }
            Server.SendMessageToALL(MessageTextBox.Text, UserNameTextBox.Text);
            TakeMessage(MessageTextBox.Text, "You");
            MessageTextBox.Text = "";
          
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            int returnValue = Server.Login(UserNameTextBox.Text);
            if (returnValue == 1)
            {
                MessageBox.Show("You are alredy log");
            }
            else
            {

                MessageBox.Show("You log");
                WelcomLabel.Content = "Welcom" + UserNameTextBox.Text;
                UserNameTextBox.IsEnabled = false;
                LoginButton.IsEnabled = false;
            }

        }
    }
}
