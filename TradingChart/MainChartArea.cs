﻿using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace MagicalNuts
{
	/// <summary>
	/// 主ChartAreaを表します
	/// </summary>
	public class MainChartArea : ChartAreaBase
	{
		/// <summary>
		/// ロウソク足のリスト
		/// </summary>
		protected List<DataTypes.Candle> Candles = null;

		/// <summary>
		/// カーソルラベルX
		/// </summary>
		private Label CursorLabelX = null;

		/// <summary>
		/// 価格表示板
		/// </summary>
		private PriceBoard PriceBoard = null;

		/// <summary>
		/// 主ChartAreaを準備します。
		/// </summary>
		/// <param name="chart">チャートコントロール</param>
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
			AxisX.ScrollBar.ButtonColor = Palette.ScrollBarColor;
			AxisX.LabelAutoFitMaxFontSize = 8;
			AxisX.LabelAutoFitMinFontSize = 8;

			// Y軸（株価）
			AxisY2.MajorGrid.LineColor = Palette.GridColor;
			AxisY2.ScrollBar.Enabled = false;
			AxisY2.ScrollBar.ButtonStyle = ScrollBarButtonStyles.None;
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

			// 価格表示板
			PriceBoard = new PriceBoard();
			PriceBoard.Top = chart.Margin.Top;
			PriceBoard.Left = chart.Margin.Left;
			chart.Controls.Add(PriceBoard);
			PriceBoard.SetCandle(null, null, null);

			base.SetUp(chart);
		}

		/// <summary>
		/// ロウソク足を設定します。
		/// </summary>
		/// <param name="candles">ロウソク足</param>
		/// <param name="digits">小数点以下の桁数</param>
		public void SetCandles(List<DataTypes.Candle> candles, int digits)
		{
			Candles = candles;
			AxisY2.LabelStyle.Format = CandleUtility.GetPriceFormat(digits);
		}

		/// <summary>
		/// カーソルを更新します。
		/// </summary>
		/// <param name="mouse">マウス座標</param>
		/// <param name="result">ヒットテストの結果</param>
		/// <param name="x">x座標</param>
		/// <param name="max_x">最大x座標</param>
		/// <param name="format">価格表示のフォーマット</param>
		public override void UpdateCursors(Point mouse, HitTestResult result, int x, int max_x, string format)
		{
			base.UpdateCursors(mouse, result, x, max_x, format);

			// カーソルラベルX
			CursorLabelX.Text = Candles[x].DateTime.ToShortDateString();
			CursorLabelX.Left = mouse.X - CursorLabelX.Width / 2;
			CursorLabelX.Top = (int)(AxisY2.ValueToPixelPosition(AxisY2.ScaleView.Position) + 1 + AxisY2.ScrollBar.Size);

			// 価格表示板
			DataTypes.Candle prev = null;
			if (x > 0) prev = Candles[x - 1];
			PriceBoard.SetCandle(Candles[x], prev, format);
		}

		/// <summary>
		/// Y軸設定を更新します。
		/// </summary>
		/// <param name="start_x">開始x座標</param>
		/// <param name="end_x">終了x座標</param>
		/// <param name="plotters">プロッターのリスト</param>
		public override void UpdateYSettings(int start_x, int end_x, List<Plotters.IPlotter> plotters)
		{
			// 価格

			// Y値取得
			List<double> values = GetYValues(start_x, end_x, plotters, AxisType.Secondary);
			
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

			// Y値取得
			values = GetYValues(start_x, end_x, plotters, AxisType.Primary);

			// 範囲決定
			if (values.Count > 0) AxisY.ScaleView.Size = values.Max() * 4;
		}
	}
}
