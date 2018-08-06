using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SpreetailBudget.Models
{
    [Serializable]
    public class Budget : BindableBase
    {
        public Budget()
        {

        }
        public Budget(string title)
        {
            _title = title;
            Categories.CollectionChanged += Categories_CollectionChanged;
        }

        private void Categories_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("TotalBudgetedAmount");
            OnPropertyChanged("TotalSpentAmount");
            OnPropertyChanged("TotalRemainingAmount");
            if (e.NewItems != null)
                foreach (BudgetCategory category in e.NewItems)
                    category.PropertyChanged += Category_PropertyChanged;

            if (e.OldItems != null)
                foreach (BudgetCategory category in e.OldItems)
                    category.PropertyChanged -= Category_PropertyChanged;
        }

        private void Category_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged("TotalBudgetedAmount");
            OnPropertyChanged("TotalSpentAmount");
            OnPropertyChanged("TotalRemainingAmount");
            OnPropertyChanged("CategoryTitle");
        }

        private string _title = "";
        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged("Title"); }
        }

        public float TotalBudgetedAmount
        {
            get { return Categories.Sum(x => x.BudgetedAmount); }
        }

        public float TotalSpentAmount
        {
            get { return Categories.Sum(x => x.SpentAmount); }
        }

        public float TotalRemainingAmount
        {
            get { return TotalBudgetedAmount - TotalSpentAmount; }
        }
        public ObservableCollection<BudgetCategory> Categories { get; } = new ObservableCollection<BudgetCategory>();
        
        
    }
}
