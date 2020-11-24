using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MagicalNuts.TradingChartSample
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();

			comboBox1.SelectedIndex = 0;
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			// JSONロード
			System.IO.StreamReader sr = new System.IO.StreamReader(
				System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("MagicalNuts.TradingChartSample.candles.json"));
			string str = sr.ReadToEnd();
			sr.Close();

			// デシリアライズ
			DataTypes.Candle[] candles = Utf8Json.JsonSerializer.Deserialize<DataTypes.Candle[]>(str);

			// セットアップ
			tradingChart1.SetUp();

			// 日足設定
			tradingChart1.SetDailyCandles(candles, 2, CandleTerm.Dayly);
		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			tradingChart1.CandleTerm = (CandleTerm)comboBox1.SelectedIndex;
		}

		private void numericUpDown1_ValueChanged(object sender, EventArgs e)
		{
			tradingChart1.ScreenCandleNum = (int)numericUpDown1.Value;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (numericUpDown1.Value - 50 < numericUpDown1.Minimum) numericUpDown1.Value = numericUpDown1.Minimum;
			else numericUpDown1.Value -= 50;
		}

		private void button2_Click(object sender, EventArgs e)
		{
			if (numericUpDown1.Value + 50 > numericUpDown1.Maximum) numericUpDown1.Value = numericUpDown1.Maximum;
			else numericUpDown1.Value += 50;
		}
	}
}
