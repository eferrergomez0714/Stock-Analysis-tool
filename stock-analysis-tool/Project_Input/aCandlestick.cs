using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Input
{   
    public class aCandlestick
    {
        // Properties of the Candlestick
        public DateTime Date { get; set; } // Date of the candlestick
        public decimal Open { get; set; }  // Opening price
        public decimal High { get; set; }  // Highest price during the period
        public decimal Low { get; set; }   // Lowest price during the period
        public decimal Close { get; set; } // Closing price
        public ulong Volume { get; set; }   // Volume of trades during the period

        // Constructor to initialize all properties
        public aCandlestick(DateTime date, decimal open, decimal high, decimal low, decimal close, ulong volume)
        {
            Date = date; //Setting the Date property.
            Open = open; //Setting the Open property.
            High = high; //Setting the High property.
            Low = low; //Setting the Low property.
            Close = close; //Setting the Close property.
            Volume = volume; //Setting the Volume property.
        }
    }
}
