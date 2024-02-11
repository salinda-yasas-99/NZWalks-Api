using AutoMapper;
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

        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository, IMapper mapper)
        {
            this.dbContext = dbContext; 
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //Get data from database
           var regions= await regionRepository.GetAllAsync();

            //Map domain models to DTOS
            /* var regionsDto = new List<RegionDto>();

             foreach(var region in regions) {
                 regionsDto.Add(new RegionDto()
                 {
                     Id = region.Id,
                     Code = region.Code,
                     Name = region.Name,
                     RegionIamgeUrl = region.RegionIamgeUrl
                 }
                 ); 
             }*/

      
            return Ok(mapper.Map<List<RegionDto>>(regions));
        }

        //get single region
        [HttpGet]
        [Route("{id:Guid}")]
        public async  Task<IActionResult> GetById([FromRoute] Guid id)
        {
            //var region = dbContext.Regions.Find(id);
            //GET Region Domian model
            var region = await regionRepository.GetByIdAsync(id);
            if(region == null)
            {
                return NotFound();

            }
            //map region domain
           /* var regionDto = new RegionDto()
            {
                    Id = region.Id,
                    Code = region.Code,
                    Name = region.Name,
                    RegionIamgeUrl = region.RegionIamgeUrl
            };*/
            return Ok(mapper.Map<RegionDto>(region));
        }

        //Add new region
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDTO addRegionRequestDTO)
        {

            //Convert DTo into domain model

            var regionDomainModel = mapper.Map<Region>(addRegionRequestDTO);
            /* var regionDomainModel = new Region
             {
                 Code = addRegionRequestDTO.Code,
                 Name = addRegionRequestDTO.Name,
                 RegionIamgeUrl = addRegionRequestDTO.RegionIamgeUrl
             };*/

            //use domain model to create Region
            regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

            //Map domain model back to DTO

            var regionDto = mapper.Map<RegionDto>(regionDomainModel);
            /* var regionDto = new RegionDto
             {
                 Id = regionDomainModel.Id,
                 Code = regionDomainModel.Code,
                 Name = regionDomainModel.Name,
                 RegionIamgeUrl = regionDomainModel.RegionIamgeUrl
             };*/

            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto );
      
        }

        //Update region
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            //Map dto to Domain model
            var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);

            /* var regionDomainModel = new Region
             {
                 Code = updateRegionRequestDto.Code,
                 Name = updateRegionRequestDto.Name,
                 RegionIamgeUrl = updateRegionRequestDto.RegionIamgeUrl
             };*/


            regionDomainModel =await regionRepository.UpdateAsync(id,regionDomainModel);

            if(regionDomainModel == null)
            {
                return NotFound();
            }

            //Convert domain Model into DTO
            regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);
            /* var regionDto = new RegionDto
             {
                 Id = regionDomainModel.Id,
                 Code = regionDomainModel.Code,
                 Name = regionDomainModel.Name,
                 RegionIamgeUrl = regionDomainModel.RegionIamgeUrl,
             };*/

            return Ok(mapper.Map<RegionDto>(regionDomainModel));
        }

        //Delete region
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id) { 

           var regionDomainModel =await regionRepository.DeleteAsync(id);

            if(regionDomainModel == null)
            {
                return NotFound();
            }
       

            //return deleted region

            //Map domain model to DTO

           /* var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionIamgeUrl = regionDomainModel.RegionIamgeUrl,
            };*/

            return Ok(mapper.Map<RegionDto>(regionDomainModel));
        }

    }
}
