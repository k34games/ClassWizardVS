using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.Win32;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Text;
using System.Collections.Generic;
using EnvDTE;
using System;

namespace ClassWizard
{
    /// <summary>
    /// Interaction logic for WizardWindowControl.
    /// </summary>
    public partial class WizardWindowControl : UserControl
    {
        List<Logic.ProjectItemInfo> classList = new List<Logic.ProjectItemInfo>();
        string targetDirectory = "";
        /// <summary>
        /// Initializes a new instance of the <see cref="WizardWindowControl"/> class.
        /// </summary>
        public WizardWindowControl()
        {
            
            this.InitializeComponent();
            //ChooseDestinationButton.MouseLeftButtonUp += SelectDestination;
            GenerateButton.IsEnabled = false;

        }
        
    

        /// <summary>
        /// Handles click on the button by displaying a message box.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        [SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", Justification = "Sample code")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Default event handler naming pattern")]
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                string.Format(System.Globalization.CultureInfo.CurrentUICulture, "Invoked '{0}'", this.ToString()),
                "WizardWindow");
        }
        [SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", Justification = "Sample code")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Default event handler naming pattern")]
        private void select_Destination_Click(object sender, RoutedEventArgs e)
        {
 
 
            System.Windows.Forms.FolderBrowserDialog _dia = new System.Windows.Forms.FolderBrowserDialog();
 
            
            _dia.RootFolder = System.Environment.SpecialFolder.Desktop;
            _dia.SelectedPath= System.IO.Directory.GetCurrentDirectory();
            _dia.ShowDialog();
            targetDirectory = _dia.SelectedPath;
            GenerateButton.IsEnabled = true;
            debugText.Text = _dia.SelectedPath;
        }
        [SuppressMessage("Microsoft.Globalization", "CA1300:SpecifyMessageBoxOptions", Justification = "Sample code")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Default event handler naming pattern")]
        private void generate_class_click(object sender, RoutedEventArgs e)
        {
            string _className = System.IO.Path.GetFileName( ClassNameEntryTextBox.Text.Trim());
 
            string filePath = System.IO.Path.Combine(targetDirectory, _className);
            string className = _className;

            GenerateClasses(filePath , className);
 
        }

        private void GenerateClasses(string filePath, string className)
        {
            bool isInheriting = availableClassesList.SelectedItems.Count > 0;
            string _newText = "";
            string _include = "";
            //.h
            if (isInheriting)
            {
                Logic.ProjectItemInfo currentSelected = (Logic.ProjectItemInfo )availableClassesList.SelectedItems[0];
                if (currentSelected.isDocument)
                {
                    //TODO finish
                    _newText = $": public {currentSelected.docInfo.documentFileName}";
                    _include = $"#include \"{Logic.GetRelativePath(filePath, currentSelected.docInfo.documentPath)}\"";
                }
            }
            FileStream _streamDotH = System.IO.File.Create(filePath + ".h");
      
            byte[] bufferDotH = new UTF8Encoding(true).GetBytes($"#pragma once\n{_include}\nclass { className}" + _newText + "\n{\n};"); 
            _streamDotH.Write(bufferDotH,  0, bufferDotH.Length);
            _streamDotH.Close();
            //.cpp
            FileStream _streamDotCpp = System.IO.File.Create(filePath + ".cpp");
            byte[] bufferDotCpp = new UTF8Encoding(true).GetBytes($"#include \"{ className}.h\"");
            _streamDotCpp.Write(bufferDotCpp, 0, bufferDotCpp.Length);
            _streamDotCpp.Close();

            Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();

            DTE dte = (DTE)Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(SDTE));
            Array _projects = dte.ActiveSolutionProjects as Array;

            if (_projects.Length != 0 && _projects != null)
            {
                Project _selectedProject = _projects.GetValue(0) as Project;

                _selectedProject.ProjectItems.AddFromFile(_streamDotH.Name);
                _selectedProject.ProjectItems.AddFromFile(_streamDotCpp.Name);

            }
            debugText.Text = "Successfully created class!";

        }
        private void clear_selected_inheritance_click(object sender, RoutedEventArgs e)
        { 
            availableClassesList.SelectedItem = null;
        }
        private void refresh_classes_click(object sender, RoutedEventArgs e)
        {

            classList = Logic.RecurseExploreCurrentProject((item) =>
            {

                ThreadHelper.ThrowIfNotOnUIThread();
                if (item.Document != null)
                { 
                    string _extension = System.IO.Path.GetExtension(item.Document.Name);
                    if (_extension.Equals(".h"))
                    {
                        return true;
                    } 
                }
                return false;

            });
            string _debugPrint = "";
            foreach (Logic.ProjectItemInfo _item in classList)
            {
                _debugPrint += $"\n{_item.docInfo.documentFileName}";
            }
            MessageBox.Show(_debugPrint);
            availableClassesList.ItemsSource = classList;
            availableClassesList.SelectionChanged += OnClassSelectedChanged;
            /*
            classList = Logic.GetItemsAllProjects((item) =>
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

            });
 */
        }

        private void OnClassSelectedChanged(object sender, SelectionChangedEventArgs e)
        {

            debugText.Text = availableClassesList.SelectedItem != null? availableClassesList.SelectedItem.ToString() :"No class selected";
        }
    }
}