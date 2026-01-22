using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolShare.DAL.Entities;

namespace ToolShare.DAL.Interfaces
{
    public interface IToolImageRepository : IGenericRepository<ToolImage>
    {
        Task<IEnumerable<ToolImage>> GetImagesByToolAsync(int toolId);
        Task<ToolImage?> GetPrimaryImageAsync(int toolId);
        Task<bool> DeleteImagesByToolAsync(int toolId);
    }
}
