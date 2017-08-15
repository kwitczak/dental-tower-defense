using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : MonoBehaviour {

    public GameObject hoverInfo;

    public void ShowHoverInfo()
    {
        hoverInfo.SetActive(true);
    }

    public void HideHoverInfo()
    {
        hoverInfo.SetActive(false);
    }
}
