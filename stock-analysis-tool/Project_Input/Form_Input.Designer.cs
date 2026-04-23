namespace Project_Input
{
    partial class Form_Input
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.button_loadData = new System.Windows.Forms.Button();
            this.openFileDialog_load = new System.Windows.Forms.OpenFileDialog();
            this.dateTimePicker_startDate = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker_endDate = new System.Windows.Forms.DateTimePicker();
            this.chart_candlesticks = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.button_updateData = new System.Windows.Forms.Button();
            this.button_Peak = new System.Windows.Forms.Button();
            this.button_Valley = new System.Windows.Forms.Button();
            this.chart_Beauty = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.beautyBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.aCandlestickBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.aCandlestickBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.chart_candlesticks)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_Beauty)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.beautyBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.aCandlestickBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.aCandlestickBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // button_loadData
            // 
            this.button_loadData.Location = new System.Drawing.Point(360, 777);
            this.button_loadData.Name = "button_loadData";
            this.button_loadData.Size = new System.Drawing.Size(123, 47);
            this.button_loadData.TabIndex = 0;
            this.button_loadData.Text = "Load Stock";
            this.button_loadData.UseVisualStyleBackColor = true;
            this.button_loadData.Click += new System.EventHandler(this.button_start_Click);
            // 
            // openFileDialog_load
            // 
            this.openFileDialog_load.FileName = "openFileDialog1";
            this.openFileDialog_load.Multiselect = true;
            this.openFileDialog_load.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog_load_FileOk);
            // 
            // dateTimePicker_startDate
            // 
            this.dateTimePicker_startDate.Location = new System.Drawing.Point(50, 787);
            this.dateTimePicker_startDate.Name = "dateTimePicker_startDate";
            this.dateTimePicker_startDate.Size = new System.Drawing.Size(252, 22);
            this.dateTimePicker_startDate.TabIndex = 2;
            this.dateTimePicker_startDate.Value = new System.DateTime(2024, 1, 1, 0, 0, 0, 0);
            // 
            // dateTimePicker_endDate
            // 
            this.dateTimePicker_endDate.Location = new System.Drawing.Point(748, 787);
            this.dateTimePicker_endDate.Name = "dateTimePicker_endDate";
            this.dateTimePicker_endDate.Size = new System.Drawing.Size(252, 22);
            this.dateTimePicker_endDate.TabIndex = 3;
            // 
            // chart_candlesticks
            // 
            chartArea1.Name = "ChartArea_OHLC";
            this.chart_candlesticks.ChartAreas.Add(chartArea1);
            this.chart_candlesticks.Location = new System.Drawing.Point(40, 12);
            this.chart_candlesticks.Name = "chart_candlesticks";
            series1.ChartArea = "ChartArea_OHLC";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Candlestick;
            series1.CustomProperties = "PriceDownColor=Red, PriceUpColor=Green";
            series1.IsXValueIndexed = true;
            series1.Name = "Series_OHLC";
            series1.XValueMember = "Date";
            series1.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
            series1.YValueMembers = "High, Low, Open, Close";
            series1.YValuesPerPoint = 6;
            this.chart_candlesticks.Series.Add(series1);
            this.chart_candlesticks.Size = new System.Drawing.Size(960, 496);
            this.chart_candlesticks.TabIndex = 2;
            this.chart_candlesticks.Text = "chart1";
            // 
            // button_updateData
            // 
            this.button_updateData.Location = new System.Drawing.Point(546, 777);
            this.button_updateData.Name = "button_updateData";
            this.button_updateData.Size = new System.Drawing.Size(123, 47);
            this.button_updateData.TabIndex = 5;
            this.button_updateData.Text = "Update";
            this.button_updateData.UseVisualStyleBackColor = true;
            this.button_updateData.Click += new System.EventHandler(this.button_update_Click);
            // 
            // button_Peak
            // 
            this.button_Peak.Location = new System.Drawing.Point(360, 725);
            this.button_Peak.Name = "button_Peak";
            this.button_Peak.Size = new System.Drawing.Size(123, 46);
            this.button_Peak.TabIndex = 6;
            this.button_Peak.Text = "Show Peak";
            this.button_Peak.UseVisualStyleBackColor = true;
            this.button_Peak.Click += new System.EventHandler(this.button_Peak_Click);
            // 
            // button_Valley
            // 
            this.button_Valley.Location = new System.Drawing.Point(546, 725);
            this.button_Valley.Name = "button_Valley";
            this.button_Valley.Size = new System.Drawing.Size(123, 46);
            this.button_Valley.TabIndex = 7;
            this.button_Valley.Text = "Show Valley";
            this.button_Valley.UseVisualStyleBackColor = true;
            this.button_Valley.Click += new System.EventHandler(this.button_Valley_Click);
            // 
            // chart_Beauty
            // 
            chartArea2.AxisX.Title = "Price";
            chartArea2.AxisY.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;
            chartArea2.AxisY.Title = "Wave Beauty";
            chartArea2.Name = "ChartArea_Beauty";
            this.chart_Beauty.ChartAreas.Add(chartArea2);
            this.chart_Beauty.Location = new System.Drawing.Point(40, 514);
            this.chart_Beauty.Name = "chart_Beauty";
            series2.ChartArea = "ChartArea_Beauty";
            series2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            series2.IsValueShownAsLabel = true;
            series2.IsXValueIndexed = true;
            series2.Name = "Series_Beauty";
            series2.XValueMember = "Price";
            series2.YValueMembers = "Beauty";
            this.chart_Beauty.Series.Add(series2);
            this.chart_Beauty.Size = new System.Drawing.Size(960, 205);
            this.chart_Beauty.TabIndex = 8;
            this.chart_Beauty.Text = "chart Beauty";
            // 
            // beautyBindingSource
            // 
            this.beautyBindingSource.DataSource = typeof(Project_Input.Beauty);
            // 
            // aCandlestickBindingSource1
            // 
            this.aCandlestickBindingSource1.DataSource = typeof(Project_Input.aCandlestick);
            // 
            // aCandlestickBindingSource
            // 
            this.aCandlestickBindingSource.DataSource = typeof(Project_Input.aCandlestick);
            // 
            // Form_Input
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1281, 853);
            this.Controls.Add(this.chart_Beauty);
            this.Controls.Add(this.button_Valley);
            this.Controls.Add(this.button_Peak);
            this.Controls.Add(this.button_updateData);
            this.Controls.Add(this.chart_candlesticks);
            this.Controls.Add(this.dateTimePicker_endDate);
            this.Controls.Add(this.dateTimePicker_startDate);
            this.Controls.Add(this.button_loadData);
            this.Name = "Form_Input";
            this.Text = "Form_Input";
            this.Load += new System.EventHandler(this.Form_Input_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart_candlesticks)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart_Beauty)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.beautyBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.aCandlestickBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.aCandlestickBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button_loadData;
        private System.Windows.Forms.OpenFileDialog openFileDialog_load;
        private System.Windows.Forms.BindingSource aCandlestickBindingSource;
        private System.Windows.Forms.DateTimePicker dateTimePicker_startDate;
        private System.Windows.Forms.DateTimePicker dateTimePicker_endDate;
        private System.Windows.Forms.BindingSource aCandlestickBindingSource1;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_candlesticks;
        private System.Windows.Forms.Button button_updateData;
        private System.Windows.Forms.Button button_Peak;
        private System.Windows.Forms.Button button_Valley;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart_Beauty;
        private System.Windows.Forms.BindingSource beautyBindingSource;
    }
}

