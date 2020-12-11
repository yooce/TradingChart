using System.Threading.Tasks;

namespace MagicalNuts.Indicators
{
	/// <summary>
	/// インジケーターのインターフェースを表します。
	/// </summary>
	public interface IIndicator
	{
		/// <summary>
		/// 非同期で準備します。
		/// </summary>
		/// <returns>非同期タスク</returns>
		Task SetUpAsync();

		/// <summary>
		/// 値を取得します。
		/// </summary>
		/// <param name="args">インジケーター引数</param>
		/// <returns>値</returns>
		double[] GetValues(IndicatorArgs args);
	}
}
