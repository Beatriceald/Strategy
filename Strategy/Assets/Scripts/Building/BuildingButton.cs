using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingButton : MonoBehaviour
{
    public BuildingPlacer BuildingPlacer;
    public GameObject BuildingPrefab;
    public Text BuildingPrice;

    private void Start() {
        int buildingPrice = BuildingPrefab.GetComponent<Building>().Price;
        BuildingPrice.text = buildingPrice.ToString();
    }

    public void TryBuy() {

        int price = BuildingPrefab.GetComponent<Building>().Price;
        if(FindObjectOfType<Resources>().Money >= price) {
            FindObjectOfType<Resources>().Money -= price;
            BuildingPlacer.CreateBuilding(BuildingPrefab);
        } else {
            Debug.Log("Мало денег");
        }
    }
}
