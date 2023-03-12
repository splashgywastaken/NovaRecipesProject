namespace NovaRecipesProject.Context.Entities.Common;

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

/// <summary>
/// Base entity
/// </summary>
[Index("Uid", IsUnique = true)]
public abstract class BaseEntity
{
    /// <summary>
    /// Generated key of entity
    /// </summary>
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public virtual int Id { get; set; }

    /// <summary>
    /// Entities GUID
    /// </summary>
    [Required]
    public virtual Guid Uid { get; set; } = Guid.NewGuid();
}