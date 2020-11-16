using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace MagicalNuts.Plotters
{
	public interface IPlotter
	{
		string GetName();
		SubChartArea[] SetChartArea(MainChartArea mainChartArea);
		Series[] GetSeries();
		void Plot(List<DataTypes.Candle> candles);
	}
}
