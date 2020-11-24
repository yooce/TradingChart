# TradingChart

![ScreenShot](https://github.com/yooce/TradingChart/blob/main/screenshot.png)

トレード用チャートコントロールです。デザインは、ユーザビリティに定評のある[TradingView](https://jp.tradingview.com/)を参考にしています。

# Requirement

.NET Framework 4.6.1

# Basic Usage

## ロウソク足チャートの表示

TradingChartは株価データを持ちませんので、まずは何らかの手段で日足データを取得し、下記`Candle`クラスのインスタンス配列を作成してください。

```C#
public class Candle
{
    public string Code { get; set; }
    public DateTime DateTime { get; set; }
    public decimal Open { get; set; }
    public decimal High { get; set; }
    public decimal Low { get; set; }
    public decimal Close { get; set; }
    public long Volume { get; set; }
}
```

TradingChartコントロールを'Form'に配置し、`SetUp`を呼んだ後、`SetDailyCandles`で日足データを設定すれば、ロウソク足チャートが表示されます。

```C#
// DataTypes.Candle[] candles = ...
tradingChart1.SetUp();
tradingChart1.SetDailyCandles(candles, 2, CandleTerm.Dayly);
```

## ロウソク足の期間変更

`CandleTerm`を設定するだけで、ロウソク足の期間を変更できます。

```C#
// 週足
tradingChart1.CandleTerm = CandleTerm.Weekly;

// 月足
tradingChart1.CandleTerm = CandleTerm.Monthly;

// 年足
tradingChart1.CandleTerm = CandleTerm.Yearly;
```

## 画面あたりの足数の変更

`ScreenCandleNum`を変更すると、画面あたりの足数を変えることができます。これを利用してチャートのズームイン、アウト機能を実装可能です。

```C#
tradingChart1.ScreenCandleNum = 250;
```

# Author

yooce

* Blog : [https://yooce.hatenablog.com/](https://yooce.hatenablog.com/)

# License

[MIT license](https://en.wikipedia.org/wiki/MIT_License).
