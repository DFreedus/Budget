using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SpreetailBudget.Models
{
    [XmlInclude(typeof(BudgetCategory))]
    public class BudgetCategory : BindableBase
    {
        private static Budget _budget;

       
        public BudgetCategory()
        {

        }
        public BudgetCategory(Budget budget, int id)
        {
            _budget = budget;
            _id = id;

            Transactions.CollectionChanged += Transactions_CollectionChanged;
        }

        public BudgetCategory(Budget budget, int id, String title)
            : this(budget, id)
        {
            _categoryTitle = title;
            _budgetedAmount = 0;
        }

        private void Transactions_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged("SpentAmount");
            OnPropertyChanged("RemainingAmount");
            OnPropertyChanged("IsPositiveNegative");
            OnPropertyChanged("BudgetTransactions");
        }


        private int _id;
        public int ID
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged("ID"); }
        }

        private string _categoryTitle = "";
        public string CategoryTitle
        {
            get { return _categoryTitle; }
            set { _categoryTitle = value; OnPropertyChanged("CategoryTitle"); }
        }

        private float _budgetedAmount;

        public float BudgetedAmount
        {
            get { return _budgetedAmount; }
            set
            {
                _budgetedAmount = value;
                OnPropertyChanged("BudgetedAmount");
                OnPropertyChanged("RemainingAmount");
                OnPropertyChanged("IsPositiveNegative");
                OnPropertyChanged("TotalBudgetedAmount");
            }
        }

        public string IsPositiveNegative
        {
            get
            {
                if (RemainingAmount < 0)
                    return "-1";
                if (RemainingAmount > 0)
                    return "1";
                else
                    return "0";
            }
        }

        public float SpentAmount
        {
            get { return Transactions.Sum(x => x.Amount);  }
        }

        public float RemainingAmount => BudgetedAmount - SpentAmount;

        public ObservableCollection<Transaction> Transactions { get; } = new ObservableCollection<Transaction>();

    }
}
