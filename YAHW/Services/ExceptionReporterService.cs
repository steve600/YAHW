using System;
using System.Windows;
using System.Windows.Threading;
using YAHW.Interfaces;
using FirstFloor.ModernUI.Windows.Controls;

namespace YAHW.Services
{
    /// <summary>
    /// <para>
    /// Exception reporter service
    /// </para>
    /// 
    /// <para>
    /// Class history:
    /// <list type="bullet">
    ///     <item>
    ///         <description>1.0: First release, working (Steffen Steinbrecher).</description>
    ///     </item>
    /// </list>
    /// </para>
    /// 
    /// <para>Author: Steffen Steinbrecher</para>
    /// <para>Date: 12.07.2015</para>
    /// </summary>
    public class ExceptionReporterService : IExceptionReporterService
    {
        public void ReportException(Exception ex)
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(
                                 () =>
                                 {
                                     ModernDialog.ShowMessage(ex.StackTrace, ex.Source, MessageBoxButton.OK, Application.Current.MainWindow);
                                     //Window window = new Window();
                                     //window.Content = new ExceptionReporter(ex);
                                     //window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                                     //window.Owner = Application.Current.MainWindow;
                                     //window.Title = "Exception-Reporter";
                                     //window.Height = 550;
                                     //window.Width = 650;
                                     //window.ShowInTaskbar = false;
                                     //window.ResizeMode = ResizeMode.NoResize;
                                     //window.ShowDialog();
                                 }));
        }
    }
}
