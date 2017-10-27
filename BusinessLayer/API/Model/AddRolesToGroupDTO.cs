using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace IO.Swagger.Model {

  /// <summary>
  /// 
  /// </summary>
  [DataContract]
  public class AddRolesToGroupDTO {
    /// <summary>
    /// Gets or Sets GroupId
    /// </summary>
    [DataMember(Name="groupId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "groupId")]
    public int? GroupId { get; set; }

    /// <summary>
    /// Gets or Sets RoleIds
    /// </summary>
    [DataMember(Name="roleIds", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "roleIds")]
    public List<int?> RoleIds { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class AddRolesToGroupDTO {\n");
      sb.Append("  GroupId: ").Append(GroupId).Append("\n");
      sb.Append("  RoleIds: ").Append(RoleIds).Append("\n");
      sb.Append("}\n");
      return sb.ToString();
    }

    /// <summary>
    /// Get the JSON string presentation of the object
    /// </summary>
    /// <returns>JSON string presentation of the object</returns>
    public string ToJson() {
      return JsonConvert.SerializeObject(this, Formatting.Indented);
    }

}
}
