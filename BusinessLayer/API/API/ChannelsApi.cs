using System;
using System.Collections.Generic;
using RestSharp;
using IO.Swagger.Client;

namespace IO.Swagger.Api
{
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public interface IChannelsApi
    {
        /// <summary>
        /// Create a channel in a group Creates a new channel in the specified group
        /// </summary>
        /// <param name="groupId">The id of the group to make the channel in</param>
        /// <param name="name">The name of the new channel</param>
        /// <returns></returns>
        void CreateChannel (int? groupId, string name);
        /// <summary>
        /// Delete a channel from a group Deletes the channel from the specified group
        /// </summary>
        /// <param name="groupId">The id of the group to delete the channel from</param>
        /// <param name="channelId">The id of the channel to delete</param>
        /// <returns></returns>
        void DeleteChannel (int? groupId, int? channelId);
        /// <summary>
        /// Modify a channel in a group Modify a channel in a group
        /// </summary>
        /// <param name="groupId">The id of the group</param>
        /// <param name="channelId">The id of the channel to update</param>
        /// <param name="channelName">The new name of the channel</param>
        /// <returns></returns>
        void UpdateChannel (int? groupId, int? channelId, string channelName);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class ChannelsApi : IChannelsApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelsApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public ChannelsApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelsApi"/> class.
        /// </summary>
        /// <returns></returns>
        public ChannelsApi(String basePath)
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
        /// Create a channel in a group Creates a new channel in the specified group
        /// </summary>
        /// <param name="groupId">The id of the group to make the channel in</param> 
        /// <param name="name">The name of the new channel</param> 
        /// <returns></returns>            
        public void CreateChannel (int? groupId, string name)
        {
            
            // verify the required parameter 'groupId' is set
            if (groupId == null) throw new ApiException(400, "Missing required parameter 'groupId' when calling CreateChannel");
            
            // verify the required parameter 'name' is set
            if (name == null) throw new ApiException(400, "Missing required parameter 'name' when calling CreateChannel");
            
    
            var path = "/groups/{groupId}/channels";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "groupId" + "}", ApiClient.ParameterToString(groupId));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                                                postBody = ApiClient.Serialize(name); // http body (model) parameter
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CreateChannel: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CreateChannel: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Delete a channel from a group Deletes the channel from the specified group
        /// </summary>
        /// <param name="groupId">The id of the group to delete the channel from</param> 
        /// <param name="channelId">The id of the channel to delete</param> 
        /// <returns></returns>            
        public void DeleteChannel (int? groupId, int? channelId)
        {
            
            // verify the required parameter 'groupId' is set
            if (groupId == null) throw new ApiException(400, "Missing required parameter 'groupId' when calling DeleteChannel");
            
            // verify the required parameter 'channelId' is set
            if (channelId == null) throw new ApiException(400, "Missing required parameter 'channelId' when calling DeleteChannel");
            
    
            var path = "/groups/{groupId}/channels/{channelId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "groupId" + "}", ApiClient.ParameterToString(groupId));
path = path.Replace("{" + "channelId" + "}", ApiClient.ParameterToString(channelId));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteChannel: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteChannel: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Modify a channel in a group Modify a channel in a group
        /// </summary>
        /// <param name="groupId">The id of the group</param> 
        /// <param name="channelId">The id of the channel to update</param> 
        /// <param name="channelName">The new name of the channel</param> 
        /// <returns></returns>            
        public void UpdateChannel (int? groupId, int? channelId, string channelName)
        {
            
            // verify the required parameter 'groupId' is set
            if (groupId == null) throw new ApiException(400, "Missing required parameter 'groupId' when calling UpdateChannel");
            
            // verify the required parameter 'channelId' is set
            if (channelId == null) throw new ApiException(400, "Missing required parameter 'channelId' when calling UpdateChannel");
            
            // verify the required parameter 'channelName' is set
            if (channelName == null) throw new ApiException(400, "Missing required parameter 'channelName' when calling UpdateChannel");
            
    
            var path = "/groups/{groupId}/channels/{channelId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "groupId" + "}", ApiClient.ParameterToString(groupId));
path = path.Replace("{" + "channelId" + "}", ApiClient.ParameterToString(channelId));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (channelName != null) queryParams.Add("channelName", ApiClient.ParameterToString(channelName)); // query parameter
                                        
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateChannel: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling UpdateChannel: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
    }
}
