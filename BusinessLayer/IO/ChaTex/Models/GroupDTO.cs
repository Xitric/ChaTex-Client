// <auto-generated>
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace IO.ChaTex.Models
{
    using Microsoft.Rest;
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public partial class GroupDTO
    {
        /// <summary>
        /// Initializes a new instance of the GroupDTO class.
        /// </summary>
        public GroupDTO()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the GroupDTO class.
        /// </summary>
        public GroupDTO(int id, string name, IList<ChannelDTO> channels = default(IList<ChannelDTO>))
        {
            Id = id;
            Name = name;
            Channels = channels;
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
        [JsonProperty(PropertyName = "Channels")]
        public IList<ChannelDTO> Channels { get; set; }

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
            if (Channels != null)
            {
                foreach (var element in Channels)
                {
                    if (element != null)
                    {
                        element.Validate();
                    }
                }
            }
        }
    }
}
