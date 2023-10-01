using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Navigation.Event;
using Avalonia.Navigation.View;
using Avalonia.Navigation.ViewModel;
using Prism.Events;

namespace Avalonia.Navigation.Helper;

public sealed class ViewLocator : IDataTemplate
{
    private readonly List<ViewCacheEntry> _viewCache = new List<ViewCacheEntry>();
    private readonly IViewModelToViewMapper _viewModelToViewMapper;

    public ViewLocator()
    {
        var container = ((App) Application.Current).Container;
        var eventAggregator = container.Resolve<IEventAggregator>();
        eventAggregator.GetEvent<AfterDetailClosedEvent>().Subscribe(AfterDetailClosed);
        
        var viewModelToViewMapper = container.Resolve<IViewModelToViewMapper>();
        viewModelToViewMapper.MapViewModelToView(typeof(ProjectDetailViewModel), typeof(ProjectDetailView));
        viewModelToViewMapper.MapViewModelToView(typeof(SystemDetailViewModel), typeof(SystemDetailView));
            
        _viewModelToViewMapper = viewModelToViewMapper;
    }

    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }

    Control? ITemplate<object?, Control?>.Build(object? data)
    {
        if (data is DetailViewModelBase viewModel)
        {
            var viewType = GetViewType(viewModel.GetType());
            var viewModelName = viewModel.GetType().Name;
            var id = viewModel.Id;

            var cacheEntry = _viewCache.FirstOrDefault(entry =>
                entry.ViewModelName == viewModelName && entry.Id == id);

            if (!EqualityComparer<ViewCacheEntry>.Default.Equals(cacheEntry, default))
            {
                return cacheEntry.View;
            }

            if (viewType != null)
            {
                var view = (Control)Activator.CreateInstance(viewType);
                var newCacheEntry = new ViewCacheEntry(view, viewModelName, id);
                _viewCache.Add(newCacheEntry);
                return view;
            }
        }

        return new TextBlock { Text = "Not Found" };
    }

    public void AfterDetailClosed(AfterDetailClosedEventArgs args)
    {
        var cacheEntry = _viewCache.FirstOrDefault(entry =>
            entry.ViewModelName == args.ViewModelName && entry.Id == args.Id);
        _viewCache.Remove(cacheEntry);
    }
    
    public bool SupportsRecycling => false;

    private Type GetViewType(Type viewModelType)
    {
        return _viewModelToViewMapper.GetViewType(viewModelType);
    }
}