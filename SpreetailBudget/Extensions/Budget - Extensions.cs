using SpreetailBudget.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreetailBudget.Extensions
{
    public static class Budget___Extensions
    {
        public static BudgetCategory GetCategoryThatContainsTransaction(this Budget budget, Transaction transaction)
        {
            if (transaction != null)
            {
                foreach (var category in budget.Categories)
                {
                    if (category.Transactions.Contains(transaction))
                    {
                        return category;
                    }
                }
            }
            return null;
        }
    }
}
