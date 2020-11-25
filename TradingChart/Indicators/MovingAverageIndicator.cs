using MagicalNuts.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicalNuts.Indicators
{
	public enum MaMethod
	{
		Sma, Ema, Smma, Lwma
	}

	public class MovingAverageIndicator : IIndicator
	{
		public int Period { get; set; } = 25;
		public MaMethod MaMethod { get; set; } = MaMethod.Sma;

		public double[] GetData(IndicatorArgs args)
		{
			// 必要期間に満たない
			if (args.Candles.Count < Period) return null;

			// 初日の移動平均取得
			int fast_period = Period;
			if (args.Candles.Count - (Period - 1) < Period) fast_period = args.Candles.Count - (Period - 1);
			double fast_ma = GetMovingAverage(args.Candles.GetRange(Period - 1, fast_period).Select(candle => (double)candle.Close).ToArray(), MaMethod.Sma, null);

			return new double[] { GetMovingAverage(args.Candles.GetRange(0, Period).Select(candle => (double)candle.Close).ToArray(), MaMethod, fast_ma) };
		}

		public static double GetMovingAverage(double[] data, MaMethod method, double? fast_ma)
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
						double a = 0.0;
						if (method == MaMethod.Ema) a = 2.0 / (data.Length + 1);
						else a = 1.0 / data.Length;
						// double ma = data.Average();
						// double ma = data.Last();
						double ma = fast_ma.Value;
						for (int i = data.Length - 2; i >= 0; i--)
						{
							ma = a * data[i] + (1.0 - a) * ma;
						}
						return ma;
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
}
