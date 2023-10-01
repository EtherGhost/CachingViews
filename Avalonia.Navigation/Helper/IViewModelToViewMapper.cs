using System;

namespace Avalonia.Navigation.Helper;

public interface IViewModelToViewMapper
{
    void MapViewModelToView(Type viewModelType, Type viewType);
    Type GetViewType(Type viewModelType);
}
