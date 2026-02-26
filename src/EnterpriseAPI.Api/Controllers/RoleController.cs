using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using EnterpriseAPI.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using EnterpriseAPI.Application.DTOs;

namespace EnterpriseAPI.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    [Authorize]
    public class RoleController : ControllerBase
    {

        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;  


        public RoleController(RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;

        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            var roleDtos = roles.Select(r => new RoleDto { Id = r.Id, Name = r.Name ?? string.Empty });
            return Ok(roleDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleById(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            return Ok(new RoleDto { Id = role.Id, Name = role.Name ?? string.Empty });
        }


        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var role = new ApplicationRole { Name = dto.Name };
            var result = await _roleManager.CreateAsync(role);

            if (result.Succeeded)
            {

                return CreatedAtAction(nameof(GetRoleById), new { id = role.Id },
                    new RoleDto { Id = role.Id, Name = role.Name ?? string.Empty });

            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty,error.Description);
            }

            return BadRequest(ModelState);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(string id, [FromBody] UpdateRoleDto dto)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                return NotFound();

            role.Name = dto.Name;
            var result = await _roleManager.UpdateAsync(role);

            if (result.Succeeded)
                return NoContent();

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                return NotFound();

            // Role atanmış kullanıcı var mı kontrol et
            var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name ?? string.Empty);
            if (usersInRole.Any())
            {
                return BadRequest("Bu role sahip kullanıcılar bulunuyor. Önce onları rolden çıkarın.");
            }

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
                return NoContent();

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return BadRequest(ModelState);
        }

    }
}
