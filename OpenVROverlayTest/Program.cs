using System;
using System.IO;
using System.Reflection;
using System.Threading;
using Valve.VR;

namespace OpenVROverlayTest
{
  class Program
  {
    static void Main(string[] args)
    {
      string ResourcePath = new FileInfo(Assembly.GetEntryAssembly().Location).Directory.FullName + "/Resources/";

      // init
      var error = EVRInitError.None;

      OpenVR.Init(ref error);
      if (error != EVRInitError.None) throw new Exception();

      OpenVR.GetGenericInterface(OpenVR.IVRCompositor_Version, ref error);
      if (error != EVRInitError.None) throw new Exception();

      OpenVR.GetGenericInterface(OpenVR.IVROverlay_Version, ref error);
      if (error != EVRInitError.None) throw new Exception();


      // create overlay, ...
      var overlay = OpenVR.Overlay;

      ulong overlayHandle = 0, thumbnailHandle = 0;

      overlay.CreateDashboardOverlay("overlayTest", "HL3", ref overlayHandle, ref thumbnailHandle);
      overlay.SetOverlayFromFile(thumbnailHandle, $"{ResourcePath}/white-lambda.png");

      overlay.SetOverlayWidthInMeters(overlayHandle, 2.5f);
      overlay.SetOverlayInputMethod(overlayHandle, VROverlayInputMethod.Mouse);
      Console.CancelKeyPress += (s, e) => overlay.DestroyOverlay(overlayHandle);

      while (true)
      {
        Thread.Sleep(10); // This sucks but will do for now
        if (overlay.IsOverlayVisible(overlayHandle))
          overlay.SetOverlayFromFile(overlayHandle, $"{ResourcePath}/hl3.jpg");
      }
    }
  }
}