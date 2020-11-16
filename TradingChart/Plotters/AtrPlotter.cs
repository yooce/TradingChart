using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace MagicalNuts.Plotters
{
	public class AtrPlotter : IPlotter
	{
		private Series Series = null;

		public AtrPlotter()
		{
			Series = new Series();
			Series.ChartType = SeriesChartType.Line;
			Series.YAxisType = AxisType.Secondary;
		}

		public string GetName()
		{
			return "ATR";
		}

		public Series[] GetSeries()
		{
			return new Series[] { Series };
		}

		public void Plot(List<DataTypes.Candle> candles)
		{
			// クリア
			Series.Points.Clear();

			// プロット
			for (int x = 0; x < candles.Count; x++)
			{
				DataPoint dp = new DataPoint(x, Math.Sin((double)x / 4.0f));
				Series.Points.Add(dp);
			}
		}

		public SubChartArea[] SetChartArea(MainChartArea mainChartArea)
		{
			SubChartArea subChartArea = new SubChartArea();
			Series.ChartArea = subChartArea.Name;
			return new SubChartArea[] { subChartArea };
		}
	}
}
