// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace IO.ChaTex
{
    using Models;
    using Newtonsoft.Json;

    /// <summary>
    /// The Web API for ChaTex
    /// </summary>
    public partial interface IChaTexWebAPI : System.IDisposable
    {
        /// <summary>
        /// The base URI of the service.
        /// </summary>
        System.Uri BaseUri { get; set; }

        /// <summary>
        /// Gets or sets json serialization settings.
        /// </summary>
        JsonSerializerSettings SerializationSettings { get; }

        /// <summary>
        /// Gets or sets json deserialization settings.
        /// </summary>
        JsonSerializerSettings DeserializationSettings { get; }


        /// <summary>
        /// Gets the IUsers.
        /// </summary>
        IUsers Users { get; }

        /// <summary>
        /// Gets the IGroups.
        /// </summary>
        IGroups Groups { get; }

        /// <summary>
        /// Gets the IChannels.
        /// </summary>
        IChannels Channels { get; }

        /// <summary>
        /// Gets the IMessages.
        /// </summary>
        IMessages Messages { get; }

        /// <summary>
        /// Gets the IRoles.
        /// </summary>
        IRoles Roles { get; }

        /// <summary>
        /// Gets the IChats.
        /// </summary>
        IChats Chats { get; }

    }
}
