// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace IO.ChaTex
{
    using Microsoft.Rest;
    using Models;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Chats operations.
    /// </summary>
    public partial interface IChats
    {
        /// <summary>
        /// Create a new chat with only the caller as a member
        /// </summary>
        /// <param name='chatName'>
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="Microsoft.Rest.HttpOperationException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="Microsoft.Rest.SerializationException">
        /// Thrown when unable to deserialize the response
        /// </exception>
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown when a required parameter is null
        /// </exception>
        Task<HttpOperationResponse<ChatDTO>> CreateChatWithHttpMessagesAsync(string chatName, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Get a list of all messages in a chat
        /// </summary>
        /// <param name='chatId'>
        /// The id of the message to get
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="Microsoft.Rest.HttpOperationException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="Microsoft.Rest.SerializationException">
        /// Thrown when unable to deserialize the response
        /// </exception>
        Task<HttpOperationResponse<IList<GetMessageDTO>>> GetMessagesInChatWithHttpMessagesAsync(int chatId, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Add users to a chat
        /// </summary>
        /// <remarks>
        /// This will add a list of users to a specific chat
        /// </remarks>
        /// <param name='addUsersToChatDTO'>
        /// Users to be added to a chat
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="Microsoft.Rest.HttpOperationException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        Task<HttpOperationResponse> AddUsersToChatWithHttpMessagesAsync(AddUsersToChatDTO addUsersToChatDTO = default(AddUsersToChatDTO), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Get alle chats for the user
        /// </summary>
        /// <remarks>
        /// Gets all chats for the specific user id
        /// </remarks>
        /// <param name='userId'>
        /// The id of the user
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        /// <exception cref="Microsoft.Rest.HttpOperationException">
        /// Thrown when the operation returned an invalid status code
        /// </exception>
        /// <exception cref="Microsoft.Rest.SerializationException">
        /// Thrown when unable to deserialize the response
        /// </exception>
        Task<HttpOperationResponse<IList<ChatDTO>>> GetAllChatsForUserWithHttpMessagesAsync(int userId, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
    }
}
