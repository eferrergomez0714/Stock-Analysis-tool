using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Net.Mime.MediaTypeNames;

namespace Project_Input
{
    public partial class Form_Input : Form
    {
        //Declaring a list for storing cladlesticks
        List<ASmartCandlestick> candlesticks;
        List<ASmartCandlestick> filteredCandleSticks;
        List<Levels> levelPrice;

        //List<aCandlestick> selectedCandlesticks;
        private bool isSelecting = false; // To track if the user is currently selecting
        private Point startPoint; // Starting point of the selection
        private Rectangle selectionRectangle; // The current selection rectangle
        private RectangleF waveRectangle;
        private bool drawWaveRectangle = false;


        bool visiblePeaks = false; // Declaring a boolean variable to make the peaks visible
        bool visibleValleys = false; // Declarin a boolean variable to make the valleys visible
        public Form_Input()
        {
             InitializeComponent();
             // Event handler assignment for rubber band function
             chart_candlesticks.MouseDown += chart_candlesticks_MouseDown;
             chart_candlesticks.MouseMove += chart_candlesticks_MouseMove;
             chart_candlesticks.MouseUp += chart_candlesticks_MouseUp;
             chart_candlesticks.Paint += chart_candlesticks_Paint;
             chart_candlesticks.Paint += chart_candlesticks_Paint_Wave;
        }
        /// <summary>
        /// constructor for the Form_input class
        /// </summary>
        /// <param name="fileName"></param>
        public Form_Input(string fileName)
        {
            InitializeComponent(); // Initialize the form components
            this.Text = Path.GetFileName(fileName); // set the form's title to the name of the csv file
            candlesticks = aCandlestickLoader.LoadFromCsv(fileName); // Load the data from the specified path
            this.updateChart(); // update the chart to display the data loaded
            //Event handlers assignment for rubber band function                  
            chart_candlesticks.MouseDown += chart_candlesticks_MouseDown;
            chart_candlesticks.MouseMove += chart_candlesticks_MouseMove;
            chart_candlesticks.MouseUp += chart_candlesticks_MouseUp;
            chart_candlesticks.Paint += chart_candlesticks_Paint;

        }

        /// <summary>
        /// Event handler for clicking the start(Load Stock) button
        /// </summary>
        private void button_start_Click(object sender, EventArgs e)
        {
            openFileDialog_load.ShowDialog(); //Show the file dialog to select a csv file.
        }

