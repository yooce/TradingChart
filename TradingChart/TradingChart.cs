using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml.Schema;

namespace MagicalNuts
{
	public class TradingChart : System.Windows.Forms.DataVisualization.Charting.Chart
	{
		public CandleTerm CandleTerm
		{
			get => _CandleTerm;
			set
			{
				_CandleTerm = value;
				if (DailyCandles != null) SetCandles();
			}
		}

		public int ScreenCandleNum
		{
			set
			{
				MainChartArea.AxisX.ScaleView.Size = value;
				UpdateChartSettings();
			}
		}

		private CandleTerm _CandleTerm = CandleTerm.Dayly;
		private MainChartArea MainChartArea = null;
		private List<SubChartArea> SubChartAreas = null;
		private double PreviousX = double.NaN;
		private List<Plotters.IPlotter> Plotters = null;
		private HorizontalLineAnnotation MovingSplitter = null;
		private bool IsScrolling = false;

		// 銘柄依存
		private List<DataTypes.Candle> DailyCandles = null;
		private List<DataTypes.Candle> Candles = null;
		private string PriceFormat = null;

		public TradingChart() : base()
		{
			SubChartAreas = new List<SubChartArea>();
			Plotters = new List<Plotters.IPlotter>();
		}

		public void SetUp()
		{
			// Chart
			ChartAreas.Clear();

			MouseWheel += new MouseEventHandler(chart_MouseWheel);
			MouseMove += new MouseEventHandler(chart_MouseMove);
			MouseDown += new MouseEventHandler(chart_MouseDown);
			MouseUp += new MouseEventHandler(chart_MouseUp);
			AnnotationPositionChanging += new EventHandler<AnnotationPositionChangingEventArgs>(chart_AnnotationPositionChanging);
			AxisScrollBarClicked += new EventHandler<ScrollBarEventArgs>(chart_AxisScrollBarClicked);

			// MainChartArea
			MainChartArea = new MainChartArea();
			ChartAreas.Add(MainChartArea);
			MainChartArea.SetUp(this);

			// Plotter
			AddPlotter(new Plotters.CandleProtter());
			AddPlotter(new Plotters.VolumePlotter());
		}

		public void SetDailyCandles<T>(List<T> candles, int digits, CandleTerm term) where T : DataTypes.Candle
		{
			// 日足設定
			DailyCandles = new List<DataTypes.Candle>();
			DailyCandles.AddRange(candles);

			// フォーマット取得
			PriceFormat = CandleUtility.GetPriceFormat(digits);

			// カーソルインターバル
			MainChartArea.CursorY.Interval = CandleUtility.GetCursorInterval(digits);

			// 期間変換
			CandleTerm = term;
		}

		private void SetCandles()
		{
			// クリア
			Series.Clear();
			MainChartArea.AxisX.CustomLabels.Clear();

			// 期間変換
			Candles = CandleUtility.ConvertTermFromDaily(DailyCandles, _CandleTerm);

			// MainChartArea
			MainChartArea.SetCandles(Candles);

			// プロット
			foreach (Plotters.IPlotter plotter in Plotters)
			{
				plotter.Plot(Candles);
			}

			// CustomLabel
			DateTime? prevLabelDateTime = null;
			for (int x = 0; x < Candles.Count; x++)
			{
				if (IsNeedCustomLabel(Candles[x], prevLabelDateTime))
				{
					// MainChartArea
					MainChartArea.AxisX.CustomLabels.Add(new CustomLabel(x - 50.0, x + 50.0, GetCustomeLabelName(prevLabelDateTime
						, Candles[x].DateTime), 0, LabelMarkStyle.None, GridTickTypes.Gridline));

					// SubChartArea
					foreach (SubChartArea subChartArea in SubChartAreas)
					{
						subChartArea.AxisX.CustomLabels.Add(
							new CustomLabel(x - 50.0, x + 50.0, "", 0, LabelMarkStyle.None, GridTickTypes.Gridline));
					}

					prevLabelDateTime = Candles[x].DateTime;
				}
			}

			// Series追加
			foreach (Plotters.IPlotter plotter in Plotters)
			{
				foreach (Series series in plotter.GetSeries())
				{
					Series.Add(series);
				}
			}

			// 初期位置
			MainChartArea.AxisX.ScaleView.Position = Candles.Count - MainChartArea.AxisX.ScaleView.Size;

			UpdateChartSettings();
		}

