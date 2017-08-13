using UnityEngine;
using UnityEngine.EventSystems;


public class Node : MonoBehaviour {

    public Color hoverColor;
    public Color notEnoughMoneyColor;

    public Vector3 positionOffset;

    // 'renderer' is a keyword
    private Renderer rend;
    private Color startColor;

    [HideInInspector]
    public GameObject turret;
    [HideInInspector]
    public TurretBlueprint turretBlueprint;
    [HideInInspector]
    public bool isUpgraded = false;


    BuildManager buildManager;
    private AudioSource buySound;

    void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
        buildManager = BuildManager.instance;
        buySound = GetComponent<AudioSource>();
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

        if (turret != null)
        {
            showStats();
            return;
        }

        if (!buildManager.CanBuild)
            return;

        BuildTurret(buildManager.GetTurretToBuild());
    }

    public void showStats()
    {
        buildManager.SelectNode(this);
    }

    void BuildTurret (TurretBlueprint blueprint)
    {
        if (!buildManager.HasMoney)
        {
            Debug.Log("Not enough money to build turret!");
            return;
        }

        buySound.Play();
        PlayerStats.Money -= blueprint.cost;

        GameObject new_turret = (GameObject)Instantiate(blueprint.prefab, GetBuildPosition(), Quaternion.identity);
        turret = new_turret;

        turret.GetComponent<Turret>().node = this;

        turretBlueprint = blueprint;

        GameObject effect = (GameObject)Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 5f);

        Debug.Log("Turret build!");
    }

    public void UpgradeTurret()
    {
        if (PlayerStats.Money < turretBlueprint.upgradeCost)
        {
            Debug.Log("Not enough money to upgrade turret!");
            return;
        }

        PlayerStats.Money -= turretBlueprint.upgradeCost;

        // Remove old turret
        //Destroy(turret);

        // Set new turret
        //GameObject new_turret = (GameObject)Instantiate(turretBlueprint.upgradedPrefab, GetBuildPosition(), Quaternion.identity);
        //turret = new_turret;
  
        turret.GetComponent<Turret>().simpleUpgrade();

        GameObject effect = (GameObject)Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 5f);

        //isUpgraded = true;

        Debug.Log("Turret upgraded!");
    }

    public void SellTurret()
    {
        PlayerStats.Money += turretBlueprint.GetSellAmount();

        GameObject effect = (GameObject)Instantiate(buildManager.sellEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 5f);

        Destroy(turret);
        turretBlueprint = null;
    }

    void OnMouseEnter ()
    {
        if (buildManager == null || EventSystem.current.IsPointerOverGameObject())
            return;

        if (!buildManager.CanBuild)
            return;

        if (buildManager.HasMoney)
        {
            rend.material.color = hoverColor;
        } else
        {
            rend.material.color = notEnoughMoneyColor;
        }

        
    }

    void OnMouseExit()
    {
        rend.material.color = startColor;
    } 
}
