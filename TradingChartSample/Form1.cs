using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
			System.IO.StreamReader sr = new System.IO.StreamReader(
				System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("MagicalNuts.TradingChartSample.candles.json"));
			string str = sr.ReadToEnd();
			sr.Close();

			List<DataTypes.Candle> candles = Utf8Json.JsonSerializer.Deserialize<List<DataTypes.Candle>>(str);

			tradingChart1.SetUp();
			tradingChart1.SetDailyCandles(candles, 2, CandleTerm.Dayly);
		}
	}
}
