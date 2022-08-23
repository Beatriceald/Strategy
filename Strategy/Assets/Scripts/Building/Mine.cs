using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Mine : Building
{

    private float _timer;

    public int MineValue = 5;

    public AudioSource CoinMined;

    private void Update() {
        CoinMining();
    }

    public void CoinMining() {
        _timer += Time.deltaTime;
        if (_timer >= 10) {
            FindObjectOfType<Resources>().Money++;
            _timer = 0;
            CoinMined.Play();
            //Debug.Log("+1 coin");
        }
    }



}
