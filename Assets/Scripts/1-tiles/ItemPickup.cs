using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public PlayerItem itemType;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // נבדוק אם מה שנכנס בנו הוא השחקן
        var mover = other.GetComponent<KeyboardMoverByTile>();
        if (mover != null)
        {
            mover.SetItem(itemType);  // נותן לשחקן את החפץ
            Debug.Log("Picked up: " + itemType);

            Destroy(gameObject); // העלמת החפץ מהמפה
        }
    }
}
