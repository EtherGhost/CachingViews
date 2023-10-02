using Autofac;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Navigation.Helper;
using Avalonia.Navigation.Startup;
using Avalonia.Navigation.View;
using Avalonia.Navigation.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using Prism.Events;

namespace Avalonia.Navigation
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.BuildServiceProvider();
            
            var bootstrapper = new Bootstrapper(serviceCollection);
            Container = bootstrapper.Bootstrap();
            
            var eventAggregator = Container.Resolve<IEventAggregator>();
            var viewModelToViewMapper = Container.Resolve<IViewModelToViewMapper>();
            
            MapViewModelsToViews(viewModelToViewMapper);

            var locator = new ViewLocator(eventAggregator, viewModelToViewMapper);
            DataTemplates.Add(locator);

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = Container.Resolve<MainWindow>();
            }

            base.OnFrameworkInitializationCompleted();
        }

        public IContainer Container { get; private set; }

        private static void MapViewModelsToViews(IViewModelToViewMapper viewModelToViewMapper)
        {
            viewModelToViewMapper.MapViewModelToView<ProjectDetailViewModel, ProjectDetailView>(vm => new ProjectDetailView());
            viewModelToViewMapper.MapViewModelToView<SystemDetailViewModel, SystemDetailView>(vm => new SystemDetailView());
        }
    }
}