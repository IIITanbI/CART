using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBc
{
    public class NBC
    {
        public class MyPair<TKey, TValue> : IComparable
            where TKey : IComparable, IComparable<TKey>
            where TValue : IComparable, IComparable<TValue>
        {
            public TKey Key { get; set; }
            public TValue Value { get; set; }
            public MyPair(TKey key, TValue value)
            {
                this.Key = key;
                this.Value = value;
            }

            public int CompareTo(object obj)
            {
                return CompareTo(obj as MyPair<TKey, TValue>);
            }
            public int CompareTo(MyPair<TKey, TValue> other)
            {
                int kc = Key.CompareTo(other.Key);
                int vc = Value.CompareTo(other.Value);

                if (kc == vc)
                    return kc;
                if (kc == 1)
                    return kc;
                if (kc == 0)
                    return vc;
                if (kc == -1)
                    return kc;

                return Key.CompareTo(other.Key);
            }
        }

        

        public List<char> GetFeatures(string name)
        {
            return new List<char>() {name.Last() };
            //return new List<char>() { name[0], name[1], name.Last() };
        }
        public Tuple<SortedDictionary<char, double>, SortedDictionary<MyPair<char, char>, double>> Train(List<Tuple<string, char>> names)
        {
            var classes = new SortedDictionary<char, double>();
            var freq = new SortedDictionary<MyPair<char, char>, double>();
            foreach (var pair in names)
            {
                string name = pair.Item1;
                char label = pair.Item2;

                if (!classes.ContainsKey(label))
                    classes[label] = 0;
                classes[label]++;

                foreach (var feature in GetFeatures(name))
                {
                    var kp = new MyPair<char, char>(label, feature);
                    if (!freq.ContainsKey(kp))
                        freq[kp] = 0;
                    freq[kp]++;
                }
            }


            foreach (var pair in freq.Keys.ToList())
            {
                freq[pair] = freq[pair] / classes[pair.Key];
            }
            foreach (var c in classes.Keys.ToList())
            {
                classes[c] /= names.Count;
            }
            return new Tuple<SortedDictionary<char, double>, SortedDictionary<MyPair<char, char>, double>>(classes, freq);
        }

        public char Classify(Tuple<SortedDictionary<char, double>, SortedDictionary<MyPair<char, char>, double>> classifier, List<char> feats)
        {
            var classes = classifier.Item1;
            var prob = classifier.Item2;

            double _min = 1e10;
            char _val = ' ';
            foreach (var c1 in classes.Keys)
            {
                double res = -Math.Log(classes[c1]);
                double res1 = 0;
                foreach (var feat in feats)
                {
                    var pp = new MyPair<char, char>(c1, feat);
                    if (prob.ContainsKey(pp))
                        res1 += -Math.Log(prob[pp]);
                    else
                        res1 += -Math.Log(1e-7);
                }

                res += res1;
                if (res < _min)
                {
                    _min = res;
                    _val = c1;
                }
            }
            return _val;
        }
    }
}
