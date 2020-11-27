using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MagicalNuts.Plotters
{
	public class PlotterManager
	{
		public class PlotterInfo
		{
			public Assembly Assembly { get; private set; }
			public string ClassName { get; private set; }

			public PlotterInfo(Assembly assembly, string cn)
			{
				Assembly = assembly;
				ClassName = cn;
			}
		}

		private List<PlotterInfo> PlotterInfos = null;

		public PlotterManager(string path)
		{
			PlotterInfos = new List<PlotterInfo>();

			// 自アプリ内
			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				PlotterInfos.AddRange(GetPlotterInfos(assembly));
			}

			// プラグイン
			if (path != null)
			{
				string dir = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location + @"\plugins");
				if (System.IO.Directory.Exists(dir))
				{
					string[] dlls = System.IO.Directory.GetFiles(dir, "*.dll");
					foreach (string dll in dlls)
					{
						PlotterInfos.AddRange(GetPlotterInfos(Assembly.LoadFrom(dll)));
					}
				}
			}
		}

		private List<PlotterInfo> GetPlotterInfos(Assembly assembly)
		{
			List<PlotterInfo> plotterInfos = new List<PlotterInfo>();
			foreach (Type type in assembly.GetTypes())
			{
				if (type.FullName == "MagicalNuts.Plotters.CandlePlotter"
					|| type.FullName == "MagicalNuts.Plotters.VolumePlotter") continue;

				if (type.IsClass && type.IsPublic && !type.IsAbstract && type.GetInterface(typeof(IPlotter).FullName) != null)
				{
					plotterInfos.Add(new PlotterInfo(assembly, type.FullName));
				}
			}
			return plotterInfos;
		}

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