		private bool IsNeedCustomLabel(DataTypes.Candle candle, DateTime? prevDateTime)
		{
			// 前回日時が無い
			if (prevDateTime == null) return true;

			// 次のDateTime算出
			DateTime nextDateTime = DateTime.MinValue;
			switch (_CandleTerm)
			{
				case CandleTerm.Dayly:
					nextDateTime = new DateTime(prevDateTime.Value.Year, prevDateTime.Value.Month, 1).AddMonths(1);
					break;
				case CandleTerm.Weekly:
					nextDateTime = new DateTime(prevDateTime.Value.Year, (prevDateTime.Value.Month - 1) / 3 * 3 + 1, 1).AddMonths(3);
					break;
				case CandleTerm.Monthly:
					nextDateTime = new DateTime(prevDateTime.Value.Year, 1, 1).AddYears(1);
					break;
				case CandleTerm.Yearly:
					nextDateTime = new DateTime(prevDateTime.Value.Year / 10 * 10, 1, 1).AddYears(10);
					break;
			}

			// 次のDateTimeを超えているか
			return candle.DateTime >= nextDateTime;
		}

		private string GetCustomeLabelName(DateTime? prev, DateTime next)
		{
			switch (_CandleTerm)
			{
				case CandleTerm.Dayly:
				case CandleTerm.Weekly:
					if (prev == null || prev.Value.Year != next.Year) return next.ToString("yyyy");
					return next.ToString("M月");
				case CandleTerm.Monthly:
				case CandleTerm.Yearly:
					return next.ToString("yyyy");
			}
			return "";
		}

		private void UpdateChartSettings()
		{
			if (double.IsNaN(MainChartArea.AxisX.ScaleView.Position)) return;

			// 開始位置決定
			int candle_start = (int)MainChartArea.AxisX.ScaleView.Position;
			if (candle_start < 0) candle_start = 0;
			else if (candle_start > Candles.Count - MainChartArea.AxisX.ScaleView.Size) candle_start = Candles.Count - (int)MainChartArea.AxisX.ScaleView.Size;

			// 終了位置決定
			int candle_end = candle_start + (int)MainChartArea.AxisX.ScaleView.Size;

			// 最高値、最安値を取得
			List<decimal> highs = new List<decimal>();
			List<decimal> lows = new List<decimal>();
			for (int i = candle_start; i < candle_end; i++)
			{
				highs.Add(Candles[i].High);
				lows.Add(Candles[i].Low);
			}
			double max = (double)highs.Max();
			double min = (double)lows.Min();

			// 範囲決定
			MainChartArea.AxisY2.ScaleView.Size = (max - min) * (12.0 / 8.0);

			// 位置決定
			MainChartArea.AxisY2.ScaleView.Position = min - (MainChartArea.AxisY2.ScaleView.Size / 4.0);

			// 出来高
			List<long> volumes = new List<long>();
			for (int i = candle_start; i < candle_end; i++)
			{
				volumes.Add(Candles[i].Volume);
			}

			long max2 = volumes.Max();
			long top2 = max2 * 4;
			MainChartArea.AxisY.ScaleView.Size = top2;
		}

