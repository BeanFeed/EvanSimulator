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

        public static float DstForm(float x1, float x2, float y1, float y2)
        {
            return MathF.Sqrt(
                MathF.Pow(x1 - x2, 2) +
                MathF.Pow(y1 - y2, 2));
        }

        public static float DstForm(PointF coord1, PointF coord2)
        {
            return MathF.Sqrt(
                MathF.Pow(coord1.X - coord2.X, 2) +
                MathF.Pow(coord2.Y - coord2.Y, 2));
        }

        public static PointF AddPositions(PointF a, PointF b)
        {
            return new PointF(
                a.X + b.X,
                a.Y + b.Y
            );
        }

        public static PointF SubtractPositions(PointF a, PointF b)
        {
            return new PointF(
                a.X - b.X,
                a.Y - b.Y
            );
        }

        public static PointF ScaleVector(PointF vec, float amt)
        {
            vec.X *= amt;
            vec.Y *= amt;

            return vec;
        }

        public static PointF NormalizeVector(PointF vec)
        {
            float m = MathF.Sqrt(vec.X * vec.X + vec.Y * vec.Y);
            return ScaleVector(vec, 1f / m);
        }

    }
}
