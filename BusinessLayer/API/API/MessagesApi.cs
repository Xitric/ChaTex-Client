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
        /// Send a message Send a message to the server
        /// </summary>
        /// <param name="message">The message object</param>
        /// <returns>GetMessage</returns>
        GetMessage CreateMessage (PostMessage message);
        /// <summary>
        /// Find message by ID Returns a message with the specified ID
        /// </summary>
        /// <param name="messageID">ID of the message to fetch</param>
        /// <returns>GetMessage</returns>
        GetMessage GetMessageByID (long? messageID);
        /// <summary>
        /// Get all messages Returns all messages in the database
        /// </summary>
        /// <returns>List&lt;GetMessage&gt;</returns>
        List<GetMessage> GetMessages ();
        /// <summary>
        /// Wait for and get the next message Blocking call that will wait for the next message to be sent to the server. Once a message arrives, it will be returned.
        /// </summary>
        /// <param name="since"></param>
        /// <returns>List&lt;GetMessage&gt;</returns>
        List<GetMessage> WaitMessage (DateTime? since);
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
        /// Send a message Send a message to the server
        /// </summary>
        /// <param name="message">The message object</param> 
        /// <returns>GetMessage</returns>            
        public GetMessage CreateMessage (PostMessage message)
        {
            
            // verify the required parameter 'message' is set
            if (message == null) throw new ApiException(400, "Missing required parameter 'message' when calling CreateMessage");
            
    
            var path = "/messages";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                                                postBody = ApiClient.Serialize(message); // http body (model) parameter
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CreateMessage: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CreateMessage: " + response.ErrorMessage, response.ErrorMessage);
    
            return (GetMessage) ApiClient.Deserialize(response.Content, typeof(GetMessage), response.Headers);
        }
    
        /// <summary>
        /// Find message by ID Returns a message with the specified ID
        /// </summary>
        /// <param name="messageID">ID of the message to fetch</param> 
        /// <returns>GetMessage</returns>            
        public GetMessage GetMessageByID (long? messageID)
        {
            
            // verify the required parameter 'messageID' is set
            if (messageID == null) throw new ApiException(400, "Missing required parameter 'messageID' when calling GetMessageByID");
            
    
            var path = "/messages/{messageID}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "messageID" + "}", ApiClient.ParameterToString(messageID));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetMessageByID: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetMessageByID: " + response.ErrorMessage, response.ErrorMessage);
    
            return (GetMessage) ApiClient.Deserialize(response.Content, typeof(GetMessage), response.Headers);
        }
    
        /// <summary>
        /// Get all messages Returns all messages in the database
        /// </summary>
        /// <returns>List&lt;GetMessage&gt;</returns>            
        public List<GetMessage> GetMessages ()
        {
            
    
            var path = "/messages";
            path = path.Replace("{format}", "json");
                
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
                throw new ApiException ((int)response.StatusCode, "Error calling GetMessages: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling GetMessages: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<GetMessage>) ApiClient.Deserialize(response.Content, typeof(List<GetMessage>), response.Headers);
        }
    
        /// <summary>
        /// Wait for and get the next message Blocking call that will wait for the next message to be sent to the server. Once a message arrives, it will be returned.
        /// </summary>
        /// <param name="since"></param> 
        /// <returns>List&lt;GetMessage&gt;</returns>            
        public List<GetMessage> WaitMessage (DateTime? since)
        {
            
            // verify the required parameter 'since' is set
            if (since == null) throw new ApiException(400, "Missing required parameter 'since' when calling WaitMessage");
            
    
            var path = "/messages/wait";
            path = path.Replace("{format}", "json");
                
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
                throw new ApiException ((int)response.StatusCode, "Error calling WaitMessage: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling WaitMessage: " + response.ErrorMessage, response.ErrorMessage);
    
            return (List<GetMessage>) ApiClient.Deserialize(response.Content, typeof(List<GetMessage>), response.Headers);
        }
    
    }
}
