using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

namespace MagicalNuts.Plotters
{
	/// <summary>
	/// 移動平均プロッターのプロパティを表します。
	/// </summary>
	public class MovingAveragePlotterProperties : Indicators.MovingAverageIndicatorProperties
	{
		/// <summary>
		/// 移動平均線の色
		/// </summary>
		[Category("移動平均")]
		[Description("移動平均線の色を設定します。")]
		[DefaultValue(typeof(Color), "144, 30, 38")]
		public Color Color { get; set; } = Color.FromArgb(144, 30, 38);
	}

	/// <summary>
	/// 移動平均のプロッターを表します。
	/// </summary>
	public class MovingAveragePlotter : IndicatorPlotter<Indicators.MovingAverageIndicator>
	{
		/// <summary>
		/// Series
		/// </summary>
		private Series Series = null;

		/// <summary>
		/// MovingAveragePlotterの新しいインスタンスを初期化します。
		/// </summary>
		public MovingAveragePlotter()
		{
			Indicator.Properties = new MovingAveragePlotterProperties();
			Series = new Series();
			Series.ChartType = SeriesChartType.Line;
			Series.YAxisType = AxisType.Secondary;
			ApplyProperties();
		}

		/// <summary>
		/// プロッター名を取得します。
		/// </summary>
		public override string Name { get => "移動平均"; }

		/// <summary>
		/// プロパティを取得します。
		/// </summary>
		public override object Properties { get => Indicator.Properties; }

		/// <summary>
		/// ChartAreaを設定します。
		/// </summary>
		/// <param name="mainChartArea">主ChartArea</param>
		/// <returns>使用する従ChartAreaの配列</returns>
		public override SubChartArea[] SetChartArea(MainChartArea mainChartArea)
		{
			Series.ChartArea = mainChartArea.Name;
			return null;
		}

		/// <summary>
		/// Seriesの配列を取得します。
		/// </summary>
		public override Series[] SeriesArray { get => new Series[] { Series }; }

		/// <summary>
		/// データをプロットします。
		/// </summary>
		/// <param name="candles">ロウソク足のリスト</param>
		public override void Plot(List<DataTypes.Candle> candles)
		{
			base.Plot(candles);

			// クリア
			Series.Points.Clear();

			// プロット
			for (int x = 0; x < candles.Count; x++)
			{
				double[] data = Indicator.GetValues(new Indicators.IndicatorArgs(GetCandlesForIndicator(x)));
				if (data == null) continue;

				Series.Points.Add(new DataPoint(x, data));
			}
		}

		/// <summary>
		/// プロパティを適用します。
		/// </summary>
		public override void ApplyProperties()
		{
			Series.Color = ((MovingAveragePlotterProperties)Indicator.Properties).Color;
		}
	}
}
