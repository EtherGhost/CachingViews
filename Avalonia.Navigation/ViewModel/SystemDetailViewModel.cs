using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Prism.Events;

namespace Avalonia.Navigation.ViewModel
{
    public class SystemDetailViewModel : DetailViewModelBase
    {
        private Model.System _system;

        public SystemDetailViewModel(IEventAggregator eventAggregator)
            : base(eventAggregator)
        {
            Systems = new List<Model.System>();
        }

        public override void LoadAsync(int id)
        {
            Id = id;
            
            for (var i = 0; i < 3; i++)
            {
                Systems.Add(new Model.System
                {
                    Id = i,
                    Name = $"System {i}"
                });
            }
            
            System = Systems.FirstOrDefault(p => p.Id == id);
            
            Title = System.Name;
        }

        public Model.System System
        {
            get => _system;
            private set
            {
                if (Equals(value, _system)) 
                    return;
                _system = value;
                OnPropertyChanged();
            }
        }

        public List<Model.System> Systems { get; }

        protected override Task OnSaveExecuteAsync()
        {
            throw new NotImplementedException();
        }

        protected override Task OnDeleteExecuteAsync()
        {
            throw new NotImplementedException();
        }

        protected override void OnResetExecute()
        {
            throw new NotImplementedException();
        }

        protected override bool OnSaveCanExecute(object arg)
        {
            return false;
        }

        protected override bool OnDeleteCanExecute(object arg)
        {
            return false;
        }

        protected override bool OnResetCanExecute()
        {
            return false;
        }
    }
}