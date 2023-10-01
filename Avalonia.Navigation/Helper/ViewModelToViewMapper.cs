using System;
using System.Collections.Generic;

namespace Avalonia.Navigation.Helper;

public class ViewModelToViewMapper : IViewModelToViewMapper
{
    private readonly Dictionary<Type, Type> _viewModelToViewMap = new Dictionary<Type, Type>();

    public void MapViewModelToView(Type viewModelType, Type viewType)
    {
        _viewModelToViewMap[viewModelType] = viewType;
    }

    public Type GetViewType(Type viewModelType)
    {
        if (_viewModelToViewMap.TryGetValue(viewModelType, out Type viewType))
        {
            return viewType;
        }
        return null;
    }
}
