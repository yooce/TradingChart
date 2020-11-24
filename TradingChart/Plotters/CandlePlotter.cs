using System.Collections.Generic;
using System.Windows.Forms.DataVisualization.Charting;

namespace MagicalNuts.Plotters
{
	/// <summary>
	/// ロウソク足プロッターを表します。
	/// </summary>
	public class CandleProtter : IndicatorPlotter<Indicators.CandleIndicator>
	{
		/// <summary>
		/// Series
		/// </summary>
		private Series Series = null;

		/// <summary>
		/// CandleProtterの新しいインスタンスを初期化します。
		/// </summary>
		public CandleProtter() : base()
		{
			Series = new Series();
			Series.ChartType = SeriesChartType.Candlestick;
			Series.YAxisType = AxisType.Secondary;
			Series["PriceUpColor"] = Palette.PriceUpColor.R.ToString() + ", " + Palette.PriceUpColor.G + ", " + Palette.PriceUpColor.B;
			Series["PriceDownColor"] = Palette.PriceDownColor.R.ToString() + ", " + Palette.PriceDownColor.G + ", " + Palette.PriceDownColor.B;
		}

		/// <summary>
		/// プロッター名を取得します。
		/// </summary>
		/// <returns>プロッター名</returns>
		public override string GetName()
		{
			return "ロウソク足";
		}

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
		/// Seriesを取得します。
		/// </summary>
		/// <returns>Seriesの配列</returns>
		public override Series[] GetSeries()
		{
			return new Series[] { Series };
		}

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
				// 値
				DataPoint dp = new DataPoint(x, Indicator.GetData(new Indicators.IndicatorArgs(GetCandlesForIndicator(x))));

				// 着色
				if (candles[x].Close >= candles[x].Open) dp.Color = Palette.PriceUpColor;
				else dp.Color = Palette.PriceDownColor;

				// 追加
				Series.Points.Add(dp);
			}
		}
	}
}
