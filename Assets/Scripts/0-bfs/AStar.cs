using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AStar
{
    public static List<Vector3Int> FindPath(TilemapGraph graph, Vector3Int start, Vector3Int goal)
    {
        var openSet  = new PriorityQueue<Vector3Int>();
        var cameFrom = new Dictionary<Vector3Int, Vector3Int>();
        var gScore   = new Dictionary<Vector3Int, int>();

        openSet.Enqueue(start, 0);
        gScore[start] = 0;

        while (openSet.Count > 0)
        {
            var current = openSet.Dequeue();

            if (current == goal)
                return ReconstructPath(cameFrom, current);

            foreach (var neighbor in graph.Neighbors(current))
            {
                TileBase tile = graph.TileAt(neighbor);            // <-- משתמשים בפונקציה החדשה
                int tentative_g = gScore[current] + graph.CostOf(tile);

                if (!gScore.ContainsKey(neighbor) || tentative_g < gScore[neighbor])
                {
                    gScore[neighbor] = tentative_g;
                    int fScore = tentative_g + Heuristic(neighbor, goal);
                    openSet.Enqueue(neighbor, fScore);
                    cameFrom[neighbor] = current;
                }
            }
        }

        return new List<Vector3Int>(); // אין נתיב
    }

    private static int Heuristic(Vector3Int a, Vector3Int b)
    {
        // מרחק מנהטן
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    private static List<Vector3Int> ReconstructPath(Dictionary<Vector3Int, Vector3Int> cameFrom, Vector3Int current)
    {
        var total = new List<Vector3Int> { current };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            total.Insert(0, current);
        }
        return total;
    }
}
