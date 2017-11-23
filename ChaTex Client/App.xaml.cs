using BusinessLayer;
using ChaTex_Client.UserControls;
using IO.ChaTex;
using Microsoft.Rest;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Unity;

namespace ChaTex_Client
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //Initialize API
            ChaTexWebAPI api = new ChaTexWebAPI(new TokenHandler());

            //Register API handlers
            IUnityContainer iocContainer = new UnityContainer();
            iocContainer.RegisterInstance<IUsers>(api.Users);
            iocContainer.RegisterInstance<IGroups>(api.Groups);
            iocContainer.RegisterInstance<IChannels>(api.Channels);
            iocContainer.RegisterInstance<IMessages>(api.Messages);
            iocContainer.RegisterInstance<IRoles>(api.Roles);
            iocContainer.RegisterInstance<IChats>(api.Chats);

            //Register windows
            iocContainer.RegisterType<Login>();
            iocContainer.RegisterType<Overview>();
            iocContainer.RegisterType<GroupView>();
            iocContainer.RegisterType<ChatView>();
            iocContainer.RegisterType<ChannelMessageView>();
            iocContainer.Resolve<Login>().Show();
        }
    }
}
