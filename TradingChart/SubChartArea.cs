using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace MagicalNuts
{
	/// <summary>
	/// TradingChartの従ChartAreaを表します。
	/// </summary>
	public class SubChartArea : ChartAreaBase
	{
		/// <summary>
		/// 分割線
		/// </summary>
		public HorizontalLineAnnotation Splitter { get; private set; }

		/// <summary>
		/// 従ChartAreaを準備します。
		/// </summary>
		/// <param name="chart">チャートコントロール</param>
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
			AxisY2.LabelAutoFitMaxFontSize = 8;
			AxisY2.LabelAutoFitMinFontSize = 8;

			base.SetUp(chart);
		}

		/// <summary>
		/// Y軸設定を更新します。
		/// </summary>
		/// <param name="start">開始x座標</param>
		/// <param name="end_x">終了x座標</param>
		/// <param name="plotters">プロッターのリスト</param>
		public override void UpdateYSettings(int start_x, int end_x, List<Plotters.IPlotter> plotters)
		{
			// Y値取得
			List<double> values = GetYValues(start_x, end_x, plotters, AxisType.Secondary);

			if (values.Count > 0)
			{
				// 最高値、最低値を取得
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