        /// <summary>
        /// Event triggered when a file is selected and confirmed
        /// </summary>
        private void openFileDialog_load_FileOk(object sender, CancelEventArgs e)
        {
            var path = openFileDialog_load.FileName; //Storing the selected file path in a variable.
            this.Text = Path.GetFileName(path); //Changing the text property of the form to the file name.
            candlesticks = aCandlestickLoader.LoadFromCsv(path); //Loading the data in the csv file to the list of candlesticks.

            if (candlesticks.Count > 1 && candlesticks[0].Date > candlesticks[1].Date) //Checking if data is ordered by newest first.
            {
                candlesticks.Reverse(); //Reversing the list to display oldest data first.
            }

            if (candlesticks.Count == 0) { return;  } //Exit the method if the candlesticks list is empty.

            updateChart(); //Calling the updateChart method

            // iterating through each file skipping the first file
            for (int i = 1; i < openFileDialog_load.FileNames.Length; i++)
            {
                var form_input = new Form_Input(openFileDialog_load.FileNames[i]); // creating a new instance of the Form_input passing the current file as a parameter
                form_input.Show(); //Show the new form
            }
        }
        /// <summary>
        /// method to find peaks and valleys in the candlestick list
        /// </summary>
        /// <param name="candlesticks"></param>
        public void findPeaksAndValleys(List<ASmartCandlestick> candlesticks)
        {   
            // iterate through the candlestick list skipping the first one
            for (int i = 1; i < candlesticks.Count - 1; i++) { 
                var previous = candlesticks[i - 1]; // storing the previous candlestick
                var curr = candlesticks[i]; // storing the current candlestick
                var next = candlesticks[i + 1]; // storing the next candlestick

                // checking if candlestick is a peak
                if (curr.High > previous.High && curr.High > next.High)
                {
                    candlesticks[i].isPeak = true;
                    AddAnnotation("Peak", curr.High, i, System.Drawing.Color.Green); // add annotation at the peak
                    AddLineAnnotation("Peak", curr.High, i, System.Drawing.Color.Green); // add line annotation at peak
                }

                // checking if candlestick is a valley
                if (curr.Low < previous.Low && curr.Low < next.Low)
                {
                    candlesticks[i].isValley = true;
                    AddAnnotation("Valley", curr.Low, i, System.Drawing.Color.Red); // add annotation at the valley
                    AddLineAnnotation("Valley", curr.Low, i, System.Drawing.Color.Red); // add line annotation at the valley
                }
            }
        }
        /// <summary>
        /// Method to add text annotation to the candlestick that are peaks or valleys
        /// </summary>
        /// <param name="text"></param>
        /// <param name="price"></param>
        /// <param name="date"></param>
        /// <param name="color"></param>
        public void AddAnnotation(string text, decimal price, int date, System.Drawing.Color color)
        {
            var annotation = new TextAnnotation // new text annotation object
            {
                Text = text, // text to display
                ForeColor = color, // color of the test
                Alignment = ContentAlignment.MiddleCenter, // Centers the text alignment
                AxisX = chart_candlesticks.ChartAreas["ChartArea_OHLC"].AxisX, // binding the annotations to the x-axis
                AxisY = chart_candlesticks.ChartAreas["ChartArea_OHLC"].AxisY, // binding the annotations to the y-axis
                AnchorX = date + 1, // x position for the annotation
                Y = (double)price, // y coordinate for the price
                Visible = false, // hiding the annotation at first
                Name = text + date, // setting a unique name for the annotation
            };
            
            if (text == "Peak") // adjust the y position upward if the annotation is a peak
            {
                annotation.Y += annotation.Y * (0.05); // add 5% to the y position
            }

            chart_candlesticks.Annotations.Add(annotation); // add the annotation to the chart
        } 
        /// <summary>
        /// Method to add a horizontal line to the chart
        /// </summary>
        /// <param name="text"></param>
        /// <param name="price"></param>
        /// <param name="date"></param>
        /// <param name="color"></param>
        public void AddLineAnnotation(string text, decimal price, int date, System.Drawing.Color color)
        {
            var line = new HorizontalLineAnnotation // create new horizontallineannotation object

            {
                AxisX = chart_candlesticks.ChartAreas["ChartArea_OHLC"].AxisX, // binds the x position to the chart x-axis
                AxisY = chart_candlesticks.ChartAreas["ChartArea_OHLC"].AxisY, // binds the y position to the chart y-axis
                ClipToChartArea = "ChartArea_OHLC", // drawing the line in the correct chart area
                LineColor = color, // setting the color
                LineWidth = 1, // setting the width
                IsInfinitive = true, // makes the line infinite
                Y = (double)price, // sets y coordinate for the price
                Visible = false, // hiding the line at first
                Name = text + "line annotation" + date, // Unique name for each peak line

            };

            chart_candlesticks.Annotations.Add(line); // add line to the chart
        }

