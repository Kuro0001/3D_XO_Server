using Microsoft.AspNetCore.Mvc;
using _3D_XO_Server.Models;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace _3D_XO_Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class NewsBlockController : ControllerBase
    {
        private DBContext _context;
        public NewsBlockController(DBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IEnumerable<NewsBlock> Get()
        {
            List<NewsBlock> result = _context.NewsBlocks.ToList();
            return _context.NewsBlocks.ToList();
        }

        [HttpGet("{pageLength}")]
        public IEnumerable<NewsBlock> Get(int pageLength)
        {
            List<NewsBlock> result = new List<NewsBlock>();
            result = _context.NewsBlocks.ToList();
            if (_context.NewsBlocks.ToList().Count > pageLength && pageLength > 0)
                result = result.GetRange(_context.NewsBlocks.ToList().Count - pageLength, pageLength);
            result.Reverse();
            return result;
        }
        [HttpGet("oneblock/{id}")]
        public NewsBlock GetBlock(int id)
        {
            return _context.NewsBlocks.Find(id);
        }

        [HttpPost]
        [Authorize(Roles = Authentication.UserRoles.Admin)]
        public ActionResult Post(NewsBlock newsblock)
        {
            if (newsblock.Id == 0)
            {
                if (newsblock.Title.Length > 0 && newsblock.Text.Length > 0)
                {
                    _context.NewsBlocks.Add(newsblock);
                    _context.SaveChanges();
                    return Ok();
                }
                else
                {
                    return NoContent();
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = Authentication.UserRoles.Admin)]
        public ActionResult<NewsBlock> Put(int id, NewsBlock note)
        {
            if (id != note.Id)
            {
                return BadRequest();
            }

            if (!_context.NewsBlocks.Any(s => s.Id == id))
                return NotFound();

            _context.Entry(note).State = EntityState.Modified;

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

        [HttpDelete("{id}")]
        [Authorize(Roles = Authentication.UserRoles.Admin)]
        public ActionResult Delete(int id)
        {
            NewsBlock note = _context.NewsBlocks.Find(id);
            if (note != null)
            {
                _context.NewsBlocks.Remove(note);
                _context.SaveChanges();
                return StatusCode(StatusCodes.Status204NoContent);
            }
            else
                return NotFound();
        }

    }
}
