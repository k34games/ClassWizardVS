using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassWizard
{
    static class Logic
    {
        private static List<string> RecurseExploreItem(ProjectItem _item,string _path, Func<ProjectItem, bool> _filter)
        {
            List<string> _result = new List<string>();
            foreach (ProjectItem childItem in _item.ProjectItems)
            { 
                if (_filter(childItem))
                {
                    _result.Add($"{_path}/{childItem.Name}");
                }
                _result.AddRange(RecurseExploreItem(childItem, $"/{_path}/{childItem.Name}", _filter));
            }
            return _result ;
        }
        public static List<string> RecurseExploreProject(Project _project, Func<ProjectItem, bool> _filter)
        {
            List<string> _result =new List<string>() ;
            foreach (ProjectItem _item in _project.ProjectItems)
            {
                //les filtres
                /*
                msg += "item " + _item.Name + " of type " + _item.GetType()+ "\n";
                foreach (ProjectItem _itemItem in _item.ProjectItems)
                {
                    msg += "item item" + _itemItem.Name + " of type " + _itemItem.GetType()+ "\n";

                }
                 */
                if (_filter(_item))
                {
                    _result.Add( _item.Name );
                }
                _result.AddRange(RecurseExploreItem(_item, $"{_project.Name}/{_item.Name}",_filter));
                /*
                Document _doc = _item.Document;
                if (_doc == null)
                    continue;
                */



            }
            return _result  ;
        }

        public static string TestMethod()
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
                #region t
                //foreach (Project _proj in prjs)
                //{
                //    msg += _proj.Name + " ";
                //    foreach (ProjectItem _item  in _proj.ProjectItems)
                //    {
                //        //les filtres
                //        /*
                //        msg += "item " + _item.Name + " of type " + _item.GetType()+ "\n";
                //        foreach (ProjectItem _itemItem in _item.ProjectItems)
                //        {
                //            msg += "item item" + _itemItem.Name + " of type " + _itemItem.GetType()+ "\n";

                //        }
                //         */

                //        msg += "\nitem " + _item.Name;
                //        Document _doc = _item.Document;
                //        if (_doc == null)
                //            continue;
                //        msg += "document of language " + _doc.Language + "\n";
                //        foreach (ProjectItem _itemItem in _item.ProjectItems)
                //        {

                //            msg += "\nitem " + _itemItem.Name;
                //            _doc = _itemItem.Document;
                //            if (_doc == null) 
                //                continue;
                //            msg += "document of language " + _doc.Language+ "\n";
                //        }


                //    }
                #endregion
                foreach (Project _proj in prjs)
                {
                    System.Collections.Generic.List<string> _testList = Logic.RecurseExploreProject(_proj, (item) =>
                    {
                        if (item.Document != null)
                        {
                            //string _extension = Path.GetExtension(Path.Combine( item.Document.Path, item.Document.Name));
                            string _extension = Path.GetExtension( item.Document.Name);
                            if (_extension.Equals(".h"))
                            {
                                return true;
                            }
                        }
                        return false  ;
 
                    }
                    );
                    _testList.ToString();
                }

                if (prjs.Properties != null)
                {
                    msg += "\nProperties:";
                    foreach (Property prop in prjs.Properties)
                    {
                        msg += "\n   " + prop.Name;
                    }
                }
                return msg;
            }
            catch (Exception ex)
            {
               return ex.Message;
            }
        }
    };
}

