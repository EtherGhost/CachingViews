using System;
using System.Collections.Generic;
using System.Linq;
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

    public ViewLocator(IEventAggregator eventAggregator, 
        IViewModelToViewMapper viewModelToViewMapper)
    {
        _viewModelToViewMapper = viewModelToViewMapper;
        
        eventAggregator.GetEvent<AfterDetailClosedEvent>().Subscribe(AfterDetailClosed);
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
                var view = CreateInstance(viewModel);
                var newCacheEntry = new ViewCacheEntry(view, viewModelName, id);
                _viewCache.Add(newCacheEntry);
                return view;
            }
        }

        return new TextBlock { Text = "Not Found" };
    }
    
    public bool SupportsRecycling => false;

    private Type GetViewType(Type viewModelType)
    {
        return _viewModelToViewMapper.GetViewType(viewModelType);
    }

    private void AfterDetailClosed(AfterDetailClosedEventArgs args)
    {
        var cacheEntry = _viewCache.FirstOrDefault(entry =>
            entry.ViewModelName == args.ViewModelName && entry.Id == args.Id);
        _viewCache.Remove(cacheEntry);
    }
    
    private static Control CreateInstance(DetailViewModelBase viewModel)
    {
        if (viewModel is ProjectDetailViewModel)
        {
            return new ProjectDetailView();
        }
        if (viewModel is SystemDetailViewModel)
        {
            return new SystemDetailView();
        }
        
        return new TextBlock { Text = "Not Found" };
    }
}