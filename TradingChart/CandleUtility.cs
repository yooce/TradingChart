﻿using System;
using System.Collections.Generic;

namespace MagicalNuts
{
	/// <summary>
	/// ロウソク足の期間を表します。
	/// </summary>
	public enum CandlePeriod
	{
		Dayly = 0, Weekly, Monthly, Yearly
	}

	/// <summary>
	/// ロウソク足のユーティリティを表します。
	/// </summary>
	public static class CandleUtility
	{
		/// <summary>
		/// 価格表示のフォーマットを取得します。
		/// </summary>
		/// <param name="digits">小数点以下の桁数</param>
		/// <returns>価格表示のフォーマット</returns>
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

		/// <summary>
		/// 価格表示のフォーマットから小数点以下の桁数を取得します。
		/// </summary>
		/// <param name="format">価格表示のフォーマット</param>
		/// <returns>小数点以下の桁数</returns>
		public static int? GetDigitsFromFormat(string format)
		{
			// 認識できないフォーマットの場合
			if (!System.Text.RegularExpressions.Regex.IsMatch(format, @"[0-9]+\.[0-9]+")) return null;

			string[] strs = format.Split('.');
			return strs[1].Length;
		}

		/// <summary>
		/// カーソルのインターバルを取得します。
		/// </summary>
		/// <param name="digits">小数点以下の桁数</param>
		/// <returns>カーソルのインターバル</returns>
		public static double GetCursorInterval(int digits)
		{
			double interval = 1;
			for (int i = 0; i < digits; i++)
			{
				interval /= 10;
			}
			return interval;
		}

		/// <summary>
		/// ロウソク足を日足から目的の足に変換します。
		/// </summary>
		/// <param name="daily">日足</param>
		/// <param name="period">ロウソク足の期間</param>
		/// <returns>目的の足のロウソク足のリスト</returns>
		public static List<DataTypes.Candle> ConvertPeriodFromDaily(List<DataTypes.Candle> daily, CandlePeriod period)
		{
			// ソート
			daily.Sort(DataTypes.Candle.Compare);

			// 日足ならそのまま返す
			if (period == CandlePeriod.Dayly) return daily;

			// 器作成
			List<DataTypes.Candle> converteds = new List<DataTypes.Candle>();

			DateTime? prevDateTime = null;
			DataTypes.Candle converted = null;
			foreach (DataTypes.Candle candle in daily)
			{
				// 次のロウソク足へ
				if (IsCandleChange(candle, prevDateTime, period))
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

				// 値設定
				if (candle.High > converted.High) converted.High = candle.High;
				if (candle.Low < converted.Low) converted.Low = candle.Low;
				converted.Close = candle.Close;
				converted.Volume += candle.Volume;

				// 日時を覚えておく
				prevDateTime = candle.DateTime;
			}

			return converteds;
		}

		/// <summary>
		/// ロウソク足の切り替えが必要かどうか判定します。
		/// </summary>
		/// <param name="candle">ロウソク足</param>
		/// <param name="prevDateTime">前回のロウソク足の日時</param>
		/// <param name="period">ロウソク足の期間</param>
		/// <returns>ロウソク足の切り替えが必要かどうか</returns>
		private static bool IsCandleChange(DataTypes.Candle candle, DateTime? prevDateTime, CandlePeriod period)
		{
			// 前回日時が無い
			if (prevDateTime == null) return true;

			// 次のDateTime算出
			DateTime nextDateTime = DateTime.MinValue;
			switch (period)
			{
				case CandlePeriod.Dayly:
					nextDateTime = prevDateTime.Value.Date.AddDays(1);
					break;
				case CandlePeriod.Weekly:
					nextDateTime = prevDateTime.Value.Date.AddDays(1);
					while (nextDateTime.DayOfWeek != DayOfWeek.Monday)
					{
						nextDateTime = nextDateTime.AddDays(1);
					}
					break;
				case CandlePeriod.Monthly:
					nextDateTime = new DateTime(prevDateTime.Value.Year, prevDateTime.Value.Month, 1).AddMonths(1);
					break;
				case CandlePeriod.Yearly:
					nextDateTime = new DateTime(prevDateTime.Value.Year, 1, 1).AddYears(1);
					break;
			}

			// 次のDateTimeを超えているか
			return candle.DateTime >= nextDateTime;
		}
	}
}
