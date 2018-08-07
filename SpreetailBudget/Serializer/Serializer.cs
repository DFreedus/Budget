using SpreetailBudget.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Serialization;

namespace SpreetailBudget.Serialize
{
    public class Serializer
    {
        private const string PATH_TO_SERIALIZE = "..\\..\\SavedBudgets\\budget.xml";
        public static void Serialize(Budget budget)
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(Budget));
                TextWriter tw = new StreamWriter(PATH_TO_SERIALIZE);
                xs.Serialize(tw, budget);

                tw.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Failed To Save Budget");
            }

        }

        public static Budget LoadBudget()
        {
            Budget budget = null;

            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(Budget));

                using (var sr = new StreamReader(PATH_TO_SERIALIZE))
                {
                    budget = (Budget)xs.Deserialize(sr);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Failed To Load Budget");
            }

            return budget;
        }


    }
}
