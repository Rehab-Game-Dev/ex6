using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.Rendering;
#endif

/**
 * This component makes its object watch a given radius, and if the target is found - it starts chasing it.
 */
[RequireComponent(typeof(Chaser))]
public class RadiusWatcher: MonoBehaviour {
    [SerializeField] float radiusToWatch = 5f;

    private Chaser chaser;
    private void Start() {
        chaser = GetComponent<Chaser>();
        chaser.enabled = false;
    }

    void Update() {
        float distanceToTarget = Vector3.Distance(
            transform.position, chaser.TargetObjectPosition());
        if (distanceToTarget <= radiusToWatch) {
            chaser.enabled = true;
        } else {
            chaser.enabled = false;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        // draw on top of everything in the Scene view
        Handles.zTest = CompareFunction.Always;
        Handles.color = Color.blue;

        // 2D top-down: circle in XY plane, so normal is Vector3.forward
        Handles.DrawWireDisc(transform.position, Vector3.forward, radiusToWatch);
    }
#endif
}
