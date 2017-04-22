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

    public GameObject standardTurretPrefab;
    public GameObject missileLauncherPrefab;

    public bool CanBuild { get { return turretToBuild != null; } }
    public bool HasMoney { get { return PlayerStats.Money >= turretToBuild.cost; } }

    private TurretBlueprint turretToBuild;
    public void SelectTurretToBuild(TurretBlueprint turret)
    {
        turretToBuild = turret;
    }

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

        Debug.Log("Turret build! Money left: " + PlayerStats.Money);
    }
}
