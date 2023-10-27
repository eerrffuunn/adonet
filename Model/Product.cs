namespace adonet
{
    public class Product
    {
        // Keep the constuctor arguments as-is.
        public Product(int id, string name, double price, int stock, int vatPercentage, string categoryName)
        {
            ID = id;
            Name = name;
            Price = price;
            Stock = stock;
            VatPercentage = vatPercentage;
            CategoryName = categoryName;

            // You may add code here for exercise 2.
            p_ID = id;
            p_Name = name;
            p_Price = price;
            p_stock = stock;

        }
        //generating new values to keeo original record of product 
        //to compare after update 
        //in concurency update will check new values to original if match it will update

        // Keep existing properties as-is.
        public int ID { get; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }
        public int VatPercentage { get; }
        public string CategoryName { get; }

        // You may add extra properties here for exercise 2.
        public int p_ID { get; }
        public string p_Name { get; set; }
        public int p_stock { get; set; }
        public double p_Price { get; set; }
    }
}
