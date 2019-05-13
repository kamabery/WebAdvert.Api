using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdvertApi.Models;
using AdvertAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace AdvertAPI.Controllers
{
    [ApiController]
    [Route("api/adverts/v1")]
    public class AdvertController : Controller
    {
        private readonly IAdvertStorageService storageService;

        public AdvertController(IAdvertStorageService storageService)
        {
            this.storageService = storageService;
        }

        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(201, Type=typeof(CreateAdvertResponse))]
        public async Task<IActionResult> Create(AdvertModel model)
        {
            try
            {
                var recordId = await storageService.Add(model);
                return StatusCode(201, new CreateAdvertResponse {Id = recordId});
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPut]
        [Route("Confirm")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Confirm(ConfirmAdverModel model)
        {
            try
            {
                await storageService.Confirm(model);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return new NotFoundResult();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}