    /// <summary>
    /// Method to update the chart without loading a different file.
    /// </summary>
    private void updateChart()
        {
            DateTime startDate = dateTimePicker_startDate.Value; //Storing the start date in a variable.
            DateTime endDate = dateTimePicker_endDate.Value; //Storing the end date in a variable.

            if (startDate > endDate) { return; } //Exiting the method if the dates are not in the correct order.

            filteredCandleSticks = filterCandlesticks(candlesticks, startDate, endDate); //Calling the filterCandlesticks method and storing the result in a varianble.
            BindingList<ASmartCandlestick> boundList = new BindingList<ASmartCandlestick>(filteredCandleSticks); //Binding the filtered list to be able to display it.

            chart_candlesticks.DataSource = boundList; //Binding the bound list to the candlestick chart.
            chart_candlesticks.DataBind(); //Updating the chart to reflect the data.
            normalizeChart(boundList); // calling the normalizeChart method passing boundlist as a parameter.
            chart_candlesticks.Annotations.Clear(); // clearing the chart to eliminate the annotations when updating the data
            visiblePeaks = false; // setting visible peaks to false 
            visibleValleys = false; // setting visible valleys to false
            findPeaksAndValleys(filteredCandleSticks); // calling the findpeaks and valleys function
        }
        /// <summary>
        /// Method to filter candlesticks by date range.
        /// </summary>
        /// <param name="candlesticks">List of candlesticks</param>
        /// <param name="startingDate">start date</param>
        /// <param name="endingDate">end date</param>
        /// <returns>A list of filtered candlesticks</returns>
        public static List<ASmartCandlestick> filterCandlesticks(List<ASmartCandlestick> candlesticks, DateTime startingDate, DateTime endingDate)
        {
            var filteredCandlesticks = new List<ASmartCandlestick>(); //Declaring a list to store the filtered data.

            var count = 0;
                
            foreach (var candlestick in candlesticks) //Iterate through the entire list passed as an argument.
            {
                if (candlestick.Date >= startingDate && candlestick.Date <= endingDate) //Check if the current candlestick is within range.
                {
                    candlestick.Index = count++;
                    filteredCandlesticks.Add(candlestick); //Add the candlestick to the filtered list.
                }
            }
            return filteredCandlesticks;
        }
        /// <summary>
        /// Normalizing the Y-axis of the chart to fit the data better.
        /// </summary>
        /// <param name="boundCandlesticks">A list of the candlesticks binded to the chart</param>
        private void normalizeChart(BindingList<ASmartCandlestick> boundCandlesticks)
        {
            if (boundCandlesticks.Count == 0) {  return; } //Exit the method if list is empty.
            double min = Math.Floor(0.98 * (double)boundCandlesticks.Min(cs => cs.Low)); //Finding the minimum low value and applying a 2% margin.
            double max = Math.Ceiling(1.02 * (double)boundCandlesticks.Max(cs => cs.High)); //Finding the maximum high value and applying a 2% margin.
            chart_candlesticks.ChartAreas["ChartArea_OHLC"].AxisY.Minimum = min; //Setting Y-axis minimum value for the OHLC chart area.
            chart_candlesticks.ChartAreas["ChartArea_OHLC"].AxisY.Maximum = max; //Setting Y-axis maximum value for the OHLC chart area.
            chart_candlesticks.ChartAreas["ChartArea_OHLC"].AxisY.Interval = Math.Ceiling((max - min) / 5); //Setting the Y-axis to interval based on the range.
            

        }
        /// <summary>
        /// Event triggered when form loads.
        /// </summary>
        private void Form_Input_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Event handler for clicking the Update button.
        /// </summary>
        private void button_update_Click(object sender, EventArgs e)
        {
            updateChart(); // Calling the updateChart method to update the chart.
        }
        /// <summary>
        /// event handler for clicking the show peak button
        /// </summary>
        private void button_Peak_Click(object sender, EventArgs e)
        {
            visiblePeaks = !visiblePeaks; //change visibility from false to true
            // Iterating through the annotations ion the chart
            foreach (var annotation in chart_candlesticks.Annotations)
            {
                //check if annotation is peak
                if (annotation.Name.StartsWith("Peak"))
                {
                    annotation.Visible = visiblePeaks; // set the visibility of the annotation
                }
            }
        }
        /// <summary>
        /// event handler for clicking the show valley button
        /// </summary>
        private void button_Valley_Click(object sender, EventArgs e)
        {
            visibleValleys = !visibleValleys; //change visibility from false to true
            // Iterating through the annotations ion the chart
            foreach (var annotation in chart_candlesticks.Annotations)
            {
                //check if annotation is valley
                if (annotation.Name.StartsWith("Valley"))
                {
                    annotation.Visible = visibleValleys; // set the visibility of the annotation

                }
            }
        }
        /// <summary>
        /// Event handler for the MouseDown event of the chart_candlesticks control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chart_candlesticks_MouseDown(object sender, MouseEventArgs e)
        {
            // Check if the left mouse button was clicked
            if (e.Button == MouseButtons.Left)
            {
                isSelecting = true; // Set a flag indicating that a selection operation is in progress
                startPoint = e.Location; // Record the starting point of the mouse click (initial position for the selection)
                selectionRectangle = new Rectangle(startPoint.X, startPoint.Y, 0, 0); // Initialize the rectangle
                Cursor = Cursors.Cross; // Change cursor for visual feedback
            }
        }

