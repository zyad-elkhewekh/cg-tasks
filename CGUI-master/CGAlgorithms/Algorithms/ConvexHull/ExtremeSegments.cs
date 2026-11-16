using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using cguh = CGUtilities.HelperMethods;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class ExtremeSegments : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            List<Point> pts = new List<Point>();
            foreach (Point pt in points)
                if (!pts.Contains(pt))
                    pts.Add(pt);
            if (pts.Count < 3)
            {
                outPoints.AddRange(pts);
                return;
            }
            for (int i = 0; i < pts.Count; i++)
            {
                for (int j = i + 1; j < pts.Count; j++)
                {
                    int leftcnt = 0, rightcnt = 0;
                    Point p = pts[i], q = pts[j];
                    Line l = new Line(p, q);
                    bool valid = true;
                    for (int k = 0; k < pts.Count && valid && (leftcnt == 0 || rightcnt == 0); k++)
                    {
                        if (k == i || k == j) continue;
                        if (cguh.CheckTurn(l, pts[k]) == Enums.TurnType.Right)
                        {
                            rightcnt++;
                        }
                        else if (cguh.CheckTurn(l, pts[k]) == Enums.TurnType.Left)
                        {
                            leftcnt++;
                        }
                        else
                        {
                            if (!cguh.PointOnSegment(pts[k], p, q)) valid = false;
                        }
                    }

                    if (valid && (rightcnt == 0 || leftcnt == 0))
                    {
                        outLines.Add(l);
                        if (!outPoints.Contains(p)) outPoints.Add(p);
                        if (!outPoints.Contains(q)) outPoints.Add(q);
                    }
                }
            }
        }

        public override string ToString()
        {
            return "Convex Hull - Extreme Segments";
        }
    }
}
