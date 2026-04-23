using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Input
{
    public class Beauty // Defining the Beauty class
    {
        public int beauty {  get; set; } // beauty property with getter and setter
        public List<(int, decimal)> confirmationPoints { get; set; } // confirmationPoints property which stores a list of tuples with getter and setter
        public double price {  get; set; } // price property with getter and setter
        /// <summary>
        /// Constructor to initialize all the parameters of the beauty class
        /// </summary>
        /// <param name="beauty"></param>
        /// <param name="price"></param>
        /// <param name="confirmationPoints"></param>
        public Beauty(int beauty, double price, List<(int, decimal)> confirmationPoints)
        {
            this.beauty = beauty;
            this.price = price;
            this.confirmationPoints = confirmationPoints;
        }
        public Beauty() { } //default constructor of the beauty class
        /// <summary>
        /// Override the ToString() method to provide a custom string representation of the object
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"Beauty Count: {beauty}, Lowest Price: {price}";
        }

    }
}
