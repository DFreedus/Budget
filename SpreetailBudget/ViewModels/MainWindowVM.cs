using SpreetailBudget.Command;
using SpreetailBudget.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SpreetailBudget.ViewModels
{
    public class MainWindowVM : BaseVM
    {

        protected object _homeView;
        protected object _budgetView;

        public MainWindowVM()
        {
            _homeView = new HomeView();
            _budgetView = new BudgetView();

            SelectedView = _homeView;
        }

        
        private ICommand _switchToBudgetViewCommand;
        public object SwitchToBudgetViewCommand
        {
            get
            {
                return _switchToBudgetViewCommand ?? (_switchToBudgetViewCommand = new RelayCommand(x => { SwitchToBudgetView(); }));
            }
        }

        private ICommand _exitApplicationCommand;
        public object ExitApplicationCommand
        {
            get
            {
                return _exitApplicationCommand ?? (_exitApplicationCommand = new RelayCommand(x => { ExitApplication(); }));
            }
        }

        private object _selectedView;
        public object SelectedView
        {
            get { return _selectedView; }
            set
            {
                _selectedView = value;
                OnPropertyChanged("SelectedView");
            }
        }

        private void ExitApplication()
        {
            Application.Current.Shutdown();
        }
        private void SwitchToBudgetView()
        {
            SelectedView = _budgetView;
        }
    }
}
