using System;
using System.Collections.Generic;
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
	}
}
