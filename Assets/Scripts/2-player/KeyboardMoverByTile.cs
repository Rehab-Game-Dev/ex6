using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

/**
 * This component allows the player to move between tiles,
 * with movement rules depending on terrain type and held items.
 */
public class KeyboardMoverByTile : KeyboardMover
{
    [SerializeField] Tilemap tilemap = null;
    [SerializeField] AllowedTiles allowedTiles = null;

    [Header("Player Item")]
    [SerializeField] PlayerItem heldItem = PlayerItem.None;

    [Header("Movement Speed")]
    [SerializeField] float baseSpeed = 3f;

    [Header("Tile References")]
    [SerializeField] TileBase grassTile;   // להריסה עם Pickaxe

    public void SetItem(PlayerItem newItem)
    {
        heldItem = newItem;
        Debug.Log("Player now holds: " + heldItem);
    }

    private TileBase TileOnPosition(Vector3 worldPosition)
    {
        Vector3Int cellPosition = tilemap.WorldToCell(worldPosition);
        return tilemap.GetTile(cellPosition);
    }

    void Update()
    {
        Vector3 newPosition = NewPosition();
        TileBase tileOnNewPosition = TileOnPosition(newPosition);

        if (tileOnNewPosition == null)
        {
            Debug.Log("Cannot walk outside map bounds.");
            return;
        }

        string t = tileOnNewPosition.name;
        bool canWalk = false;

        // -------------------------------------
        //         TERRAIN WALK RULES
        // -------------------------------------

        // Grass = always walkable
        if (t == "grass")
            canWalk = true;

        // Water requires boat
        else if (t == "deep_sea" || t == "medium_sea" || t == "shallow_sea")
            canWalk = (heldItem == PlayerItem.Boat);

        // Mountain / hills require goat or pickaxe
        else if (t == "mountains" || t == "hills")
            canWalk = (heldItem == PlayerItem.Goat || heldItem == PlayerItem.Pickaxe);

        // If tile is in the allowed tiles list
        else if (allowedTiles.Contains(tileOnNewPosition))
            canWalk = true;

        // -------------------------------------
        //         HANDLE MOVEMENT SPEED
        // -------------------------------------

        float speedMultiplier = 1f;

        switch (t)
        {
            case "grass":
                speedMultiplier = 1f;
                break;

            case "forest":
                speedMultiplier = 0.6f;
                break;

            case "hills":
            case "mountains":
                speedMultiplier = 0.4f;
                break;

            case "shallow_sea":
            case "medium_sea":
            case "deep_sea":
                speedMultiplier = 0.3f;
                break;
        }

        // -------------------------------------
        //         APPLY MOVEMENT
        // -------------------------------------

        if (canWalk)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                newPosition,
                baseSpeed * speedMultiplier * Time.deltaTime
            );

            // -------------------------------------
            //      PICKAXE BREAKS MOUNTAINS
            // -------------------------------------
            if (heldItem == PlayerItem.Pickaxe && (t == "mountains" || t == "hills"))
            {
                Vector3Int cellPos = tilemap.WorldToCell(newPosition);
                tilemap.SetTile(cellPos, grassTile);
                Debug.Log("Mountain tile broken → replaced with grass");
            }
        }
        else
        {
            Debug.LogWarning("Cannot walk on: " + t + " — missing required item.");
        }
    }
}
