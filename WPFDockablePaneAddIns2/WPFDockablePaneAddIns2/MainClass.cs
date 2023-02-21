using System;
using System.IO;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.Attributes;

namespace WPFDockablePaneAddIns2
{
    public class MainClass : IExternalApplication
    {
        // execute when app open
        public Result OnStartup(UIControlledApplication application)
        {
            // create ribbon panel (in addins tab)
            RibbonPanel ribbonPanel = application.CreateRibbonPanel(Tab.AddIns, "TwentyTwo Sample");
            // this assembly
            Assembly assembly = Assembly.GetExecutingAssembly();
            // assembly path 
            string assemblyPath = assembly.Location;

            // Create Show Button
            PushButton showButton = ribbonPanel.AddItem(new PushButtonData("Show Window", "Show", assemblyPath,
                "WPFDockablePaneAddIns2.Show")) as PushButton;
            // btn tooltip
            showButton.ToolTip = "Show the registered dockable window.";
            // show button icon images
            showButton.LargeImage = GetResourceImage(assembly, "WPFDockablePaneAddIns2.Resources.show32.png");
            showButton.Image = GetResourceImage(assembly, "WPFDockablePaneAddIns2.Resources.show16.png");

            // register dockablepane
            RegisterDockablePane(application);

            // return result
            return Result.Succeeded;
        }

        // execute when app close
        public Result OnShutdown(UIControlledApplication application)
        {
            // return result
            return Result.Succeeded;
        }
        // get embedded images from assembly resources
        public ImageSource GetResourceImage(Assembly assembly, string imageName)
        {
            try
            {
                // bitmap stream to construct bitmap frame
                Stream resource = assembly.GetManifestResourceStream(imageName);
                // return image data
                return BitmapFrame.Create(resource);
            }
            catch
            {
                return null;
            }
        }

        // register dockable pane 
        public Result RegisterDockablePane(UIControlledApplication application)
        {
            // dockablepaneviewer (customcontrol)
            DockablePaneViewer window = new DockablePaneViewer();

            // register in application with a new guid
            DockablePaneId dockID = new DockablePaneId(new Guid("{29F293CF-5486-45CB-8C10-60CF2A33DDAB}"));
            try
            {
                application.RegisterDockablePane(dockID, "TwentyTwo DockablePane Sample",
                    window as IDockablePaneProvider);
                
            }
            catch(Exception ex)
            {
                TaskDialog.Show("Error Message", ex.Message);
                return Result.Failed;
            }
            return Result.Succeeded;
        }

    }
    // external command class
    [Transaction(TransactionMode.Manual)]
    public class Show : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                // dockable window id
                DockablePaneId id = new DockablePaneId(new Guid("{29F293CF-5486-45CB-8C10-60CF2A33DDAB}"));
                DockablePane dockableWindow = commandData.Application.GetDockablePane(id);
                dockableWindow.Show();
            }
            catch (Exception ex)
            {
                // show error info dialog
                TaskDialog.Show("Info Message", ex.Message);
            }
            // return result
            return Result.Succeeded;
        }
    }
}
