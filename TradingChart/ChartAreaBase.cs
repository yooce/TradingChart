using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace MagicalNuts
{
	public abstract class ChartAreaBase : ChartArea
	{
		protected abstract int XCount { get; }

		private Label CursorLabelY = null;

		public ChartAreaBase()
		{
			// 一意な名前
			Name = Guid.NewGuid().ToString();
		}

		public virtual void SetUp(Chart chart)
		{
			// カーソルX
			CursorX.LineColor = Palette.CursorColor;
			CursorX.LineDashStyle = ChartDashStyle.Dash;
			CursorX.Interval = 0;

			// カーソルY
			CursorY.LineColor = Palette.CursorColor;
			CursorY.LineDashStyle = ChartDashStyle.Dash;
			CursorY.AxisType = AxisType.Secondary;
			CursorY.Interval = 0;

			// カーソルラベルY
			CursorLabelY = new Label();
			SetUpCursorLabel(CursorLabelY);
			CursorLabelY.Visible = false;
			chart.Controls.Add(CursorLabelY);
		}

		protected void SetUpCursorLabel(Label label)
		{
			label.BackColor = Palette.CursorLabelColor;
			label.ForeColor = Color.White;
			label.AutoSize = true;
			label.TextAlign = ContentAlignment.MiddleCenter;
		}

		protected int GetXFromMouse(Point mouse)
		{
			return (int)(AxisX.PixelPositionToValue(mouse.X) + 0.5);
		}

		public virtual void Update(Point mouse, HitTestResult result, string format)
		{
			if (result.ChartArea == this)
			{
				// マウスが自エリア内の場合

				// カーソル
				CursorX.SetCursorPixelPosition(mouse, false);
				CursorY.SetCursorPixelPosition(mouse, false);

				// カーソルラベルY
				CursorLabelY.Text = AxisY2.PixelPositionToValue(mouse.Y).ToString(format);
				CursorLabelY.Top = mouse.Y - CursorLabelY.Height / 2;
				if (AxisX.ScaleView.Position + AxisX.ScaleView.Size >= XCount)
					CursorLabelY.Left = (int)(AxisX.ValueToPixelPosition(AxisX.ScaleView.Position + AxisX.ScaleView.Size) + 1);
				else CursorLabelY.Left = (int)(AxisX.ValueToPixelPosition(AxisX.ScaleView.Position + AxisX.ScaleView.Size + 1) + 1);
				CursorLabelY.Visible = true;
			}
			else
			{
				// マウスが他エリア内の場合

				// カーソル
				CursorY.Position = double.NaN;

				// カーソルラベルY
				CursorLabelY.Visible = false;
			}
		}

		public abstract void UpdateYSettings(int start, int end, List<Plotters.IPlotter> plotters);

		protected List<double> GetValues(int start, int end, List<Plotters.IPlotter> plotters, AxisType at)
		{
			List<double> values = new List<double>();
			foreach (Plotters.IPlotter plotter in plotters)
			{
				foreach (Series series in plotter.GetSeries())
				{
					// 自ChartAreaでなければスキップ
					if (series.ChartArea != Name) continue;

					// 目的のY軸でなければスキップ
					if (series.YAxisType != at) continue;

					foreach (DataPoint dp in series.Points)
					{
						// xが範囲外ならスキップ
						if (dp.XValue < start || dp.XValue >= end) continue;

						foreach (double y in dp.YValues)
						{
							values.Add(y);
						}
					}
				}
			}
			return values;
		}
	}
}
