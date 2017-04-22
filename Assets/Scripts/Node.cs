using UnityEngine;
using UnityEngine.EventSystems;


public class Node : MonoBehaviour {

    public Color hoverColor;
    public Vector3 positionOffset;

    // 'renderer' is a keyword
    private Renderer rend;
    private Color startColor;

    [Header("Optional")]
    public GameObject turret;
    BuildManager buildManager;

    void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
        buildManager = BuildManager.instance;
    }

    public Vector3 GetBuildPosition ()
    {
        return transform.position + positionOffset;
    }

    void OnMouseDown()
    {
        // Avoid highlighting node if its beneath UI
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (!buildManager.CanBuild)
            return;

        if (turret != null)
        {
            Debug.Log("Can't build there!");
            return;
        }

        // Build a turret
        buildManager.BuildTurretOn(this);
    }

    void OnMouseEnter ()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (!buildManager.CanBuild)
            return;

        rend.material.color = hoverColor;
    }

    void OnMouseExit()
    {
        rend.material.color = startColor;
    } 
}
