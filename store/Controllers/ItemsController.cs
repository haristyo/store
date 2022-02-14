using Microsoft.AspNetCore.Mvc;     
using store.core.Entities;
using store.data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace testing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly AppsContext _context;
        public ItemsController(AppsContext context)
        {
            _context = context;
        }


        [HttpGet]
        public ActionResult<Item> Get()
        {
            //var list = new item();
            //var items = _context.Items
            var items = _context.Items.ToListAsync().Result;
            return Ok(items);
        }
        [HttpPost]
        public ActionResult<Item> Post([FromBody] Item data)
        {
            var items = new Item()
            {

                Name = data.Name,
                Price = data.Price,
                Code = data.Code
            };
            _context.Items.Add(items);
            _context.SaveChanges();

            return Ok(items);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Item>> GetById([FromRoute] int id)
        {
            var items = await _context.Items.FirstOrDefaultAsync(f => f.Id == id);
            return Ok(items);
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Item>> Delete([FromRoute] int id)
        {
            var items = await _context.Items.FindAsync(id);
            _context.Remove(items);
            _context.SaveChanges();
            // var items = result;
            return Ok("Berhasil dihapus");
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Item>> UpdateData([FromRoute] int id, [FromBody] Item data)
        {

            var items = await _context.Items.FindAsync(id);
            items.Name = data.Name;
            items.Price = data.Price;
            _context.SaveChanges();

            return Ok(items);
        }

        //[HttpPost]
    }

}
