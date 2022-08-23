using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resources : MonoBehaviour
{
    public Text MoneyText;
    public int Money;

    private void Start() {
        MoneyText.text = Money.ToString();
    }

    private void Update() {
        MoneyText.text = Money.ToString();
    }
}
