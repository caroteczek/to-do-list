using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ToDoList.Api.Tests
{
    [TestClass]
    public class ToDoListControllerTests
    {
        [TestMethod]
        public void Get_Returns_All_Products()
        {
            // Arrange
            List<Product> products = [
                new Product("Product 1", ProductCategory.Meble, "opis", 900m),
            new Product("Product 10", ProductCategory.Meble, "opis", 100m),
            new Product("Product 4", ProductCategory.Książka, "opis ksiązki", 100m),
            new Product("Product 8", ProductCategory.Meble, "opis", 150m),
            new Product("Product 2", ProductCategory.Meble, "opis", 10m),
        ];
            MockProductRepository mock = new MockProductRepository(products);
            ProductController sut = new ProductController(mock);

            // Act
            List<ProductDto> result = sut.Get().ToList();

            // Assert
            Assert.AreEqual(5, result.Count());
            result.ForEach(x =>
            {
                Product? product = products.Find(p => p.Nazwa == x.Name);
                Assert.IsNotNull(product);
            });
        }
    }
}
