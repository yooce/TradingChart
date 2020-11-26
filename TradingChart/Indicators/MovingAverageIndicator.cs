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

		private double? PreviousMa = null;

		public double[] GetData(IndicatorArgs args)
		{
			// 必要期間に満たない
			if (args.Candles.Count < Period) return null;

			// 移動平均
			double ma = GetMovingAverage(args.Candles.GetRange(0, Period).Select(candle => (double)candle.Close).ToArray(), MaMethod, PreviousMa);

			// 次回のために覚えておく
			PreviousMa = ma;

			return new double[] { ma };
		}

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
}
