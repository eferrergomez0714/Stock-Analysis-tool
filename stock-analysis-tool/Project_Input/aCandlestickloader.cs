using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Project_Input
{
    public static class aCandlestickLoader 
    {
        /// <summary>
        /// Method for loading the candlesticks data from a csv file.
        /// </summary>
        /// <param name="filePath">The path to the csv file we are uploading</param>
        /// <returns>A list with all the candlesticks in the file</returns>
        /// <exception cref="FileNotFoundException">Throws an exception if the file wasn't found</exception>
        /// <exception cref="FormatException">Throws an exception if the file doesn't have the right format.</exception>
        public static List<ASmartCandlestick> LoadFromCsv(string filePath)
        {
            var candlesticks = new List<ASmartCandlestick>(); //Declaring a list of candlesticks to store the candlesticks in the file.

                if (!File.Exists(filePath)) //Checking if file exist for the specified path.
                {
                    throw new FileNotFoundException("The CSV file could not be found."); // Throwing an exemption with a message if the file is not found.
                }

                using (var reader = new StreamReader(filePath)) //Opening the file using a stream reader.
                {
                    string line; //Declaring a variable to store each line from the file.
                    bool isFirstLine = true; //Boolean variable to skip the header of the file.
                    char[] delimeters = { ',', '\\', '"' }; //Delimiters for splitting the line.

                    while ((line = reader.ReadLine()) != null) //Iterate through each line from the file untill file is empty.
                    {
                        // Skip header line
                        if (isFirstLine)
                        {
                            isFirstLine = false;
                            continue;
                        }

                        var values = line.Split(delimeters, StringSplitOptions.RemoveEmptyEntries); //Splitting the line based on delimeters and removing empty entries.

                        if (values.Length != 6)  // Checking if the CSV has 6 columns: Time, Open, High, Low, Close, Volume.
                            throw new FormatException("Unexpected number of columns in CSV file."); //Throwing an exemption with a message if the file doesn't have the right format.

                        DateTime time = DateTime.ParseExact(values[0], "yyyy-MM-dd", CultureInfo.InvariantCulture); // Parse date in yyyy-MM-dd format.
                        decimal open = Math.Round(100*decimal.Parse(values[1], CultureInfo.InvariantCulture)) / 100; // Parse open price and round it to 2 decimal places.
                        decimal high = Math.Round(100*decimal.Parse(values[2], CultureInfo.InvariantCulture)) / 100; // Parse High price and round it to 2 decimal places
                        decimal low = Math.Round(100*decimal.Parse(values[3], CultureInfo.InvariantCulture)) / 100; // Parse Low price and round it to 2 decimal places
                        decimal close = Math.Round(100*decimal.Parse(values[4], CultureInfo.InvariantCulture)) / 100; // Parse Close price and round it to 2 decimal places
                        ulong volume = ulong.Parse(values[5], CultureInfo.InvariantCulture); // Parse Volume as an unsigned long integer.

                        var candlestick = new ASmartCandlestick(time, open, high, low, close, volume); //Create a candlestick object with the parsed values.
                        candlesticks.Add(candlestick); //Add candlestick to the list.
                    }
                }
                    return candlesticks;
        }
    }
}

