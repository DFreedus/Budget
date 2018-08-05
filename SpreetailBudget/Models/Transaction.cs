using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreetailBudget.Models
{
    public class Transaction : BindableBase
    {
        public Transaction()
        {
            _description = "";
            _amount = 0;
            _categoryID = 0;
        }

        private int _categoryID;
        public int CategoryID
        {
            get { return _categoryID; }
            set { _categoryID = value; OnPropertyChanged("CategoryID"); }
        }
        private string _description;
        public string Description
        {
            get { return _description; }
            set { _description = value; OnPropertyChanged("Description"); }
        }

        private float _amount;
        public float Amount
        {
            get { return _amount; }
            set { _amount = value; OnPropertyChanged("Amount"); }
        }

        private DateTime? _processDate = DateTime.Now;
        public DateTime? ProcessDate
        {
            get { return _processDate; }
            set { _processDate = value; OnPropertyChanged("ProcessDate"); }
        }

        private bool _isFiltered = false;
        public bool IsFiltered
        {
            get { return _isFiltered; }
            set { _isFiltered = value; OnPropertyChanged("IsFiltered"); }
        }
    }
}
