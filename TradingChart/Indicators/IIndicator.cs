using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace MagicalNuts.Indicators
{
	public interface IIndicator
	{
		double[] GetData(IndicatorArgs args);
	}
}
