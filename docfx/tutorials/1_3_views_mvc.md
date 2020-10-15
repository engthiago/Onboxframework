# WPF Views and MVC

Onbox ships with some MVC libraries based on WPF by default and it provides a lot of features out the box like:

- Hooks up to Revit main window handler automatically.
- Resolves View instances using IOC Container.
- Lifecycle hooks: `OnInit`, `OnInitAsync`, `RunOnInitFunc`, `OnAfterInit`, and `OnDestroy`.
- Provides interceptor for `CanClose` dialog.
- Provides functionality for async load resources, like http calls, hooking up loading spinners, error icons and ability to hide its contents depending on error or load states.
- Provides navigation functionality.
- Progress indicator that works nicely with Revit's threading particularities.
- Adds some cool custom components that have Revit's "look and feel".

The main `Onbox.MVC` library is designed with Revit in mind but it can actually be used on any regular Windows WPF application as it has no refence to Revit on its code. In the other hand `Onbox.MVC.Revit` provides functionality specific Revit functionality like the progress indicator mentioned above.

## Exploring "HelloWPFView" Window

1. Going back to Visual Studio, open **WPFViewCommand.cs** under **Revit->Commands** folder. This command is very similar to the one we explored before, it is just showing a WPF abstracted window instead:

``` C#
    [Transaction(TransactionMode.Manual)]
    public class WPFViewCommand : RevitAppCommand<App>
    {
        public override Result Execute(IContainerResolver container, ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Asks the container for a new instance of a view
            var wpfView = container.Resolve<IHelloWpfView>();
            wpfView.ShowDialog();

            return Result.Succeeded;
        }
    }
```

2. Now go to **Views** folder and open **HellopWPFView.xaml**. This view is very similar to a regular WPF view but instead of deriving from ``Window`` it derives from ``RevitMvcViewBase``.

``` C#
<rmvc:RevitMvcViewBase x:Class="MyFirstOnboxRevitApp.Views.HelloWpfView"
					   DataContext="{Binding RelativeSource={RelativeSource Self}}"
					   xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
					   xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"

                       //... Omitted to fit in this documentation
```

3. Hit **F7** on your keyboard to access this view`s code behind. Notice that we have an interface to define the abstraction for HellopWpfView:

``` C#

    /// <summary>
    /// A contract a view designed to have Revit as parent window
    /// </summary>
    public interface IHelloWpfView : IRevitMvcViewBase, IMvcViewModal
    {
    }

```

Idealy this interface should be moved to another project where you will maintain all the abstractions for your application, Onbox will pre-define this interface, but it is up to the developer to move it to where it should be depending on the application's architecture.

4. Have a look on the ``HelloWpfView`` partial class:

``` C#

    /// <summary>
    /// A view designed to have Revit as parent window
    /// </summary>
    public partial class HelloWpfView : RevitMvcViewBase, IHelloWpfView
    {
        public string AppName { get; set; }

        // You can inject any service that you have added to the container in constructors
        public HelloWpfView(IRevitAppData revitAppData) : base(revitAppData)
        {
            InitializeComponent();
            AppName = Assembly.GetExecutingAssembly().GetName().Name;
        }
    }

```

You can see that the constructor for this class takes an ``IRevitAppData`` that the container injects for us. This helps you get information about Revit's running process and to hook up this View to Revit main Window handler.

## Conclusion

Congratulations! Now you have the basic knowlodge of how Onbox works, you have explored some of its functionalities and hopefuly are starting to grasp some of its benefits. If you are a seasoned Web Developer, all these must be very familiar to you, if you only surfed the waters of Revit API or is fairly new to programming, you might be a bit lost, but don't give up, there are indeed lots of great things comming for you. 

And... our journey doesn't end here, let's build something new now!