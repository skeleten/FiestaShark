using Microsoft.Win32;
using ScriptNET.Runtime;
using System;
using System.Reflection;
using System.Windows.Forms;

namespace MapleShark {
	internal static class Program {
		[STAThread]
		private static void Main(string[] pArgs) {
			RegisterFileAssociation(".msb", "MapleShark", "MapleShark Binary File", Assembly.GetExecutingAssembly().Location, string.Empty, 0);
			RuntimeHost.Initialize();
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			_MainForm = new MainForm(pArgs);
			Application.Run(_MainForm);
		}
	  internal static string AssemblyVersion {
	    get {
	      return Assembly.GetExecutingAssembly()
								.GetName()
								.Version
								.ToString();
	    }
	  }
	  internal static string AssemblyCopyright {
	    get {
	      return ((AssemblyCopyrightAttribute)Assembly.GetExecutingAssembly()
								.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false)[0])
								.Copyright;
	    }
	  }

		internal static MainForm MainForm { get { return _MainForm; } }


		private static MainForm _MainForm;
		private static void RegisterFileAssociation(string pExtension, string pProgramId, string pDescription, string pEXE, string pIconPath, int pIconIndex) {
			try {
			  if (pExtension.Length == 0) return;

				if(!pExtension.StartsWith("."))
					pExtension = "." + pExtension;

				using (RegistryKey key = Registry.ClassesRoot.OpenSubKey(pExtension))
					if (key == null)
					  SetRegisteryKeyValue(Registry.ClassesRoot, pExtension, pProgramId);

				using (RegistryKey extKey = Registry.ClassesRoot.OpenSubKey(pExtension)) {
				using (RegistryKey key = extKey.OpenSubKey(pProgramId))
					if (key == null) 
						using (RegistryKey progIdKey = Registry.ClassesRoot.CreateSubKey(pProgramId)) {
							progIdKey.SetValue(string.Empty, pDescription);

							SetRegisteryKeyValue(progIdKey, "DefaultIcon", string.Format("\"{0}\",{1}", pIconPath, pIconIndex));
							SetRegisteryKeyValue(progIdKey, @"shell\open\command", string.Format("\"{0}\" \"%1\"", pEXE));
							}
				}
			} catch (Exception) { }
		}

		private static void SetRegisteryKeyValue(RegistryKey progIdKey, string path, string value) {
			using (RegistryKey command = progIdKey.CreateSubKey(path)) command.SetValue(string.Empty, value);
		}
	}
}
