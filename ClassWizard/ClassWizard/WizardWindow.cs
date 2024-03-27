using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Runtime.InteropServices;

namespace ClassWizard
{
    /// <summary>
    /// This class implements the tool window exposed by this package and hosts a user control.
    /// </summary>
    /// <remarks>
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane,
    /// usually implemented by the package implementer.
    /// <para>
    /// This class derives from the ToolWindowPane class provided from the MPF in order to use its
    /// implementation of the IVsUIElementPane interface.
    /// </para>
    /// </remarks>
    [Guid("3668cdc1-1850-409d-b354-283d6227ddb9")]
    public class WizardWindow : ToolWindowPane , IVsWindowFrame
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WizardWindow"/> class.
        /// </summary>
        public WizardWindow() : base(null)
        {
            this.Caption = "WizardWindow";

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            this.Content = new WizardWindowControl();
           
        }

        public int Show()
        {
            throw new NotImplementedException();
        }

        public int Hide()
        {
            throw new NotImplementedException();
        }

        public int IsVisible()
        {
            throw new NotImplementedException();
        }

        public int ShowNoActivate()
        {
            throw new NotImplementedException();
        }

        public int CloseFrame(uint grfSaveOptions)
        {
            throw new NotImplementedException();
        }

        public int SetFramePos(VSSETFRAMEPOS dwSFP, ref Guid rguidRelativeTo, int x, int y, int cx, int cy)
        {
            throw new NotImplementedException();
        }

        public int GetFramePos(VSSETFRAMEPOS[] pdwSFP, out Guid pguidRelativeTo, out int px, out int py, out int pcx, out int pcy)
        {
            throw new NotImplementedException();
        }

        public int GetProperty(int propid, out object pvar)
        {
            throw new NotImplementedException();
        }

        public int SetProperty(int propid, object var)
        {
            throw new NotImplementedException();
        }

        public int GetGuidProperty(int propid, out Guid pguid)
        {
            throw new NotImplementedException();
        }

        public int SetGuidProperty(int propid, ref Guid rguid)
        {
            throw new NotImplementedException();
        }

        public int QueryViewInterface(ref Guid riid, out IntPtr ppv)
        {
            throw new NotImplementedException();
        }

        public int IsOnScreen(out int pfOnScreen)
        {
            throw new NotImplementedException();
        }
    }
}
