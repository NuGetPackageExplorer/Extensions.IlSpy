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
        private string _assemblyPath;

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



                var assemblyRawData = stream.ToByteArray();

                Assembly = Assembly.Load(assemblyRawData);

                var assemblyFileName = $"{Assembly.GetName().Name}{extension}";
                _assemblyPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), assemblyFileName);

                var file = File.Create(_assemblyPath);
                var assemblyMemoryStream = new MemoryStream(assemblyRawData);
                assemblyMemoryStream.CopyTo(file);
                file.Close();
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

        public Assembly Assembly { get; private set; }

        private Window ParentWindow => Parent as Window;

        private void OnButtonOKClick(object sender, RoutedEventArgs e)
        {
            Path = txtPath.Text.Trim();

            if (IsPathToILSpyValid())
            {
                Process.Start(Path, _assemblyPath);

                if (Parent is Window window) window.Close();
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

            if (!Path.EndsWith("ilspy.exe",StringComparison.OrdinalIgnoreCase))
                return false;

            return true;
        }

        public string Path { get; private set; }

        private void OnButtonBrowseClick(object sender, RoutedEventArgs e)
        {
            try
            {
                var openFileDialog = new OpenFileDialog
                {
                    Filter = "ILSpy|ILSpy.exe",
                    FilterIndex = 1,
                    ShowReadOnly = true,
                    Multiselect = false
                };
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
                ParentWindow?.Close();
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

        private void HandleException(Exception exception)
        {
            MessageBox.Show("Please report this exception on https://github.com/NuGetPackageExplorer/Extensions.IlSpy.\n\nMessage: " + exception, "Oops! Unexpected exception of type " + exception.GetType().Name,
                                MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
