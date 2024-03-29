using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

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
    public class WizardWindow : ToolWindowPane
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WizardWindow"/> class.
        /// </summary>
        public WizardWindow() : base(null)
        {
            this.Caption = "WizardWindow";
            //https://learn.microsoft.com/fr-fr/dotnet/desktop/wpf/windows/how-to-open-common-system-dialog-box?view=netdesktop-8.0
            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            this.Content = new WizardWindowControl();

             
        }
 
    };
}
