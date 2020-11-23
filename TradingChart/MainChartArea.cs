using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace MagicalNuts
{
	public class MainChartArea : ChartAreaBase
	{
		protected List<DataTypes.Candle> Candles = null;
		private Label CursorLabelX = null;
		private PriceBoard PriceBoard = null;

		protected override int XCount { get => Candles.Count; }

		public override void SetUp(Chart chart)
		{
			// サイズ
			Position.X = 0;
			Position.Y = 0;
			Position.Width = 100;
			Position.Height = 100;

			// X軸
			AxisX.ScaleView.Size = 200;
			AxisX.MajorTickMark.Enabled = false;
			AxisX.MajorGrid.LineColor = Palette.GridColor;
			AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.SmallScroll;
			AxisX.ScrollBar.ButtonColor = Color.FromArgb(127, Color.Silver);
			AxisX.LabelAutoFitMaxFontSize = 8;
			AxisX.LabelAutoFitMinFontSize = 8;

			// Y軸（株価）
			AxisY2.ScaleView.Size = 3000;
			AxisY2.MajorGrid.LineColor = Palette.GridColor;
			AxisY2.ScrollBar.Enabled = false;
			AxisY2.ScrollBar.ButtonStyle = ScrollBarButtonStyles.None;
			AxisY2.LabelStyle.Format = "0.00";
			AxisY2.LabelAutoFitMaxFontSize = 8;
			AxisY2.LabelAutoFitMinFontSize = 8;
			AxisY2.MajorTickMark.Size = 0.3f;
			
			// Y軸（出来高）
			AxisY.MajorGrid.Enabled = false;
			AxisY.Enabled = AxisEnabled.False;
			AxisY.LabelStyle.Enabled = false;

			// カーソルラベルX
			CursorLabelX = new Label();
			SetUpCursorLabel(CursorLabelX);
			chart.Controls.Add(CursorLabelX);

			// 価格表示
			PriceBoard = new PriceBoard();
			PriceBoard.Top = chart.Margin.Top;
			PriceBoard.Left = chart.Margin.Left;
			chart.Controls.Add(PriceBoard);
			PriceBoard.SetCandle(null, null);

			base.SetUp(chart);
		}

		public void SetCandles(List<DataTypes.Candle> candles)
		{
			Candles = candles;
		}

		public override void Update(Point mouse, HitTestResult result, string format)
		{
			base.Update(mouse, result, format);

			// X取得
			int x = GetXFromMouse(mouse);

			// カーソルラベルX
			CursorLabelX.Text = Candles[x].DateTime.ToShortDateString();
			CursorLabelX.Left = mouse.X - CursorLabelX.Width / 2;
			CursorLabelX.Top = (int)(AxisY2.ValueToPixelPosition(AxisY2.ScaleView.Position) + 1 + AxisY2.ScrollBar.Size);

			// 価格表示
			PriceBoard.SetCandle(Candles[x], format);
		}

		public override void UpdateYSettings(int start, int end, List<Plotters.IPlotter> plotters)
		{
			// 価格

			// 値取得
			List<double> values = GetValues(start, end, plotters, AxisType.Secondary);
			
			if (values.Count > 0)
			{
				// 最高値、最安値取得
				double max = (double)values.Max();
				double min = (double)values.Min();

				// 範囲決定
				AxisY2.ScaleView.Size = (max - min) * (12.0 / 8.0);

				// 位置決定
				AxisY2.ScaleView.Position = min - AxisY2.ScaleView.Size / 4.0;
			}

			// 出来高

			// 値取得
			values = GetValues(start, end, plotters, AxisType.Primary);

			// 範囲決定
			if (values.Count > 0) AxisY.ScaleView.Size = values.Max() * 4;
		}
	}
}
