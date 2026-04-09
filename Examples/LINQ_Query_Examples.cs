using System;
using System.Collections.Generic;

List<Product> Products = 
    [
        new Product { Id = 1, Name = "Laptop", Price = 899.99m, Category = "Electronics" },
        new Product { Id = 2, Name = "Smartphone", Price = 599.99m, Category = "Electronics" },
        new Product { Id = 3, Name = "Tablet", Price = 499.99m, Category = "Electronics" },
        new Product { Id = 4, Name = "Shoes", Price = 59.99m, Category = "Apparel" },
        new Product { Id = 5, Name = "Desk Chair", Price = 149.99m, Category = "Office" },
        new Product { Id = 6, Name = "Coffee Maker", Price = 79.50m, Category = "Home" },
        new Product { Id = 7, Name = "T-Shirt", Price = 19.99m, Category = "Apparel" },
        new Product { Id = 8, Name = "Wireless Mouse", Price = 29.99m, Category = "Electronics" },
        new Product { Id = 9, Name = "Monitor", Price = 199.99m, Category = "Electronics" },
        new Product { Id = 10, Name = "Sci-Fi Novel", Price = 14.99m, Category = "Books" },
        new Product { Id = 11, Name = "Headphones", Price = 129.99m, Category = "Electronics" },
        new Product { Id = 12, Name = "Standing Desk", Price = 299.99m, Category = "Office" }
    ];

    List<Sale> Sales = 
    [
        new Sale { ProductId = 1, QuantitySold = 5, Date = new DateTime(2026, 3, 1) },
        new Sale { ProductId = 2, QuantitySold = 10, Date = new DateTime(2026, 3, 2) },
        new Sale { ProductId = 3, QuantitySold = 7, Date = new DateTime(2026, 3, 3) },
        new Sale { ProductId = 1, QuantitySold = 3, Date = new DateTime(2026, 3, 4) },
        new Sale { ProductId = 4, QuantitySold = 20, Date = new DateTime(2026, 3, 5) },
        new Sale { ProductId = 5, QuantitySold = 2, Date = new DateTime(2026, 3, 6) },
        new Sale { ProductId = 6, QuantitySold = 1, Date = new DateTime(2026, 3, 7) },
        new Sale { ProductId = 8, QuantitySold = 15, Date = new DateTime(2026, 3, 8) },
        new Sale { ProductId = 2, QuantitySold = 5, Date = new DateTime(2026, 3, 9) },
        new Sale { ProductId = 9, QuantitySold = 4, Date = new DateTime(2026, 3, 10) },
        new Sale { ProductId = 7, QuantitySold = 25, Date = new DateTime(2026, 3, 11) },
        new Sale { ProductId = 10, QuantitySold = 12, Date = new DateTime(2026, 3, 12) },
        new Sale { ProductId = 4, QuantitySold = 8, Date = new DateTime(2026, 3, 13) },
        new Sale { ProductId = 11, QuantitySold = 6, Date = new DateTime(2026, 3, 14) },
        new Sale { ProductId = 12, QuantitySold = 1, Date = new DateTime(2026, 3, 15) },
    ];

// Query 1 - Find all Electronics
Console.WriteLine("--- Query 1 ---");

var query = from product in Products
            where product.Category == "Electronics"
            select product;

foreach (var device in query)
{
    Console.WriteLine($"{device.Name} - {device.Price}$");
}

// Query 2 - Get only the names of products in the price range of 50 to 100
Console.WriteLine("--- Query 2 ---");

var query2 = from product in Products
            where product.Price >= 50
            where product.Price <= 100
            select product;

foreach (var product in query2)
{
    Console.WriteLine($"{product.Name} [{product.Category}] - {product.Price}");
}

// Query 3 - Find the cheapest product
Console.WriteLine("--- Query 3 ---");

// Another approach is sorting and selecting the first element.
var minPrice = Products.Min(p => p.Price);
var minProduct = Products.Single(p => p.Price == minPrice);
Console.WriteLine($"{minProduct.Name} [{minProduct.Category}] - {minProduct.Price}");

// Query 4 - Calculate the total quantity of all items sold
Console.WriteLine("--- Query 4 ---");

var quantity = Sales.Sum(s => s.QuantitySold);
Console.WriteLine($"Total quantity: {quantity}");

// Query 5 - Find the best-selling product (by quantity)
Console.WriteLine("--- Query 5 ---");

// Another approach is calculating quantity sum for each product ID, sorting list in descending order,
// selecting first element. Then selecting single product which matches highest quantity product ID. 
var joinedSales = Sales.Join(Products, sale => sale.ProductId, product => product.Id, (sale, product) => new {
    product.Name,
    product.Category,
    product.Price,
    sale.QuantitySold,
    sale.Date,
    TotalRevenue = product.Price * sale.QuantitySold
}).ToList();
var maxQuantity = joinedSales.Max(s => s.QuantitySold);
var popularProduct = joinedSales.Single(s => s.QuantitySold == maxQuantity);

Console.WriteLine($"{popularProduct.Name} - {popularProduct.QuantitySold} sold");

// Query 6 - Calculate the average Revenue per year
Console.WriteLine("--- Query 6 ---");

var revenue = joinedSales.Sum(s => s.TotalRevenue);
Console.WriteLine($"Revenue: {revenue}$");

public class Product
{
    public int Id { get; init; }
    public string Name { get; init; } 
    public decimal Price { get; init; }
    public string Category { get; init; }
}

public class Sale
{
    public int ProductId {get; init;}
    public int QuantitySold {get; init;}
    public DateTime Date {get; init;}
}