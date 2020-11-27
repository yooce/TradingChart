using System;
using System.Collections.Generic;
using System.Reflection;

namespace MagicalNuts.Plotters
{
	/// <summary>
	/// プロッター管理を表します。
	/// </summary>
	public class PlotterManager
	{
		/// <summary>
		/// プロッター情報を表します。
		/// </summary>
		public class PlotterInfo
		{
			/// <summary>
			/// プロッターが属すアセンブリを取得します。
			/// </summary>
			public Assembly Assembly { get; private set; }

			/// <summary>
			/// プロッターのクラス名を取得します。
			/// </summary>
			public string ClassName { get; private set; }

			/// <summary>
			/// PlotterInfoの新しいインスタンスを初期化します。
			/// </summary>
			/// <param name="assembly">アセンブリ</param>
			/// <param name="cn">クラス名</param>
			public PlotterInfo(Assembly assembly, string cn)
			{
				Assembly = assembly;
				ClassName = cn;
			}
		}

		/// <summary>
		/// プロッター情報のリスト
		/// </summary>
		private List<PlotterInfo> PlotterInfos = null;

		/// <summary>
		/// PlotterManagerクラスの新しいインスタンスを初期化します。
		/// </summary>
		/// <param name="plugin_path">プラグインのパス</param>
		public PlotterManager(string plugin_path)
		{
			// プラグイン情報収集
			PlotterInfos = new List<PlotterInfo>();

			// 自アプリ内
			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				PlotterInfos.AddRange(GetPlotterInfos(assembly));
			}

			// プラグイン
			if (plugin_path != null)
			{
				if (System.IO.Directory.Exists(plugin_path))
				{
					string[] dlls = System.IO.Directory.GetFiles(plugin_path, "*.dll");
					foreach (string dll in dlls)
					{
						PlotterInfos.AddRange(GetPlotterInfos(Assembly.LoadFrom(dll)));
					}
				}
			}
		}

		/// <summary>
		/// プロッター情報のリストを取得します。
		/// </summary>
		/// <param name="assembly">アセンブリ</param>
		/// <returns>プロッター情報のリスト</returns>
		private List<PlotterInfo> GetPlotterInfos(Assembly assembly)
		{
			List<PlotterInfo> plotterInfos = new List<PlotterInfo>();
			foreach (Type type in assembly.GetTypes())
			{
				// ロウソク足と出来高プロッターは除外
				if (type.FullName == "MagicalNuts.Plotters.CandlePlotter"
					|| type.FullName == "MagicalNuts.Plotters.VolumePlotter") continue;

				// クラス、公開、抽象クラスでない、IPlotterを継承している、が条件
				if (type.IsClass && type.IsPublic && !type.IsAbstract && type.GetInterface(typeof(IPlotter).FullName) != null)
				{
					plotterInfos.Add(new PlotterInfo(assembly, type.FullName));
				}
			}
			return plotterInfos;
		}

		/// <summary>
		/// プロッターのリストを取得します。
		/// </summary>
		public List<IPlotter> Plotters
		{
			get
			{
				List<IPlotter> plotters = new List<IPlotter>();
				foreach (PlotterInfo pi in PlotterInfos)
				{
					plotters.Add((IPlotter)pi.Assembly.CreateInstance(pi.ClassName));
				}
				return plotters;
			}
		}
	}
}
