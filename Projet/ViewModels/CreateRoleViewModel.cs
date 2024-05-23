using System.ComponentModel.DataAnnotations;

namespace Projet.ViewModels

{
    public class CreateRoleViewModel
    {
        [Required]
        [Display(Name = "Role")]
        public string RoleName { get; set; }
    }
}
