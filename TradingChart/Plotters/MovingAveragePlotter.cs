using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace MagicalNuts.Plotters
{
	/// <summary>
	/// 移動平均インジケーターのプロッター用拡張を表します。
	/// </summary>
	public class MovingAverageIndicatorEx : Indicators.MovingAverageIndicator
	{
		/// <summary>
		/// 色を設定または取得します。
		/// </summary>
		[Category("プロット")]
		[DisplayName("色")]
		[Description("色を設定します。")]
		[DefaultValue(typeof(Color), "144, 30, 38")]
		public Color Color { get; set; } = Color.FromArgb(144, 30, 38);
	}

	/// <summary>
	/// 移動平均のプロッターを表します。
	/// </summary>
	public class MovingAveragePlotter : IndicatorPlotter<MovingAverageIndicatorEx>
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
			Series = new Series();
			Series.ChartType = SeriesChartType.Line;
			Series.YAxisType = AxisType.Secondary;
		}

		/// <summary>
		/// プロッター名を取得します。
		/// </summary>
		public override string Name => "移動平均";

		/// <summary>
		/// プロパティを取得します。
		/// </summary>
		public override object Properties => Indicator;

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
				decimal[] data = Indicator.GetValues(GetCandleCollection(x));
				if (data == null) continue;

				Series.Points.Add(new DataPoint(x, ConvertDecimalToDoubleArray(data)));
			}
		}

		/// <summary>
		/// 非同期で準備します。
		/// </summary>
		/// <returns>非同期タスク非同期タスク</returns>
		public override async Task SetUpAsync()
		{
			await base.SetUpAsync();
			Series.Color = ((MovingAverageIndicatorEx)Indicator).Color;
		}
	}
}
