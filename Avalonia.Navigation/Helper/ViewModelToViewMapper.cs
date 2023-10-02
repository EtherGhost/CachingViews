using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Navigation.ViewModel;

namespace Avalonia.Navigation.Helper;

public class ViewModelToViewMapper : IViewModelToViewMapper
{
    private readonly Dictionary<Type, Func<DetailViewModelBase, Control>> _viewModelToViewMap = 
        new Dictionary<Type, Func<DetailViewModelBase, Control>>();

    public void MapViewModelToView<TViewModel, TView>(Func<TViewModel, TView> factory) 
        where TViewModel : DetailViewModelBase
        where TView : Control
    {
        _viewModelToViewMap[typeof(TViewModel)] = vm => factory((TViewModel)vm);
    }

    public Control CreateView<TViewModel>(TViewModel viewModel) where TViewModel : DetailViewModelBase
    {
        var viewModelType = viewModel.GetType();

        if (_viewModelToViewMap.TryGetValue(viewModelType, out var factory))
        {
            return factory(viewModel);
        }

        return new TextBlock { Text = "Not Found" };
    }

}