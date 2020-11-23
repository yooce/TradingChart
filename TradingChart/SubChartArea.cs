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

		public override void UpdateYSettings(int start, int end, List<Plotters.IPlotter> plotters)
		{
			// 値取得
			List<double> values = GetValues(start, end, plotters, AxisType.Secondary);

			if (values.Count > 0)
			{
				// 最高値、最安値を取得
				double max = (double)values.Max();
				double min = (double)values.Min();

				// 範囲決定
				AxisY2.ScaleView.Size = (max - min) * (12.0 / 10.0);

				// 位置決定
				AxisY2.ScaleView.Position = min - AxisY2.ScaleView.Size / 10.0;
			}
		}
	}
}
