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
    public interface IGroupsApi
    {
        /// <summary>
        /// Add access rights for roles to a group This will add access rights for a list of roles to a specific group
        /// </summary>
        /// <param name="addRolesToGroupDTO">A list of roles, and a group</param>
        /// <returns></returns>
        void AddRolesToGroup (AddRolesToGroupDTO addRolesToGroupDTO);
        /// <summary>
        /// Add users to a group This will add a list of users to a specific group
        /// </summary>
        /// <param name="addUsersToGroupDTO">A list of users, and a group</param>
        /// <returns></returns>
        void AddUsersToGroup (AddUsersToGroupDTO addUsersToGroupDTO);
        /// <summary>
        /// Create a new group Creates a new group with the caller as the group administrator
        /// </summary>
        /// <param name="createGroupDTO">The object containing group information.</param>
        /// <returns></returns>
        void CreateGroup (CreateGroupDTO createGroupDTO);
        /// <summary>
        /// Delete a group Deletes the group with the specified id
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        void DeleteGroup (int? groupId);
        /// <summary>
        /// Remove access rights for roles from a group This will remove access for a list of roles from a specific group
        /// </summary>
        /// <param name="groupId">The Id of the group</param>
        /// <param name="roleIds">The Ids of all the roles</param>
        /// <returns></returns>
        void DeleteRolesFromGroup (int? groupId, List<int?> roleIds);
        /// <summary>
        /// Delete a list of users from a group This will delete a list of users from the specific group
        /// </summary>
        /// <param name="groupId">The Id of the group</param>
        /// <param name="userIds">The Ids of all the users</param>
        /// <returns></returns>
        void DeleteUsersFromGroup (int? groupId, List<int?> userIds);
        /// <summary>
        /// Mark or unmark a user as administrator Give a group member administrator rights or remove administrator rights from a group administrator
        /// </summary>
        /// <param name="groupId">The id of the group to affect</param>
        /// <param name="userId">The id of the user to mark or unmark</param>
        /// <param name="isAdministrator">true to mark the user as group administrator, false to unmark</param>
        /// <returns></returns>
        void MarkUserAsAdministrator (int? groupId, int? userId, bool? isAdministrator);
    }
  
    /// <summary>
    /// Represents a collection of functions to interact with the API endpoints
    /// </summary>
    public class GroupsApi : IGroupsApi
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GroupsApi"/> class.
        /// </summary>
        /// <param name="apiClient"> an instance of ApiClient (optional)</param>
        /// <returns></returns>
        public GroupsApi(ApiClient apiClient = null)
        {
            if (apiClient == null) // use the default one in Configuration
                this.ApiClient = Configuration.DefaultApiClient; 
            else
                this.ApiClient = apiClient;
        }
    
        /// <summary>
        /// Initializes a new instance of the <see cref="GroupsApi"/> class.
        /// </summary>
        /// <returns></returns>
        public GroupsApi(String basePath)
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
        /// Add access rights for roles to a group This will add access rights for a list of roles to a specific group
        /// </summary>
        /// <param name="addRolesToGroupDTO">A list of roles, and a group</param> 
        /// <returns></returns>            
        public void AddRolesToGroup (AddRolesToGroupDTO addRolesToGroupDTO)
        {
            
    
            var path = "/groups/roles";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                                                postBody = ApiClient.Serialize(addRolesToGroupDTO); // http body (model) parameter
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling AddRolesToGroup: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling AddRolesToGroup: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Add users to a group This will add a list of users to a specific group
        /// </summary>
        /// <param name="addUsersToGroupDTO">A list of users, and a group</param> 
        /// <returns></returns>            
        public void AddUsersToGroup (AddUsersToGroupDTO addUsersToGroupDTO)
        {
            
    
            var path = "/groups/users";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                                                postBody = ApiClient.Serialize(addUsersToGroupDTO); // http body (model) parameter
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling AddUsersToGroup: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling AddUsersToGroup: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Create a new group Creates a new group with the caller as the group administrator
        /// </summary>
        /// <param name="createGroupDTO">The object containing group information.</param> 
        /// <returns></returns>            
        public void CreateGroup (CreateGroupDTO createGroupDTO)
        {
            
    
            var path = "/groups";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
                                                postBody = ApiClient.Serialize(createGroupDTO); // http body (model) parameter
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.POST, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling CreateGroup: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling CreateGroup: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Delete a group Deletes the group with the specified id
        /// </summary>
        /// <param name="groupId"></param> 
        /// <returns></returns>            
        public void DeleteGroup (int? groupId)
        {
            
            // verify the required parameter 'groupId' is set
            if (groupId == null) throw new ApiException(400, "Missing required parameter 'groupId' when calling DeleteGroup");
            
    
            var path = "/groups/{groupId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "groupId" + "}", ApiClient.ParameterToString(groupId));
    
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
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteGroup: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteGroup: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Remove access rights for roles from a group This will remove access for a list of roles from a specific group
        /// </summary>
        /// <param name="groupId">The Id of the group</param> 
        /// <param name="roleIds">The Ids of all the roles</param> 
        /// <returns></returns>            
        public void DeleteRolesFromGroup (int? groupId, List<int?> roleIds)
        {
            
            // verify the required parameter 'groupId' is set
            if (groupId == null) throw new ApiException(400, "Missing required parameter 'groupId' when calling DeleteRolesFromGroup");
            
            // verify the required parameter 'roleIds' is set
            if (roleIds == null) throw new ApiException(400, "Missing required parameter 'roleIds' when calling DeleteRolesFromGroup");
            
    
            var path = "/groups/roles";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (groupId != null) queryParams.Add("groupId", ApiClient.ParameterToString(groupId)); // query parameter
                                    postBody = ApiClient.Serialize(roleIds); // http body (model) parameter
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteRolesFromGroup: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteRolesFromGroup: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Delete a list of users from a group This will delete a list of users from the specific group
        /// </summary>
        /// <param name="groupId">The Id of the group</param> 
        /// <param name="userIds">The Ids of all the users</param> 
        /// <returns></returns>            
        public void DeleteUsersFromGroup (int? groupId, List<int?> userIds)
        {
            
            // verify the required parameter 'groupId' is set
            if (groupId == null) throw new ApiException(400, "Missing required parameter 'groupId' when calling DeleteUsersFromGroup");
            
            // verify the required parameter 'userIds' is set
            if (userIds == null) throw new ApiException(400, "Missing required parameter 'userIds' when calling DeleteUsersFromGroup");
            
    
            var path = "/groups/users";
            path = path.Replace("{format}", "json");
                
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (groupId != null) queryParams.Add("groupId", ApiClient.ParameterToString(groupId)); // query parameter
                                    postBody = ApiClient.Serialize(userIds); // http body (model) parameter
    
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.DELETE, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteUsersFromGroup: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling DeleteUsersFromGroup: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
        /// <summary>
        /// Mark or unmark a user as administrator Give a group member administrator rights or remove administrator rights from a group administrator
        /// </summary>
        /// <param name="groupId">The id of the group to affect</param> 
        /// <param name="userId">The id of the user to mark or unmark</param> 
        /// <param name="isAdministrator">true to mark the user as group administrator, false to unmark</param> 
        /// <returns></returns>            
        public void MarkUserAsAdministrator (int? groupId, int? userId, bool? isAdministrator)
        {
            
            // verify the required parameter 'groupId' is set
            if (groupId == null) throw new ApiException(400, "Missing required parameter 'groupId' when calling MarkUserAsAdministrator");
            
            // verify the required parameter 'userId' is set
            if (userId == null) throw new ApiException(400, "Missing required parameter 'userId' when calling MarkUserAsAdministrator");
            
            // verify the required parameter 'isAdministrator' is set
            if (isAdministrator == null) throw new ApiException(400, "Missing required parameter 'isAdministrator' when calling MarkUserAsAdministrator");
            
    
            var path = "/groups/{groupId}/{userId}";
            path = path.Replace("{format}", "json");
            path = path.Replace("{" + "groupId" + "}", ApiClient.ParameterToString(groupId));
path = path.Replace("{" + "userId" + "}", ApiClient.ParameterToString(userId));
    
            var queryParams = new Dictionary<String, String>();
            var headerParams = new Dictionary<String, String>();
            var formParams = new Dictionary<String, String>();
            var fileParams = new Dictionary<String, FileParameter>();
            String postBody = null;
    
             if (isAdministrator != null) queryParams.Add("isAdministrator", ApiClient.ParameterToString(isAdministrator)); // query parameter
                                        
            // authentication setting, if any
            String[] authSettings = new String[] {  };
    
            // make the HTTP request
            IRestResponse response = (IRestResponse) ApiClient.CallApi(path, Method.PUT, queryParams, postBody, headerParams, formParams, fileParams, authSettings);
    
            if (((int)response.StatusCode) >= 400)
                throw new ApiException ((int)response.StatusCode, "Error calling MarkUserAsAdministrator: " + response.Content, response.Content);
            else if (((int)response.StatusCode) == 0)
                throw new ApiException ((int)response.StatusCode, "Error calling MarkUserAsAdministrator: " + response.ErrorMessage, response.ErrorMessage);
    
            return;
        }
    
    }
}
