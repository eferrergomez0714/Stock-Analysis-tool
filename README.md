Stock Analysis Application
---
Overview:
---
This Windows Forms application is designed to visualize and analyze stock market data. Developed in C#, it enables users to import, explore, and interact with financial data using candlestick charts. The application also incorporates advanced analytical features such as pattern recognition, Fibonacci level analysis, and basic trend prediction.

Features:
---
Data Visualization

- Candlestick Charts: Displays stock data (Open, High, Low, Close) using standard candlestick representations, with green indicating upward movement and red indicating downward movement.

- User Controls:
  - Load data from .csv files (Daily, Weekly, Monthly intervals).
  - Select stock symbol, date range, and time interval.
  - Dynamic filtering and automatic axis scaling for improved readability.

Advanced Analysis
---
- Enhanced Candlestick Model:
  A custom SmartCandlestick class extends basic candlestick functionality by supporting:
    - Pattern detection (e.g., Doji, Hammer, Marubozu)
    - Computation of candle body size, price range, and upper/lower shadows
- Chart Annotations:
    - Highlights local peaks (green markers) and valleys (red markers)
    - Draws horizontal support/resistance lines at key levels

Predictive Tools
---
- Wave Selection:
  - Interactive selection of candlestick waves using a drag (“rubber band”) interface
  - Built-in validation to ensure meaningful selections
- Fibonacci Analysis:
  - Automatically calculates and overlays Fibonacci retracement levels based on selected waves
- Beauty Function:
  - Measures how closely price movements align with Fibonacci levels
  - Visualizes this alignment to help identify potential future highs and lows

How It Works
---
- Data Import:
  -Load historical stock data (e.g., from Yahoo Finance) in .csv format containing: Date, Open, High, Low, Close, and Volume.
-Visualization:
  -Charts are normalized to handle gaps such as weekends and market holidays.
- Analysis Workflow:
  - Detect candlestick patterns
  - Identify key turning points (peaks and valleys)
  - Apply Fibonacci levels and evaluate trend behavior using the beauty metric

Installation
---
git clone [https://github.com/username/stock-analysis-app.git](https://github.com/eferrergomez0714/Stock-Analysis-tool.git)
1. Open the solution in Visual Studio
2. Build and run the application

Usage
---
1. Add .csv files to the Stock Data Samples folder
2. Launch the application
3. Load a dataset and choose the desired date range and interval
4. Use the visualization and analysis tools to explore patterns and trends

Requirements
---
- Visual Studio 2022 or later
- .NET Framework (Windows Forms)

Future Enhancements
---
- Expand candlestick pattern recognition with more advanced techniques
- Integrate machine learning models for improved prediction accuracy
- Add support for real-time market data streaming
