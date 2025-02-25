using com.teamseven.musik.be.Services.QueryDB;
using Microsoft.AspNetCore.Mvc;

namespace com.teamseven.musik.be.Controllers.MainFlowFunction
{
    [ApiController]
    [Route("api/genre")]
    public class GenreController : ControllerBase
    {
        private readonly GenreService _service;

        public GenreController(GenreService service)
        {
            _service = service;
        }



    }
}
