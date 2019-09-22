using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using UnityPlayer;

namespace Template
{
	sealed class App : IFrameworkView
	{
		private WinRTBridge.WinRTBridge m_bridge;
		private AppCallbacks appCallbacks;

		public App()
		{
			appCallbacks = new AppCallbacks(false);
		}

		public void Initialize(CoreApplicationView applicationView)
		{
			// Setup scripting bridge
			m_bridge = new WinRTBridge.WinRTBridge();
			appCallbacks.SetBridge(m_bridge);

			appCallbacks.SetCoreApplicationViewEvents(applicationView);
		}

		public void SetWindow(CoreWindow coreWindow)
		{
			appCallbacks.SetCoreWindowEvents(coreWindow);

			appCallbacks.InitializeD3DWindow();
		}

		public void Load(string entryPoint)
		{
		}

		public void Run()
		{
			appCallbacks.Run();
		}

		public void Uninitialize()
		{
		}

		[MTAThread]
		static void Main(string[] args)
		{
			var direct3DApplicationSource = new Direct3DApplicationSource();
			CoreApplication.Run(direct3DApplicationSource);
		}
	}

	sealed class Direct3DApplicationSource : IFrameworkViewSource
	{
		public IFrameworkView CreateView()
		{
			return new App();
		}
	}



}
