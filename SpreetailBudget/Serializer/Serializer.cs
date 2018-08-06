using SpreetailBudget.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            catch (Exception e) { }

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
            catch (Exception e) { }

            return budget;
        }


    }
}
