using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EPlast.BussinessLayer.Interfaces.City;
using EPlast.BussinessLayer.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly ILoggerService<CitiesController> _logger;
        private readonly ICityService _cityService;
        private readonly IMapper _mapper;

        public CitiesController(ILoggerService<CitiesController> logger, ICityService cityService, IMapper mapper)
        {
            _logger = logger;
            _cityService = cityService;
            _mapper = mapper;
        }

    }
}