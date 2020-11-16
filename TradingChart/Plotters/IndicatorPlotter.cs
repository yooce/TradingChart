using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace MagicalNuts.Plotters
{
	public abstract class IndicatorPlotter<T> : IPlotter where T : Indicators.IIndicator, new()
	{
		private List<DataTypes.Candle> ReversedCandles = null;
		protected T Indicator;

		public IndicatorPlotter()
		{
			Indicator = new T();
		}

		public abstract string GetName();
		public abstract SubChartArea[] SetChartArea(MainChartArea mainChartArea);
		public abstract Series[] GetSeries();
		public virtual void Plot(List<DataTypes.Candle> candles)
		{
			ReversedCandles = new List<DataTypes.Candle>();
			ReversedCandles.AddRange(candles);
			ReversedCandles.Reverse();
		}

		protected List<DataTypes.Candle> GetCandlesForIndicator(int x)
		{
			return ReversedCandles.GetRange(ReversedCandles.Count - x - 1, x + 1);
		}
	}
}
