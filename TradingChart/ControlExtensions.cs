using System.Drawing;
using System.Windows.Forms;

namespace MagicalNuts
{
	/// <summary>
	/// Controlの拡張を表します。
	/// </summary>
	public static class ControlExtensions
	{
		/// <summary>
		/// 左寄せをします。
		/// </summary>
		/// <param name="me">自コントロール</param>
		/// <param name="baseCtrl">基準コントロール</param>
		public static void AlignLeft(this Control me, Control baseCtrl)
		{
			me.Left = baseCtrl.Left + baseCtrl.Width + baseCtrl.Margin.Right + me.Margin.Left;
		}

		/// <summary>
		/// 上寄せをします。
		/// </summary>
		/// <param name="me">自コントロール</param>
		/// <param name="baseCtrl">基準コントロール</param>
		public static void AlignTop(this Control me, Control baseCtrl)
		{
			me.Top = baseCtrl.Top + baseCtrl.Height + baseCtrl.Margin.Bottom + me.Margin.Top;
		}

		/// <summary>
		/// クリップボードにキャプチャ画像をコピーします。
		/// </summary>
		/// <param name="me">自コントロール</param>
		/// <param name="zoom">倍率</param>
		public static void CopyToClipboard(this Control me, double zoom)
		{
			// キャプチャ
			Bitmap bmp = new Bitmap(me.Width, me.Height);
			me.DrawToBitmap(bmp, new Rectangle(0, 0, me.Width, me.Height));

			// 拡縮
			Bitmap canvas = new Bitmap((int)(me.Width * zoom), (int)(me.Height * zoom));
			Graphics g = Graphics.FromImage(canvas);
			g.DrawImage(bmp, 0, 0, (int)(bmp.Width * zoom), (int)(bmp.Height * zoom));

			// クリップボードにコピー
			Clipboard.SetImage(canvas);

			// 終了
			g.Dispose();
			canvas.Dispose();
			bmp.Dispose();
		}
	}
}
