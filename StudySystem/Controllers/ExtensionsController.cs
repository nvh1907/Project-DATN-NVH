// <copyright file="ExtensionsController.cs" ownedby="Xuan Truong">
//  Copyright (c) XuanTruong. All rights reserved.
//  FileType: Visual CSharp source file
//  Created On: 29/09/2023
//  Last Modified On: 29/09/2023
//  Description: ExtensionsController.cs
// </copyright>

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudySystem.Application.Service.Interfaces;
using StudySystem.Data.Models.Request;
using StudySystem.Data.Models.Response;
using StudySystem.Middlewares;

namespace StudySystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExtensionsController : ControllerBase
    {
        private readonly IExtensionsService _extensionsService;
        private readonly ILogger<ExtensionsController> _logger;
        private readonly IBannerService _bannerService;
        public ExtensionsController(ILogger<ExtensionsController> logger, IExtensionsService extensionsService, IBannerService bannerService)
        {

            _logger = logger;
            _extensionsService = extensionsService;
            _bannerService = bannerService;
        }

        [HttpGet("~/api/price-number")]
        public async Task<ActionResult<StudySystemAPIResponse<object>>> ConvertPriceToNumber(long price)
        {
            try
            {
                var rs = await _extensionsService.ConvertPriceToWords(price);
                return new StudySystemAPIResponse<object>(StatusCodes.Status200OK, new Response<object>(true, rs));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new BadHttpRequestException("can't convert");
            }


        }

        [HttpPost("~/api/create-banner")]
        [Authorize]
        [AuthPermission]
        public async Task<ActionResult<StudySystemAPIResponse<object>>> CreateBanner([FromForm] BannerDataRequestModel requestModel
            )
        {
            var rs = await _bannerService.CreateBanner(requestModel).ConfigureAwait(false);
            return new StudySystemAPIResponse<object>(StatusCodes.Status200OK, new Response<object>(rs, new object()));
        }

        [HttpGet("~/api/get-data-banner")]
        public async Task<ActionResult<StudySystemAPIResponse<BannerResponseModel>>> Getbanner()
        {
            var rs = await _bannerService.GetBanner().ConfigureAwait(false);
            return new StudySystemAPIResponse<BannerResponseModel>(StatusCodes.Status200OK, new Response<BannerResponseModel>(true, rs));
        }

        [HttpPost("~/api/update-banner")]
        [Authorize]
        [AuthPermission]
        public async Task<ActionResult<StudySystemAPIResponse<object>>> UpdateBanner(int id, bool active)
        {
            var rs = await _bannerService.UpdateBanner(id, active).ConfigureAwait(false);
            return new StudySystemAPIResponse<object>(StatusCodes.Status200OK, new Response<object>(rs, new object()));
        }

        [HttpDelete("~/api/delete-id")]
        [Authorize]
        [AuthPermission]
        public async Task<ActionResult<StudySystemAPIResponse<object>>> DeleteById(int id)
        {
            var rs = await _bannerService.DeleteById(id).ConfigureAwait(false);
            return new StudySystemAPIResponse<object>(StatusCodes.Status200OK, new Response<object>(rs, new object()));
        }
    }
}
