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
  public class GetMessageDTO {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="Id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "Id")]
    public int? Id { get; set; }

    /// <summary>
    /// Gets or Sets CreationTime
    /// </summary>
    [DataMember(Name="CreationTime", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "CreationTime")]
    public DateTime? CreationTime { get; set; }

    /// <summary>
    /// Gets or Sets Content
    /// </summary>
    [DataMember(Name="Content", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "Content")]
    public string Content { get; set; }

    /// <summary>
    /// Gets or Sets Sender
    /// </summary>
    [DataMember(Name="Sender", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "Sender")]
    public UserDTO Sender { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class GetMessageDTO {\n");
      sb.Append("  Id: ").Append(Id).Append("\n");
      sb.Append("  CreationTime: ").Append(CreationTime).Append("\n");
      sb.Append("  Content: ").Append(Content).Append("\n");
      sb.Append("  Sender: ").Append(Sender).Append("\n");
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
