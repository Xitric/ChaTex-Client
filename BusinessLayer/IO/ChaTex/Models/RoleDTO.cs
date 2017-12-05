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

    public partial class RoleDTO
    {
        /// <summary>
        /// Initializes a new instance of the RoleDTO class.
        /// </summary>
        public RoleDTO()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the RoleDTO class.
        /// </summary>
        public RoleDTO(int id, string name, bool isDeleted)
        {
            Id = id;
            Name = name;
            IsDeleted = isDeleted;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Id")]
        public int Id { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "IsDeleted")]
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (Name == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Name");
            }
        }
    }
}
