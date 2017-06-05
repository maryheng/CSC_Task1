using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using CSC_Task1.Models;
using Newtonsoft.Json;
using System.Net;

namespace CSC_Task1.APIs
{
    // [Route("products/api/v1/")]
    [Route("api/v1/")]
    // localhost:51471/api/v1/{name}
    public class ProductsV1Controller : Controller
    {
        private ProductRepository products = GlobalVariable.products;
        
        // Code for Version 1

        [HttpGet]
        [Route("version")]
        // localhost:51471/api/v1/version
        public string[] GetVersion()
        {
            return new string[]
            {
                "This is version 2!"
            };
        }

        [HttpGet]
        [Route("message")]
        // localhost:9000/api/v1/message?name1=ji&name2=jii1&name3=ji3
        public HttpResponseMessage GetMultipleNames(String name1, string name2, string name3)
        {
            var response = new HttpResponseMessage();
            response.Content = new StringContent("<html><body>Hello World " +
            " name1 =" + name1 +
            " name2= " + name2 +
            " name3=" + name3 +
            " is provided in path parameter</body></html>");
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;
        }

        [HttpGet]
        // [Route("products")]
        // localhost:51471/api/v1/products
        public IEnumerable<Product> Get()
        {
            return products.getAllProducts();
        }

        [HttpGet("{id:int:min(1)}")]
        // localhost:51471/api/v1/products/1
        public IActionResult Get(int id)
        {
            var product = products.getAllProducts().FirstOrDefault((p) => p.Id == id); //find by ID
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        // GET api/searchByCategory/
        [HttpGet("searchByCategory/{category}")]
        public IActionResult GetByCategory(string category)
        {
            var product = products.getAllProducts() //get products 
                .FirstOrDefault(p => p.Category.ToUpper() == category.ToUpper());
            //find first by converting both strings to upper case
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]string value)
        {
            var customMessage = "";
            var newProductInput = JsonConvert.DeserializeObject<dynamic>(value);
            try
            {
                if (ModelState.IsValid)
                { // create new product, extract values from formData and input into obj
                    Product product = new Product();
                    product.Id = Convert.ToInt32(newProductInput.id.Value);
                    product.Name = newProductInput.name.Value;
                    product.Category = newProductInput.category.Value;
                    product.Price = Convert.ToDecimal(newProductInput.price.Value);

                    products.AddProducts(product);
                }
                else
                {
                    return BadRequest(HttpStatusCode.BadRequest);
                }
            }
            catch (Exception e)
            {
                customMessage = "Unable to save new product " + newProductInput.name.Value;
                object httpFailRequestResultMessage = new { message = customMessage };
                //Return a bad http request message to the client
                return BadRequest(httpFailRequestResultMessage);
            }
            return new OkObjectResult(new { message = "Saved Product record." });

        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]string value)
        {
            var customMessage = "";
            var ProductChangeInput = JsonConvert.DeserializeObject<dynamic>(value);
            try
            {
                foreach (var product in products.getAllProducts())
                {
                    if (product.Id == id)
                    { // getting the values and put it in the rpoduct
                        product.Name = ProductChangeInput.name.Value;
                        product.Category = ProductChangeInput.category.Value;
                        product.Price = Convert.ToDecimal(ProductChangeInput.price.Value);
                    }
                }

            }
            catch (Exception e)
            {
                customMessage = "Unable to Update new product " + ProductChangeInput.name.Value;
                object httpFailRequestResultMessage = new { message = customMessage };
                //Return a bad http request message to the client
                return BadRequest(httpFailRequestResultMessage);
            }
            return new OkObjectResult(new { message = "Updated Product record." });
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var customMessage = "";
            var productName = "";
            try
            {
                var productToRemove = products.getAllProducts().Single(product => product.Id == id);
                productName = productToRemove.Name;
                products.DeleteProduct(id);
            }
            catch (Exception e)
            {
                customMessage = "Unable to Delete product!";
                object httpFailRequestResultMessage = new { message = customMessage };
                //Return a bad http request message to the client
                return BadRequest(httpFailRequestResultMessage);
            }

            return new OkObjectResult(new { message = "Deleted " + productName + " Product." });

        }


    }
}