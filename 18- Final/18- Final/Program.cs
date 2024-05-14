using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace _18__Final
{
    public class Product
    {
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public double Price { get; set; }
        public int StockQuantity { get; set; }

        public Product(string name, string brand, string model, double price, int stockQuantity)
        {
            Name = name;
            Brand = brand;
            Model = model;
            Price = price;
            StockQuantity = stockQuantity;
        }

        public override string ToString() //How the information is display when user selects option 2 to view the items
        {
            return "Name: " + Name + " Brand: " + Brand + " Model: " + Model + " Price: " + Price + " Stock Quantity: " + StockQuantity;
        }
    }

    public class ProductCsvReader
    {
        private readonly string filePath;
    
        public ProductCsvReader(string filePath) //Initiates the file path from the cs to csv
        {
            this.filePath = filePath;
        }

        public List<Product> ReadProductsFromCsv() //Script to read the information from the csv file
        {
            List<Product> products = new List<Product>();

            try
            {
                using (var reader = new StreamReader(filePath))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();

                        if (line != null)
                        {
                            var values = line.Split(',');
                            string name = values[0];
                            string brand = values[1];
                            string model = values[2];
                            double price = double.Parse(values[3]);
                            int stockQuantity = int.Parse(values[4]);

                            products.Add(new Product(name, brand, model, price, stockQuantity));
                        }
                    }
                }
            }

            catch (Exception) 
            {
                Console.WriteLine("Inventory is currently empty, please select 'Load Inventory from File' to start a new manifest. \n");
            }

            return products;
        }

        public void WriteProductsToCsv(List<Product> products) //Clarifies format of the information being written to the csv
        {
            try
            {
                using(var writer = new StreamWriter(filePath))
                {
                    foreach (var product in products)
                    {
                        string line = (product.Name + "," + product.Brand + "," + product.Model + "," + product.Price + "," + product.StockQuantity);
                        
                        writer.WriteLine(line);
                    }
                }
                Console.Clear();
                Console.WriteLine("Inventory saved. ");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occured while writing to the csv file: " + ex.Message);
            }
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            string csvFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Programming", "18- Final", "18- Final", "Products.csv");
        
            ProductCsvReader csvReader = new ProductCsvReader(csvFilePath);

            List<Product> products = csvReader.ReadProductsFromCsv();
            bool inventoryLoad = false; //Requires loading
            bool inventorySave = false; //Requires saving

            while (true) //Main menu interface
            {
                Console.WriteLine("Electronics Inventory Management System");
                Console.WriteLine("1. Add Item");
                Console.WriteLine("2. View All Items");
                Console.WriteLine("3. Search for Item");
                Console.WriteLine("4. Save Inventory to File");
                Console.WriteLine("5. Load Inventory to File");
                Console.WriteLine("6. Exit");
                
                string? choice = Console.ReadLine();
                Console.Clear();
                switch (choice) //Clarifies options 1-6
                {
                    case "1":
                        if (inventoryLoad)
                        {
                            AddItem(products);
                            inventorySave = true;
                        }
                        else
                        {
                            Console.WriteLine("Please load inventory before adding new items.");
                        }
                        break;
                    case "2":
                        if (inventoryLoad)
                            ViewItems(products);
                        else
                            Console.WriteLine("Please load inventory.");
                        break;
                    case "3":
                        if (inventoryLoad)
                            SearchItem(products);
                        else
                            Console.WriteLine("Please load inventory.");
                        break;
                    case "4":
                            SaveInventory(csvReader, products);
                            inventorySave = false;
                        break;
                    case "5":
                            products = LoadInventory(csvReader);
                            inventoryLoad = true;
                            inventorySave = false;
                       break;
                    case "6":
                        if (inventorySave) //Gives user the option to close without saving, but requires them to save if they so desire.
                        {
                            Console.WriteLine("Would you like to save before exiting? (Y/N)");
                            string? save = Console.ReadLine()?.ToUpper();
                            if (save == "Y") //Closes after saving
                            {
                                SaveInventory(csvReader, products);
                                Environment.Exit(0);
                            }

                            else if (save == "N") //Closes without saving
                            {
                                Environment.Exit(0);
                            }
                        }

                        Environment.Exit(0); //Allows user to exit after opening program
                        break;
                    default:
                        Console.WriteLine("Invalid option. Please choose again.");
                        break;
                }
            }
        }
        
        static void AddItem(List<Product> products) //Add Product
        {
            string name = "";
            string brand = "";
            string model = "";

            while (true) //Product Name
            {
                try
                {
                    Console.WriteLine("Enter the product name: ");
                    name = Console.ReadLine()?.Trim() ?? "";
                    if (string.IsNullOrEmpty(name))
                    {
                        throw new ArgumentException("Name cannot be empty.");
                    }
                    break;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            while (true) //Product Brand
            {
                try
                {
                    Console.WriteLine("Enter the product brand: ");
                    brand = Console.ReadLine()?.Trim() ?? "";
                    if (string.IsNullOrEmpty(brand))
                    {
                        throw new ArgumentException("Brand cannot be empty.");
                    }
                    break;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            while (true) //Product Model
            {
                try
                {
                    Console.WriteLine("Enter the product model: ");
                    model = Console.ReadLine()?.Trim() ?? "";
                    if (string.IsNullOrEmpty(model))
                    {
                        throw new ArgumentException("Model cannot be empty.");
                    }
                    break;
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            double price = 0;
            bool validPrice = false;

            while (!validPrice) //Product Price
            {
                Console.WriteLine("Enter the product price: ");
                string priceInput = Console.ReadLine() ?? "0";
                try
                {
                    price = double.Parse(priceInput);
                    if (price >= 0)
                    {
                        validPrice = true;
                    }
                    else
                    {
                        Console.WriteLine("Please enter a valid input.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Invalid input. Please enter a valid price." + ex.Message);
                }
            }

            int stockQuantity = 0;
            bool validStockQuantity = false;

            while (!validStockQuantity) //Product Stock Quantity
            {
                Console.WriteLine("Enter the stock quantity: ");
                string stockQuantityInput = Console.ReadLine() ?? "0";
                try
                {
                    stockQuantity = int.Parse(stockQuantityInput);
                    if(stockQuantity >= 1)
                    {
                        validStockQuantity = true;
                    }
                    else
                    {
                        Console.WriteLine("Please enter a valid input");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Invalid input. Please enter a valid stock quantity: " + ex.Message);
                }
            }

            products.Add(new Product(name, brand, model, price, stockQuantity)); //Add information to "products" which gets written to the csv
            Console.Clear();
            Console.WriteLine("Product added. ");
        }

        static void ViewItems(List<Product> products) //View Method
        {
            foreach (Product product in products)
            {
                Console.WriteLine(product.ToString());
            }
            Console.WriteLine("\n");
        }

        static void SearchItem(List<Product> products) //Trims and lowercases user input the clarify search from csv
        {
            Console.WriteLine("Search by item name: ");
            string? searchItem = Console.ReadLine()?.Trim().ToLower();
            Console.WriteLine();

            bool found = false;
            foreach (Product product in products)
            {
                if (product.Name?.ToLower() == searchItem)
                {
                    Console.WriteLine("Item found: ");
                    Console.WriteLine(product.ToString());
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                Console.WriteLine("Item not found." + "\n");
            }
            Console.WriteLine("Press any key to refresh");
            Console.ReadLine();
            Console.Clear();
        }
        
        static void SaveInventory(ProductCsvReader csvReader, List<Product> products) //Saves user information to csv
        {
            csvReader.WriteProductsToCsv(products);
            Console.WriteLine("Inventory saved. \n");
        }

        static List<Product> LoadInventory(ProductCsvReader csvReader) //Loads user information from csv
        {
            List<Product> products = csvReader.ReadProductsFromCsv();
            Console.WriteLine("Inventory Loaded. \n");
            return products;
        }
    }
}
