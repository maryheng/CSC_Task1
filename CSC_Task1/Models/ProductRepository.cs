using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSC_Task1.Models
{
    public class ProductRepository
    {
        private List<Product> products = new List<Product>
        {
            new Product{Id = 1, Name = "Apple", Category = "Fruit", Price = 3 },
            new Product{Id = 2, Name = "Barbie Doll", Category = "Toy", Price = 20 },
            new Product{Id = 3, Name = "Nuggets", Category = "Food", Price = 2 },
            new Product{Id = 4, Name = "Teh Peng", Category = "Drink", Price = 1.50M }
        };


        public List<Product> getAllProducts()
        {
            return products;
        }

        public void AddProducts(Product productInput)
        {
            products.Add(productInput);
        }

        public void DeleteProduct(int index)
        {
            var productToRemove = products.Single(product => product.Id == index);
            products.Remove(productToRemove);
        }

        public void updateProduct(int index, Product ProductChangeInput)
        {
            foreach (var product in products)
            {
                if (product.Id == index)
                {
                    product.Name = ProductChangeInput.Name;
                    product.Category = ProductChangeInput.Category;
                    product.Price = ProductChangeInput.Price;
                }
            }
        }

        public Product getProductById(int index)
        {
            foreach (var product in products)
            {
                if (product.Id == index)
                {
                    return product;
                }
            }
            return null;
        }

    }

    public static class GlobalVariable
    {
        public static ProductRepository products = new ProductRepository();
    }
}
