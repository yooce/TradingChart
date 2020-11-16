using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace MagicalNuts.Plotters
{
	public class CandleProtter : IndicatorPlotter<Indicators.CandleIndicator>
	{
		private Series Series = null;

		public CandleProtter() : base()
		{
			Series = new Series();
			Series.ChartType = SeriesChartType.Candlestick;
			Series.YAxisType = AxisType.Secondary;
			Series["PriceUpColor"] = Palette.PriceUpColor.R.ToString() + ", " + Palette.PriceUpColor.G + ", " + Palette.PriceUpColor.B;
			Series["PriceDownColor"] = Palette.PriceDownColor.R.ToString() + ", " + Palette.PriceDownColor.G + ", " + Palette.PriceDownColor.B;
		}

		public override string GetName()
		{
			return "ロウソク足";
		}

		public override SubChartArea[] SetChartArea(MainChartArea mainChartArea)
		{
			Series.ChartArea = mainChartArea.Name;
			return null;
		}

		public override Series[] GetSeries()
		{
			return new Series[] { Series };
		}

		public override void Plot(List<DataTypes.Candle> candles)
		{
			base.Plot(candles);

			// クリア
			Series.Points.Clear();

			// プロット
			for (int x = 0; x < candles.Count; x++)
			{
				DataPoint dp = new DataPoint(x, Indicator.GetData(new Indicators.IndicatorArgs(GetCandlesForIndicator(x))));
				if (candles[x].Close >= candles[x].Open) dp.Color = Palette.PriceUpColor;
				else dp.Color = Palette.PriceDownColor;
				Series.Points.Add(dp);
			}
		}
	}
}
