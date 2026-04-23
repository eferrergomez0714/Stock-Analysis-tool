using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Input
{
    public class Levels // Defining a level class
    {
        /// <summary>
        /// Constructor of levels class to initialize the properties level and price at leevel
        /// </summary>
        /// <param name="level"></param>
        /// <param name="priceAtLevel"></param>
        public Levels(double level, decimal priceAtLevel)
        {
            this.level = level; //Assigning input parameter level to property level
            this.priceAtLevel = priceAtLevel; // Assigning input parameter priceAtLevel to property priceAtLevel
        }
        // 
        public double level {  get; set; } // Property level with getter and setter
        public decimal priceAtLevel { get; set; } // Property priceAtLevel with getter and setter
    }
}
