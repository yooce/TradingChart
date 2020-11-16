using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace MagicalNuts.Plotters
{
	public class VolumePlotter : IndicatorPlotter<Indicators.VolumeIndicator>
	{
		private Series Series = null;

		public VolumePlotter()
		{
			Series = new Series();
			Series.ChartType = SeriesChartType.Column;
		}

		public override string GetName()
		{
			return "出来高";
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
				if (candles[x].Close >= candles[x].Open) dp.Color = Color.FromArgb(127, Palette.PriceUpColor);
				else dp.Color = Color.FromArgb(127, Palette.PriceDownColor);
				Series.Points.Add(dp);
			}
		}
	}
}
