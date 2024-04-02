using EnvDTE;
using EnvDTE100;
using EnvDTE80;
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
            MessageBox.Show(Logic.TestMethod());
            /*
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            DTE test = (DTE)Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(SDTE));
            //gets the solution
            if (test == null)
            {
                return;
            }
            Solution _solution = test.Solution;
            if (_solution == null || _solution.Projects == null)
            {
                return;
            }
            //TODO give selection
            //get project 0
            Project _project = _solution.Projects.Item(new object());
            if (_project == null ||_solution.Projects == null)
            {
                return;
            }
            ProjectItems _items = _project.ProjectItems;
            string _out = "";
            for (int i  = 0; i < _items.Count; i++)
            {
                _out += $"item: {_items.Item(i).Name} type: {_items.Item(0).Document.Type}\n";

            }
            MessageBox.Show(_out);
 */

        }
    };
}
