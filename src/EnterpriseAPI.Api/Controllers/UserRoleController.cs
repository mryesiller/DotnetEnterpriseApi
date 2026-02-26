using EnterpriseAPI.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseAPI.Api.Controllers;

[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
[ApiController]
public class UserRoleController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public UserRoleController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    // Kullanıcıya rol ata
    [HttpPost("{userId}/roles/{roleName}")]
    public async Task<IActionResult> AssignRole(string userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound("Kullanıcı bulunamadı.");

        var roleExists = await _roleManager.RoleExistsAsync(roleName);
        if (!roleExists) return NotFound("Rol bulunamadı.");

        var result = await _userManager.AddToRoleAsync(user, roleName);
        if (result.Succeeded) return Ok();

        return BadRequest(result.Errors);
    }

    // Kullanıcıdan rol çıkar
    [HttpDelete("{userId}/roles/{roleName}")]
    public async Task<IActionResult> RemoveRole(string userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound("Kullanıcı bulunamadı.");

        var result = await _userManager.RemoveFromRoleAsync(user, roleName);
        if (result.Succeeded) return NoContent();

        return BadRequest(result.Errors);
    }

    // Kullanıcının rollerini listele
    [HttpGet("{userId}/roles")]
    public async Task<IActionResult> GetUserRoles(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound("Kullanıcı bulunamadı.");

        var roles = await _userManager.GetRolesAsync(user);
        return Ok(roles);
    }
}