﻿using IO.Swagger.Api;
using IO.Swagger.Client;
using IO.Swagger.Model;
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

namespace ChaTex_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MessagesApi messageApi = new MessagesApi();
        Window Login;
        Window Overview;
        public MainWindow()
        {
            InitializeComponent();
            this.Hide();
            //PromptLogin();// - c for test
            Window ov = new CreateNewGroup();
            ov.Show();
        }
        private void PromptLogin() {
            Login = new Login(this);
            Login.Show();
        }

        public void CompletedLogin() {
            Login.Close();
            Overview = new Overview();
            Overview.Show();
        }

        private void btnSendRequest_Click(object sender, RoutedEventArgs e)
        {

            //var person = new Person();
            //person.Name = "André";

            //var message = new Message();
            //message.Id = 1;
            //message.Sender = person;
            //message.Content = "Hemmelig besked..";
            //message.CreationTime = DateTime.Now;

            ////messageApi.ApiMessagesPost(message);
            //var allMessages = messageApi.ApiMessagesGet();
            //MessageBox.Show("A request has been sent! :) ");

        }
    }
}
