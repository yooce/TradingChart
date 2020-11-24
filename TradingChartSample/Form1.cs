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
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			// JSONロード
			System.IO.StreamReader sr = new System.IO.StreamReader(
				System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("MagicalNuts.TradingChartSample.candles.json"));
			string str = sr.ReadToEnd();
			sr.Close();

			// デシリアライズ
			List<DataTypes.Candle> candles = Utf8Json.JsonSerializer.Deserialize<List<DataTypes.Candle>>(str);

			// セットアップ
			tradingChart1.SetUp();

			// 日足設定
			tradingChart1.SetDailyCandles(candles, 2, CandleTerm.Dayly);
		}
	}
}
