using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebServer.Models;

namespace WebServer.Controllers{
    [Route("api/[controller]")]
    public class ProductsController:Controller{

        [HttpGet]
        public ActionResult Get()
        {
            if (FakeData.Products != null)
                return Ok(FakeData.Products);
            else
                return NotFound();
        }
        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            if (FakeData.Products.ContainsKey(id))
            {
                var product = FakeData.Products[id];
                return Ok(product);
            }
            else
                return NotFound();
        }

        [HttpGet("price/{from}/{to}")]
        public ActionResult Get(int from, int to)
        {
            var low = 0;
            var high = 0;
            if (FakeData.Products.ContainsKey(from))
                low = from;
            if (FakeData.Products.ContainsKey(to))
                high = to;
            else
                high = FakeData.Products.Keys.Max();
            var products = FakeData.Products.Values.Where(p => p.Price >= low && p.Price <= high).ToArray();
            if (low>high)
            {
               products = FakeData.Products.Values.Where(p => p.Price >= high && p.Price <= low).ToArray();
            }
            if (products.Length > 0)
            {
                return Ok(products);
            }
            else
                return NotFound();
        }
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            if (FakeData.Products.ContainsKey(id))
            {
                FakeData.Products.Remove(id);
                return Ok();
            }
            else
                return NoContent();
        }
        [HttpPost]
        public ActionResult Post([FromBody]Product product)
        {
            product.ID = FakeData.Products.Keys.Max() + 1;
            FakeData.Products.Add(product.ID, product);
            return Created($"index", product);
        }
            
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody]Product product)
        {
            if (FakeData.Products.ContainsKey(id))
            {
                var copy = FakeData.Products[id];
                copy.ID = product.ID;
                copy.Name = product.Name;
                copy.Price = product.Price;
                return Ok();
            }
            else
                return NotFound();
        }

        [HttpPut("raise/{para}")]
        public ActionResult Raise(double para)
        {
            var product = FakeData.Products;
            foreach (var pro in product)
            {
                pro.Value.Price*= para;
            }
            return Ok();
        }
    }
}