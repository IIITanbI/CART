using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DT_Algorithm;
using System.IO;

namespace CART
{
    class Program
    {
        static void Main(string[] args)
        {
            Tree tree = new Tree();
            DataSet dataSet = new DataSet();


            var lines = File.ReadAllLines("input.txt");

            var ht = lines[0].Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
            var hn = lines[1].Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);


            if (hn.Length != ht.Length)
            {
                throw new ArgumentException("azaza");
            }

            foreach (var line in lines.Skip(2))
            {
                var item = new Item();

                var values = line.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (values.Length != ht.Length)
                {
                    throw new ArgumentException("azaza2");
                }

                for (int i = 0; i < values.Length; i++)
                {
                    string name = hn[i];
                    string value = values[i];
                    AttributeType type;
                    if (ht[i] == "c") type = AttributeType.CategoricalNominal;
                    else type = AttributeType.Numerical;

                    item.Attributes[name] = new MyAttribute(name, value, type);
                }

                dataSet.Items.Add(item);
            }

            tree.Data = dataSet;
            tree.ClassificationAttributeName = hn.Last();
            tree.Build();
            //var gini = Utility.Gini(dataSet, hn.Last());

            Console.WriteLine();
        }
    }
}
