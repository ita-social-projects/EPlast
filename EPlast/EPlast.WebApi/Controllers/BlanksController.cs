using AutoMapper;
using EPlast.BLL.DTO.Blank;
using EPlast.BLL.Interfaces.Blank;
using EPlast.BLL.Interfaces.Logging;
using EPlast.WebApi.Models.Blank;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EPlast.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlanksController : ControllerBase
    {
        private readonly ILoggerService<BlanksController> _logger;
        private readonly IMapper _mapper;
        private readonly IBlankBiographyDocumentService _blankBiographyDocumentService;

        public BlanksController(ILoggerService<BlanksController> logger,
            IMapper mapper, 
            IBlankBiographyDocumentService blankBiographyDocumentService)
        {
            _logger = logger;
            _mapper = mapper;
            _blankBiographyDocumentService = blankBiographyDocumentService;
        }

        /// <summary>
        /// Add a document to the blank
        /// </summary>
        /// <param name="document">An information about a specific document</param>
        /// <returns>A newly created Blank document</returns>
        [HttpPost("AddDocument/{userId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> AddDocument(BlankBiographyDocumentsViewModel document)
        {
            var documentDTO = _mapper.Map<BlankBiographyDocumentsViewModel, BlankBiographyDocumentsDTO>(document);

            await _blankBiographyDocumentService.AddDocumentAsync(documentDTO);
            _logger.LogInformation($"Document with id {{{documentDTO.ID}}} was added.");

            return Ok(documentDTO);
        }
    }
}
