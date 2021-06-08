using UnityEngine;
using Cinemachine;

public class SwitchConfineBoundingShape : MonoBehaviour
{
    // // Start is called before the first frame update
    // void Start()
    // {
    //     SwitchBoundingShape();
    // }

    private void OnEnable()
    {
        EventHandler.AfterSceneLoadEvent += SwitchBoundingShape;
    }

    private void OnDisable()
    {
        EventHandler.AfterSceneLoadEvent -= SwitchBoundingShape;
    }



    /// Switch the collider that cinemachine uses to define the edges of the screen
    private void SwitchBoundingShape()
    {
        // Get the polygon collider on the boundsConfinder gameobject whicch is used by Cinemachine to prevent the camera going beyond the screen edges
        PolygonCollider2D polygonCollider2D = GameObject.FindGameObjectWithTag(Tags.boundsConfiner).GetComponent<PolygonCollider2D>();

        CinemachineConfiner confiner = GetComponent<CinemachineConfiner>();

        confiner.m_BoundingShape2D = polygonCollider2D;

        // Since the bounds have changed need to call this to clear the cache.

        confiner.InvalidatePathCache();
    }
}
