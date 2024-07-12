using System.Collections.Generic;
using System.Threading.Tasks;
using DockerMvc.Models;

namespace DockerMvc.Interface
{
    public interface IAuthService
    {
        Task<Profile> Authenticate(string email, string password);
        Task<List<string>> GetRoles(int profileId);
    }
}