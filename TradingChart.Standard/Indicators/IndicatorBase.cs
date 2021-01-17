using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagicalNuts.Indicators
{
	/// <summary>
	/// 価格の種類を表します。
	/// </summary>
	public enum PriceType
	{
		Open = 0, High, Low, Close
	}

	/// <summary>
	/// インジケーターのインターフェースを表します。
	/// </summary>
	public abstract class IndicatorBase
	{
		/// <summary>
		/// 直近の添え字が０、古いほど添え字が大きいロウソク足のリスト
		/// </summary>
		public List<DataTypes.Candle> Candles { get; set; }

		protected DateTime DateTime(int i)
		{
			return Candles[i].DateTime;
		}

		protected double Open(int i)
		{
			return (double)Candles[i].Open;
		}

		protected double High(int i)
		{
			return (double)Candles[i].High;
		}

		protected double Low(int i)
		{
			return (double)Candles[i].Low;
		}

		protected double Close(int i)
		{
			return (double)Candles[i].Close;
		}

		protected long Volume(int i)
		{
			return Candles[i].Volume;
		}

		protected double TradingValue(int i)
		{
			return (double)Candles[i].TradingValue;
		}

		/// <summary>
		/// 価格を取得します。
		/// </summary>
		/// <param name="pt">価格の種類</param>
		/// <returns>価格</returns>
		protected double Price(PriceType pt, int i)
		{
			double price = 0.0;
			switch (pt)
			{
				case PriceType.Open:
					price = Open(i);
					break;
				case PriceType.High:
					price = High(i);
					break;
				case PriceType.Low:
					price = Low(i);
					break;
				case PriceType.Close:
					price = Close(i);
					break;
			}
			return price;
		}

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
		public abstract double[] GetValues();
	}
}
