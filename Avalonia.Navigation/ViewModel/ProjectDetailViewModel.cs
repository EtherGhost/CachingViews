using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Navigation.Model;
using Prism.Events;

namespace Avalonia.Navigation.ViewModel
{
    public class ProjectDetailViewModel : DetailViewModelBase
    {
        private Project _project;

        public ProjectDetailViewModel(IEventAggregator eventAggregator) 
            : base(eventAggregator)
        {
            
            Managers = new ObservableCollection<Manager>();
            Projects = new List<Project>();
        }

        public override void LoadAsync(int id)
        {
            Id = id;
            
            for (var i = 0; i < 3; i++)
            {
                Managers.Add(new Manager
                {
                    Id = i,
                    Name = $"Manager {i}"
                });
            }

            for (var i = 0; i < 3; i++)
            {
                Projects.Add(new Project
                {
                    Id = i,
                    Name = $"Project {i}",
                    ManagerId = Managers[i].Id
                });
            }
            
            Project = Projects.FirstOrDefault(p => p.Id == id);
            
            Title = Project.Name;
        }

        public Project Project
        {
            get => _project;
            private set
            {
                if (Equals(value, _project)) 
                    return;
                _project = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Manager> Managers { get; }
        public List<Project> Projects { get; }

        protected override Task OnSaveExecuteAsync()
        {
            throw new System.NotImplementedException();
        }

        protected override Task OnDeleteExecuteAsync()
        {
            throw new System.NotImplementedException();
        }

        protected override void OnResetExecute()
        {
            throw new System.NotImplementedException();
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