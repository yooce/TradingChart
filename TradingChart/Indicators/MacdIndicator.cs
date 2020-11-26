using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicalNuts.Indicators
{
	public class MacdIndicator : IIndicator
	{
		public int FastPeriod { get; set; } = 12;
		public int SlowPeriod { get; set; } = 26;
		public int SignalPeriod { get; set; } = 9;

		private MovingAverageIndicator FastMaIndicator = null;
		private MovingAverageIndicator SlowMaIndicator = null;

		private Queue<double> MacdQueue = null;
		private double? PreviousSignal = null;

		public MacdIndicator()
		{
			FastMaIndicator = new MovingAverageIndicator();
			FastMaIndicator.Period = FastPeriod;
			FastMaIndicator.MaMethod = MaMethod.Ema;
			SlowMaIndicator = new MovingAverageIndicator();
			SlowMaIndicator.Period = SlowPeriod;
			SlowMaIndicator.MaMethod = MaMethod.Ema;
		}

		public double[] GetData(IndicatorArgs args)
		{
			// 必要期間に満たない
			if (args.Candles.Count < FastPeriod || args.Candles.Count < SlowPeriod) return null;

			// キュー作成
			if (MacdQueue == null) MacdQueue = new Queue<double>();

			// 移動平均
			double fast_ma = FastMaIndicator.GetData(new IndicatorArgs(args.Candles))[0];
			double slow_ma = SlowMaIndicator.GetData(new IndicatorArgs(args.Candles))[0];

			// MACD
			double macd = fast_ma - slow_ma;

			// キューに格納
			MacdQueue.Enqueue(macd);
			if (MacdQueue.Count > SignalPeriod) MacdQueue.Dequeue();

			// 必要期間に満たない
			if (MacdQueue.Count < SignalPeriod) return null;

			// シグナル
			double signal = MovingAverageIndicator.GetMovingAverage(MacdQueue.ToArray(), MaMethod.Sma, PreviousSignal);

			// 次回のために覚えておく
			PreviousSignal = signal;

			return new double[] { macd, signal };
		}
	}
}
