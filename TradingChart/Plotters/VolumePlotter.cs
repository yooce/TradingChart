using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace MagicalNuts.Plotters
{
	/// <summary>
	/// 出来高プロッターを表します。
	/// </summary>
	public class VolumePlotter : IndicatorPlotter<Indicators.VolumeIndicator>
	{
		/// <summary>
		/// Series
		/// </summary>
		private Series Series = null;

		/// <summary>
		/// VolumePlotterクラスの新しいインスタンスを初期化します。
		/// </summary>
		public VolumePlotter() : base()
		{
			Series = new Series();
			Series.ChartType = SeriesChartType.Column;
		}

		/// <summary>
		/// プロッター名を取得します。
		/// </summary>
		/// <returns>プロッター名</returns>
		public override string GetName()
		{
			return "出来高";
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
				if (candles[x].Close >= candles[x].Open) dp.Color = Color.FromArgb(127, Palette.PriceUpColor);
				else dp.Color = Color.FromArgb(127, Palette.PriceDownColor);

				// 追加
				Series.Points.Add(dp);
			}
		}
	}
}
