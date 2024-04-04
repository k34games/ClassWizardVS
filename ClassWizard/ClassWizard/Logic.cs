using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace ClassWizard
{
    static class Logic
    {
        public class ProjectItemInfo 
        {
            public class DocumentInfo
            {
                public string documentPath;
                public string documentFileName => System.IO.Path.GetFileName(documentPath);
                public override string ToString()
                {
                    return documentFileName;
                }
            }
            public ProjectItem item  ;
            public string solutionPath;
            public bool isDocument;
            DocumentInfo docInfo;
            public ProjectItemInfo(ProjectItem _item, string _solutionPath, bool _isDocument, string _documentPath)
            {
                item = _item;
                solutionPath = _solutionPath;

                isDocument = _isDocument;
                docInfo.documentPath = _documentPath;
            }
            public override string ToString()
            {
                return isDocument ? docInfo.ToString() : item.Name;
            }
        }
        private static List<ProjectItemInfo> RecurseExploreItem(ProjectItem _item,string _path, Func<ProjectItem, bool> _filter)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
            List<ProjectItemInfo> _result = new List<ProjectItemInfo>();
            foreach (ProjectItem childItem in _item.ProjectItems)
            { 
                if (_filter(childItem))
                {
                    ProjectItemInfo _info;
                    Document _doc = childItem.Document;
                    if (_doc==null)
                    {
                     _info = new ProjectItemInfo( childItem, _path, false, "");

                    }
                    else
                    {
                        _info = new ProjectItemInfo(childItem, _path,true, System.IO.Path.Combine(_doc.Path, _doc.Name));
                    }
                    _result.Add(_info);
                }
                _result.AddRange(RecurseExploreItem(childItem, $"/{_path}/{childItem.Name}", _filter));
            }
            return _result ;
        }
        public static List<ProjectItemInfo> RecurseExploreProject(Project _project, Func<ProjectItem, bool> _filter)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

            List<ProjectItemInfo> _result =new List<ProjectItemInfo>() ;
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
 
                    Document _doc = _item.Document;
                    ProjectItemInfo _info ;
                    //= new ProjectItemInfo(_item, $"{_project.Name}/{_item.Name}", _item.Document != null)
                    if (_doc == null)
                    {
                        _info = new ProjectItemInfo(_item, $"{_project.Name}/{_item.Name}", false, "");

                    }
                    else
                    {
                        _info = new ProjectItemInfo(_item, $"{_project.Name}/{_item.Name}", true, System.IO.Path.Combine(_doc.Path, _doc.Name));
                    }
                    _result.Add(_info);
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

        public static List<ProjectItemInfo> RecurseExploreCurrentProject(Func<ProjectItem, bool> _filter)
        {
            /*
                 * works, however it feels a bit hiffy, will try to improve when the rest is done
                 */
            List<ProjectItemInfo> _result = new List<ProjectItemInfo>();
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

            DTE dte = (DTE)Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(SDTE));
            // Open a project before running this sample  
            Array _projects = dte.ActiveSolutionProjects as Array;
            if (_projects.Length != 0 && _projects != null)
            {
                Project _selectedProject = _projects.GetValue(0) as Project;
                //get the project path

                _result = Logic.RecurseExploreProject(_selectedProject, (item) =>
                {
                    if (item.Document != null)
                    {
                        //string _extension = Path.GetExtension(Path.Combine( item.Document.Path, item.Document.Name));
                        string _extension = System.IO.Path.GetExtension(item.Document.Name);
                        if (_extension.Equals(".h"))
                        {
                            return true;
                        }
                    }
                    return false;

                }
                );
            }
            else
            {
                MessageBox.Show("No Project in solution or selected");
                // Console.WriteLine("No Project in solution or selected");
            }
            return _result;
        }
        
        public static List<ProjectItemInfo> GetItemsAllProjects(Func<ProjectItem, bool> _filter)
        {
            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

            DTE dte = (DTE)Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(SDTE));
          // Open a project before running this sample  
                Projects prjs = dte.Solution.Projects;

                List<ProjectItemInfo> resultList = new List<ProjectItemInfo>();
                foreach (Project _proj in prjs)
                {
                     resultList.AddRange(Logic.RecurseExploreProject(_proj, _filter ));
                    
                }
                return resultList ;

            

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
                //         

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
  
                /*
                 * works, however it feels a bit hiffy, will try to improve when the rest is done
                 */
                Array _projects = dte.ActiveSolutionProjects as Array;
                if (_projects.Length != 0 && _projects != null)
                {
                    Project _selectedProject = _projects.GetValue(0) as Project;
                    //get the project path

                    System.Collections.Generic.List<ProjectItemInfo> _testList = Logic.RecurseExploreProject(_selectedProject, (item) =>
                    {
                        if (item.Document != null)
                        {
                            //string _extension = Path.GetExtension(Path.Combine( item.Document.Path, item.Document.Name));
                            string _extension = System.IO.Path.GetExtension(item.Document.Name);
                            if (_extension.Equals(".h"))
                            {
                                return true;
                            }
                        }
                        return false;

                    }
                    );
                }
                else
                {
                    MessageBox.Show("No Project in solution or selected");
                   // Console.WriteLine("No Project in solution or selected");
                } 
                /*
                foreach (Project _proj in prjs)
                {
 
                    System.Collections.Generic.List<ProjectItemInfo> _testList = Logic.RecurseExploreProject(_proj, (item) =>
                    {
                        if (item.Document != null)
                        {
                            //string _extension = Path.GetExtension(Path.Combine( item.Document.Path, item.Document.Name));
                            string _extension = System.IO.Path.GetExtension( item.Document.Name);
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
                */
                return msg;
            }
            catch (Exception ex)
            {
               return ex.Message;
            }
        }

    }
}