		private void chart_MouseMove(object sender, MouseEventArgs e)
		{
			if (MovingSplitter != null)
			{
				// 分割線の操作

				// 配置
				for (int i = 0; i < SubChartAreas.Count; i++)
				{
					if (SubChartAreas[i].Splitter == MovingSplitter)
					{
						if (i == 0)
						{
							// 前がMainChartArea
							float bottom = SubChartAreas[i].Position.Y + SubChartAreas[i].Position.Height;
							MainChartArea.Position.Height = (float)MovingSplitter.Y;
							SubChartAreas[i].Position.Height = (float)(bottom - MovingSplitter.Y);
							SubChartAreas[i].Position.Y = (float)MovingSplitter.Y;
						}
						else
						{
							// 前がSubChartArea
							float bottom = SubChartAreas[i].Position.Y + SubChartAreas[i].Position.Height;
							SubChartAreas[i - 1].Position.Height = (float)MovingSplitter.Y - SubChartAreas[i - 1].Position.Y;
							SubChartAreas[i].Position.Height = (float)(bottom - MovingSplitter.Y);
							SubChartAreas[i].Position.Y = (float)MovingSplitter.Y;
						}
						break;
					}
				}

				// 再描画
				Update();
			}
			else if (IsScrolling)
			{
				// スクロールバーの操作

				UpdateChartSettings();
			}
			else
			{
				// ロウソク足が無かったら何もしない
				if (Candles == null) return;

				// マウス座標取得
				Point mouse = e.Location;

				// プロットエリアのヒットテスト
				HitTestResult[] results = HitTest(mouse.X, mouse.Y, false, ChartElementType.PlottingArea);
				foreach (var result in results)
				{
					if (result.ChartElementType != ChartElementType.PlottingArea || result.ChartArea == null) continue;

					// グラフ上の位置取得
					int x = (int)(MainChartArea.AxisX.PixelPositionToValue(mouse.X) + 0.5);
					if (x < 0 || x >= Candles.Count) continue;

					// カーソル
					MainChartArea.Update(mouse, result, PriceFormat);
					foreach (SubChartArea subChartArea in SubChartAreas)
					{
						subChartArea.Update(mouse, result, PriceFormat);
					}
				}

				// スクロール
				if (e.Button.HasFlag(MouseButtons.Left))
				{
					MainChartArea.AxisX.ScaleView.Position -= (MainChartArea.AxisX.PixelPositionToValue(mouse.X) - PreviousX);
					UpdateChartSettings();
				}
			}
		}

		private void chart_MouseDown(object sender, MouseEventArgs e)
		{
			PreviousX = MainChartArea.AxisX.PixelPositionToValue(e.Location.X);
		}

		private void chart_AxisScrollBarClicked(object sender, ScrollBarEventArgs e)
		{
			IsScrolling = true;
		}

		private void chart_MouseUp(object sender, MouseEventArgs e)
		{
			MovingSplitter = null;
			IsScrolling = false;
			UpdateChartSettings();
		}

		private void chart_MouseWheel(object sender, MouseEventArgs e)
		{
			MainChartArea.AxisX.ScaleView.Position += e.Delta / 120 * 60;
			UpdateChartSettings();
		}

		private void chart_AnnotationPositionChanging(object sender, AnnotationPositionChangingEventArgs e)
		{
			e.NewLocationX = 0;
			MovingSplitter = sender as HorizontalLineAnnotation;
		}

		public void AddPlotter(Plotters.IPlotter plotter)
		{
			// ChartArea設定
			SubChartArea[] subChartAreas = plotter.SetChartArea(MainChartArea);

			// SubChartAreaを使う場合
			if (subChartAreas != null)
			{
				foreach (SubChartArea subChartArea in subChartAreas)
				{
					AddSubChartArea(subChartArea);
				}
			}

			Plotters.Add(plotter);
		}

		private void AddSubChartArea(SubChartArea subChartArea)
		{
			// セットアップ
			subChartArea.SetUp(this);

			// MainChartAreaと連動
			subChartArea.AlignWithChartArea = MainChartArea.Name;
			subChartArea.AlignmentStyle
				= AreaAlignmentStyles.Position | AreaAlignmentStyles.PlotPosition | AreaAlignmentStyles.Cursor | AreaAlignmentStyles.AxesView;

			// 配置
			if (SubChartAreas.Count == 0)
			{
				// 前がMainChartArea
				subChartArea.Splitter.Y = 75;
				MainChartArea.Position.Height = (float)subChartArea.Splitter.Y;
			}
			else
			{
				// 前がSubChartArea
				SubChartArea prevSubChartArea = SubChartAreas.Last();
				subChartArea.Splitter.Y = prevSubChartArea.Splitter.Y + prevSubChartArea.Position.Height / 2;
				prevSubChartArea.Position.Height = prevSubChartArea.Position.Height / 2;
			}
			subChartArea.Position.X = 0;
			subChartArea.Position.Y = (float)subChartArea.Splitter.Y;
			subChartArea.Position.Width = 100;
			subChartArea.Position.Height = 100 - (float)subChartArea.Splitter.Y;

			// 追加
			Annotations.Add(subChartArea.Splitter);
			SubChartAreas.Add(subChartArea);
			ChartAreas.Add(subChartArea);
		}
	}
}
