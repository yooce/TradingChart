using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagicalNuts.Indicators
{
	/// <summary>
	/// インジケーターのインターフェースを表します。
	/// </summary>
	public abstract class IndicatorBase
	{
		/// <summary>
		/// 非同期で準備します。
		/// </summary>
		/// <returns>非同期タスク</returns>
		public abstract Task SetUpAsync();

		/// <summary>
		/// 値を取得します。
		/// </summary>
		/// <param name="args">インジケーター引数</param>
		/// <returns>値</returns>
		public abstract double[] GetValues(DataTypes.CandleCollection candles);
	}
}
