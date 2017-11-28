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
    /// Messages operations.
    /// </summary>
    public partial interface IMessages
    {
        /// <summary>
        /// Get the messages from a specific channel
        /// </summary>
        /// <remarks>
        /// Get a number of messages from the specified channel
        /// </remarks>
        /// <param name='channelId'>
        /// The id of the channel to delete
        /// </param>
        /// <param name='fromIndex'>
        /// The index of the first message to get, beginning from the most
        /// recently posted message. This defaults to 0, meaning the most
        /// recent message
        /// </param>
        /// <param name='count'>
        /// The amount of messages to get. This defaults to 25
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
        Task<HttpOperationResponse<IList<GetMessageDTO>>> GetMessagesWithHttpMessagesAsync(int channelId, int? fromIndex = default(int?), int? count = default(int?), Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Create a new message
        /// </summary>
        /// <remarks>
        /// Create a new message in a specific channel
        /// </remarks>
        /// <param name='channelId'>
        /// The id of the channel to post the message to
        /// </param>
        /// <param name='messageContentDTO'>
        /// Content of the message
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
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown when a required parameter is null
        /// </exception>
        Task<HttpOperationResponse> CreateMessageWithHttpMessagesAsync(int channelId, MessageContentDTO messageContentDTO, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Wait for and get new messages, message deletions, and message edits
        /// in a channel
        /// </summary>
        /// <remarks>
        /// This request will not return from the service until at least one
        /// new message event has occurred
        /// </remarks>
        /// <param name='channelId'>
        /// The id of the channel to listen to
        /// </param>
        /// <param name='since'>
        /// The time to get message events since
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
        Task<HttpOperationResponse<IList<MessageEventDTO>>> GetMessageEventsWithHttpMessagesAsync(int channelId, System.DateTime since, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Delete a message
        /// </summary>
        /// <remarks>
        /// Delete the message with the specified id
        /// </remarks>
        /// <param name='messageId'>
        /// The id of the message to delete
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
        Task<HttpOperationResponse> DeleteMessageWithHttpMessagesAsync(int messageId, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Edit a message
        /// </summary>
        /// <remarks>
        /// Edit the message with the specified id
        /// </remarks>
        /// <param name='messageId'>
        /// The id of the message to delete
        /// </param>
        /// <param name='newContent'>
        /// The new content of the message
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
        /// <exception cref="Microsoft.Rest.ValidationException">
        /// Thrown when a required parameter is null
        /// </exception>
        Task<HttpOperationResponse> EditMessageWithHttpMessagesAsync(int messageId, string newContent, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Get a message
        /// </summary>
        /// <remarks>
        /// Get a message with the specified id
        /// </remarks>
        /// <param name='messageId'>
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
        Task<HttpOperationResponse<GetMessageDTO>> GetMessageWithHttpMessagesAsync(int messageId, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
    }
}
