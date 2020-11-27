using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;

namespace MagicalNuts.Plotters
{
	public class MacdPlotterProperties : Indicators.MacdIndicatorProperties
	{
		/// <summary>
		/// MACDの色を設定または取得します。
		/// </summary>
		[Category("MACD")]
		[Description("MACDの色を設定します。")]
		[DefaultValue(typeof(Color), "0, 143, 250")]
		public Color MacdColor { get; set; } = Color.FromArgb(0, 143, 250);

		/// <summary>
		/// シグナルの色を設定または取得します。
		/// </summary>
		[Category("MACD")]
		[Description("シグナルの色を設定します。")]
		[DefaultValue(typeof(Color), "255, 103, 36")]
		public Color SignalColor { get; set; } = Color.FromArgb(255, 103, 36);

		/// <summary>
		/// オシレーターがプラスで増加時の色を設定または取得します。
		/// </summary>
		[Category("MACD")]
		[Description("オシレーターがプラスで増加時の色を設定します。")]
		[DefaultValue(typeof(Color), "0, 167, 154")]
		public Color PlusUpColor { get; set; } = Palette.PriceUpColor;

		/// <summary>
		/// オシレーターがプラスで減少時の色を設定または取得します。
		/// </summary>
		[Category("MACD")]
		[Description("オシレーターがプラスで減少時の色を設定します。")]
		[DefaultValue(typeof(Color), "169, 224, 219")]
		public Color PlusDownColor { get; set; } = Color.FromArgb(169, 224, 219);

		/// <summary>
		/// オシレーターがマイナスで増加時の色を設定または取得します。
		/// </summary>
		[Category("MACD")]
		[Description("オシレーターがマイナスで増加時の色を設定します。")]
		[DefaultValue(typeof(Color), "255, 204, 210")]
		public Color MinusUpColor { get; set; } = Color.FromArgb(255, 204, 210);

		/// <summary>
		/// オシレーターがマイナスで減少時の色を設定または取得します。
		/// </summary>
		[Category("MACD")]
		[Description("オシレーターがマイナスで減少時の色を設定します。")]
		[DefaultValue(typeof(Color), "254, 77, 84")]
		public Color MinusDownColor { get; set; } = Palette.PriceDownColor;
	}

	/// <summary>
	/// MACDのプロッターを表します。
	/// </summary>
	public class MacdPlotter : IndicatorPlotter<Indicators.MacdIndicator>
	{
		/// <summary>
		/// Series
		/// </summary>
		private Series[] Series = null;

		/// <summary>
		/// MacdPlotterの新しいインスタンスを初期化します。
		/// </summary>
		public MacdPlotter()
		{
			Indicator.Properties = new MacdPlotterProperties();
			Series = new Series[3];
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
						Series[i].ChartType = SeriesChartType.Line;
						break;
					case 2:
						Series[i].ChartType = SeriesChartType.Column;
						break;
				}
			}
			ApplyProperties();
		}

		/// <summary>
		/// プロッター名を取得します。
		/// </summary>
		public override string Name { get => "MACD"; }

		/// <summary>
		/// プロパティを取得します。
		/// </summary>
		public override object Properties => Indicator.Properties;

		/// <summary>
		/// Seriesの配列を取得します。
		/// </summary>
		public override Series[] SeriesArray { get => Series; }

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

				// MACD
				Series[0].Points.Add(new DataPoint(x, data[0]));

				// MACDシグナル
				Series[1].Points.Add(new DataPoint(x, data[1]));

				// MACDオシレーター
				Series[2].Points.Add(new DataPoint(x, data[0] - data[1]));
			}

			// オシレーター色
			SetOscillatorColors();
		}

		private void SetOscillatorColors()
		{
			MacdPlotterProperties properties = (MacdPlotterProperties)Properties;

			DataPoint prevDp = null;
			foreach (DataPoint dp in Series[2].Points)
			{
				if (dp.YValues[0] >= 0)
				{
					if (prevDp != null && prevDp.YValues[0] > dp.YValues[0]) dp.Color = properties.PlusDownColor;
					else dp.Color = properties.PlusUpColor;
				}
				else
				{
					if (prevDp != null && prevDp.YValues[0] < dp.YValues[0]) dp.Color = properties.MinusUpColor;
					else dp.Color = properties.MinusDownColor;
				}
				prevDp = dp;
			}
		}

		/// <summary>
		/// ChartAreaを設定します。
		/// </summary>
		/// <param name="mainChartArea">主ChartArea</param>
		/// <returns>使用する従ChartAreaの配列</returns>
		public override SubChartArea[] SetChartArea(MainChartArea mainChartArea)
		{
			SubChartArea subChartArea = new SubChartArea();
			foreach (Series series in Series)
			{
				series.ChartArea = subChartArea.Name;
			}
			return new SubChartArea[] { subChartArea };
		}

		/// <summary>
		/// プロパティを適用します。
		/// </summary>
		public override void ApplyProperties()
		{
			MacdPlotterProperties properties = (MacdPlotterProperties)Properties;
			Series[0].Color = properties.MacdColor;
			Series[1].Color = properties.SignalColor;
			SetOscillatorColors();
		}
	}
}
