using UnityEngine;
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

    public void SetTarget(Node target)
    {
        this.target = target;

        transform.position = target.GetBuildPosition();

        // Stats display
        Turret turret = target.turret.GetComponent<Turret>();
        dmgText.text = "OBR: " + turret.bulletDamage.ToString();
        spdText.text = "SZYB: " + turret.fireRate.ToString();
        upgradeText.text = turret.upgradeText;

        if (turret.towerLevel < 20)
        {
            upgradeCost.text = target.turretBlueprint.upgradeCost + " PLN";
            upgradeButton.interactable = true;
        } else
        {
            upgradeCost.text = "MAX";
            upgradeButton.interactable = false;
        }

        //sellAmount.text = target.turretBlueprint.GetSellAmount() + " PLN";
        ui.SetActive(true);
    }

    public void Hide()
    {
        ui.SetActive(false);
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

}
