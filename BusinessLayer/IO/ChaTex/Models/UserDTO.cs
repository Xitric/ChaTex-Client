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

    public partial class UserDTO
    {
        /// <summary>
        /// Initializes a new instance of the UserDTO class.
        /// </summary>
        public UserDTO()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the UserDTO class.
        /// </summary>
        public UserDTO(int id, string firstName, string lastName, string email, bool me, string middleInitial = default(string), bool? isDeleted = default(bool?))
        {
            Id = id;
            FirstName = firstName;
            MiddleInitial = middleInitial;
            LastName = lastName;
            Email = email;
            Me = me;
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
        [JsonProperty(PropertyName = "FirstName")]
        public string FirstName { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "MiddleInitial")]
        public string MiddleInitial { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "LastName")]
        public string LastName { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Email")]
        public string Email { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Me")]
        public bool Me { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "IsDeleted")]
        public bool? IsDeleted { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (FirstName == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "FirstName");
            }
            if (LastName == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "LastName");
            }
            if (Email == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Email");
            }
        }
    }
}
