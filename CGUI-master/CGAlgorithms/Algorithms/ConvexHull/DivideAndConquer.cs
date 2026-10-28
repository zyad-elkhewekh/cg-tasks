using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class DivideAndConquer : Algorithm
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

            //run the recursion
            List<Point> hull = ComputeDivideAndConquer(unique);
            outPoints.AddRange(hull);
        }

        /// <summary>
        /// Recursively splits points into left and right halves, solves them, and merges with upper/lower tangents.
        /// Returns the convex hull vertices in Clockwise order.
        /// </summary>
        private List<Point> ComputeDivideAndConquer(List<Point> pts)
        {
            int n = pts.Count;
            //base case
            if (n <= 5)
            {
                return BaseHullClockwise(pts);
            }

            //divide
            int mid = n / 2;
            List<Point> leftPts = pts.GetRange(0, mid);
            List<Point> rightPts = pts.GetRange(mid, n - mid);

            //conquer
            List<Point> A = ComputeDivideAndConquer(leftPts);
            List<Point> B = ComputeDivideAndConquer(rightPts);

            //body
            //Return Base Input Cases
            if (A.Count == 0) return B;
            if (B.Count == 0) return A;
            if (A.Count <= 2 || B.Count <= 2)
            {
                List<Point> combined = new List<Point>(A);
                combined.AddRange(B);
                combined.Sort((p1, p2) =>
                {
                    int c = p1.X.CompareTo(p2.X);
                    return c != 0 ? c : p1.Y.CompareTo(p2.Y);
                });
                return BaseHullClockwise(combined);
            }

            int nA = A.Count;
            int nB = B.Count;

            //find rightmost point in A by taking the largest X then Y (if x are = then y)
            int i = 0;
            for (int k = 1; k < nA; k++)
            {
                if (A[k].X > A[i].X || (A[k].X == A[i].X && A[k].Y > A[i].Y))
                    i = k;
            }

            //find leftmost point in A by taking the Smalest X then Y (if x are = then y)
            int j = 0;
            for (int k = 1; k < nB; k++)
            {
                if (B[k].X < B[j].X || (B[k].X == B[j].X && B[k].Y < B[j].Y))
                    j = k;
            }

            //inetialize the first tangent (top bridge)
            int itop = i, jtop = j;
            //moving both points up till both have no changes (arrived to the topmost point in the Hulls A and B)
            bool changed = true;
            while (changed)
            {
                changed = false;
                //if next point on A is above the line, move ia up
                while (CrossProduct(A[itop], B[jtop], A[(itop - 1 + nA) % nA]) > 0)
                {
                    itop = (itop - 1 + nA) % nA;
                    changed = true;
                }
                //if next point on B is above the line, move ja up
                while (CrossProduct(A[itop], B[jtop], B[(jtop + 1) % nB]) > 0)
                {
                    jtop = (jtop + 1) % nB;
                    changed = true;
                }
            }

            //inetialize the second tangent (bottom bridge)
            int ibottom = i, jbottom = j;
            //moving both points down till both have no changes (arrived to the bottomost point in the Hulls A and B)
            changed = true;
            while (changed)
            {
                changed = false;
                //if next point on A is below the line, move ib down
                while (CrossProduct(A[ibottom], B[jbottom], A[(ibottom + 1) % nA]) < 0)
                {
                    ibottom = (ibottom + 1) % nA;
                    changed = true;
                }
                //if next point on B is below the line, move jb down
                while (CrossProduct(A[ibottom], B[jbottom], B[(jbottom - 1 + nB) % nB]) < 0)
                {
                    jbottom = (jbottom - 1 + nB) % nB;
                    changed = true;
                }
            }

            //initialize the final array result
            List<Point> merged = new List<Point>();

            //start from the top of Hull A
            merged.Add(A[itop]);

            //move to top B point and add it
            int curr = jtop;
            merged.Add(B[curr]);

            //move on the rightmost part from top to bottom
            while (curr != jbottom)
            {
                curr = (curr + 1) % nB;
                merged.Add(B[curr]);
            }

            //move to bottom A point
            curr = ibottom;

            //add bottom of A only if (itop != ibottom (itop == merged[0]), and ibottom != jbottom (both hulls share the same bottom point))
            if (!A[curr].Equals(merged[merged.Count - 1]) && !A[curr].Equals(merged[0]))
                merged.Add(A[curr]);

            //move on leftmost part from bottom to top
            while (curr != itop)
            {
                curr = (curr + 1) % nA;
                if (curr != itop && !A[curr].Equals(merged[merged.Count - 1]))
                    merged.Add(A[curr]);
            }

            //remove collinear points along the edges
            if (merged.Count > 2)
            {
                List<Point> cleaned = new List<Point>();
                for (int k = 0; k < merged.Count; k++)
                {
                    Point prevP = merged[(k - 1 + merged.Count) % merged.Count];
                    Point currP = merged[k];
                    Point nextP = merged[(k + 1) % merged.Count];
                    if (CrossProduct(prevP, currP, nextP) != 0)
                        cleaned.Add(currP);
                }
                return cleaned;
            }
            return merged;
        }

        /// <summary>
        /// Solves small base case point sets directly in Clockwise order.
        /// </summary>
        private static List<Point> BaseHullClockwise(List<Point> pts)
        {
            //Base Hulls
            if (pts.Count <= 1) return new List<Point>(pts);
            if (pts.Count == 2) return new List<Point>(pts);

            //Construct the hull same way as "Incremental":
            //make the upper chain
            List<Point> upper = new List<Point>();
            foreach (Point p in pts)
            {
                while (upper.Count >= 2 && CrossProduct(upper[upper.Count - 2], upper[upper.Count - 1], p) >= 0)
                {
                    upper.RemoveAt(upper.Count - 1);
                }
                upper.Add(p);
            }
            //make the lower chain
            List<Point> lower = new List<Point>();
            for (int i = pts.Count - 1; i >= 0; i--)
            {
                Point p = pts[i];
                while (lower.Count >= 2 && CrossProduct(lower[lower.Count - 2], lower[lower.Count - 1], p) >= 0)
                {
                    lower.RemoveAt(lower.Count - 1);
                }
                lower.Add(p);
            }
            upper.RemoveAt(upper.Count - 1);
            lower.RemoveAt(lower.Count - 1);
            upper.AddRange(lower);
            return upper;
        }

        /// <summary>
        /// Computes 2D cross product of vectors (o -> a) and (o -> b).
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
            return "Convex Hull - Divide & Conquer";
        }
    }
}
