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
            TestMethod();
        }
        void TestMethod()
        {
            DTE dte = (DTE)Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(SDTE));
            try
            {   // Open a project before running this sample  
                Projects prjs = dte.Solution.Projects;
                string msg = "There are " + prjs.Count.ToString() + " projects in this collection.";
                msg += "\nThe application containing this Projects collection: " + prjs.DTE.Name;
                msg += "\nThe parent object of the Projects collection: " + prjs.Parent.Name;
                msg += "\nThe GUID representing the Projects type: " + prjs.Kind;
                msg += "\nThe projects name are:\n ";
                /*
                 * en gros de ce que je comprends
                 * les filtres ET les fichiers sont des items
                 * //TODO explorer la piste des documents
                 * */
                foreach (Project _proj in prjs)
                {
                    msg += _proj.Name + " ";
                    foreach (ProjectItem _item  in _proj.ProjectItems)
                    {
                        //les filtres
                        /*
                        msg += "item " + _item.Name + " of type " + _item.GetType()+ "\n";
                        foreach (ProjectItem _itemItem in _item.ProjectItems)
                        {
                            msg += "item item" + _itemItem.Name + " of type " + _itemItem.GetType()+ "\n";
                            
                        }
                         */

                        msg += "\nitem " + _item.Name;
                        Document _doc = _item.Document;
                        if (_doc == null)
                            continue;
                        msg += "document of language " + _doc.Language + "\n";
                        foreach (ProjectItem _itemItem in _item.ProjectItems)
                        {

                            msg += "\nitem " + _itemItem.Name;
                            _doc = _itemItem.Document;
                            if (_doc == null) 
                                continue;
                            msg += "document of language " + _doc.Language+ "\n";
                        }
                        
 
                    }
                }
                if (prjs.Properties != null)
                {
                    msg += "\nProperties:";
                    foreach (Property prop in prjs.Properties)
                    {
                        msg += "\n   " + prop.Name;
                    }
                }
                MessageBox.Show(msg, "Projects Collection");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    };
}
