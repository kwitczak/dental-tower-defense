using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NodeUI : MonoBehaviour {

    public GameObject ui;
    private Node target;

    public Text upgradeText;
    public Text upgradeCost;
    public Text dmgText;
    public Text dmgBonusText;
    public Text spdText;
    public Text spdTextBonus;
    public Button upgradeButton;

    public Text sellAmount;
    private bool shown = false;

    private float timeSinceLastCall;
    public void Update()
    {
        timeSinceLastCall += Time.deltaTime;
        HideIfClickedOutside();
    }

    public void SetTarget(Node target)
    {
        this.target = target;

        transform.position = target.GetBuildPosition();

        // Stats display
        Turret turret = target.turret.GetComponent<Turret>();
        dmgText.text = "OBR: " + turret.showDamage().ToString();
        spdText.text = "SZYB: " + turret.fireRate.ToString();
        upgradeText.text = turret.upgradeText;

        if (turret.towerLevel < 20)
        {
            upgradeCost.text = target.turretBlueprint.nextUpgradeCost(turret) + " PLN";
            upgradeButton.interactable = true;
        } else
        {
            upgradeCost.text = "MAX";
            upgradeButton.interactable = false;
        }

        //sellAmount.text = target.turretBlueprint.GetSellAmount() + " PLN";
        timeSinceLastCall = 0;
        ui.SetActive(true);
        shown = true;
    }

    public void Hide()
    {
        ui.SetActive(false);
        shown = false;
        target = null;
    }

    public void Upgrade()
    {
        target.UpgradeTurret();
        BuildManager.instance.DeselectNode();
    }

    public void Sell()
    {
        target.SellTurret();
        BuildManager.instance.DeselectNode();
    }

    public void showUpgradeStats()
    {
        Turret turret = target.turret.GetComponent<Turret>();
        // Stats display
        dmgBonusText.text = "+ " + turret.nextDamageUpdate().ToString();
        spdTextBonus.text = "+ " + turret.nextSpeedUpdate().ToString();
    }

    public void hideUpgradeStats()
    {
        dmgBonusText.text = "";
        spdTextBonus.text = "";
    }

    private void HideIfClickedOutside()
    {
        if (Input.GetMouseButtonUp(0) && shown && timeSinceLastCall >= 0.5 &&
            !EventSystem.current.IsPointerOverGameObject())
        {
            BuildManager.instance.DeselectNode();
        }
    }

}
