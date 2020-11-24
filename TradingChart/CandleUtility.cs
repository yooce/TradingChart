using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MagicalNuts
{
	public enum CandleTerm
	{
		Dayly = 0, Weekly, Monthly, Yearly
	}

	public static class CandleUtility
	{
		public static string GetPriceFormat(int digits)
		{
			string format = "0";
			for (int i = 0; i < digits; i++)
			{
				if (i == 0) format += ".";
				format += "0";
			}
			return format;
		}

		public static int? GetDigitsFromFormat(string format)
		{
			// 認識できないフォーマットの場合
			if (!System.Text.RegularExpressions.Regex.IsMatch(format, @"[0-9]+\.[0-9]+")) return null;

			string[] strs = format.Split('.');
			return strs[1].Length;
		}

		public static double GetCursorInterval(int digits)
		{
			double interval = 1;
			for (int i = 0; i < digits; i++)
			{
				interval /= 10;
			}
			return interval;
		}

		public static List<DataTypes.Candle> ConvertTermFromDaily(List<DataTypes.Candle> daily, CandleTerm term)
		{
			// ソート
			daily.Sort(DataTypes.Candle.Compare);

			// 日足ならそのまま返す
			if (term == CandleTerm.Dayly) return daily;

			// 器作成
			List<DataTypes.Candle> converteds = new List<DataTypes.Candle>();

			DateTime? prevDateTime = null;
			DataTypes.Candle converted = null;
			foreach (DataTypes.Candle candle in daily)
			{
				// ロウソク足切替
				if (IsCandleChange(candle, prevDateTime, term))
				{
					// ロウソク足追加
					if (converted != null) converteds.Add(converted);

					// 新規ロウソク足
					converted = new DataTypes.Candle();
					converted.DateTime = candle.DateTime;
					converted.Open = candle.Open;
					converted.High = candle.Open;
					converted.Low = candle.Open;
					converted.Close = candle.Open;
					converted.Volume = 0;
				}

				if (candle.High > converted.High) converted.High = candle.High;
				if (candle.Low < converted.Low) converted.Low = candle.Low;
				converted.Close = candle.Close;
				converted.Volume += candle.Volume;

				prevDateTime = candle.DateTime;
			}

			return converteds;
		}

		private static bool IsCandleChange(DataTypes.Candle candle, DateTime? prevDateTime, CandleTerm term)
		{
			// 前回日時が無い
			if (prevDateTime == null) return true;

			// 次のDateTime算出
			DateTime nextDateTime = DateTime.MinValue;
			switch (term)
			{
				case CandleTerm.Dayly:
					nextDateTime = prevDateTime.Value.Date.AddDays(1);
					break;
				case CandleTerm.Weekly:
					nextDateTime = prevDateTime.Value.Date.AddDays(1);
					while (nextDateTime.DayOfWeek != DayOfWeek.Monday)
					{
						nextDateTime = nextDateTime.AddDays(1);
					}
					break;
				case CandleTerm.Monthly:
					nextDateTime = new DateTime(prevDateTime.Value.Year, prevDateTime.Value.Month, 1).AddMonths(1);
					break;
				case CandleTerm.Yearly:
					nextDateTime = new DateTime(prevDateTime.Value.Year, 1, 1).AddYears(1);
					break;
			}

			// 次のDateTimeを超えているか
			return candle.DateTime >= nextDateTime;
		}
	}
}
