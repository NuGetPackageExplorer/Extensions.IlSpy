using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using Microsoft.Win32;

namespace PackageExplorer.Extensions.ILSpy
{
    public partial class ConfigurationControl
    {
        private string assemblyPath;

        public ConfigurationControl()
        {
            InitializeComponent();
        }

        public ConfigurationControl(string extension, Stream stream)
            : this()
        {
            ProcessAssemblyStream(extension, stream);
        }

        private void ProcessAssemblyStream(string extension, Stream stream)
        {
            try
            {
                byte[] assemblyRawData = stream.ToByteArray();

                Assembly = Assembly.Load(assemblyRawData);

                string assemblyFileName = string.Format("{0}{1}", Assembly.GetName().Name, extension);
                assemblyPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), assemblyFileName);

                FileStream file = File.Create(assemblyPath);
                MemoryStream assemblyMemoryStream = new MemoryStream(assemblyRawData);
                assemblyMemoryStream.CopyTo(file);
                file.Close();
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

        public Assembly Assembly { get; private set; }

        private Window ParentWindow { get { return Parent as Window; } }

        private void OnButtonOKClick(object sender, RoutedEventArgs e)
        {
            Path = txtPath.Text.Trim();

            if(IsPathToILSpyValid())
            {
                Process.Start(Path, assemblyPath);

                var window = Parent as Window;
                if (window != null) window.Close();
            }
            else
            {
                MessageBox.Show(ParentWindow, "Please provide a valid path to ILSpy.exe", "Invalid path",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private bool IsPathToILSpyValid()
        {
            if (string.IsNullOrWhiteSpace(Path))
                return false;

            if (!Path.ToLowerInvariant().EndsWith("ilspy.exe"))
                return false;

            return true;
        }

        public string Path { get; private set; }

        private void OnButtonBrowseClick(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "ILSpy|ILSpy.exe";
                openFileDialog.FilterIndex = 1;
                openFileDialog.ShowReadOnly = true;
                openFileDialog.Multiselect = false;
                if (openFileDialog.ShowDialog() == true)
                {
                    txtPath.Text = openFileDialog.FileName;
                }
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

        private void OnButtonCancelClick(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ParentWindow != null)
                    ParentWindow.Close();
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

        private void HandleException(Exception exception)
        {
            MessageBox.Show("Please report this exception on https://github.com/NuGetPackageExplorer/Extensions.IlSpy.\n\nMessage: " + exception.Message, "Oops! Unexpected exception of type " + exception.GetType().Name,
                                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
