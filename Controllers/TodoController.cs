using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using TodoBackend.Data;
using TodoBackend.Models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class TodoController : ControllerBase
    {
        private readonly DbContextBase _context;

        public TodoController(DbContextBase context)
        {
            _context = context;
            // if (_context.TodoItems.Count() == 0)
            // {
            //     _context.TodoItems.Add(new TodoItem { Title = "Item1",Description = "ok g",Userid=3 });
            //     _context.SaveChanges();
            // }
        }
        [HttpGet]
        [Route("[action]")]
        [Authorize]

        public ActionResult<List<TodoItem>> GetAll()
        {
            return _context.TodoItems.ToList();
        }

        [HttpPost]
        [Route("[action]")]
        [Authorize]
        public IActionResult Insert([FromBody] TodoItem model)
        {
            model.Datetime = DateTime.Now;
            _context.TodoItems.Add(model);
            _context.SaveChanges();
            return Ok(1);
        }
        [HttpPost]
        [Route("[action]")]
        [Authorize]
        public IActionResult Update([FromBody] TodoItem model)
        {
            model.Datetime = DateTime.Now;
            _context.TodoItems.Update(model);
            _context.SaveChanges();
            return Ok(1);
        }

        [HttpPost]
        [Route("[action]")]
        [Authorize]
        public IActionResult FindbyId([FromBody] Params obj)
        {
            var userid = Convert.ToInt32(obj.paramstring);

            var item = _context.TodoItems.Where(f => f.Userid == userid).ToList();
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost]
        [Route("[action]")]
        [Authorize]
        public IActionResult DeletebyId([FromBody] Params obj)
        {
            var id = Convert.ToInt32(obj.paramstring);
            TodoItem item = _context.TodoItems.Where(f => f.Id == id).FirstOrDefault();
            if (item == null)
            {
                return NotFound();
            }

            //   entities.DeleteObject(stud);
            _context.TodoItems.Remove(item);
            _context.SaveChanges();

            return Ok(1);
        }


        [HttpGet("{id}")]
        // [Route("[action]")]  

        public ActionResult<TodoItem> GetById(int id)
        {
            var item = _context.TodoItems.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }

    }
}
