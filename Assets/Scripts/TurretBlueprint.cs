using System.Collections;
using UnityEngine;

// To make variables visible in the unity inspector
[System.Serializable]
public class TurretBlueprint {

    public GameObject prefab;
    public int cost;

    public GameObject upgradedPrefab;
    public int upgradeCost;
}
