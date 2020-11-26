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
	/// <summary>
	/// トレード用チャートコントロールを表します。
	/// </summary>
	public class TradingChart : System.Windows.Forms.DataVisualization.Charting.Chart
	{
		/// <summary>
		/// ロウソク足の期間を取得または設定します。
		/// </summary>
		public CandlePeriod CandlePeriod
		{
			get => _CandlePeriod;
			set
			{
				_CandlePeriod = value;
				if (DailyCandles != null) SetCandles();
			}
		}

		/// <summary>
		/// 画面あたりの足数を設定します。
		/// </summary>
		public int ScreenCandleNum
		{
			set
			{
				MainChartArea.AxisX.ScaleView.Size = value;
				UpdateYSettings();
			}
		}

		/// <summary>
		/// ロウソク足の期間
		/// </summary>
		private CandlePeriod _CandlePeriod = CandlePeriod.Dayly;

		/// <summary>
		/// 主ChartArea
		/// </summary>
		private MainChartArea MainChartArea = null;

		/// <summary>
		/// 従ChartAreaのリスト
		/// </summary>
		private List<SubChartArea> SubChartAreas = null;

		/// <summary>
		/// 前回のx座標
		/// </summary>
		private double PreviousX = double.NaN;

		/// <summary>
		/// プロッターのリスト
		/// </summary>
		private List<Plotters.IPlotter> Plotters = null;

		/// <summary>
		/// 分割線のリスト
		/// </summary>
		private HorizontalLineAnnotation MovingSplitter = null;

		/// <summary>
		/// スクロール中かどうかを示します。
		/// </summary>
		private bool IsScrolling = false;

		/// <summary>
		/// 日足のリスト
		/// </summary>
		private List<DataTypes.Candle> DailyCandles = null;

		/// <summary>
		/// 表示中のロウソク足のリスト
		/// </summary>
		private List<DataTypes.Candle> Candles = null;

		/// <summary>
		/// 価格表示フォーマット
		/// </summary>
		private string PriceFormat = null;

		/// <summary>
		/// TradingChartクラスの新しいインスタンスを初期化します。
		/// </summary>
		public TradingChart() : base()
		{
			SubChartAreas = new List<SubChartArea>();
			Plotters = new List<Plotters.IPlotter>();
		}

		/// <summary>
		/// チャートを準備します。
		/// </summary>
		public void SetUp()
		{
			// Chart
			ChartAreas.Clear();

			// Chartイベント
			MouseWheel += new MouseEventHandler(chart_MouseWheel);
			MouseMove += new MouseEventHandler(chart_MouseMove);
			MouseDown += new MouseEventHandler(chart_MouseDown);
			MouseUp += new MouseEventHandler(chart_MouseUp);
			AnnotationPositionChanging += new EventHandler<AnnotationPositionChangingEventArgs>(chart_AnnotationPositionChanging);
			AxisScrollBarClicked += new EventHandler<ScrollBarEventArgs>(chart_AxisScrollBarClicked);

			// 主ChartArea
			MainChartArea = new MainChartArea();
			ChartAreas.Add(MainChartArea);
			MainChartArea.SetUp(this);

			// デフォルトプロッター
			AddPlotter(new Plotters.CandleProtter());
			AddPlotter(new Plotters.VolumePlotter());
		}

		/// <summary>
		/// 日足を設定します。
		/// </summary>
		/// <typeparam name="T">ロウソク足の型を指定します。</typeparam>
		/// <param name="candles">ロウソク足の配列</param>
		/// <param name="digits">小数点以下の桁数</param>
		/// <param name="period">表示するロウソク足の期間</param>
		public void SetDailyCandles<T>(T[] candles, int digits, CandlePeriod period) where T : DataTypes.Candle
		{
			// 日足設定
			DailyCandles = new List<DataTypes.Candle>();
			DailyCandles.AddRange(candles);

			// 価格表示フォーマット取得
			PriceFormat = CandleUtility.GetPriceFormat(digits);

			// カーソルインターバル
			MainChartArea.CursorY.Interval = CandleUtility.GetCursorInterval(digits);

			// 期間変換
			CandlePeriod = period;
		}

		/// <summary>
		/// ロウソク足を設定します。
		/// </summary>
		private void SetCandles()
		{
			// クリア
			Series.Clear();
			MainChartArea.Clear();
			foreach (SubChartArea subChartArea in SubChartAreas)
			{
				subChartArea.Clear();
			}

			// 期間変換
			Candles = CandleUtility.ConvertPeriodFromDaily(DailyCandles, _CandlePeriod);

			// 主ChartArea
			MainChartArea.SetCandles(Candles, CandleUtility.GetDigitsFromFormat(PriceFormat).Value);

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
					// 主ChartArea
					MainChartArea.AxisX.CustomLabels.Add(new CustomLabel(x - 50.0, x + 50.0, GetCustomeLabelName(prevLabelDateTime
						, Candles[x].DateTime), 0, LabelMarkStyle.None, GridTickTypes.Gridline));

					// 従ChartArea
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
				foreach (Series series in plotter.SeriesArray)
				{
					Series.Add(series);
				}
			}

			// 初期位置
			MainChartArea.AxisX.ScaleView.Position = Candles.Count - MainChartArea.AxisX.ScaleView.Size;

			// Y軸設定更新
			UpdateYSettings();
		}

		/// <summary>
		/// CustomLabelが必要かどうかを判定します。
		/// </summary>
		/// <param name="candle">ロウソク足</param>
		/// <param name="prevDateTime">前回CustomLabelを表示した日時</param>
		/// <returns>CustomLabelが必要かどうか</returns>
		private bool IsNeedCustomLabel(DataTypes.Candle candle, DateTime? prevDateTime)
		{
			// 前回日時が無い
			if (prevDateTime == null) return true;

			// 次のDateTime算出
			DateTime nextDateTime = DateTime.MinValue;
			switch (_CandlePeriod)
			{
				case CandlePeriod.Dayly:
					nextDateTime = new DateTime(prevDateTime.Value.Year, prevDateTime.Value.Month, 1).AddMonths(1);
					break;
				case CandlePeriod.Weekly:
					nextDateTime = new DateTime(prevDateTime.Value.Year, (prevDateTime.Value.Month - 1) / 3 * 3 + 1, 1).AddMonths(3);
					break;
				case CandlePeriod.Monthly:
					nextDateTime = new DateTime(prevDateTime.Value.Year, 1, 1).AddYears(1);
					break;
				case CandlePeriod.Yearly:
					nextDateTime = new DateTime(prevDateTime.Value.Year / 10 * 10, 1, 1).AddYears(10);
					break;
			}

			// 次のDateTimeを超えているか
			return candle.DateTime >= nextDateTime;
		}

		/// <summary>
		/// CustomeLabelの名前を取得します。
		/// </summary>
		/// <param name="prev">前回の日時</param>
		/// <param name="next">今回の日時</param>
		/// <returns></returns>
		private string GetCustomeLabelName(DateTime? prev, DateTime next)
		{
			switch (_CandlePeriod)
			{
				case CandlePeriod.Dayly:
				case CandlePeriod.Weekly:
					if (prev == null || prev.Value.Year != next.Year) return next.ToString("yyyy");
					return next.ToString("M月");
				case CandlePeriod.Monthly:
				case CandlePeriod.Yearly:
					return next.ToString("yyyy");
			}
			return "";
		}

		/// <summary>
		/// Y軸設定を更新します。
		/// </summary>
		private void UpdateYSettings()
		{
			// ウィンドウサイズ変更時にPositionがNaNの場合は何もしない
			if (double.IsNaN(MainChartArea.AxisX.ScaleView.Position)) return;

			// 開始位置決定
			int start = (int)MainChartArea.AxisX.ScaleView.Position;
			if (start < 0) start = 0;
			else if (start > Candles.Count - MainChartArea.AxisX.ScaleView.Size) start = Candles.Count - (int)MainChartArea.AxisX.ScaleView.Size;

			// 終了位置決定
			int end = start + (int)MainChartArea.AxisX.ScaleView.Size;

			// 主ChartArea
			MainChartArea.UpdateYSettings(start, end, Plotters);

			// 従ChartArea
			foreach (SubChartArea subChartArea in SubChartAreas)
			{
				subChartArea.UpdateYSettings(start, end, Plotters);
			}
		}

		/// <summary>
		/// プロッターを追加します。
		/// </summary>
		/// <param name="plotter">プロッター</param>
		public void AddPlotter(Plotters.IPlotter plotter)
		{
			// ChartArea設定
			SubChartArea[] subChartAreas = plotter.SetChartArea(MainChartArea);

			// 従ChartAreaを使う場合
			if (subChartAreas != null)
			{
				foreach (SubChartArea subChartArea in subChartAreas)
				{
					AddSubChartArea(subChartArea);
				}
			}

			// 追加
			Plotters.Add(plotter);
		}

		/// <summary>
		/// 従ChartAreaを追加します。
		/// </summary>
		/// <param name="subChartArea">従ChartArea</param>
		private void AddSubChartArea(SubChartArea subChartArea)
		{
			// 準備
			subChartArea.SetUp(this);

			// 主ChartAreaとの連動設定
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

		#region イベントハンドラ

		/// <summary>
		/// MouseMoveイベントを処理します。
		/// </summary>
		/// <param name="sender">イベント発行主体</param>
		/// <param name="e">イベント引数</param>
		private void chart_MouseMove(object sender, MouseEventArgs e)
		{
			if (MovingSplitter != null)
			{
				// 分割線の操作

				// ChartArea配置
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

				// Y軸設定更新
				UpdateYSettings();
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
					// プロットエリアでなければスキップ
					if (result.ChartElementType != ChartElementType.PlottingArea || result.ChartArea == null) continue;

					// グラフ上の位置取得
					int x = (int)(MainChartArea.AxisX.PixelPositionToValue(mouse.X) + 0.5);

					// ロウソク足の範囲外ならスキップ
					if (x < 0 || x >= Candles.Count) continue;

					// カーソル更新
					MainChartArea.UpdateCursors(mouse, result, x, Candles.Count - 1, PriceFormat);
					foreach (SubChartArea subChartArea in SubChartAreas)
					{
						subChartArea.UpdateCursors(mouse, result, x, Candles.Count - 1, PriceFormat);
					}
				}

				// スクロール
				if (e.Button.HasFlag(MouseButtons.Left))
				{
					// マウス移動分だけチャートも移動
					MainChartArea.AxisX.ScaleView.Position -= (MainChartArea.AxisX.PixelPositionToValue(mouse.X) - PreviousX);

					// Y軸設定更新
					UpdateYSettings();
				}
			}
		}

		/// <summary>
		/// MouseDownイベントを処理します。
		/// </summary>
		/// <param name="sender">イベント発行主体</param>
		/// <param name="e">イベント引数</param>
		private void chart_MouseDown(object sender, MouseEventArgs e)
		{
			// x座標を覚えておく
			PreviousX = MainChartArea.AxisX.PixelPositionToValue(e.Location.X);
		}

		/// <summary>
		/// AxisScrollBarClickedイベントを処理します。
		/// </summary>
		/// <param name="sender">イベント発行主体</param>
		/// <param name="e">イベント引数</param>
		private void chart_AxisScrollBarClicked(object sender, ScrollBarEventArgs e)
		{
			IsScrolling = true;
		}

		/// <summary>
		/// MouseUpイベントを処理します。
		/// </summary>
		/// <param name="sender">イベント発行主体</param>
		/// <param name="e">イベント引数</param>
		private void chart_MouseUp(object sender, MouseEventArgs e)
		{
			MovingSplitter = null;
			IsScrolling = false;

			// Y軸設定更新
			UpdateYSettings();
		}

		/// <summary>
		/// MouseWheelイベントを処理します。
		/// </summary>
		/// <param name="sender">イベント発行主体</param>
		/// <param name="e">イベント引数</param>
		private void chart_MouseWheel(object sender, MouseEventArgs e)
		{
			// ホイールを動かした分だけチャートも移動
			MainChartArea.AxisX.ScaleView.Position += e.Delta / 120 * 60;

			// Y軸設定更新
			UpdateYSettings();
		}

		/// <summary>
		/// AnnotationPositionChangingイベントを処理します。
		/// </summary>
		/// <param name="sender">イベント発行主体</param>
		/// <param name="e">イベント引数</param>
		private void chart_AnnotationPositionChanging(object sender, AnnotationPositionChangingEventArgs e)
		{
			// x座標は０固定
			e.NewLocationX = 0;

			// 操作中の分割線を覚えておく
			MovingSplitter = sender as HorizontalLineAnnotation;
		}

		#endregion
	}
}
