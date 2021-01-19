using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MagicalNuts.Indicators
{
	/// <summary>
	/// 移動平均の計算方法
	/// </summary>
	public enum MaMethod
	{
		Sma,	// 単純移動平均
		Ema,    // 指数移動平均
		Smma,   // 平滑移動平均
		Lwma    // 加重移動平均
	}

	/// <summary>
	/// 移動平均インジケーターを表します。
	/// </summary>
	public class MovingAverageIndicator : IIndicator
	{
		/// <summary>
		/// 期間を設定または取得します。
		/// </summary>
		[Category("移動平均")]
		[DisplayName("期間")]
		[Description("対象期間を設定します。")]
		public int Period { get; set; } = 25;

		/// <summary>
		/// 計算方法を設定または取得します。
		/// </summary>
		[Category("移動平均")]
		[DisplayName("計算方法")]
		[Description("計算方法を設定します。")]
		public MaMethod MaMethod { get; set; } = MaMethod.Sma;

		/// <summary>
		/// 前回の移動平均
		/// </summary>
		[Browsable(false)]
		private double? PreviousMa = null;

		/// <summary>
		/// 非同期で準備します。
		/// </summary>
		/// <returns>非同期タスク</returns>
		public async Task SetUpAsync()
		{
			PreviousMa = null;
		}

		/// <summary>
		/// 値を取得します。
		/// </summary>
		/// <param name="candles">ロウソク足のコレクション</param>
		/// <returns>値</returns>
		public double[] GetValues(DataTypes.CandleCollection candles)
		{
			// 必要期間に満たない
			if (candles.Count < Period) return null;

			// 移動平均
			double ma = GetMovingAverage(candles.GetRange(0, Period).Select(candle => (double)candle.Close).ToArray(), MaMethod, PreviousMa);

			// 次回のために覚えておく
			PreviousMa = ma;

			return new double[] { ma };
		}

		/// <summary>
		/// 移動平均を取得します。
		/// </summary>
		/// <param name="data">対象データ</param>
		/// <param name="method">移動平均の計算方法</param>
		/// <param name="prev_ma">前回の移動平均</param>
		/// <returns>移動平均</returns>
		public static double GetMovingAverage(double[] data, MaMethod method, double? prev_ma)
		{
			switch (method)
			{
				case MaMethod.Sma:
					{
						return data.Average();
					}
				case MaMethod.Ema:
				case MaMethod.Smma:
					{
						// 係数
						double a = 0.0;
						if (method == MaMethod.Ema) a = 2.0 / (data.Length + 1);
						else a = 1.0 / data.Length;

						// 初回の移動平均
						if (prev_ma == null) prev_ma = data.Last();

						return a * data[0] + (1.0 - a) * prev_ma.Value;
					}
				case MaMethod.Lwma:
					{
						double sum1 = 0.0, sum2 = 0.0;
						for (int i = 0; i < data.Length; i++)
						{
							sum1 += (data.Length - i) * data[i];
							sum2 += i + 1;
						}
						return sum1 / sum2;
					}
			}
			return 0.0;
		}
	}

	/// <summary>
	/// 移動平均インジケーターのPropertyGrid用変換器を表します。
	/// </summary>
	public class MovingAverageIndicatorConverter : ExpandableObjectConverter
	{
		/// <summary>
		/// 変換器が型を変換できるかどうかを取得します。
		/// </summary>
		/// <param name="context">コンテキスト</param>
		/// <param name="destinationType">型</param>
		/// <returns>変換器が型を変換できるかどうか</returns>
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return false;
		}

		/// <summary>
		/// 変換されたオブジェクトを取得します。
		/// </summary>
		/// <param name="context">コンテキスト</param>
		/// <param name="culture">カルチャ</param>
		/// <param name="value">オブジェクト</param>
		/// <param name="destinationType">型</param>
		/// <returns>変換されたオブジェクト</returns>
		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			return null;
		}

		/// <summary>
		/// 変換器が型を復元できるかどうかを取得します。
		/// </summary>
		/// <param name="context">コンテキスト</param>
		/// <param name="sourceType">型</param>
		/// <returns>変換器が型を復元できるかどうか</returns>
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return false;
		}

		/// <summary>
		/// 復元されたオブジェクトを取得します。
		/// </summary>
		/// <param name="context">コンテキスト</param>
		/// <param name="culture">カルチャ</param>
		/// <param name="value">オブジェクト</param>
		/// <returns>復元されたオブジェクト</returns>
		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			return null;
		}
	}
}
