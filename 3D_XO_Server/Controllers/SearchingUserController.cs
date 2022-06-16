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
    public class SearchingUserController : ControllerBase
    {
        private readonly DBContext _context;
        public SearchingUserController(DBContext context)
        {
            _context = context;
        }

        [HttpPost("start-search-2/{userid}")]
        [Authorize]
        public async Task<IActionResult> StartSearching2(string userid)
        {
            if (userid != null)
            {
                CheckInactiveUsers();
                List<SearchingUser> users = _context.SearchingUsers.ToList();
                if (users.Exists(x => !x.UserId.Equals(userid)))
                {
                    SearchingUser user2 = users.Find(x => !x.UserId.Equals(userid));
                    GameResult gr = _context.GameResults.ToList()[_context.GameResults.ToList().FindIndex(x => x.Name.Equals("continues"))];
                    Game gameNew = new Game()
                    {
                        User1Id = userid,
                        User2Id = user2.UserId,
                        Date = DateTime.Now,
                        Result = gr
                    };
                    _context.SearchingUsers.Remove(user2);
                    _context.SaveChanges();
                    _context.Games.Add(gameNew);
                    _context.SaveChanges();
                    int gameIndex = _context.Games.ToList().FindLastIndex(x => x.User1Id.Equals(gameNew.User1Id) && x.User2Id.Equals(gameNew.User2Id));
                    Game gameActuale = _context.Games.ToList()[gameIndex];
                    GamesState newState = new GamesState()
                    {
                        GameId = gameActuale.Id,
                        UserId = gameActuale.User1Id,
                        IsReaded = true,
                        State = "start",
                        Time = DateTime.Now
                    };
                    _context.GamesStates.Add(newState);
                    _context.SaveChanges();
                    return Ok(new
                    {
                        Id = gameActuale.Id,
                        User1Id = gameActuale.User1Id,
                        User2Id = gameActuale.User2Id,
                        Date = gameActuale.Date
                    });
                }
                else
                {
                    SearchingUser userNew = new SearchingUser()
                    {
                        UserId = userid,
                        Time = DateTime.Now
                    };
                    _context.SearchingUsers.Add(userNew);
                    _context.SaveChanges();
                    return NoContent();
                }
            }
            else return BadRequest();
        }

        [HttpPost("game-search-2/{userid}")]
        [Authorize]
        public async Task<IActionResult> GameSearch2(string userid)
        {
            if (userid != null)
            {
                CheckInactiveUsers();
                List<SearchingUser> users = _context.SearchingUsers.ToList();
                List<Game> games = _context.Games.ToList();
                GameResult res = _context.GameResults.ToList()[_context.GameResults.ToList().FindIndex(x => x.Name.Equals("continues"))];
                if (games.Exists(x => (x.User1Id.Equals(userid) || x.User2Id.Equals(userid)) && x.Result.Id == res.Id))
                {
                    Game gameActuale = games.Find(x => (x.User1Id.Equals(userid) || x.User2Id.Equals(userid)) && x.Result.Id == res.Id);
                    return Ok(new
                    {
                        Id = gameActuale.Id,
                        User1Id = gameActuale.User1Id,
                        User2Id = gameActuale.User2Id,
                        Date = gameActuale.Date
                    });
                }
                else
                {
                    SearchingUser user = users.Find(x => x.UserId.Equals(userid));
                    if (user != null)
                    {
                        user.Time = DateTime.Now;
                        _context.Entry(user).State = EntityState.Modified;
                        _context.SaveChanges();
                    }
                    return NoContent();
                }
            }
            else return BadRequest();
        }















        [HttpPost("start-search/{userid}")]
        [Authorize]
        public ActionResult StartSearching(string userid)
        {
            CheckInactiveUsers();
            List<SearchingUser> users = _context.SearchingUsers.ToList();
            SearchingUser user = null;
            int resultContinueId = _context.GameResults.ToList()[_context.GameResults.ToList().FindIndex(x => x.Name.Equals("continues"))].Id;
            if (!users.Exists(x => x.UserId.Equals(userid)))
            {
                if (!_context.Games.ToList().Exists(x => x.Result.Id == resultContinueId && (x.User1Id.Equals(userid) || x.User2Id.Equals(userid))))
                {
                    user = new SearchingUser()
                    {
                        UserId = userid,
                        Time = DateTime.Now
                    };
                    _context.Add(user);
                    _context.SaveChanges();
                }
            }
            else
            {
                user = users.Find(x => x.UserId.Equals(userid));
                user.Time = DateTime.Now;
                _context.Entry(user).State = EntityState.Modified;
                _context.SaveChanges();
            }
            return Ok();
        }

        [HttpPost("end-search/{userid}")]
        [Authorize]
        public ActionResult EndSearching(string userid)
        {
            CheckInactiveUsers();
            List<SearchingUser> users = _context.SearchingUsers.ToList();
            SearchingUser user = null;
            if (users.Exists(x => x.UserId == userid))
            {
                user = new SearchingUser()
                {
                    UserId = userid,
                    Time = DateTime.Now
                };
                _context.SearchingUsers.Remove(user);
            }
            try
            {
                _context.SaveChanges();
                return Ok();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("game-search/{userid}")]
        [Authorize]
        public IActionResult GameSearch(string userid)
        {
            CheckInactiveUsers();
            List<SearchingUser> users = _context.SearchingUsers.ToList();
            int index1 = users.FindIndex(x => x.UserId.Equals(userid));
            int index2 = -1;
            /*
            while (users.Exists(x => DateTime.Now.Subtract(x.Time).TotalSeconds > 30))
            {
                int i = users.FindIndex(x => DateTime.Now.Subtract(x.Time).TotalSeconds > 30);
                _context.SearchingUsers.Remove(users[i]);
                _context.SaveChanges();
                users = _context.SearchingUsers.ToList();
            }
            */
            int resultContinueId = _context.GameResults.ToList()[_context.GameResults.ToList().FindIndex(x => x.Name.Equals("continues"))].Id;
            if (_context.Games.ToList().Exists(x => (x.User1Id.Equals(userid) || x.User2Id.Equals(userid)) && x.Result.Id == resultContinueId))
            {
                int gameIndex = _context.Games.ToList().FindLastIndex(x => (x.User1Id.Equals(userid) || x.User2Id.Equals(userid)) && x.Result.Id == resultContinueId);
                Game g = _context.Games.ToList().Find(x => x.Id == gameIndex);
                return Ok();
            }
            if (users.Exists(x => x.UserId.Equals(userid)))
                if (users.Exists(x => !x.UserId.Equals(userid)))
                {
                    index2 = users.FindIndex(x => !x.UserId.Equals(userid) && (DateTime.Now.Subtract(x.Time).TotalSeconds < 30));
                    GameResult gr = _context.GameResults.ToList()[_context.GameResults.ToList().FindIndex(x => x.Name.Equals("continues"))];
                    if (index2 > 0)
                    {
                        Game game = new Game()
                        {
                            User1Id = users[index1].UserId,
                            User2Id = users[index2].UserId,
                            Date = DateTime.Now,
                            Result = gr
                        };                        

                        _context.SearchingUsers.Remove(users[index1]);
                        _context.SaveChanges();
                        users = _context.SearchingUsers.ToList();
                        index2 = users.FindIndex(x => x.UserId.Equals(game.User2Id));
                        _context.SearchingUsers.Remove(users[index2]);
                        _context.SaveChanges();
                        _context.Games.Add(game);
                        _context.SaveChanges();

                        int gameIndex = _context.Games.ToList().FindLastIndex(x => x.User1Id.Equals(game.User1Id) && x.User2Id.Equals(game.User2Id));
                        Game newGame = _context.Games.ToList()[gameIndex];
                        GamesState newState = new GamesState()
                        {
                            GameId = newGame.Id,
                            UserId = newGame.User1Id,
                            IsReaded = true,
                            State = "000000000000000000000000000000000000000000000",
                            Time = DateTime.Now
                        };
                        _context.GamesStates.Add(newState);
                        _context.SaveChanges();
                        Game g = _context.Games.ToList().Find(x => x.Id == newGame.Id);
                        return Ok(new
                        {
                            Id = g.Id,
                            User1Id = g.User1Id,
                            User2Id = g.User2Id,
                            Date = g.Date
                        });
                    }
                }
                else
                {
                    SearchingUser user = _context.SearchingUsers.ToList().Find(x => x.UserId.Equals(userid));
                    if (user != null)
                    {
                        user.Time = DateTime.Now;
                        _context.Entry(user).State = EntityState.Modified;
                        _context.SaveChanges();
                    }
                }
            return NoContent();
        }

        public void CheckInactiveUsers()
        {
            //TODO не проверяет
            int index = -1;
            if (_context.SearchingUsers.ToList().Exists(x => DateTime.Now.Subtract(x.Time).TotalSeconds > 15))
                index = _context.SearchingUsers.ToList().FindLastIndex(x => DateTime.Now.Subtract(x.Time).TotalSeconds > 15);
            if (index > -1)
            {
                _context.SearchingUsers.Remove(_context.SearchingUsers.ToList()[index]);
                _context.SaveChanges();
                CheckInactiveUsers();
            }
            return;
        }









    }
}
