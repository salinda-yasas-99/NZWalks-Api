using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.Runtime.InteropServices;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository)
        {
            this.dbContext = dbContext; 
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //Get data from database
           var regions= await dbContext.Regions.ToListAsync();

            //Map domain models to DTOS
            var regionsDto = new List<RegionDto>();

            foreach(var region in regions) {
                regionsDto.Add(new RegionDto()
                {
                    Id = region.Id,
                    Code = region.Code,
                    Name = region.Name,
                    RegionIamgeUrl = region.RegionIamgeUrl
                }
                ); 
            }


            return Ok(regionsDto);
        }

        //get single region
        [HttpGet]
        [Route("{id:Guid}")]
        public async  Task<IActionResult> GetById([FromRoute] Guid id)
        {
            //var region = dbContext.Regions.Find(id);
            //GET Region Domian model
            var region = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if(region == null)
            {
                return NotFound();

            }
            //map region domain
            var regionDto = new RegionDto()
            {
                    Id = region.Id,
                    Code = region.Code,
                    Name = region.Name,
                    RegionIamgeUrl = region.RegionIamgeUrl
            };
            return Ok(regionDto);
        }

        //Add new region
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDTO addRegionRequestDTO)
        {
            //Convert DTo into domain model
            var regionDomainModel = new Region
            {
                Code = addRegionRequestDTO.Code,
                Name = addRegionRequestDTO.Name,
                RegionIamgeUrl = addRegionRequestDTO.RegionIamgeUrl
            };

            //use domain model to create Region
            await dbContext.Regions.AddAsync(regionDomainModel);
            await dbContext.SaveChangesAsync();

            //Map domain model back to DTO
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionIamgeUrl = regionDomainModel.RegionIamgeUrl
            };

            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto );
      
        }

        //Update region
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
           var regionDomainModel = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if(regionDomainModel == null)
            {
                return NotFound();
            }

            //Map DTo to domain model
            regionDomainModel.Code = updateRegionRequestDto.Code;
            regionDomainModel.Name = updateRegionRequestDto.Name;
            regionDomainModel.RegionIamgeUrl = updateRegionRequestDto.RegionIamgeUrl;
            dbContext.SaveChangesAsync();

            //Convert domain Model into DTO
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionIamgeUrl = regionDomainModel.RegionIamgeUrl,
            };

            return Ok(regionDto);
        }

        //Delete region
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id) { 
           var regionDomainModel =await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if(regionDomainModel == null)
            {
                return NotFound();
            }
            //delete
            dbContext.Regions.Remove(regionDomainModel);
             await dbContext.SaveChangesAsync();

            //return deleted region
            //Map domain model to DTO
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionIamgeUrl = regionDomainModel.RegionIamgeUrl,
            };

            return Ok(regionDto);
        }

    }
}
