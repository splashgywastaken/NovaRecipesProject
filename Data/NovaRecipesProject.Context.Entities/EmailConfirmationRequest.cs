using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NovaRecipesProject.Context.Entities.Common;

namespace NovaRecipesProject.Context.Entities;

/// <summary>
/// Entity used to keep data about emails confirmation status
/// </summary>
public class EmailConfirmationRequest : BaseEntity
{
    /// <summary>
    /// User's email that needs to be confirmed
    /// </summary>
    public string Email { get; set; } = null!;
    /// <summary>
    /// Request's date and time of creation, used to tell if too much time went since request was created
    /// </summary>
    [Required, DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTime RequestCreationDataTime { get; set; }
}