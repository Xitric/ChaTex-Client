using System;
using System.Collections.Generic;
using RestSharp;
using IO.Swagger.Client;
using IO.Swagger.Model;
using System.Threading;
using System.Threading.Tasks;

namespace IO.Swagger.Api
{
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface IMessagesApi
    {
        /// <summary>
        /// Create a new message Create a new message in a specific channel
        /// </summary>
        /// <param name="channelId">The id of the channel to post the message to</param>
        /// <param name="messageContentDTO">Content of the message</param>
        /// <returns></returns>
        void CreateMessage (int? channelId, MessageContentDTO messageContentDTO);
        /// <summary>
        /// Delete a message Delete the message with the specified id
        /// </summary>
        /// <param name="messageId">The id of the message to delete</param>
        /// <returns></returns>
        void DeleteMessage (int? messageId);
        /// <summary>
        /// Get a message Get a message with the specified id
        /// </summary>
        /// <param name="messageId">The id of the message to get</param>
        /// <returns>GetMessageDTO</returns>
        GetMessageDTO GetMessage (int? messageId);
        /// <summary>
        /// Get the messages from a specific channel Get a number of messages from the specified channel
        /// </summary>
        /// <param name="channelId">The id of the channel to delete</param>
        /// <param name="fromIndex">The index of the first message to get, beginning from the most recently posted message. This defaults to 0, meaning the most recent message</param>
        /// <param name="count">The amount of messages to get. This defaults to 25</param>
        /// <returns>List&lt;GetMessageDTO&gt;</returns>
        List<GetMessageDTO> GetMessages (int? channelId, int? fromIndex, int? count);
        /// <summary>
        /// Wait for and get new messages, message deletions, and message edits in a channel This request will not return from the service until at least one new message event has occurred
        /// </summary>
        /// <param name="channelId">The id of the channel to listen to</param>
        /// <param name="since">The time to get message events since</param>
        /// <returns>List&lt;MessageEventDTO&gt;</returns>
        List<MessageEventDTO> GetMessageEvents (int? channelId, DateTime? since, CancellationToken cancellation);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class MessagesApi : IMessagesApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessagesApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public MessagesApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="MessagesApi"/> class.
        /// </summary>
        /// <returns></returns>
        public MessagesApi(String basePath)
        {
            this.ApiClient = new ApiClient(basePath);
        }
    
        /// <summary>
        /// Sets the base path of the API client.
        /// </summary>
        /// <param name="basePath">The base path</param>
        /// <value>The base path</value>
        public void SetBasePath(String basePath)
        {
            this.ApiClient.BasePath = basePath;
        }
    
        /// <summary>
        /// Gets the base path of the API client.
        /// </summary>
        /// <param name="basePath">The base path</param>
        /// <value>The base path</value>
        public String GetBasePath(String basePath)
        {
            return this.ApiClient.BasePath;
        }
    
        /// <summary>
        /// Gets or sets the API client.
        /// </summary>
        /// <value>An instance of the ApiClient</value>
        public ApiClient ApiClient {get; set;}
    
        /// <summary>
        /// Create a new message Create a new message in a specific channel
        /// </summary>
        /// <param name="channelId">The id of the channel to post the message to</param> 
        /// <param name="messageContentDTO">Content of the message</param> 
        /// <returns></returns>            
        public void CreateMessage (int? channelId, MessageContentDTO messageContentDTO)
        {
            
            // verify the required parameter 'channelId' is set
            if (channelId == null) throw new ApiException(400, "Missing required parameter 'channelId' when calling CreateMessage");
            
            // verify the required parameter 'messageContentDTO' is set
            if (messageContentDTO == null) throw new ApiException(400, "Missing required parameter 'messageContentDTO' when calling CreateMessage");
            
            var path = "/channels/{channelId}/messages";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "channelId" + "}", ApiClient.ParameterToString(channelId));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                                                postBody = ApiClient.Serialize(messageContentDTO); // http body (model) parameter
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CreateMessage: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CreateMessage: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Delete a message Delete the message with the specified id
        /// </summary>
        /// <param name="messageId">The id of the message to delete</param> 
        /// <returns></returns>            
        public void DeleteMessage (int? messageId)
        {
            
            // verify the required parameter 'messageId' is set
            if (messageId == null) throw new ApiException(400, "Missing required parameter 'messageId' when calling DeleteMessage");
            
    
            var path = "/messages/{messageId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "messageId" + "}", ApiClient.ParameterToString(messageId));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                                                    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteMessage: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteMessage: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Get a message Get a message with the specified id
        /// </summary>
        /// <param name="messageId">The id of the message to get</param> 
        /// <returns>GetMessageDTO</returns>            
        public GetMessageDTO GetMessage (int? messageId)
        {
            
            // verify the required parameter 'messageId' is set
            if (messageId == null) throw new ApiException(400, "Missing required parameter 'messageId' when calling GetMessage");
            
    
            var path = "/messages/{messageId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "messageId" + "}", ApiClient.ParameterToString(messageId));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                                                    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling GetMessage: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetMessage: " + response.ErrorMessage, response.ErrorMessage);
    
