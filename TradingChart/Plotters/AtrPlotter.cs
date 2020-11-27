using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

namespace MagicalNuts.Plotters
{
	/// <summary>
	/// ATRプロッターのプロパティを表します。
	/// </summary>
	public class AtrPlotterProperties : Indicators.AtrIndicatorProperties
	{
		/// <summary>
		/// 小数点以下の桁数を設定または取得します。
		/// </summary>
		[Category("ATR")]
		[Description("小数点以下の桁数を設定します。")]
		[DefaultValue(2)]
		public int Digits { get; set; } = 2;

		/// <summary>
		/// 色を設定または取得します。
		/// </summary>
		[Category("ATR")]
		[Description("色を設定します。")]
		[DefaultValue(typeof(Color), "163, 9, 27")]
		public Color Color { get; set; } = Color.FromArgb(163, 9, 27);
	}

	/// <summary>
	/// ATRのプロッターを表します。
	/// </summary>
	public class AtrPlotter : IndicatorPlotter<Indicators.AtrIndicator>
	{
		/// <summary>
		/// Series
		/// </summary>
		private Series Series = null;

		/// <summary>
		/// 従ChartArea
		/// </summary>
		private SubChartArea SubChartArea = null;

		/// <summary>
		/// AtrPlotterクラスの新しいインスタンスを初期化します。
		/// </summary>
		public AtrPlotter()
		{
			Indicator.Properties = new AtrPlotterProperties();
			Series = new Series();
			Series.ChartType = SeriesChartType.Line;
			Series.YAxisType = AxisType.Secondary;
			ApplyProperties();
		}

		/// <summary>
		/// プロッター名を取得します。
		/// </summary>
		public override string Name => "ATR";

		/// <summary>
		/// Seriesの配列を取得します。
		/// </summary>
		public override Series[] SeriesArray => new Series[] { Series };

		/// <summary>
		/// プロパティを取得します。
		/// </summary>
		public override object Properties => Indicator.Properties;

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
		/// ChartAreaを設定します。
		/// </summary>
		/// <param name="mainChartArea">主ChartArea</param>
		/// <returns>使用する従ChartAreaの配列</returns>
		public override SubChartArea[] SetChartArea(MainChartArea mainChartArea)
		{
			SubChartArea = new SubChartArea();
			Series.ChartArea = SubChartArea.Name;
			ApplyProperties();
			return new SubChartArea[] { SubChartArea };
		}

		/// <summary>
		/// プロパティを適用します。
		/// </summary>
		public override void ApplyProperties()
		{
			Series.Color = ((AtrPlotterProperties)Properties).Color;
			if (SubChartArea != null)
				SubChartArea.AxisY2.LabelStyle.Format = CandleUtility.GetPriceFormat(((AtrPlotterProperties)Properties).Digits);
		}
	}
}
