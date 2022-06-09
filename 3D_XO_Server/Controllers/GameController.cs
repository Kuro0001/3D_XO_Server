using _3D_XO_Server.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace _3D_XO_Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly DBContext _context;
        public GameController(DBContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IEnumerable<Game> Get()
        {
            return _context.Games.ToList();
        }
    }
}
