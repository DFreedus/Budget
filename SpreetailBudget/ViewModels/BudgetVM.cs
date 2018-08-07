using SpreetailBudget.Command;
using SpreetailBudget.Extensions;
using SpreetailBudget.Models;
using SpreetailBudget.Serialize;
using SpreetailBudget.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SpreetailBudget.ViewModels
{
    public class BudgetVM : BaseVM
    {
        public BudgetVM()
        {
            Budget = new Budget("New Budget");

        }

        #region Commands

        private ICommand _addCategoryCommand;
        public object AddCategoryCommand
        {
            get
            {
                return _addCategoryCommand ?? (_addCategoryCommand = new RelayCommand(x => { AddCategory(); }));
            }
        }

        private ICommand _saveBudgetCommand;
        public object SaveBudgetCommand
        {
            get
            {
                return _saveBudgetCommand ?? (_saveBudgetCommand = new RelayCommand(x => { SaveBudget(); }));
            }
        }

        

        private ICommand _loadExistingBudgetCommand;
        public object LoadExistingBudgetCommand
        {
            get
            {
                return _loadExistingBudgetCommand ?? (_loadExistingBudgetCommand = new RelayCommand(x => { LoadBudget(); }));
            }
        }

        private ICommand _clearCurrentBudgetCommand;
        public object ClearCurrentBudgetCommand
        {
            get
            {
                return _clearCurrentBudgetCommand ?? (_clearCurrentBudgetCommand = new RelayCommand(x => { ClearBudget(); }));
            }
        }

        

        private ICommand _newTransactionCommand;
        public object NewTransactionCommand
        {
            get
            {
                return _newTransactionCommand ?? (_newTransactionCommand = new RelayCommand(x => { NewTransaction(); }));
            }

        }

        private ICommand _removeTransactionCommand;
        public object RemoveTransactionCommand
        {
            get
            {
                return _removeTransactionCommand ?? (_removeTransactionCommand = new RelayCommand(x => { RemoveTransaction(SelectedTransaction); }));
            }
        }

        private ICommand _removeBudgetTransaction;
        public object RemoveBudgetTransaction
        {
            get
            {
                return _removeBudgetTransaction ?? (_removeBudgetTransaction = new RelayCommand(x => { RemoveBudgetTransactions(SelectedBudgetTransaction); }));
            }
        }
        
        private ICommand _addTransactionsToBudgetCommand;
        public object AddTransactionsToBudgetCommand
        {
            get
            {
                return _addTransactionsToBudgetCommand ?? (_addTransactionsToBudgetCommand = new RelayCommand(x => { AddTransactionsToBudget(); }));
            }
        }

        private ICommand _clearCategoryFilterCommand;
        public object ClearCategoryFilterCommand
        {
            get
            {
                return _clearCategoryFilterCommand ?? (_clearCategoryFilterCommand = new RelayCommand(x => { ClearCategoryFilter(); }));
            }
        }

        private ICommand _clearDateRangeFilterCommand;
        public object ClearDateRangeFilterCommand
        {
            get
            {
                return _clearDateRangeFilterCommand ?? (_clearDateRangeFilterCommand = new RelayCommand(x => { ClearDateFilter(); }));
            }
        }

        #endregion Commands

        #region command execution methods
        private void SaveBudget()
        {
            ClearDateFilter();
            CategoryFilter = null;
            OnPropertyChanged("FilteredBudgetTransactions");
            Serializer.Serialize(Budget);
        }
        private void LoadBudget()
        {
            var budget = Serializer.LoadBudget();
            if (budget != null)
                Budget = budget;
            OnPropertyChanged("BudgetCategories");
            OnPropertyChanged("BudgetTransactions");
            OnPropertyChanged("FilteredBudgetTransactions");

        }

        private void ClearBudget()
        {
            var messageBoxResult = MessageBox.Show("Are you sure you want to clear your current budget?\nThis action can't be undone!", "Proceed?", MessageBoxButton.YesNo);
            switch (messageBoxResult)
            {
                case (MessageBoxResult.Yes):
                    {
                        foreach (var category in Budget.Categories)
                            category.Transactions.Clear();
                        Budget.Categories.Clear();
                        OnPropertyChanged("BudgetCategories");
                        OnPropertyChanged("BudgetTransactions");
                        OnPropertyChanged("FilteredBudgetTransactions");
                        ClearDateFilter();
                        break;
                    }
                default:
                    return;
            }
        }

        private void NewTransaction()
        {
            var transaction = new Transaction();
            AllTransactions.Add(transaction);
        }

        private void AddTransactionsToBudget()
        {
            foreach (var transaction in AllTransactions)
            {
                var category = AllCategories.FirstOrDefault(x => x.ID == transaction.CategoryID);
                if (category != null)
                    category.Transactions.Add(transaction);
            }
            
            OnPropertyChanged("Budget");
            OnPropertyChanged("BudgetCategories");
            OnPropertyChanged("BudgetTransactions");
            OnPropertyChanged("FilteredBudgetTransactions");
            AllTransactions.Clear();
        }

        private void RemoveTransaction(Transaction selectedTransaction)
        {
            if (selectedTransaction == null)
                return;
            else
                AllTransactions.Remove(selectedTransaction);
        }

        private void RemoveBudgetTransactions(Transaction selectedBudgetTransaction)
        {
            if (selectedBudgetTransaction == null)
                return;
            else
            {
                var category = Budget.GetCategoryThatContainsTransaction(selectedBudgetTransaction);
                if (category != null)
                {
                    category.Transactions.Remove(selectedBudgetTransaction);
                    OnPropertyChanged("FilteredBudgetTransactions");
                    OnPropertyChanged("BudgetCategories");
                    OnPropertyChanged("Budget");
                }
            }
        }

        private void AddCategory()
        {
            Budget.Categories.Add(new BudgetCategory(Budget.Categories.Count() + 1, ""));
            OnPropertyChanged("Budget");
            OnPropertyChanged("BudgetCategories");
        }

        private void ClearCategoryFilter()
        {
            CategoryFilter = null;
        }

        private void ClearDateFilter()
        {
            FromDateFilter = ToDateFilter = null;

        }
        #endregion command execution methods

        #region ObservableCollections
        private Dictionary<int, string> _transactionCategories = new Dictionary<int, string>();
        public Dictionary<int, string> TransactionCategories
        {
            get
            {
                _transactionCategories.Clear();
                AllCategories.ToList().ForEach(x => _transactionCategories.Add(x.ID, x.CategoryTitle));
                return _transactionCategories;
            }
        }

        public ObservableCollection<Transaction> AllTransactions { get; } = new ObservableCollection<Transaction>();

        private ObservableCollection<BudgetCategory> _allCategories = new ObservableCollection<BudgetCategory>();
        public ObservableCollection<BudgetCategory> AllCategories
        {
            get
            {
                _allCategories.Clear();
                foreach (var category in Budget.Categories)
                {
                    _allCategories.Add(category);
                }

                return _allCategories;
            }
        }
        private ObservableCollection<BudgetCategory> _budgetCategories = new ObservableCollection<BudgetCategory>();
        public ObservableCollection<BudgetCategory> BudgetCategories
        {
            get
            {
                _budgetCategories.Clear();
                foreach (var category in Budget.Categories)
                {
                    _budgetCategories.Add(category);
                }
                return _budgetCategories;
            }
        }

        private ObservableCollection<Transaction> _budgetTransactions = new ObservableCollection<Transaction>();
        public ObservableCollection<Transaction> BudgetTransactions
        {
            get
            {
                _budgetTransactions.Clear();
                foreach (var category in Budget.Categories)
                {
                    foreach (var transaction in category.Transactions)
                        _budgetTransactions.Add(transaction);
                }
                return _budgetTransactions;
            }
        }

        private ObservableCollection<Transaction> _filteredBudgetTransactions = new ObservableCollection<Transaction>();
        public ObservableCollection<Transaction> FilteredBudgetTransactions
        {
            get
            {
                _filteredBudgetTransactions.Clear();
                foreach (var category in Budget.Categories)
                {
                    foreach (var transaction in category.Transactions.Where(x => x.IsVisible))
                    {
                        _filteredBudgetTransactions.Add(transaction);
                    }
                }
                return _filteredBudgetTransactions;
            }
        }

        #endregion ObservableCollections

        private Transaction _selectedTransaction;
        public Transaction SelectedTransaction
        {
            get { return _selectedTransaction; }
            set { _selectedTransaction = value; OnPropertyChanged("SelectedTransactions"); }
        }

        private Transaction _selectedBudgetTransaction;
        public Transaction SelectedBudgetTransaction
        {
            get { return _selectedBudgetTransaction; }
            set { _selectedBudgetTransaction = value; OnPropertyChanged("SelectedBudgetTransaction"); }
        }

        private BudgetCategory _categoryFilter;
        public BudgetCategory CategoryFilter
        {
            get { return _categoryFilter; }
            set
            {
                _categoryFilter = value;
                OnPropertyChanged("CategoryFilter");
                FilterTransactions();
            }
        }

        private DateTime? _fromDateFilter;
        public DateTime? FromDateFilter
        {
            get { return _fromDateFilter; }
            set
            {
                _fromDateFilter = value;
                OnPropertyChanged("FromDateFilter");
                if (_toDateFilter != null)
                    FilterTransactions();
            }
        }

        private DateTime? _toDateFilter;
        public DateTime? ToDateFilter
        {
            get { return _toDateFilter; }
            set
            {
                _toDateFilter = value;
                OnPropertyChanged("ToDateFilter");
                if (_fromDateFilter != null)
                    FilterTransactions();
            }
        }
       
        private void FilterTransactions()
        {
            BudgetTransactions.ToList().ForEach(x => x.IsVisible = true);

            if (_fromDateFilter != null && _toDateFilter != null)
            {
                BudgetTransactions.Where(y => y.ProcessDate < _fromDateFilter || y.ProcessDate > _toDateFilter).ToList().ForEach(x => x.IsVisible = false);
            }
            if (CategoryFilter != null)
            {
                BudgetTransactions.Where(y => y.CategoryID != CategoryFilter.ID).ToList().ForEach(x => x.IsVisible = false);
            }
            OnPropertyChanged("FilteredBudgetTransactions");
        }

        private Budget _budget;
        public Budget Budget { get { return _budget; } set { _budget = value; OnPropertyChanged("Budget"); } }
    }
}
