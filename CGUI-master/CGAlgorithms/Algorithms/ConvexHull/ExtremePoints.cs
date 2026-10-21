using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CGUtilities.HelperMethods;

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
                        foreach (Point c in points.Where(c => !c.Equals(p) && !c.Equals(a) && !c.Equals(b)))
                        {
                            if (CGUtilities.HelperMethods.PointInTriangle(p, a, b, c) == 0)
                            {
                                isExtreme = false;
                                break;
                            }
                        }
                    }
                }
                if (isExtreme) outPoints.Add(p);
            }
        }

        public override string ToString()
        {
            return "Convex Hull - Extreme Points";
        }
    }
}
