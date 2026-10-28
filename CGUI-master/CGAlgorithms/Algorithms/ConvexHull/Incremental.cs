using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class Incremental : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            //Make Sure outPoints is empty
            if (outPoints == null)
                outPoints = new List<Point>();
            else
                outPoints.Clear();
            
            //handle empty input
            if (points == null || points.Count == 0)
                return;

            //filter the duplicates
            List<Point> unique = new List<Point>();
            foreach (Point p in points)
            {
                if (!unique.Any(u => u.Equals(p)))
                    unique.Add(p);
            }

            //handle base case (one point or two points (line))
            if (unique.Count <= 2)
            {
                outPoints.AddRange(unique);
                return;
            }

            //sort by x then y (if x are = then y)
            unique.Sort((a, b) =>
            {
                int cmp = a.X.CompareTo(b.X);
                if (cmp != 0) return cmp;
                return a.Y.CompareTo(b.Y);
            });

            //move in the top half from left to right, if point on left or on line take it and remove the last (point is out of the hull)
            //else if it is right then it is inside the poligon
            List<Point> upper = new List<Point>();
            foreach (Point p in unique)
            {
                while (upper.Count >= 2 && CrossProduct(upper[upper.Count - 2], upper[upper.Count - 1], p) >= 0)
                {
                    upper.RemoveAt(upper.Count - 1);
                }
                upper.Add(p);
            }

            //move in the bottom half from right to left, if point on left or on line take it and remove the last (point is out of the hull)
            //else if it is right then it is inside the poligon
            List<Point> lower = new List<Point>();
            for (int i = unique.Count - 1; i >= 0; i--)
            {
                Point p = unique[i];
                while (lower.Count >= 2 && CrossProduct(lower[lower.Count - 2], lower[lower.Count - 1], p) >= 0)
                {
                    lower.RemoveAt(lower.Count - 1);
                }
                lower.Add(p);
            }

            //remove dulicate point on the far left and far right
            upper.RemoveAt(upper.Count - 1);
            lower.RemoveAt(lower.Count - 1);

            //merge bottom and top halfs
            outPoints.AddRange(upper);
            outPoints.AddRange(lower);
        }

        /// <summary>
        /// Computes the 2D cross product of vectors (o -> a) and (o -> b).
        /// Return > 0: Left turn (Counter-Clockwise)
        /// Return < 0: Right turn (Clockwise)
        /// Return == 0: Collinear
        /// </summary>
        private static double CrossProduct(Point o, Point a, Point b)
        {
            double val = (a.X - o.X) * (b.Y - o.Y) - (a.Y - o.Y) * (b.X - o.X);
            if (Math.Abs(val) < 1e-9) return 0.0;
            return val;
        }

        public override string ToString()
        {
            return "Convex Hull - Incremental";
        }
    }
}
