using System;
using Avalonia.Controls;
using Avalonia.Navigation.ViewModel;

namespace Avalonia.Navigation.Helper;

public interface IViewModelToViewMapper
{
    void MapViewModelToView<TViewModel, TView>(Func<TViewModel, TView> factory) 
        where TViewModel : DetailViewModelBase
        where TView : Control;

    Control CreateView<TViewModel>(TViewModel viewModel) where TViewModel : DetailViewModelBase;
}
