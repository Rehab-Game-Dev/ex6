using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

/**
 * A graph that represents walkable tiles in the map.
 */
public class TilemapGraph : IGraph<Vector3Int>
{
    private Tilemap tilemap;
    private TileBase[] allowedTiles;

    public TilemapGraph(Tilemap tilemap, TileBase[] allowedTiles)
    {
        this.tilemap = tilemap;
        this.allowedTiles = allowedTiles;
    }

    // -------- COST FUNCTION FOR A* --------
    public int CostOf(TileBase tile)
    {
        if (tile == null)
            return 100000;

        switch (tile.name)
        {
            case "grass":          return 1;
            case "forest":         return 3;
            case "hills":          
            case "mountains":      return 5;
            case "shallow_sea":
            case "medium_sea":
            case "deep_sea":       return 8;
            default:               return 1;
        }
    }

    // -------- GET TILE AT POSITION --------
    public TileBase TileAt(Vector3Int pos)
    {
        return tilemap.GetTile(pos);
    }

    // -------- NEIGHBORS --------
    static readonly Vector3Int[] DIRS =
    {
        new Vector3Int( 1, 0, 0),
        new Vector3Int(-1, 0, 0),
        new Vector3Int( 0, 1, 0),
        new Vector3Int( 0,-1, 0),
    };

    public IEnumerable<Vector3Int> Neighbors(Vector3Int node)
    {
        foreach (var dir in DIRS)
        {
            var n = node + dir;
            var tile = tilemap.GetTile(n);

            if (allowedTiles.Contains(tile))
                yield return n;
        }
    }
}
