﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

namespace MagicalNuts.Plotters
{
	/// <summary>
	/// ボリンジャーバンドプロッターのプロパティを表します。
	/// </summary>
	public class BollingerBandPlotterProperties : Indicators.BollingerBandIndicatorProperties
	{
		/// <summary>
		/// 移動平均線の色を取得または設定します。
		/// </summary>
		[Category("ボリンジャーバンド")]
		[Description("移動平均線の色を設定します。")]
		[DefaultValue(typeof(Color), "144, 30, 38")]
		public Color MaColor { get; set; } = Color.FromArgb(144, 30, 38);

		/// <summary>
		/// ボリンジャーバンドの色を取得または設定します。
		/// </summary>
		[Category("ボリンジャーバンド")]
		[Description("ボリンジャーバンドの色を設定します。")]
		[DefaultValue(typeof(Color), "0, 133, 131")]
		public Color BandColor { get; set; } = Color.FromArgb(0, 133, 131);

		/// <summary>
		/// ボリンジャーバンドのアルファ値を取得または設定します。
		/// </summary>
		[Category("ボリンジャーバンド")]
		[Description("ボリンジャーバンドのアルファ値を設定します。")]
		[DefaultValue(10)]
		public int BandAlpha { get; set; } = 10;

		/// <summary>
		/// 高速モードかどうかを取得または設定します。
		/// </summary>
		[Category("ボリンジャーバンド")]
		[Description("高速モードかどうかを設定します。")]
		[DefaultValue(true)]
		public bool FastMode { get; set; } = true;
	}

	/// <summary>
	/// ボリンジャーバンドのプロッターを表します。
	/// </summary>
	public class BollingerBandPlotter : IndicatorPlotter<Indicators.BollingerBandIndicator>
	{
		/// <summary>
		/// Series
		/// </summary>
		private Series[] Series = null;

		/// <summary>
		/// BollingerBandPlotterクラスの新しいインスタンスを初期化します。
		/// </summary>
		public BollingerBandPlotter()
		{
			Indicator.Properties = new BollingerBandPlotterProperties();
			Series = new Series[4];
			for (int i = 0; i < Series.Length; i++)
			{
				Series[i] = new Series();
				Series[i].YAxisType = AxisType.Secondary;
				switch (i)
				{
					case 0:
						Series[i].ChartType = SeriesChartType.Line;
						break;
					case 1:
						Series[i].ChartType = SeriesChartType.Range;
						break;
					default:
						Series[i].ChartType = SeriesChartType.Line;
						break;
				}
			}
			ApplyProperties();
		}

		/// <summary>
		/// プロッター名を取得します。
		/// </summary>
		public override string Name => "ボリンジャーバンド";

		/// <summary>
		/// プロパティを取得します。
		/// </summary>
		public override object Properties => Indicator.Properties;

		/// <summary>
		/// Seriesの配列を取得します。
		/// </summary>
		public override Series[] SeriesArray => Series;

		/// <summary>
		/// データをプロットします。
		/// </summary>
		/// <param name="candles">ロウソク足のリスト</param>
		public override void Plot(List<DataTypes.Candle> candles)
		{
			base.Plot(candles);

			// クリア
			foreach (Series series in Series)
			{
				series.Points.Clear();
			}

			// プロット
			for (int x = 0; x < candles.Count; x++)
			{
				double[] data = Indicator.GetValues(new Indicators.IndicatorArgs(GetCandlesForIndicator(x)));
				if (data == null) continue;

				Series[0].Points.Add(new DataPoint(x, data[0]));
				Series[1].Points.Add(new DataPoint(x, new double[] { data[1], data[2] }));
				Series[2].Points.Add(new DataPoint(x, data[1]));
				Series[3].Points.Add(new DataPoint(x, data[2]));
			}
		}

		/// <summary>
		/// ChartAreaを設定します。
		/// </summary>
		/// <param name="mainChartArea">主ChartArea</param>
		/// <returns>使用する従ChartAreaの配列</returns>
		public override SubChartArea[] SetChartArea(MainChartArea mainChartArea)
		{
			foreach (Series series in Series)
			{
				series.ChartArea = mainChartArea.Name;
			}
			return null;
		}

		/// <summary>
		/// プロパティを適用します。
		/// </summary>
		public override void ApplyProperties()
		{
			BollingerBandPlotterProperties properties = (BollingerBandPlotterProperties)Properties;

			// 移動平均
			Indicator.MovingAverageIndicator.Properties.Period = properties.Period;

			// 色
			for (int i = 0; i < Series.Length; i++)
			{
				switch (i)
				{
					case 0:
						Series[0].Color = properties.MaColor;
						break;
					case 1:
						Series[1].Color = Color.FromArgb(properties.BandAlpha, properties.BandColor);
						break;
					default:
						Series[i].Color = properties.BandColor;
						break;
				}
			}

			// 高速モード
			Series[1].Enabled = !properties.FastMode;
		}
	}
}
