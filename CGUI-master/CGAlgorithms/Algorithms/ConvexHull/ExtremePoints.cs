using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cguh = CGUtilities.HelperMethods;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class ExtremePoints : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            foreach (Point p in points)
            {
                bool isExtreme = true;
                foreach (Point a in points.Where(a => !a.Equals(p)))
                {
                    foreach (Point b in points.Where(b => !b.Equals(p) && !b.Equals(a)))
                    {
                        if (cguh.CheckTurn(new Line(a, b), p) == Enums.TurnType.Colinear &&
                            p.X >= Math.Min(a.X, b.X) && p.X <= Math.Max(a.X, b.X) &&
                            p.Y >= Math.Min(a.Y, b.Y) && p.Y <= Math.Max(a.Y, b.Y)) // make sure that if they are colinear, p is actually in the middle and not on either side, which can make it an extreme point still
                        {
                            isExtreme = false;
                            break;
                        }
                        foreach (Point c in points.Where(c => !c.Equals(p) && !c.Equals(a) && !c.Equals(b)))
                        {
                            if (cguh.CheckTurn(new Line(a, b), c) == Enums.TurnType.Colinear)
                            {
                                continue; // skip if my three points are colinear, aka, dont even make up a triangle
                            }
                            if (cguh.PointInTriangle(p, a, b, c) != Enums.PointInPolygon.Outside) // reject both onEdge and inside
                            {
                                isExtreme = false;
                                break;   
                            }
                        }
                    }
                }
                if (isExtreme && !outPoints.Contains(p)) outPoints.Add(p); // make sure to skip duplicates
            }
        }

        public override string ToString()
        {
            return "Convex Hull - Extreme Points";
        }
    }
}