            return (GetMessageDTO) ApiClient.Deserialize(response.Content, typeof(GetMessageDTO), response.Headers);
        }
    
        /// <summary>
        /// Get the messages from a specific channel Get a number of messages from the specified channel
        /// </summary>
        /// <param name="channelId">The id of the channel to delete</param> 
        /// <param name="fromIndex">The index of the first message to get, beginning from the most recently posted message. This defaults to 0, meaning the most recent message</param> 
        /// <param name="count">The amount of messages to get. This defaults to 25</param> 
        /// <returns>List&lt;GetMessageDTO&gt;</returns>            
        public List<GetMessageDTO> GetMessages (int? channelId, int? fromIndex, int? count)
        {
            
            // verify the required parameter 'channelId' is set
            if (channelId == null) throw new ApiException(400, "Missing required parameter 'channelId' when calling GetMessages");
            
    
            var path = "/channels/{channelId}/messages";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "channelId" + "}", ApiClient.ParameterToString(channelId));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
            if (fromIndex != null) queryParams.Add("fromIndex", ApiClient.ParameterToString(fromIndex)); // query parameter
            if (count != null) queryParams.Add("count", ApiClient.ParameterToString(count)); // query parameter
                                        
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling GetMessages: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetMessages: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<GetMessageDTO>) ApiClient.Deserialize(response.Content, typeof(List<GetMessageDTO>), response.Headers);
        }

        /// <summary>
        /// Wait for and get new messages, message deletions, and message edits in a channel This request will not return from the service until at least one new message event has occurred
        /// </summary>
        /// <param name="channelId">The id of the channel to listen to</param> 
        /// <param name="since">The time to get message events since</param> 
        /// <returns>List&lt;MessageEventDTO&gt;</returns>            
        public List<MessageEventDTO> GetMessageEvents (int? channelId, DateTime? since, CancellationToken cancellation)
        {

            // verify the required parameter 'channelId' is set
            if (channelId == null) throw new ApiException(400, "Missing required parameter 'channelId' when calling GetMessageEvents");

            // verify the required parameter 'since' is set
            if (since == null) throw new ApiException(400, "Missing required parameter 'since' when calling GetMessageEvents");


            var path = "/channels/{channelId}/messages/live";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "channelId" + "}", ApiClient.ParameterToString(channelId));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
            if (since != null) queryParams.Add("since", ApiClient.ParameterToString(since)); // query parameter
                                        
            // authentication setting, if any
            String[] authSettings = new String[] {  };

            // make the HTTP request
            IRestResponse response = (IRestResponse)ApiClient.CallApiCancellable(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings, cancellation);

            if (((int)response.StatusCode) >= 400)
                throw new ApiException((int)response.StatusCode, "Error calling GetMessageEvents: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException((int)response.StatusCode, "Error calling GetMessageEvents: " + response.ErrorMessage, response.ErrorMessage);

            return (List<MessageEventDTO>)ApiClient.Deserialize(response.Content, typeof(List<MessageEventDTO>), response.Headers);
        }
    
    }
}
