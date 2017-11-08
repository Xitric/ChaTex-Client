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
  public class CreateGroupDTO {
    /// <summary>
    /// Gets or Sets GroupName
    /// </summary>
    [DataMember(Name="groupName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "groupName")]
    public string GroupName { get; set; }

    /// <summary>
    /// Gets or Sets AllowEmployeeSticky
    /// </summary>
    [DataMember(Name="allowEmployeeSticky", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "allowEmployeeSticky")]
    public bool? AllowEmployeeSticky { get; set; }

    /// <summary>
    /// Gets or Sets AllowEmployeeAcknowledgeable
    /// </summary>
    [DataMember(Name="allowEmployeeAcknowledgeable", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "allowEmployeeAcknowledgeable")]
    public bool? AllowEmployeeAcknowledgeable { get; set; }

    /// <summary>
    /// Gets or Sets AllowEmployeeBookmark
    /// </summary>
    [DataMember(Name="allowEmployeeBookmark", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "allowEmployeeBookmark")]
    public bool? AllowEmployeeBookmark { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class CreateGroupDTO {\n");
      sb.Append("  GroupName: ").Append(GroupName).Append("\n");
      sb.Append("  AllowEmployeeSticky: ").Append(AllowEmployeeSticky).Append("\n");
      sb.Append("  AllowEmployeeAcknowledgeable: ").Append(AllowEmployeeAcknowledgeable).Append("\n");
      sb.Append("  AllowEmployeeBookmark: ").Append(AllowEmployeeBookmark).Append("\n");
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
