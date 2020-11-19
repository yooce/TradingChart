using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MagicalNuts
{
	public static class ControlExtensions
	{
		public static void AlignLeft(this Control me, Control baseCtrl)
		{
			me.Left = baseCtrl.Left + baseCtrl.Width + baseCtrl.Margin.Right + me.Margin.Left;
		}

		public static void AlignTop(this Control me, Control baseCtrl)
		{
			me.Top = baseCtrl.Top + baseCtrl.Height + baseCtrl.Margin.Bottom + me.Margin.Top;
		}

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
