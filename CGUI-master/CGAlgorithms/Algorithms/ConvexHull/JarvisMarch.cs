using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class JarvisMarch : Algorithm
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

            //find the bottom left point to start with
            Point start = unique[0];
            for (int i = 1; i < unique.Count; i++)
            {
                if (unique[i].Y < start.Y || (unique[i].Y == start.Y && unique[i].X < start.X))
                {
                    start = unique[i];
                }
            }

            //the main loop
            Point current = start;
            while (true)
            {
                outPoints.Add(current);

                //put a random point initialy in the next point (any point other than current)
                Point nextPoint = unique[0].Equals(current) ? unique[1] : unique[0];

                //iterate counter clock wise to find the best point next to the current
                for (int i = 0; i < unique.Count; i++)
                {
                    Point candidate = unique[i];
                    if (candidate.Equals(current))
                        continue;

                    double turn = CrossProduct(current, nextPoint, candidate);

                    //if the point is on the left then it is gauranteed to be farther than the inetial point
                    if (turn > 0)
                    {
                        nextPoint = candidate;
                    }
                    //if the point is collinear then we take the farthest one using the ecludian distance rule
                    //note: use epsilon to avoid noisy double errors
                    else if (Math.Abs(turn) < 1e-9)
                    {
                        //note: compare the distance squared to avoid wasting time on "Math.sqrt"
                        if (DistanceSquared(current, candidate) > DistanceSquared(current, nextPoint))
                        {
                            nextPoint = candidate;
                        }
                    }
                    //else if on the right we ignore it
                }

                current = nextPoint;

                //when we reach the start point then we end the loop
                if (current.Equals(start))
                    break;
            }
        }

        /// <summary>
        /// Computes 2D cross product of vectors (p -> q) and (p -> r).
        /// Return > 0: Left turn (r is to the left of pq, more counter-clockwise)
        /// Return < 0: Right turn (r is to the right of pq, clockwise)
        /// Return == 0: Collinear
        /// </summary>
        private static double CrossProduct(Point p, Point q, Point r)
        {
            double val = (q.X - p.X) * (r.Y - p.Y) - (q.Y - p.Y) * (r.X - p.X);
            if (Math.Abs(val) < 1e-9) return 0.0;
            return val;
        }

        /// <summary>
        /// Squared Euclidean distance between two points (avoids expensive Math.Sqrt).
        /// </summary>
        private static double DistanceSquared(Point a, Point b)
        {
            double dx = a.X - b.X;
            double dy = a.Y - b.Y;
            return dx * dx + dy * dy;
        }

        public override string ToString()
        {
            return "Convex Hull - Jarvis March";
        }
    }
}
