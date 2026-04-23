using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Project_Input
{
    public class ASmartCandlestick : aCandlestick // Smart candlestick class that inherits the properties of aCandlestick class
    {
        private int _Index = -1;
        public int Index
        {
            get { return _Index; }
            set { _Index = value; }
        }
        // Properties of smart candlestick
        public decimal Range { get; set; } // The size of the whole candlestick
        public decimal BodyRange { get; set; } // the sixe of the body
        public decimal TopPrice { get; set; } // The highest of the open and close
        public decimal BottomPrice { get; set; } // The lowest of the open and close
        public decimal UpperTail {  get; set; } // The size of the upper tail
        public decimal LowerTail { get; set; } // The size of the lower tail
        // Constructor to initialize the properties
        public ASmartCandlestick(DateTime date, decimal open, decimal high, decimal low, decimal close, ulong volume)
             : base(date, open, high, low, close, volume) // Calling the base class constructor
        {
            Range = high - low; // Calculating the size of the candlestick
            BodyRange = Math.Abs(open - close); // Calculating the body
            TopPrice = Math.Max(open, close); // Calculating the max price between the open and close
            BottomPrice = Math.Min(open, close); // Calculating the min price between open and close
            UpperTail = high - TopPrice;  // Calculate the upper tail
            LowerTail = BottomPrice - low; // Calculate the lower tail
        }

       
        public bool IsBullish { get { return Close > Open; } } // Determine if the candlestick is bullish
        public bool IsBearish { get { return Close < Open; } } // Determine if the candlestick is bearish
        public bool IsNeutral { get { return Close == Open; } } // Determine if the candlestick is Neutral
        /// <summary>
        /// Determine if candlestick is marobozu
        /// </summary>
        /// <returns>boolean value</returns>
        public bool IsMarubozu() 
        {
            // Checking that it has no upper or lower tail. It can be bearish or bullish
            return (IsBullish && UpperTail == 0 && LowerTail == 0) || (IsBearish && UpperTail == 0 && LowerTail == 0);
        }
        /// <summary>
        /// Determine if the candlestick is hammer
        /// </summary>
        /// <returns>boolean value</returns>
        public bool IsHammer()
        {
            // Checking that the lower tail is at least twice as bigger as the body
            return (BodyRange < Range && LowerTail > BodyRange * 2 && UpperTail < BodyRange);
        }

        public bool IsDoji { get { return Close == Open;  } } // Determine if the candlestick is doji
        /// <summary>
        /// Determine if the candlestick is dragon fly doji
        /// </summary>
        /// <returns>boolean value</returns>
        public bool IsDragonflyDoji()
        {
            // it has no upper tail and has a long lower tail
            return IsDoji && UpperTail == 0 && LowerTail > BodyRange * 2;
        }
        /// <summary>
        /// Determine if the candlestick is gravestone doji
        /// </summary>
        /// <returns>boolean value</returns>
        public bool IsGravestoneDoji()
        {
            // it has no lower tail and has a long upper tail
            return IsDoji && LowerTail == 0 && UpperTail > BodyRange * 2;
        }

       public bool isPeak { get; set; }
       public bool isValley { get; set; }
       public List<decimal> beauty {  get; set; } = new List<decimal>();

    }
}
