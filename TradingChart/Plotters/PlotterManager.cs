using System;
using System.Collections.Generic;
using System.Reflection;

namespace MagicalNuts.Plotters
{
	/// <summary>
	/// プロッター管理を表します。
	/// </summary>
	public class PlotterManager : PluginManager<IPlotter>
	{
		/// <summary>
		/// PlotterManagerクラスの新しいインスタンスを初期化します。
		/// </summary>
		/// <param name="plugin_path">プラグインのパス</param>
		public PlotterManager(string plugin_path) : base(plugin_path)
		{
		}

		/// <summary>
		/// 除外する型かどうか判定します。
		/// </summary>
		/// <param name="type">型</param>
		/// <returns>除外する型かどうか</returns>
		protected override bool IsExclude(Type type)
		{
			return type.FullName == "MagicalNuts.Plotters.CandlePlotter"
				|| type.FullName == "MagicalNuts.Plotters.VolumePlotter";
		}
	}
}
