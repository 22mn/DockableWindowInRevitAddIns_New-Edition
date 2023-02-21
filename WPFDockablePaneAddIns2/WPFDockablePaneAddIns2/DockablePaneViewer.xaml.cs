using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Reflection;
using System.Windows.Navigation;

namespace WPFDockablePaneAddIns2
{
    /// <summary>
    /// Interaction logic for DockablePaneViewer.xaml
    /// </summary>
    public partial class DockablePaneViewer : Page, IDockablePaneProvider
    {
        public DockablePaneViewer()
        {
            //initialize wpf control
            InitializeComponent();

            // to hide unwanted JS Script pop-ups
            wb.Loaded += (a, b) => { HideJsScriptErrors(wb, true); };
        }

        // hidejsscript method
        public void HideJsScriptErrors(WebBrowser wb, bool hide)
        {
            // IWebBrowser2 interface
            // Exposes methods that are implemented by the 
            // WebBrowser control
            // Searches for the specified field, using the 
            // specified binding constraints.
            FieldInfo fld = typeof(WebBrowser).GetField(
              "_axIWebBrowser2",
              BindingFlags.Instance | BindingFlags.NonPublic);

            if (null != fld)
            {
                object obj = fld.GetValue(wb);
                if (null != obj)
                {
                    // Silent: Sets or gets a value that indicates 
                    // whether the object can display dialog boxes.
                    // HRESULT IWebBrowser2::get_Silent(VARIANT_BOOL *pbSilent);
                    // HRESULT IWebBrowser2::put_Silent(VARIANT_BOOL bSilent);
                    obj.GetType().InvokeMember("Silent",
                      BindingFlags.SetProperty, null, obj,
                      new object[] { true });
                }
            }

            wb.Navigated += (a, b) => { HideJsScriptErrors(wb, hide); };
        }

        // setupdockablepane abstract method
        public void SetupDockablePane(DockablePaneProviderData data)
        {
            // wpf object with pane's interface
            data.FrameworkElement = this as FrameworkElement;
            // initial state position
            data.InitialState = new DockablePaneState
            {
                DockPosition = DockPosition.Tabbed,
                TabBehind = DockablePanes.BuiltInDockablePanes.ProjectBrowser
            };
        }
    }
}
