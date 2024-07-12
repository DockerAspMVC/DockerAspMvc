
using DockerMvc.Models;
using Microsoft.EntityFrameworkCore;

namespace DockerMvc.Data
{
    public class BaseDbContext : DbContext 
    {
        public BaseDbContext(DbContextOptions<BaseDbContext> options) : base(options) 
        {

        }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleProfile> RoleProfiles { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Productos> Productos { get; set; }
        public DbSet<SubCategoria> SubCategorias { get; set; }
        
        
        

    }
    
}