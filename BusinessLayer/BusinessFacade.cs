using IO.ChaTex;

namespace BusinessLayer
{
    /// <summary>
    /// This class is used to hide some of the dependencies used by the API code. That way the API can be initialized
    /// by the UI without making the UI dependent on the NuGet packages used by the business layer.
    /// </summary>
    public class BusinessFacade
    {
        private readonly IChaTexWebAPI api;

        public IUsers Users { get => api.Users; }
        public IGroups Groups { get => api.Groups; }
        public IChannels Channels { get => api.Channels; }
        public IMessages Messages { get => api.Messages; }
        public IRoles Roles { get => api.Roles; }
        public IChats Chats { get => api.Chats; }

        public BusinessFacade()
        {
            api = new ChaTexWebAPI();
        }
    }
}
