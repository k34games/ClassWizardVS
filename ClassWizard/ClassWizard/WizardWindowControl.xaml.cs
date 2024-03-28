﻿using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.Win32;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;


namespace ClassWizard
{
    /// <summary>
    /// Interaction logic for WizardWindowControl.
    /// </summary>
    public partial class WizardWindowControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WizardWindowControl"/> class.
        /// </summary>
        public WizardWindowControl()
        {
            
            this.InitializeComponent();
            //ChooseDestinationButton.MouseLeftButtonUp += SelectDestination;
        }
        
        private void SelectDestination(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

            //System.Windows.Forms.FolderBrowserDialog _dia = new System.Windows.Forms.FolderBrowserDialog();
            //debugText.Text = _dia.SelectedPath;

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
            /*
            MessageBox.Show(
                    string.Format(System.Globalization.CultureInfo.CurrentUICulture, "Invoked '{0}'", this.ToString()),
                    "WizardWindow"
            );
            */
            //TODO find current folder
            System.Windows.Forms.FolderBrowserDialog _dia = new System.Windows.Forms.FolderBrowserDialog();
            _dia.RootFolder = System.Environment.SpecialFolder.MyComputer;
            _dia.ShowDialog();
            debugText.Text = _dia.SelectedPath;
        }
    }
}