using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.VisualBasic;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class SQLRegionsRepository : IRegionRepository
    {
        private readonly NZWalksDbContext dbContext;
        public SQLRegionsRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;

        }
        public async Task<List<Region>> GetAllAsync()
        {
            return await dbContext.Regions.ToListAsync();

        }
    }
}
