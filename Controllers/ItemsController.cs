using Shop_Bridge.Services;
using Shop_Bridge.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System;
using System.Net;
using System.Text;
using Microsoft.AspNetCore.Html;

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

        //////[HttpGet("product/{id: length(24)}")]
        //////public ContentResult GetHtml()
        //////{
        //////    string code = "";
        //////    code += "<!DOCTYPE html>";
        //////    code += "<html>";
        //////    code += "<head>";
        //////    code += "  <meta http-equiv=\"content-type\" content=\"text/html; charset=utf-8\">";
        //////    code += " <link href=\"css/bootstrap.min.css\" rel=\"stylesheet\">";
        //////    code += "    <link href=\"css/StyleSheet.css\" rel=\"stylesheet\">";
        //////    code += "  <title>Product Page</title>";
        //////    code += "</head>";
        //////    code += "<body>";
        //////    code += "  <div class='container'>";
        //////    code += "<div class=\"card mb-3\">";
        //////    //code += "  <img src=\"data:image/jpg;base64," + item.ImageBase64 + "\" class=\"card-img-top\" alt=\"...\">";
        //////    code += "  <div class=\"card-body\">";
        //////    //code += "    <h5 class=\"card-title\">" + item.ItemName + "</h5>";
        //////   // code += "    <p class=\"card-text\">" + item.About + "</p>";
        //////    //code += "    <p class=\"card-text\"><small class=\"text-muted\">" + item.Price + "</small></p>";
        //////    code += "  </div>";
        //////    code += "</div>";
        //////    code += "</div>";
        //////    code += " &lt;script src=\"https://code.jquery.com/jquery-3.2.1.slim.min.js\" integrity=\"sha384-KJ3o2DKtIkvYIK3UENzmM7KCkRr/rE9/Qpg6aAZGJwFDMVNA/GpGFF93hXpG5KkN\" crossorigin=\"anonymous\">&lt;/script&gt;";
        //////    code += "    &lt;script>window.jQuery || document.write('&lt;script src=\"js/jquery-slim.min.js\"><\\/script>')&lt;/script&gt;";
        //////    code += "    &lt;script src=\"js/popper.min.js\">&lt;/script&gt;";
        //////    code += "    &lt;script src=\"js/bootstrap.min.js\">&lt;/script&gt;";
        //////    code += "    &lt;script src=\"js/holder.min.js\">&lt;/script&gt;";
        //////    code += "</body>";
        //////    code += "</html>";

        //////    return new ContentResult
        //////    {
        //////        ContentType = "text/html",
        //////        StatusCode = (int)HttpStatusCode.OK,
        //////        Content = code
        //////    };
        //////}

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
        public IActionResult Update([FromForm] Uploader Data)
        {
            var item = _itemService.Get(Data.Id);

            if (item == null)
            {
                return NotFound();
            }


            item.About = Data.About;
            item.ItemName = Data.ItemName;
            item.Price = Data.Price;

            string s;
            using (var ms = new MemoryStream())
            {
                Data.ImageFile.CopyTo(ms);
                var fileBytes = ms.ToArray();
                //s = fileBytes;
                s = Convert.ToBase64String(fileBytes);

            }
            item.ImageBase64 = s;

            
            _itemService.Update(Data.Id, item);

            return NoContent();
        }

        //string id,Item itemIn

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