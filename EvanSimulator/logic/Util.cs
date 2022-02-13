using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvanSimulator.logic
{
    internal static class Util
    {
        public static string RandomString(Form game, int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[game.random.Next(s.Length)]).ToArray());
        }

        public static KeyValuePair<TA, TB> RandomDictItem<TA, TB>(Form game, Dictionary<TA, TB> dict) where TA : class where TB : class
        {
            return dict.ElementAt(game.random.Next(0, dict.Count));
        }

    }
}
