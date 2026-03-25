using System;

namespace Course_work_6_Sem
{
    internal class Product : IComparable<Product>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }

        public string Manufacturer { get; set; }
        public string Supplier { get; set; }
        public string Country { get; set; }
        public DateTime ProductionDate { get; set; }

        public Product(int id, string name, double price, string manufacturer, string supplier, string country, DateTime productionDate)
        {
            Id = id;
            Name = name;
            Price = price;
            Manufacturer = manufacturer;
            Supplier = supplier;
            Country = country;
            ProductionDate = productionDate;
        }

        public int CompareTo(Product other)
        {
            if (other == null) return 1;

            int countryComparison = string.Compare(this.Country, other.Country, StringComparison.Ordinal);
            if (countryComparison != 0) return countryComparison;

            int manufacturerComparison = string.Compare(this.Manufacturer, other.Manufacturer, StringComparison.Ordinal);
            if (manufacturerComparison != 0) return manufacturerComparison;

            int supplierComparison = string.Compare(this.Supplier, other.Supplier, StringComparison.Ordinal);
            if (supplierComparison != 0) return supplierComparison;

            return this.Price.CompareTo(other.Price);
        }

        public override string ToString()
        {
            return $"[ID: {Id}] {Country}, {Manufacturer} ({Supplier}) | {Name} - ${Price:F2} | Date: {ProductionDate:yyyy-MM-dd}";
        }
    }
}