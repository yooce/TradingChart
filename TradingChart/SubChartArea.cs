using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace MagicalNuts
{
	public class SubChartArea : ChartAreaBase
	{
		public HorizontalLineAnnotation Splitter { get; private set; }

		protected override int XCount => 0;

		public override void SetUp(Chart chart)
		{
			// 分割線
			Splitter = new HorizontalLineAnnotation();
			Splitter.AllowMoving = true;
			Splitter.LineWidth = 2;
			Splitter.LineColor = Palette.SplitterColor;
			Splitter.X = 0;
			Splitter.Width = 100;

			// X軸
			AxisX.ScaleView.Size = 150;
			AxisX.ScrollBar.Enabled = false;
			AxisX.MajorTickMark.Enabled = false;
			AxisX.MajorGrid.LineColor = Palette.GridColor;

			// Y軸
			AxisY2.MajorGrid.LineColor = Palette.GridColor;
			AxisY2.ScrollBar.Enabled = false;
			AxisY2.ScrollBar.ButtonStyle = ScrollBarButtonStyles.None;
			AxisY2.Crossing = 0.0;

			base.SetUp(chart);
		}
	}
}