        /// <summary>
        /// Event handler for the MouseMove event of the chart_candlesticks control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chart_candlesticks_MouseMove(object sender, MouseEventArgs e)
        {
            // Check if a selection operation is in progress
            if (isSelecting)
            {
                // Calculate the top-left corner of the rectangle by choosing the minimum x and y coordinates
                int x = Math.Min(startPoint.X, e.X);
                int y = Math.Min(startPoint.Y, e.Y);
                // Calculate the width and height of the rectangle as absolute differences
                int width = Math.Abs(e.X - startPoint.X);
                int height = Math.Abs(e.Y - startPoint.Y);
                // Update the selection rectangle with the calculated dimensions
                selectionRectangle = new Rectangle(x, y, width, height);

                chart_candlesticks.Invalidate(); // Trigger redraw
            }
        }

        /// <summary>
        ///Event handler for the MouseUp event of the chart_candlesticks control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chart_candlesticks_MouseUp(object sender, MouseEventArgs e)
        {
            // Check if a selection operation is in progress
            if (isSelecting)
            {
                isSelecting = false; // Check if a selection operation is in progress
                Cursor = Cursors.Default; // Reset cursor
                // Check if the selection rectangle has valid dimensions (non-zero width and height)
                if (selectionRectangle.Width > 0 && selectionRectangle.Height > 0)
                {
                    // Retrieve the X-axis of the chart from the specified chart area
                    var xAxis = chart_candlesticks.ChartAreas["ChartArea_OHLC"].AxisX;
                    // Convert the left and right pixel positions of the rectangle into chart axis values
                    double xStart = xAxis.PixelPositionToValue(selectionRectangle.Left);
                    double xEnd = xAxis.PixelPositionToValue(selectionRectangle.Right);

                    // Ensure the range is properly ordered
                    if (xStart > xEnd) { (xStart, xEnd) = (xEnd, xStart); }
                    // Get the candlesticks that fall within the selected X-axis range
                    var selectedCandlesticks = GetCandlesticksInSelection(filteredCandleSticks, xStart, xEnd);
                    // Validate the selected candlesticks to determine if they form a valid wave
                    IsValidWave(selectedCandlesticks);
                }

                selectionRectangle = Rectangle.Empty; // Reset rectangle
                chart_candlesticks.Invalidate(); // Redraw chart
            }
        }
        /// <summary>
        /// Method to validate if the selected candlesticks form a valid wave
        /// </summary>
        /// <param name="selectedCandlesticks">list of candlesticks selected with the mouse</param>
        /// <returns>true if the wave is valid</returns>
        public bool IsValidWave(List<ASmartCandlestick> selectedCandlesticks)
        {
            // Check for null or insufficient data or if the first candlestick is neither a peak nor a valley
            if (selectedCandlesticks == null || selectedCandlesticks.Count == 0 || (!selectedCandlesticks[0].isPeak && !selectedCandlesticks[0].isValley))
            {
                MessageBox.Show("Invalid input or starting candlestick is not a peak", "Wave Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false; // Exit if validation fails
            }
            var downWard = false; // Variable to track the direction of the wave (downward if true, upward if false)
            // If the wave starts with a peak
            if (selectedCandlesticks[0].isPeak)
            {
                // Validate the High values (all must be <= the first peak's High)
                for (int i = 1; i < selectedCandlesticks.Count; i++)
                {
                    if (selectedCandlesticks[i].High > selectedCandlesticks[0].High)
                    {
                        MessageBox.Show("Not a valid wave: a High value exceeds the peak", "Wave Validation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false; // Exit if validation fails
                    }
                }

                // Find the last valley
                int lastValleyIndex = -1;
                for (int j = selectedCandlesticks.Count - 1; j > 0; j--)
                {
                    if (selectedCandlesticks[j].isValley)
                    {
                        lastValleyIndex = j; // Save the index of the last valley
                        break;
                    }
                }

                // If no valley is found, it's not a valid wave
                if (lastValleyIndex == -1)
                {
                    MessageBox.Show("Not a valid wave: no valley found", "Wave Validation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false; // Exit if validation fails
                }

                // Validate the Low values (all must be >= the last valley's Low)
                for (int k = lastValleyIndex; k > 0; k--)
                {
                    if (selectedCandlesticks[k].Low < selectedCandlesticks[lastValleyIndex].Low)
                    {
                        MessageBox.Show("Not a valid wave: a Low value is below the valley", "Wave Validation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false; // Exit if validation fails
                    }
                }
                downWard = true; // Mark the wave as downward and call the draw_wavwe_rect methods for drawing the wave
                draw_wave_rect(selectedCandlesticks, lastValleyIndex, downWard);
            }
            else // If the wave starts with a valley
            {
                // Validate the Low values (all must be >= the first valley's High)
                for (int i = 1; i < selectedCandlesticks.Count; i++)
                {
                    if (selectedCandlesticks[i].Low < selectedCandlesticks[0].Low)
                    {
                        MessageBox.Show("Not a valid wave: a Low value is lower then the valley", "Wave Validation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false; // Exit if validation fails
                    }
                }

                // Find the last Peak
                int lastPeakIndex = -1;
                for (int j = selectedCandlesticks.Count - 1; j > 0; j--)
                {
                    if (selectedCandlesticks[j].isPeak)
                    {
                        lastPeakIndex = j; // Save the index of the last peak
                        break;
                    }
                }

                // If no peak is found, it's not a valid wave
                if (lastPeakIndex == -1)
                {
                    MessageBox.Show("Not a valid wave: no peak found", "Wave Validation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false; // Exit if validation fails
                }

                // Validate the High values (all must be <= the last peak's High)
                for (int k = lastPeakIndex; k > 0; k--)
                {
                    if (selectedCandlesticks[k].High > selectedCandlesticks[lastPeakIndex].High)
                    {
                        MessageBox.Show("Not a valid wave: a high value is above the peak", "Wave Validation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false; // Exit if validation fails
                    }
                }
                // call the draw_wavwe_rect methods for drawing the wave
                draw_wave_rect(selectedCandlesticks, lastPeakIndex, downWard);
            }
            
            // If all checks pass, the wave is valid
            return true;
        }
        /// <summary>
        /// Method to draw a rectangular region representing the wave and calculate Fibonacci levels and wave beauty
        /// </summary>
        /// <param name="selectedCandlesticks">list of candlesticks selected with the mouse</param>
        /// <param name="waveEnd">index of last candlestick of the wave</param>
        /// <param name="downWard">bool parameter to indicate if it is a downward wave</param>
        /// <returns>the wave rectangle</returns>
        public RectangleF draw_wave_rect(List<ASmartCandlestick> selectedCandlesticks, int waveEnd, bool downWard)
        {
            
            // Get chart and axis references
            var chartArea = chart_candlesticks.ChartAreas["ChartArea_OHLC"];
            var xAxis = chartArea.AxisX;
            var yAxis = chartArea.AxisY;

            // Calculate X range (first and last candlestick)
            double xStart = xAxis.ValueToPixelPosition(selectedCandlesticks[0].Index + 1);
            double xEnd = xAxis.ValueToPixelPosition(selectedCandlesticks[waveEnd].Index + 1);


            // Calculate Y range (highest and lowest values)
            decimal highestPrice = selectedCandlesticks.Max(c => c.High);
            decimal lowestPrice = selectedCandlesticks.Min(c => c.Low);

            // Convert the highest and lowest prices to pixel positions for drawing
            double yStart = yAxis.ValueToPixelPosition((double)highestPrice);
            double yEnd = yAxis.ValueToPixelPosition((double)lowestPrice);

            // Store rectangle dimensions for drawing
            float rectX = (float)Math.Min(xStart, xEnd);
            float rectY = (float)Math.Min(yStart, yEnd);
            float rectWidth = (float)Math.Abs(xEnd - xStart);
            float rectHeight = (float)Math.Abs(yEnd - yStart);

            // Create a rectangle to represent the wave
            waveRectangle = new RectangleF(rectX, rectY, rectWidth, rectHeight);
            drawWaveRectangle = true; // Flag to indicate the rectangle should be drawn
            Beauty wave_beauty = new Beauty(); // Object to store wave beauty information
            // Attach an event handler to the chart's Paint event for drawing Fibonacci lines and beauty points
            chart_candlesticks.Paint += (s, e) =>
            {
                // Calculate Fibonacci levels and draw them
                levelPrice = DrawFibonacciLines(e, waveRectangle, (double)highestPrice, (double)lowestPrice, downWard);
                // Find the beauty points within the wave and draw them
                wave_beauty = find_beauty(selectedCandlesticks, 0, waveEnd, levelPrice, (double)lowestPrice); // Pass levelPrice here
                foreach (var (index, price) in wave_beauty.confirmationPoints)
                {
                    // Convert the index and price to pixel positions for drawing
                    double xPixel = chartArea.AxisX.ValueToPixelPosition((double)index + 1);
                    double yPixel = chartArea.AxisY.ValueToPixelPosition((double)price);
                    // Draw a small circle at each confirmation point
                    using (Brush brush = new SolidBrush(Color.Navy))
                    {
                        e.Graphics.FillEllipse(brush, (float)xPixel - 3, (float)yPixel - 3, 6, 6);
                    }
                }

            };
            // Calculate beauty for multiple levels by adjusting the lowest price decrementally
            var lowestPriceRate = lowestPrice / 20; // Rate of adjustment for lowest price
            var beautyList = new List<Beauty>(); // List to store beauty results for each level
            var tempLowestPrice = lowestPrice; // Temporary variable to store the adjusted lowest price
            // Generate beauty objects for 5 levels of Fibonacci lines
            for (int i = 0; i < 5; i++)
            {
                // Calculate Fibonacci levels and find beauty at the current level
                levelPrice = CalculateFibonacciLines(waveRectangle, (double)highestPrice, (double)tempLowestPrice, downWard);
                wave_beauty = find_beauty(selectedCandlesticks, 0, waveEnd, levelPrice, (double)tempLowestPrice); // Pass levelPrice here
                beautyList.Add(wave_beauty); // Add the result to the list
                tempLowestPrice -= lowestPriceRate; // Decrease the lowest price for the next level
            }

            // Bind the beauty results to the beauty chart for visualization
            BindingList<Beauty> boundList_beauty_price = new BindingList<Beauty>(beautyList); //Binding the filtered list to be able to display it.
            chart_Beauty.DataSource = boundList_beauty_price; //Binding the bound list to the candlestick chart.
            chart_Beauty.DataBind(); //Updating the chart to reflect the data.
            chart_Beauty.ResumeLayout(true);    // Resume Layout for visualization

            // Redraw the chart
            chart_candlesticks.Invalidate();
            return waveRectangle; // Return the rectangle representing the wave
        }
        /// <summary>
        /// Method to calculate Fibonacci levels based on a given rectangle, price range, and direction (upward or downward)
        /// </summary>
        /// <param name="rectangle">rectangle containing the valid wave</param>
        /// <param name="highestPrice">highest price of the wave</param>
        /// <param name="lowestPrice">lowest price of the wave</param>
        /// <param name="downWard">bool value to know if wave is downward</param>
        /// <returns>List with all the fibonacci levels</returns>
        private List<Levels> CalculateFibonacciLines(RectangleF rectangle ,double highestPrice, double lowestPrice, bool downWard)
        {
            // Define Fibonacci levels
            double[] fibonacciLevels = { 0.0, 0.236, 0.382, 0.5, 0.628, 0.764, 1.0 };
            var levelPrice = new List<Levels>(); // Create a list to store the levels and their corresponding prices

            float yPosition; // Variable to store the calculated Y-coordinate for each level
            // Loop through each Fibonacci level
            foreach (var level in fibonacciLevels)
            {
                // Calculate the Y-coordinate for the level based on the direction (downward or upward)
                yPosition = downWard
                ? rectangle.Y + (rectangle.Height * (1 - (float)level)) // For downward waves, position decreases
                : rectangle.Y + (rectangle.Height * ((float)level)); //For upward waves, positon increases
                // Calculate the price at the current Fibonacci level
                var priceAtLevel = downWard
                    ? (decimal)lowestPrice + (decimal)level * ((decimal)highestPrice - (decimal)lowestPrice)
                    : (decimal)highestPrice - (decimal)level * ((decimal)highestPrice - (decimal)lowestPrice);

                levelPrice.Add(new Levels(level, priceAtLevel)); // Add the level and its price to the list
            }
            // Return the list of levels and their corresponding prices
            return levelPrice;
        }
        /// <summary>
        /// Method to draw Fibonacci lines on the chart and return the levels and their corresponding prices
        /// </summary>
        /// <param name="e"></param>
        /// <param name="rectangle">rectangle containing the valid wave</param>
        /// <param name="highestPrice">highest price of the wave</param>
        /// <param name="lowestPrice">lowest price of the wave</param>
        /// <param name="downWard">bool value to know if wave is downward</param>
        /// <returns>List with all the fibonacci levels</returns>
        private List<Levels> DrawFibonacciLines(PaintEventArgs e,RectangleF rectangle, double highestPrice, double lowestPrice, bool downWard)
        {
            // Define Fibonacci levels
            double[] fibonacciLevels = { 0.0, 0.236, 0.382, 0.5, 0.628, 0.764, 1.0 };
            var levelPrice = new List<Levels>();


            // Drawing resources
            using (var penFibonacci = new Pen(Color.Red, 1))
            using (var font = new Font("Arial", 8))
            using (var brush = new SolidBrush(Color.Black))
            {
                // Obtain the graphics context for drawing on the chart
                var graphics = chart_candlesticks.CreateGraphics();

                float yPosition; // Variable to store the Y-coordinate for each level
                // Loop through each Fibonacci level
                foreach (var level in fibonacciLevels)
                {
                    // Calculate the Y-coordinate of the current level based on the wave direction
                    yPosition = downWard
                    ? rectangle.Y + (rectangle.Height * (1 - (float)level)) // For downward waves, position decreases
                    : rectangle.Y + (rectangle.Height * ((float)level)); // For upward waves, position increases
                    // Calculate the price associated with the current Fibonacci level
                    var priceAtLevel = downWard
                        ? (decimal)lowestPrice + (decimal)level * ((decimal)highestPrice - (decimal)lowestPrice) //for downward waves
                        : (decimal)highestPrice - (decimal)level * ((decimal)highestPrice - (decimal)lowestPrice); // for upward waves

                    levelPrice.Add(new Levels(level, priceAtLevel)); // Add the level and its price to the list

                    // Draw the horizontal Fibonacci line
                    e.Graphics.DrawLine(
                        penFibonacci,
                        rectangle.X,
                        yPosition,
                        rectangle.X + rectangle.Width,
                        yPosition
                    );

                    // Label the line with the Fibonacci percentage
                    string label = $"{level * 100:0}%";
                    e.Graphics.DrawString(label, font, brush, rectangle.X - 30, yPosition - 8);
                    
                }
                
            }
            // Return the list of levels and their prices
            return levelPrice;
        }
        /// <summary>
        /// Method to find beauty points in candlestick data that are close to Fibonacci levels
        /// </summary>
        /// <param name="selectedCandlesticks">candlesticks selected in the wave</param>
        /// <param name="start">index of the first candlestick</param>
        /// <param name="end">index of the last candlestick</param>
        /// <param name="levelPrice">list of prices at each level</param>
        /// <param name="lowestPrice">lowest price of the wave</param>
        /// <returns>a beauty object</returns>
        public Beauty find_beauty(List<ASmartCandlestick> selectedCandlesticks, int start, int end, List<Levels> levelPrice, double lowestPrice)
        {
            List<(int, decimal)> points = new List<(int, decimal)> (); // List to store points where the candlestick values are near Fibonacci levels

            int beauty_count = 0; // Counter to keep track of the number of beauty points found
            // Loop through the selected candlesticks in the specified range (from start to end)
            for (int i = start; i <= end; i++)
            {
                // Loop through each Fibonacci level
                foreach (var level in levelPrice)
                {
                    // Check if the candlestick's low price is within 0.5% of the Fibonacci level
                    if ((decimal)0.995*level.priceAtLevel <= selectedCandlesticks[i].Low && selectedCandlesticks[i].Low <= (decimal)1.005 * level.priceAtLevel)
                    {
                        // Increment beauty count and add the point to the list (index and high price)
                        beauty_count++;
                        points.Add((selectedCandlesticks[i].Index, selectedCandlesticks[i].Low));
                    }
                    // Check if the candlestick's low price is within 0.5% of the Fibonacci level
                    if ((decimal)0.995 * level.priceAtLevel <= selectedCandlesticks[i].High && selectedCandlesticks[i].High <= (decimal)1.005 * level.priceAtLevel)
                    {
                        // Increment beauty count and add the point to the list (index and high price)
                        beauty_count++;
                        points.Add((selectedCandlesticks[i].Index, selectedCandlesticks[i].High));
                    }
                    // Check if the candlestick's low price is within 0.5% of the Fibonacci level
                    if ((decimal)0.995 * level.priceAtLevel <= selectedCandlesticks[i].Open && selectedCandlesticks[i].Open <= (decimal)1.005 * level.priceAtLevel)
                    {
                        // Increment beauty count and add the point to the list (index and high price)
                        beauty_count++;
                        points.Add((selectedCandlesticks[i].Index, selectedCandlesticks[i].Open));
                    }
                    // Check if the candlestick's low price is within 0.5% of the Fibonacci level
                    if ((decimal)0.995 * level.priceAtLevel <= selectedCandlesticks[i].Close && selectedCandlesticks[i].Close <= (decimal)1.005 * level.priceAtLevel)
                    {
                        // Increment beauty count and add the point to the list (index and high price)
                        beauty_count++;
                        points.Add((selectedCandlesticks[i].Index, selectedCandlesticks[i].Close));
                    }
                }
            }
            // Return a new Beauty object containing the total count of beauty points, the lowest price, and the list of points
            return new Beauty(beauty_count, lowestPrice, points);
        }
        /// <summary>
        /// Event handler for painting a wave rectangle on the candlestick chart
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chart_candlesticks_Paint_Wave(object sender, PaintEventArgs e)
        {
            // Check if a wave rectangle needs to be drawn
            if (drawWaveRectangle)
            {
                // Create a blue pen with a width of 2 for drawing the rectangle
                using (var pen = new Pen(Color.Blue, 2))
                {
                    // Draw the rectangle representing the wave on the chart using the pen
                    e.Graphics.DrawRectangle(pen, waveRectangle.X, waveRectangle.Y, waveRectangle.Width, waveRectangle.Height);
                }
            }
        }

        /// <summary>
        /// Event handler for painting on the candlestick chart
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chart_candlesticks_Paint(object sender, PaintEventArgs e)
        {
            // Check if the user is currently selecting a region and if the selection rectangle is not empty
            if (isSelecting && selectionRectangle != Rectangle.Empty)
            {
                // Create a semi-transparent light blue brush for filling the selection area
                using (Brush selectionBrush = new SolidBrush(Color.FromArgb(128, Color.LightBlue)))
                {
                    // Fill the selection area with the light blue color using the brush
                    e.Graphics.FillRectangle(selectionBrush, selectionRectangle); // Fill the selection area
                }
                // Create a blue pen for drawing the border of the selection rectangle
                using (Pen borderPen = new Pen(Color.Blue, 2))
                {
                    // Draw the border of the selection rectangle with the specified pen
                    e.Graphics.DrawRectangle(borderPen, selectionRectangle); // Draw the rectangle border
                }
            }
        }
        /// <summary>
        /// Method to get candlesticks within the range of the wave
        /// </summary>
        /// <param name="candlesticks">list of the filtered candlesticks displayed in the chart</param>
        /// <param name="xStart">index where the selection starts</param>
        /// <param name="xEnd">index where the selection ends</param>
        /// <returns>a list of candlesticks within the range</returns>
        private List<ASmartCandlestick> GetCandlesticksInSelection(List<ASmartCandlestick> candlesticks, double xStart, double xEnd)
        {   // Filter the candlesticks that fall within the given x-axis range
            // Adjust indices by subtracting 1 to account for possible 0-based index mismatch or other offset considerations
            var selectedCandlesticks = filteredCandleSticks
                 .Where(c => c.Index >= xStart - 1 && c.Index <= xEnd - 1)
                 .ToList();

            // Return the list of selected candlesticks
            return selectedCandlesticks;
        }

                

    }
}
