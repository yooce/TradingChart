using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace MagicalNuts.Plotters
{
	public class AtrPlotter : IndicatorPlotter<Indicators.AtrIndicator>
	{
		private Series Series = null;

		public AtrPlotter()
		{
			Series = new Series();
			Series.ChartType = SeriesChartType.Line;
			Series.YAxisType = AxisType.Secondary;
		}

		public override string GetName()
		{
			return "ATR";
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
				double[] data = Indicator.GetData(new Indicators.IndicatorArgs(GetCandlesForIndicator(x)));
				if (data == null) continue;

				Series.Points.Add(new DataPoint(x, data));
			}
		}

		public override SubChartArea[] SetChartArea(MainChartArea mainChartArea)
		{
			SubChartArea subChartArea = new SubChartArea();
			Series.ChartArea = subChartArea.Name;
			return new SubChartArea[] { subChartArea };
		}
	}
}
