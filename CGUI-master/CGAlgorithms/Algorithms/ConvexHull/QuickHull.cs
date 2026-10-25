using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cguh = CGUtilities.HelperMethods;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class QuickHull : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {

            if (outPoints == null)
                outPoints = new List<Point>();
            else
                outPoints.Clear();

            if (points == null || points.Count == 0)
                return;

            points = points.Distinct().ToList();
            if (points.Count <= 3)
            {
                outPoints = points.Distinct().ToList();
                return;
            }

            Point a = points.MinBy(p => p.X);
            Point b = points.MaxBy(p => p.X);
            // Point c = points.MaxBy(p => p.Y);
            // Point d = points.MinBy(p => p.Y);

            if (!outPoints.Contains(a)) outPoints.Add(a);
            if (!outPoints.Contains(b)) outPoints.Add(b);
            // if (!outPoints.Contains(c)) outPoints.Add(c);
            // if (!outPoints.Contains(d)) outPoints.Add(d);

            List<Point> sw = new List<Point>();
            List<Point> se = new List<Point>();
            List<Point> nw = [];
            List<Point> ne = [];

            foreach (Point p in points)
            {

                if (p.Equals(a) || p.Equals(b) /*|| p.Equals(c) || p.Equals(d)*/)
                    continue;

                double cross = cguh.CrossProduct(a.Vector(b), a.Vector(p));
                if (cross > 0/*!a.Equals(b) && cguh.CheckTurn(new Line(a, b), p) == Enums.TurnType.Right*/)
                {
                    sw.Add(p);
                }
                else if (cross < 0/*!b.Equals(c) && cguh.CheckTurn(new Line(b, c), p) == Enums.TurnType.Right*/)
                {
                    se.Add(p);
                }
                // if (!c.Equals(d) && cguh.CheckTurn(new Line(c, d), p) == Enums.TurnType.Right)
                // {
                //     ne.Add(p);
                // }
                // if (!d.Equals(a) && cguh.CheckTurn(new Line(d, a), p) == Enums.TurnType.Right)
                // {
                //     nw.Add(p);
                // }
            }

            FindHull(a, b, sw, outPoints);
            FindHull(b, a, se, outPoints);
            // FindHull(c, d, ne, outPoints);
            // FindHull(d, a, nw, outPoints);

            
        }

        void FindHull(Point a, Point b, List<Point> p, List<Point> outP)
        {
            if (p.Count == 0)
            {
                return;
            }

            Point c = p.MaxBy(tempP => Math.Abs(cguh.CrossProduct(a.Vector(b), a.Vector(tempP))));
            if (!outP.Contains(c)) outP.Add(c);

            List<Point> p1 = new List<Point>();
            List<Point> p2 = new List<Point>();
            foreach (Point tempP in p)
            {
                if (tempP.Equals(c) || tempP.Equals(a) || tempP.Equals(b)) continue;

                if (/*!a.Equals(c) && cguh.CheckTurn(new Line(a, c), tempP) == Enums.TurnType.Right*/cguh.CrossProduct(
                        a.Vector(c), a.Vector(tempP)) > 0)
                {
                    p1.Add(tempP);
                }

                if (/*!c.Equals(b) && cguh.CheckTurn(new Line(c, b), tempP) == Enums.TurnType.Right*/cguh.CrossProduct(
                        c.Vector(b), c.Vector(tempP)) > 0)
                {
                    p2.Add(tempP);
                }
            }

            FindHull(a, c, p1, outP);
            FindHull(c, b, p2, outP);
        }

        public override string ToString()
        {
            return "Convex Hull - Quick Hull";
        }
    }
}
