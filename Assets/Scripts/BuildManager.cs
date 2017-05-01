using UnityEngine;

public class BuildManager : MonoBehaviour {

    public static BuildManager instance;
    void Awake ()
    {
        if (instance != null)
        {
            Debug.LogError("More then one BuildManager in scene!");
            return;
        }
        instance = this;
    }

    public bool CanBuild { get { return turretToBuild != null; } }
    public bool HasMoney { get { return PlayerStats.Money >= turretToBuild.cost; } }

    private TurretBlueprint turretToBuild;
    public void SelectTurretToBuild(TurretBlueprint turret)
    {
        turretToBuild = turret;

        DeselectNode();
    }

    public NodeUI nodeUI;
    private Node selectedNode;
    public void SelectNode (Node node)
    {
        Debug.Log("Select Node");
        if (selectedNode == node)
        {
            DeselectNode();
            return;
        }
            
        turretToBuild = null;

        selectedNode = node;
        nodeUI.SetTarget(node);
    }

    public void DeselectNode()
    {
        selectedNode = null;
        nodeUI.Hide();
    }

    public GameObject buildEffect;
    public void BuildTurretOn (Node node)
    {
        if (!HasMoney)
        {
            Debug.Log("Not enough money to build turret!");
            return;
        }

        PlayerStats.Money -= turretToBuild.cost;

        GameObject turret = (GameObject)Instantiate(turretToBuild.prefab, node.GetBuildPosition(), Quaternion.identity);
        node.turret = turret;

        GameObject effect = (GameObject)Instantiate(buildEffect, node.GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 5f);
        
        Debug.Log("Turret build! Money left: " + PlayerStats.Money);
    }
}
