using System;
using System.IO;
using System.Windows;
using NuGetPackageExplorer.Types;

namespace PackageExplorer.Extensions.ILSpy
{
    [PackageContentViewerMetadata(0, ".dll")]
    internal class ILSpyPackageContentViewer : IPackageContentViewer
    {
        private ConfigurationControl configControl;
        private Window dialog;

        public object GetView(string extension, Stream stream)
        {
            try
            {
                configControl = new ConfigurationControl(extension, stream);
                dialog = new Window();
                dialog.Content = configControl;
                dialog.Topmost = true;
                dialog.Width = 300;
                dialog.Height = 120;
                dialog.ResizeMode = ResizeMode.NoResize;
                dialog.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                dialog.ShowInTaskbar = false;
                dialog.ShowDialog();

                return configControl.Assembly.FullName;
            }
            catch (Exception exception)
            {
                return exception.ToString();
            }
        }
    }
}