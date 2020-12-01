using System.Drawing;
using System.Windows.Forms;

namespace MagicalNuts
{
	/// <summary>
	/// 価格表示板を表します。
	/// </summary>
	public partial class PriceBoard : UserControl
	{
		/// <summary>
		/// PriceBoardクラスの新しいインスタンスを初期化します。
		/// </summary>
		public PriceBoard()
		{
			InitializeComponent();
		}

		/// <summary>
		/// マージンを設定します。
		/// </summary>
		/// <param name="pos">マージン座標</param>
		public void SetMargin(Point pos)
		{
			// X
			labelOpen.Left = pos.X + labelOpen.Margin.Left;
			labelOpenValue.AlignLeft(labelOpen);

			// Y
			labelOpen.Top = pos.Y + labelOpen.Margin.Top;
			labelOpenValue.Top = labelOpen.Top;
			labelHigh.Top = labelOpen.Top;
			labelHighValue.Top = labelOpen.Top;
			labelLow.Top = labelOpen.Top;
			labelLowValue.Top = labelOpen.Top;
			labelClose.Top = labelOpen.Top;
			labelCloseValue.Top = labelOpen.Top;
			labelUpDown.Top = labelOpen.Top;
			labelUpDownP.Top = labelOpen.Top;
			labelVolume.Top = labelOpen.Top;
			labelVolumeValue.Top = labelOpen.Top;
		}

		/// <summary>
		/// ロウソク足を設定します。
		/// </summary>
		/// <param name="cur">現在のロウソク足</param>
		/// <param name="prev">前のロウソク足</param>
		/// <param name="format">価格表示のフォーマット</param>
		public void SetCandle(DataTypes.Candle cur, DataTypes.Candle prev, string format)
		{
			// 現在のロウソク足が無い場合は非表示
			if (cur == null)
			{
				Visible = false;
				return;
			}

			// 値設定
			labelOpenValue.Text = cur.Open.ToString(format);
			labelHighValue.Text = cur.High.ToString(format);
			labelLowValue.Text = cur.Low.ToString(format);
			labelCloseValue.Text = cur.Close.ToString(format);
			labelVolumeValue.Text = cur.Volume.ToString();
			if (prev != null)
			{
				decimal diff = cur.Close - prev.Close;
				decimal diff_p = (cur.Close / prev.Close - 1) * 100;
				if (diff >= 0)
				{
					labelUpDown.Text = "+" + diff.ToString(format);
					labelUpDownP.Text = "(+" + diff_p.ToString("0.00") + "%)";
				}
				else
				{
					labelUpDown.Text = diff.ToString(format);
					labelUpDownP.Text = "(" + diff_p.ToString("0.00") + "%)";
				}
			}
			else
			{
				labelUpDown.Text = "-";
				labelUpDownP.Text = "(-%)";
			}

			// 色設定
			if (cur.Close - cur.Open >= 0)
			{
				labelOpenValue.ForeColor = Palette.PriceUpColor;
				labelHighValue.ForeColor = Palette.PriceUpColor;
				labelLowValue.ForeColor = Palette.PriceUpColor;
				labelCloseValue.ForeColor = Palette.PriceUpColor;
				labelVolumeValue.ForeColor = Palette.PriceUpColor;
				labelUpDown.ForeColor = Palette.PriceUpColor;
				labelUpDownP.ForeColor = Palette.PriceUpColor;
			}
			else
			{
				labelOpenValue.ForeColor = Palette.PriceDownColor;
				labelHighValue.ForeColor = Palette.PriceDownColor;
				labelLowValue.ForeColor = Palette.PriceDownColor;
				labelCloseValue.ForeColor = Palette.PriceDownColor;
				labelVolumeValue.ForeColor = Palette.PriceDownColor;
				labelUpDown.ForeColor = Palette.PriceDownColor;
				labelUpDownP.ForeColor = Palette.PriceDownColor;
			}

			// 配置
			labelHigh.AlignLeft(labelOpenValue);
			labelHighValue.AlignLeft(labelHigh);
			labelLow.AlignLeft(labelHighValue);
			labelLowValue.AlignLeft(labelLow);
			labelClose.AlignLeft(labelLowValue);
			labelCloseValue.AlignLeft(labelClose);
			labelUpDown.AlignLeft(labelCloseValue);
			labelUpDownP.AlignLeft(labelUpDown);
			labelVolume.AlignLeft(labelUpDownP);
			labelVolumeValue.AlignLeft(labelVolume);

			// 表示
			Visible = true;
		}
	}
}
