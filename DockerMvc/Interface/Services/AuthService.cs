using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DockerMvc.Data;
using DockerMvc.Models;
using Microsoft.EntityFrameworkCore;

namespace DockerMvc.Interface
{
    public class AuthService : IAuthService
    {
        private readonly BaseDbContext _context;

        public AuthService(BaseDbContext context)
        {
            _context = context;
        }

        public async Task<Profile> Authenticate(string email, string password)
        {
            var user = await _context.Profiles.SingleOrDefaultAsync(x => x.ProEmail == email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.ProPassword))
            {
                return null;
            }

            return user;
        }

        public async Task<List<string>> GetRoles(int profileId)
        {
            var roles = await (from rp in _context.RoleProfiles
                join r in _context.Roles on rp.RoleId equals r.RoleId
                where rp.ProId == profileId
                select r.RoleName).ToListAsync();

            return roles;
        }
    }

}