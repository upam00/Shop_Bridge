using Shop_Bridge.Services;
using Shop_Bridge.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System;

namespace Shop_Bridge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly ItemService _itemService;

        public ItemsController(ItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet]
        public ActionResult<List<Item>> Get() =>
            _itemService.Get();

        [HttpGet("{id:length(24)}", Name = "GetItem")]
        public ActionResult<Item> Get(string id)
        {
            var item = _itemService.Get(id);

            if (item == null)
            {
                return NotFound();
            }

            return item;
        }

        [HttpPost]
        public ActionResult<Item> Create([FromForm] Uploader Data)
        {

            string s;
            using (var ms = new MemoryStream())
            {
                Data.ImageFile.CopyTo(ms);
                var fileBytes = ms.ToArray();
                //s = fileBytes;
                s = Convert.ToBase64String(fileBytes);

            }




            Item newItem = new Item();
            newItem.Id = Data.Id;
            newItem.ItemName = Data.ItemName;
            newItem.Price = Data.Price;
            newItem.About = Data.About;
            newItem.ImageBase64 = s;



            _itemService.Create(newItem);

            return CreatedAtRoute("GetItem", new { id = newItem.Id.ToString() }, newItem);
        }

        [HttpPost("items/populate")]
        public IActionResult Populate()
        {
            string[] image = { "iVBORw0KGgoAAAANSUhEUgAAADIAAAAyCAYAAAAeP4ixAAAARElEQVR42u3PMREAAAgEIE1u9DeDqwcN6FSmHmgRERERERERERERERERERERERERERERERERERERERERERERERERkYsFbE58nZm0+8AAAAAASUVORK5CYII=", "iVBORw0KGgoAAAANSUhEUgAAADIAAAAyCAYAAAAeP4ixAAAARUlEQVR42u3PQREAAAQAMBpoLDoZfN3WYBlTHQ+kiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiMjFAvcETe9JDKwVAAAAAElFTkSuQmCC", "iVBORw0KGgoAAAANSUhEUgAAADIAAAAyCAYAAAAeP4ixAAAARElEQVR42u3PQREAAAQAMFr7iU4GX7c1WEZNxwMpIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiIiInKxZ0diP6aFmBkAAAAASUVORK5CYII=" };
            Random rn = new Random(20);

            for (int i = 1; i <= 50; i++)
            {
                _itemService.Create(new Item
                {
                    ItemName = new Bogus.DataSets.Lorem().Word(),
                    About = new Bogus.DataSets.Lorem().Paragraph(),
                    Price = new Bogus.Randomizer().Number(10, 100),
                    ImageBase64 = image[rn.Next(3)]
                });
            }

            return Ok(new
            {
                message = "success"
            });
        }



        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Item itemIn)
        {
            var item = _itemService.Get(id);

            if (item == null)
            {
                return NotFound();
            }

            _itemService.Update(id, itemIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var item = _itemService.Get(id);

            if (item == null)
            {
                return NotFound();
            }

            _itemService.Remove(item.Id);

            return NoContent();
        }

        [HttpGet("SSP")]
        public IActionResult SearchSortPage([FromQuery(Name = "s")] string s,
            [FromQuery(Name = "sort")] string sort,
            [FromQuery(Name = "page")] int page)
        {
            
            return Ok(_itemService.Query(s, sort, page));

        }

        
    }
}