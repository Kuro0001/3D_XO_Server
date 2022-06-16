using _3D_XO_Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        [Authorize]
        public IEnumerable<Game> Get()
        {
            return _context.Games.ToList();
        }

        [HttpGet("start-active-game-search/{userid}")]
        [Authorize]
        public async Task<IActionResult> GetById(string userid)
        {
            int index = -1;
            CheckOverdueGames();
            List<Game> gamesList = new List<Game>();
            if (_context.Games.ToList().Exists(x => x.User1Id.Equals(userid) || x.User2Id.Equals(userid)))
                gamesList = await _context.Games.Where(x => x.User1Id.Equals(userid) || x.User2Id.Equals(userid)).ToListAsync();
            int resultContinueId = _context.GameResults.ToList()[_context.GameResults.ToList().FindIndex(x => x.Name.Equals("continues"))].Id;
            if (gamesList.Exists(x => x.Result.Id == resultContinueId))
                index = gamesList.FindLastIndex(x => x.Result.Id == resultContinueId);
            if (index != -1)
            {
                Game g = gamesList[index];
                    return Ok(new
                    {
                        Id = g.Id,
                        User1Id = g.User1Id,
                        User2Id = g.User2Id,
                        Date = g.Date
                    });
            }
            else
                return NoContent();
        }

        public void CheckOverdueGames()
        {
            int index = -1;
            int resultContinueId = _context.GameResults.ToList()[_context.GameResults.ToList().FindIndex(x => x.Name.Equals("continues"))].Id;
            List<Game> games = _context.Games.ToList().FindAll(x => x.Result.Id == resultContinueId);
            List<GamesState> states = new List<GamesState>();
            for (int i = 0; i < games.Count; i++)
            {
                states.Add(_context.GamesStates.ToList().FindLast(x => x.GameId == games[i].Id));
            }
            if (states.Exists(x => DateTime.Now.Subtract(x.Time).TotalMinutes >= 3))
            {
                index = states.FindIndex(x => DateTime.Now.Subtract(x.Time).TotalMinutes >= 3);
                Game newForm = games[index];
                newForm.Result = _context.GameResults.ToList()[_context.GameResults.ToList().FindIndex(x => x.Name.Equals("technical draw"))];
                _context.Games.Update(newForm);
                _context.SaveChanges();
                CheckOverdueGames();
            }
            return;
        }

        [HttpPost("end-game/{gameid}&{result}")]
        [Authorize(Roles = Authentication.UserRoles.User)]
        public ActionResult Put(int gameid, string result)
        {
            Game newForm = _context.Games.ToList()[_context.Games.ToList().FindIndex(x => x.Id == gameid)];
            newForm.Result = _context.GameResults.ToList()[_context.GameResults.ToList().FindIndex(x => x.Name == result)];
            if (newForm.Result != null && newForm.Result != null)
            {
                _context.Games.Update(newForm);
                _context.SaveChanges();
                return Ok();
            }
            return BadRequest();
        }
    }
}
