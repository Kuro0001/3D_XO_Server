using Microsoft.AspNetCore.Mvc;
using _3D_XO_Server.Models;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace _3D_XO_Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GamesStateController : ControllerBase
    {
        private readonly DBContext _context;
        public GamesStateController(DBContext context)
        {
            _context = context;
        }

        [HttpPost("set-state/{gameid}&{userid}&{state}")]
        public ActionResult SetState(int gameid, string userid, string state)
        {
            if (state.Length > 0 && gameid > -1 && !userid.Equals(string.Empty))
            {
                GamesState block = new GamesState()
                {
                    UserId = userid,
                    GameId = gameid,
                    State = state,
                    IsReaded = false,
                    Time = DateTime.Now
                };
                _context.GamesStates.Add(block);
                _context.SaveChanges();
                return Ok();
            }
            else
            {
                return NoContent();
            }
        }

        [HttpGet("get-state/{gameid}&{userid}")]
        public  IActionResult Get(int gameid, string userid)
        {
            if (gameid > 0 && !userid.Equals(string.Empty))
            {
                List<GamesState> states = _context.GamesStates.ToList().FindAll(x => x.GameId == gameid);
                if (!states[states.Count - 1].UserId.Equals(userid))
                {
                    GamesState block = states[states.Count - 1];
                    /*
                    block.IsReaded = true;
                    _context.Entry(block).State = EntityState.Modified;
                    _context.SaveChanges();
                    */
                    return Ok(new
                    {
                        Id = block.Id,
                        GameId = block.GameId,
                        UserId = block.UserId,
                        IsReaded = block.IsReaded,
                        State = block.State,
                        Time = block.Time,
                    });
                }
                else
                {
                    return NoContent();
                }
            }
            return BadRequest();
        }
        [HttpGet("get-last-state/{gameid}&{userid}")]
        [Authorize]
        public IActionResult GetLast(int gameid, string userid)
        {
            if (gameid > 0 && !userid.Equals(string.Empty))
            {
                GamesState block = _context.GamesStates.ToList()[_context.GamesStates.ToList().Count - 1];
                return Ok(new
                {
                    Id = block.Id,
                    GameId = block.GameId,
                    UserId = block.UserId,
                    IsReaded = block.IsReaded,
                    State = block.State,
                    Time = block.Time,
                });
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
