﻿
namespace MagicalNuts.TradingChartSample
{
	partial class Form1
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.tradingChart1 = new MagicalNuts.TradingChart();
			((System.ComponentModel.ISupportInitialize)(this.tradingChart1)).BeginInit();
			this.SuspendLayout();
			// 
			// tradingChart1
			// 
			this.tradingChart1.CandleTerm = MagicalNuts.CandleTerm.Dayly;
			this.tradingChart1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tradingChart1.Location = new System.Drawing.Point(0, 0);
			this.tradingChart1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.tradingChart1.Name = "tradingChart1";
			this.tradingChart1.Size = new System.Drawing.Size(1008, 537);
			this.tradingChart1.TabIndex = 0;
			this.tradingChart1.Text = "tradingChart1";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1008, 537);
			this.Controls.Add(this.tradingChart1);
			this.Font = new System.Drawing.Font("Yu Gothic UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			((System.ComponentModel.ISupportInitialize)(this.tradingChart1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private MagicalNuts.TradingChart tradingChart1;
	}
}

