using System.ComponentModel.DataAnnotations;
 
namespace EnterpriseAPI.Application.DTOs
{
    public class RoleDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }=string.Empty;

    }

    public class CreateRoleDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }

    public class UpdateRoleDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;
    }
}
