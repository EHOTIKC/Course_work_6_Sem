using System;

namespace Course_work_6_Sem
{
    internal class Product : IComparable<Product>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }

        public Product(int id, string name, double price)
        {
            Id = id;
            Name = name;
            Price = price;
        }

        public int CompareTo(Product other)
        {
            if (other == null) return 1;
            return this.Price.CompareTo(other.Price);
        }

        public override string ToString()
        {
            return $"[ID: {Id}] {Name} - ${Price:F2}";
        }
    }
}