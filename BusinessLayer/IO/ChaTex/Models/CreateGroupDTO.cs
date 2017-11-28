// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace IO.ChaTex.Models
{
    using Microsoft.Rest;
    using Newtonsoft.Json;
    using System.Linq;

    public partial class CreateGroupDTO
    {
        /// <summary>
        /// Initializes a new instance of the CreateGroupDTO class.
        /// </summary>
        public CreateGroupDTO()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the CreateGroupDTO class.
        /// </summary>
        public CreateGroupDTO(string groupName, bool allowEmployeeSticky, bool allowEmployeeAcknowledgeable, bool allowEmployeeBookmark)
        {
            GroupName = groupName;
            AllowEmployeeSticky = allowEmployeeSticky;
            AllowEmployeeAcknowledgeable = allowEmployeeAcknowledgeable;
            AllowEmployeeBookmark = allowEmployeeBookmark;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "groupName")]
        public string GroupName { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "allowEmployeeSticky")]
        public bool AllowEmployeeSticky { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "allowEmployeeAcknowledgeable")]
        public bool AllowEmployeeAcknowledgeable { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "allowEmployeeBookmark")]
        public bool AllowEmployeeBookmark { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (GroupName == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "GroupName");
            }
        }
    }
}