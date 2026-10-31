using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cguh = CGUtilities.HelperMethods;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class GrahamScan : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            Point v = points.MinBy(p => p.Y);
            List<Point> sorted = new List<Point>(points); // fill a new list and sort it
            sorted.Sort((p, q) =>
            {
                Point vp = v.Vector(p);
                Point vq = v.Vector(q); // sort based on angle, aka turn type also cw or ccw

                switch (cguh.CheckTurn(vp, vq))
                {
                    case Enums.TurnType.Left: return -1; // q is counterclockwise of p, so p first
                    case Enums.TurnType.Right: return 1; // q first
                    default:
                        // same angle from the anchor: closer point first
                        double dp = vp.X * vp.X + vp.Y * vp.Y;
                        double dq = vq.X * vq.X + vq.Y * vq.Y;
                        return dp.CompareTo(dq);
                }
            });

            outPoints.Add(v); // add the anchor to output (L) and remove it from points list
            sorted.Remove(v);

            foreach(Point pi in sorted) // iterate over all points still in og list
            {
                // loop as long as the L stack has more than 2 points (to check top and previous(top)
                while (outPoints.Count() >= 2 && cguh.CheckTurn(new Line(outPoints[outPoints.Count - 2],
                           outPoints[outPoints.Count - 1]), pi) != Enums.TurnType.Left) // check the turn of last 3 points
                { // out[out.count - 2] is previous(top) out.count-1 is top and pi is new point to check
                    // if the turn is not left remove the top else push and continue
                    outPoints.RemoveAt(outPoints.Count - 1);
                }

                outPoints.Add(pi);
            }
        }

        public override string ToString()
        {
            return "Convex Hull - Graham Scan";
        }
    }
}
