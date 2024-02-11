using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Identity.Client;
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

        public async Task<Region> CreateAsync(Region region)
        {
           await dbContext.Regions.AddAsync(region);
            await dbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region> DeleteAsync(Guid id)
        {
            var existingREgion = await dbContext.Regions.FirstOrDefaultAsync(x=>x.Id == id);
            if (existingREgion == null) 
            {
                return null;
            }
           dbContext.Regions.Remove(existingREgion);
            await dbContext.SaveChangesAsync();
            return existingREgion;
        }

        public async Task<List<Region>> GetAllAsync()
        {
            return await dbContext.Regions.ToListAsync();

        }

        public async Task<Region?> GetByIdAsync(Guid id)
        {
            return await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

        }

        public async Task<Region> UpdateAsync(Guid id, Region region)
        {
            var existingRegion = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (existingRegion == null)
            {
                return null;
            }

            existingRegion.Code =region.Code;
            existingRegion.Name =region.Name;
            existingRegion.RegionIamgeUrl = region.RegionIamgeUrl;

            await dbContext.SaveChangesAsync();
            return existingRegion;

        }




    }
}
