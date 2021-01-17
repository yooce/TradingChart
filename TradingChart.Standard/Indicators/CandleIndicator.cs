using System.Threading.Tasks;

namespace MagicalNuts.Indicators
{
	/// <summary>
	/// ロウソク足インジケーターを表します。
	/// </summary>
	public class CandleIndicator : IndicatorBase
	{
		/// <summary>
		/// 非同期で準備します。
		/// </summary>
		/// <returns>非同期タスク</returns>
		public override async Task SetUpAsync() { }

		/// <summary>
		/// 値を取得します。
		/// </summary>
		/// <param name="args">インジケーター引数</param>
		/// <returns>値</returns>
		public override double[] GetValues()
		{
			return new double[4] { High(0), Low(0), Open(0), Close(0) };
		}
	}
}
