using System.ComponentModel.DataAnnotations;

namespace ProfileStore.Client.Models
{
    public class Scope
    {
        [Required]
        [StringLength(64)]
        [RegularExpression(@"^[a-z0-9.]{1,64}$", ErrorMessage =
            "Start the string with a letter or number and it must contain a letter, number, and underline (_) only. Do not use capital letters.")]
        public string ScopeId { get; set; }

        [StringLength(256)] public string Description { get; set; }
    }
}
