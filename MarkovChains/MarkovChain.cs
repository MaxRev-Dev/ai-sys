using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChainDict = System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, uint>>;
using WeightDict = System.Collections.Generic.Dictionary<string, uint>;

namespace MarkovChains
{
    public class MarkovChain
    {
        private readonly Random _rand = new Random();
        private readonly string nullW = "";
        private readonly char sep = ' ';

        public ChainDict Learn(string text, int Ngram)
        {
            var dict = new ChainDict();
            var prev = nullW;
            foreach (var word in GetChunk(text, Ngram))
            {
                if (dict.ContainsKey(prev))
                {
                    var w = dict[prev];
                    if (w.ContainsKey(word))
                        w[word] += 1;
                    else
                        w.Add(word, 1);
                }
                else
                    dict.Add(prev, new WeightDict { { word, 1 } });

                prev = word;
            }

            return dict;
        }

        private IEnumerable<string> GetChunk(string text, int Ngram)
        {
            var ls = text.Split(new[] { sep }, StringSplitOptions.RemoveEmptyEntries);

            for (var i = 0; i < ls.Length - Ngram; ++i)
            {
                var sb = new StringBuilder();
                sb.Append(ls.Skip(i).Take(Ngram).Aggregate((w, k) => w + sep + k));
                yield return sb.ToString();
            }
        }

        public string GenerateString(ChainDict dict, int len, bool preserveLength)
        {
            var sb = new StringBuilder();

            var ucStr = dict.Keys.Skip(1).Where(word =>
                char.IsUpper(word.First())).ToList();

            if (ucStr.Count > 0)
                sb.Append(ucStr.ElementAt(_rand.Next(0, ucStr.Count)));

            var last = sb.ToString();
            sb.Append(" ");

            WeightDict w;

            for (uint i = 0; i < len; ++i)
            {
                w = dict.ContainsKey(last) ? dict[last] : dict[nullW];
                last = Choose(w);
                sb.Append(last.Split(sep).Last()).Append(sep);
            }

            if (preserveLength) return sb.ToString();

            while (last.Any() && last.Last() != '.')
            {
                w = dict.ContainsKey(last) ? dict[last] : dict[nullW];
                last = Choose(w);
                sb.Append(last.Split(sep).Last()).Append(sep);
            }

            return sb.ToString();
        }

        private string Choose(WeightDict dict)
        {
            var total = dict.Sum(t => t.Value);

            while (true)
            {
                var k = dict.ElementAt(_rand.Next(0, dict.Count));
                if (_rand.NextDouble() < (double)k.Value / total)
                    return k.Key;
            }
        }
    }
}