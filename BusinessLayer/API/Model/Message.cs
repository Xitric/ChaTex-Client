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
  public class Message {
    /// <summary>
    /// Gets or Sets Id
    /// </summary>
    [DataMember(Name="id", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "id")]
    public long? Id { get; set; }

    /// <summary>
    /// Gets or Sets CreationTime
    /// </summary>
    [DataMember(Name="creationTime", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "creationTime")]
    public DateTime? CreationTime { get; set; }

    /// <summary>
    /// Gets or Sets Content
    /// </summary>
    [DataMember(Name="content", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "content")]
    public string Content { get; set; }

    /// <summary>
    /// Gets or Sets Sender
    /// </summary>
    [DataMember(Name="sender", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "sender")]
    public Person Sender { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class Message {\n");
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
