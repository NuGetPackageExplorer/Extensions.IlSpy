using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using NuGetPackageExplorer.Types;

namespace PackageExplorer.Extensions.ILSpy
{
    [PackageContentViewerMetadata(0, ".dll")]
    internal class ILSpyPackageContentViewer : IPackageContentViewer
    {
        private ConfigurationControl _configControl;
        private Window _dialog;

        public object GetView(IPackageContent selectedFile, IReadOnlyList<IPackageContent> peerFiles)
        {
            try
            {
                _configControl = new ConfigurationControl(selectedFile.Extension, selectedFile.GetStream());
                _dialog = new Window
                {
                    Content = _configControl,
                    Topmost = true,
                    Width = 300,
                    Height = 120,
                    ResizeMode = ResizeMode.NoResize,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    ShowInTaskbar = false
                };
                _dialog.ShowDialog();

                return _configControl.Assembly.FullName;
            }
            catch (Exception exception)
            {
                return exception.ToString();
            }
        }
    }
}
