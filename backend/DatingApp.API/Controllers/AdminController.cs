﻿using DatingApp.API.DTOs;
using DatingApp.API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Controllers
{
	public class AdminController : BaseApiController
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly RoleManager<AppRole> _roleManager;

		public AdminController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
		{
			this._userManager = userManager;
			this._roleManager = roleManager;
		}

		[Authorize(Policy = "RequireAdminRole")]
		[HttpGet("all-roles")]
		public async Task<ActionResult> GetRoles()
		{
			return Ok(await _roleManager.Roles.ToListAsync());
		}

		[Authorize(Policy = "RequireAdminRole")]
		[HttpGet("users-with-roles")]
		public async Task<ActionResult> GetUsersWithRoles()
		{
			var users = await _userManager.Users
				.OrderBy(u => u.UserName)
				.Select(u => new
				{
					u.Id,
					Username = u.UserName,
					Roles = u.UserRoles.Select(r => r.Role.Name).ToList()
				}).ToListAsync();
			return Ok(users);
		}

		[Authorize(Policy = "RequireAdminRole")]
		[HttpPut("edit-roles")]
		public async Task<ActionResult> EditRoles(MemberRoleUpdateDto memberRoleDto)
		{
			var user = await _userManager.FindByNameAsync(memberRoleDto.Username);

			if (user == null) return NotFound();

			var userRoles = await _userManager.GetRolesAsync(user);

			var result = await _userManager.AddToRolesAsync(user, memberRoleDto.Roles.Except(userRoles));
			if (!result.Succeeded) return BadRequest("Failed to add roles");

			result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(memberRoleDto.Roles));
			if (!result.Succeeded) return BadRequest("Faild to remove from roles");

			return Ok(await _userManager.GetRolesAsync(user));
		}

		[Authorize(Policy = "ModeratePhotoRole")]
		[HttpGet("hptos-to-moderate")]
		public ActionResult GetPhotosForModeration()
		{
			return Ok("Admins or moderators  can see this");
		}
	}
}