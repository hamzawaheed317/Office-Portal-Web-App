using Microsoft.AspNetCore.Authentication;
using OfficePortal.Models.Account_Models;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Security.Principal;
using Microsoft.Extensions.Logging;
using System.Reflection.PortableExecutable;
using DirectoryEntry = System.DirectoryServices.DirectoryEntry;

namespace WebApplication1.Services
{
    public interface IActiveDirectoryService
    {
        UserInfo GetUserInfo();
        bool AuthenticateUser(string username, string password, out UserInfo userInfo);
    }

    public class ActiveDirectoryService : IActiveDirectoryService
    {
        private readonly ILogger<ActiveDirectoryService> _logger;

        public ActiveDirectoryService(ILogger<ActiveDirectoryService> logger)
        {
            _logger = logger;
        }

        public UserInfo GetUserInfo()
        {
            /*   var identity = WindowsIdentity.GetCurrent();
               if (identity == null)
               {
                   _logger.LogWarning("No current Windows identity found.");
                   return null;
               }

               using var context = new PrincipalContext(ContextType.Domain);
               var userPrincipal = UserPrincipal.FindByIdentity(context, identity.Name);
               if (userPrincipal == null) return null;

               var directoryEntry = userPrincipal.GetUnderlyingObject() as DirectoryEntry;

               return new UserInfo
               {
                   Employee_Name = userPrincipal.DisplayName ?? "Unknown",
                   email = userPrincipal.EmailAddress,
                   Department = directoryEntry?.Properties["department"]?.Value?.ToString(),
                   Job_Title = directoryEntry?.Properties["title"]?.Value?.ToString(),
                   Grade = directoryEntry?.Properties["employeeGrade"]?.Value?.ToString(),
                   Employee_Number = directoryEntry.Properties["employeeNumber"]?.Value?.ToString() ?? "0", // Convert to string if Employee_Number is a string property
                   role = string.Join(", ", userPrincipal.GetGroups().Select(g => g.Name))
               };
            */

            List<string> attendees = new List<string>
{
    "Muhammad",
    "Latin",
    "Arabia"
};

            return new UserInfo
            {
                Employee_Name = "John Doe Jack",
              
                Department = "IT",
                Job_Title = "Software Engineer",
                Grade = "A1",
                Employee_Number = "12345", // Ensure Employee_Number is a string if necessary
                role = "Line Manager",
                Admin_email = "hasanmughal538@gmail.com",
                USer_role = "Admin",
                Attendies = attendees
            };



        }

        public bool AuthenticateUser(string username, string password, out UserInfo userInfo)
        {
            userInfo = null;
            try
            {
                using var context = new PrincipalContext(ContextType.Domain);
                if (!context.ValidateCredentials(username, password))
                    return false;

                var userPrincipal = UserPrincipal.FindByIdentity(context, username);
                if (userPrincipal == null) return false;

                var directoryEntry = userPrincipal.GetUnderlyingObject() as DirectoryEntry;
                userInfo = new UserInfo
                {
                    Employee_Name = userPrincipal.DisplayName ?? "Unknown",
                    email = userPrincipal.EmailAddress,
                    Department = directoryEntry?.Properties["department"]?.Value?.ToString(),
                    Job_Title = directoryEntry?.Properties["title"]?.Value?.ToString(),
                    Grade = directoryEntry?.Properties["employeeGrade"]?.Value?.ToString(),
                    Employee_Number = directoryEntry.Properties["employeeNumber"]?.Value?.ToString() ?? "0", // Convert to string if Employee_Number is a string property
                    role = string.Join(", ", userPrincipal.GetGroups().Select(g => g.Name))
                };
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during Active Directory authentication.");
                return false;
            }
        }
    }
}
