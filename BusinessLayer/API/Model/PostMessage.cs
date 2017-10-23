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
  public class PostMessage {
    /// <summary>
    /// Gets or Sets Content
    /// </summary>
    [DataMember(Name="Content", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "Content")]
    public string Content { get; set; }

    /// <summary>
    /// Gets or Sets Author
    /// </summary>
    [DataMember(Name="Author", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "Author")]
    public long? Author { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class PostMessage {\n");
      sb.Append("  Content: ").Append(Content).Append("\n");
      sb.Append("  Author: ").Append(Author).Append("\n");
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
