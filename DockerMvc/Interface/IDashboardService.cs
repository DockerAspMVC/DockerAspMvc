using System.Collections.Generic;
using System.Threading.Tasks;
using DockerMvc.Models;

namespace DockerMvc.Interface
{
    public interface IDashboardService
    {
        Task<int> GetUserRegistrationsCount();
        Task<int> GetNewOrdersCount();
        Task<List<Productos>> GetProductData();
    }
}