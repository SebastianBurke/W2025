using System.ComponentModel.DataAnnotations;
using System.Linq;
using HIAAAServices.DAL.Interfaces;
using HIAAAServices.DAL.Repositories;
using HIAAAServices.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
namespace HIAAAServices.Models;

public class UniqueRoleCodeAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        var roleRepository = validationContext.GetService(typeof(IRole)) as IRole;
        var applicationRepository = validationContext.GetService(typeof(IApp)) as IApp;
        
        if (roleRepository == null || applicationRepository == null)
        {
            return new ValidationResult("Services not available.");
        }

        string newRoleCode = value?.ToString() ?? string.Empty;
        
        var roleInstance = (RoleDTO)validationContext.ObjectInstance;
        long appId = roleInstance.AppId;

        var allRoles = roleRepository.ReadAllRoles().Result
            .Where(r => r.AppUserRoles != null)
            .ToList();

        var allAppRoles = allRoles.Where(r => r.AppUserRoles.FirstOrDefault().Appid == appId).ToList();
        
        var existingRole = allAppRoles.FirstOrDefault(r => r.Roleid == roleInstance.Roleid);
        
        if (existingRole != null)
        {
            allAppRoles.Remove(existingRole);
        }
        
        bool roleCodeExists = allAppRoles.Any(u => u.Rolecode.Substring(u.Rolecode.IndexOf("_") + 1).Equals(newRoleCode, StringComparison.OrdinalIgnoreCase));

        if (roleCodeExists)
        {
            return new ValidationResult("Role code is already in use");
        }

        return ValidationResult.Success;
    }
}