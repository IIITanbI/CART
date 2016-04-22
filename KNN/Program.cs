using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KNN
{

    public class Item
    {
        public Dictionary<string, object> dict;
        public string ClassName;
    }
    public class KNN
    {
        public Func<object, object, double> DistanceFunction { get; set; } = DefaultDistance;
        public List<Item> Data;
        public int K { get; set; } = 1;

        SortedDictionary<double, List<Item>> Distances = new SortedDictionary<double, List<Item>>();

        public string Classify(Item newItem)
        {
            foreach (var item in Data)
            {
                var dist = DistanceFunction(item, newItem);

                if (!Distances.ContainsKey(dist))
                    Distances[dist] = new List<Item>();

                Distances[dist].Add(item);
            }

            int count = 0;

            var knn = new List<Item>();
            foreach (var list in Distances.Values)
            {
                foreach (var item in list)
                {
                    if (count > this.K) break;
                    knn.Add(item);
                    count++;
                }
            }

            string res = Votes(knn, newItem);
            return res;
        }

        public string Votes(List<Item> items, Item newItem)
        {
            var map = new Dictionary<string, double>();

            var classes = new List<string>();
            foreach (var item in items)
            {
                var _class = item.ClassName;
                map[_class] = 0;
                classes.Add(_class);
            }


            foreach(var _class in classes)
            {
                double vote = 0;
                foreach(var item in items)
                {
                    double dist = DefaultDistance(item, newItem);
                    double sqr_dist = dist * dist;
                    vote += 1.0 / sqr_dist;
                }
                map[_class] = vote;
            }

            double max = 0;
            string _classify = null;
            foreach (var pair in map)
            {
                if (pair.Value > max)
                {
                    max = pair.Value;
                    _classify = pair.Key;
                }
            }

            return _classify;
        }


        public static double DefaultDistance(object a, object b)
        {
            return 2.0;
        }
    }
}
