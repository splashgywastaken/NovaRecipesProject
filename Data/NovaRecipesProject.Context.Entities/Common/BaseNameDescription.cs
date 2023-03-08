using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace NovaRecipesProject.Context.Entities.Common
{
    /// <summary>
    /// Base data for entities, inherits BaseEntity class
    /// Basically their name and description
    /// Name is required, while description could be empty
    /// </summary>
    public abstract class BaseNameDescription : BaseEntity
    {
        /// <summary>
        /// Entity's name
        /// </summary>
        [Required]
        [MaxLength(128)]
        public virtual string Name { get; set; } = null!;
        /// <summary>
        /// Entity's description
        /// </summary>
        [MaxLength(2000)]
        public virtual string? Description { get; set; }
    }
}
