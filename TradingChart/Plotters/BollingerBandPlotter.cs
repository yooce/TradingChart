using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace MagicalNuts.Plotters
{
	public class BollingerBandPlotter : IndicatorPlotter<Indicators.BollingerBandIndicator>
	{
		private Series[] Series = null;

		public BollingerBandPlotter()
		{
			Series = new Series[4];
			for (int i = 0; i < Series.Length; i++)
			{
				Series[i] = new Series();
				Series[i].YAxisType = AxisType.Secondary;
				switch (i)
				{
					case 0:
						Series[i].ChartType = SeriesChartType.Line;
						Series[i].Color = Color.FromArgb(144, 30, 38);
						break;
					case 1:
						Series[i].ChartType = SeriesChartType.Range;
						Series[i].Color = Color.FromArgb(10, 0, 133, 131);
						break;
					default:
						Series[i].ChartType = SeriesChartType.Line;
						Series[i].Color = Color.FromArgb(0, 133, 131);
						break;
				}
			}
		}

		public override string GetName()
		{
			return "ボリンジャーバンド";
		}

		public override Series[] GetSeries()
		{
			return Series;
		}

		public override void Plot(List<DataTypes.Candle> candles)
		{
			base.Plot(candles);

			// クリア
			foreach (Series series in Series)
			{
				series.Points.Clear();
			}

			// プロット
			for (int x = 0; x < candles.Count; x++)
			{
				double[] data = Indicator.GetData(new Indicators.IndicatorArgs(GetCandlesForIndicator(x)));
				if (data == null) continue;

				Series[0].Points.Add(new DataPoint(x, data[0]));
				Series[1].Points.Add(new DataPoint(x, new double[] { data[1], data[2] }));
				Series[2].Points.Add(new DataPoint(x, data[1]));
				Series[3].Points.Add(new DataPoint(x, data[2]));
			}
		}

		public override SubChartArea[] SetChartArea(MainChartArea mainChartArea)
		{
			foreach (Series series in Series)
			{
				series.ChartArea = mainChartArea.Name;
			}
			return null;
		}
	}
}
