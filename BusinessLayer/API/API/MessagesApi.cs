using System;
using System.Collections.Generic;
using RestSharp;
using IO.Swagger.Client;
using IO.Swagger.Model;

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
        /// <param name="messageContent">Content of the message</param>
        /// <returns></returns>
        void CreateMessage (int? channelId, string messageContent);
        /// <summary>
        /// Get the messages from a specific channel Get a number of messages from the specified channel
        /// </summary>
        /// <param name="channelId">The id of the channel to delete</param>
        /// <param name="fromIndex">The index of the first message to get, beginning from the most recently posted message. This defaults to 0, meaning the most recent message</param>
        /// <param name="count">The amount of messages to get. This defaults to 25</param>
        /// <returns>List&lt;GetMessageDTO&gt;</returns>
        List<GetMessageDTO> GetMessages (int? channelId, int? fromIndex, int? count);
        /// <summary>
        /// Wait for and get new messages sent to a channel This request will not return from the service until at least one new message has been posted
        /// </summary>
        /// <param name="channelId">The id of the channel to delete</param>
        /// <param name="since">The time to get messages since. This defaults to the current time</param>
        /// <returns>List&lt;GetMessageDTO&gt;</returns>
        List<GetMessageDTO> GetMessagesSince (int? channelId, DateTime? since);
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
        /// <param name="messageContent">Content of the message</param> 
        /// <returns></returns>            
        public void CreateMessage (int? channelId, string messageContent)
        {
            
            // verify the required parameter 'channelId' is set
            if (channelId == null) throw new ApiException(400, "Missing required parameter 'channelId' when calling CreateMessage");
            
            // verify the required parameter 'messageContent' is set
            if (messageContent == null) throw new ApiException(400, "Missing required parameter 'messageContent' when calling CreateMessage");
            
    
            var path = "/channels/{channelId}/messages";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "channelId" + "}", ApiClient.ParameterToString(channelId));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (messageContent != null) queryParams.Add("messageContent", ApiClient.ParameterToString(messageContent)); // query parameter
                                        
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
        /// Wait for and get new messages sent to a channel This request will not return from the service until at least one new message has been posted
        /// </summary>
        /// <param name="channelId">The id of the channel to delete</param> 
        /// <param name="since">The time to get messages since. This defaults to the current time</param> 
        /// <returns>List&lt;GetMessageDTO&gt;</returns>            
        public List<GetMessageDTO> GetMessagesSince (int? channelId, DateTime? since)
        {
            
            // verify the required parameter 'channelId' is set
            if (channelId == null) throw new ApiException(400, "Missing required parameter 'channelId' when calling GetMessagesSince");
            
    
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
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.GET, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling GetMessagesSince: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetMessagesSince: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<GetMessageDTO>) ApiClient.Deserialize(response.Content, typeof(List<GetMessageDTO>), response.Headers);
        }
    
    }
}
