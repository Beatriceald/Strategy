using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState {
    Idle,
    WalkToBuilding,
    WalkToUnit,
    Attack
}

public class Enemy : MonoBehaviour {

    public EnemyState CurrentEnemyState;

    public int Health;
    private int _maxHealth;

    public Building TargetBuilding;
    public Unit TargetUnit;
    public float DistanceToFollow = 7f;
    public float DistanceToAttack = 1f;

    public NavMeshAgent NavMeshAgent;

    public float AttackPeriod = 1f;
    private float _timer;

    public GameObject HealthBarPrefab;
    private HealthBar _healthBar;

    void Start() {
        SetState(EnemyState.WalkToBuilding);
        _maxHealth = Health;
        GameObject healthBar = Instantiate(HealthBarPrefab);
        _healthBar = healthBar.GetComponent<HealthBar>();
        _healthBar.Setup(transform);
    }

    // Update is called once per frame
    void Update() {
        if (CurrentEnemyState == EnemyState.Idle) {
            FindClosestBuilding();
            if (TargetBuilding) {
                SetState(EnemyState.WalkToBuilding);
            }
            FindClosestUnit();
        } else if (CurrentEnemyState == EnemyState.WalkToBuilding) {
            FindClosestUnit();
            if (TargetBuilding == null) {
                SetState(EnemyState.Idle);
            }

        } else if (CurrentEnemyState == EnemyState.WalkToUnit) {

            if (TargetUnit) {
                NavMeshAgent.SetDestination(TargetUnit.transform.position);
                float distance = Vector3.Distance(transform.position, TargetUnit.transform.position);
                if (distance > DistanceToFollow) {
                    SetState(EnemyState.WalkToBuilding);
                }
                if (distance < DistanceToAttack) {
                    SetState(EnemyState.Attack);
                }
            }

        } else if (CurrentEnemyState == EnemyState.Attack) {
            if (TargetUnit) {
                NavMeshAgent.SetDestination(TargetUnit.transform.position);

                float distance = Vector3.Distance(transform.position, TargetUnit.transform.position);
                if (distance > DistanceToAttack) {
                    SetState(EnemyState.WalkToUnit);
                }
                _timer += Time.deltaTime;
                if (_timer > AttackPeriod) {
                    _timer = 0;
                    // отнять здоровье юниту
                    TargetUnit.TakeDamage(1);
                }
            } else {
                SetState(EnemyState.WalkToBuilding);
            }
            
        }
    }

    public void SetState(EnemyState enemyState) {
        CurrentEnemyState = enemyState;
        if (CurrentEnemyState == EnemyState.Idle) {
            //
        } else if (CurrentEnemyState == EnemyState.WalkToBuilding) {
            FindClosestBuilding();
            if (TargetBuilding) {
                NavMeshAgent.SetDestination(TargetBuilding.transform.position);
            } else {
                SetState(EnemyState.Idle);
            }
        } else if (CurrentEnemyState == EnemyState.WalkToUnit) {

        } else if (CurrentEnemyState == EnemyState.Attack) {
            _timer = 0;
        }
    }

    public void FindClosestBuilding() {
        Building[] allBuildings = FindObjectsOfType<Building>();

        float minDistance = Mathf.Infinity;
        Building closestBuilding = null;

        for (int i = 0; i < allBuildings.Length; i++) {
            float distance = Vector3.Distance(transform.position, allBuildings[i].transform.position);
            if (distance < minDistance) {
                minDistance = distance;
                closestBuilding = allBuildings[i];
            }
        }
        TargetBuilding = closestBuilding;
    }

    public void FindClosestUnit() {
        Unit[] allUnits = FindObjectsOfType<Unit>();

        float minDistance = Mathf.Infinity;
        Unit closestUnit = null;

        for (int i = 0; i < allUnits.Length; i++) {
            float distance = Vector3.Distance(transform.position, allUnits[i].transform.position);
            if (distance < minDistance) {
                minDistance = distance;
                closestUnit = allUnits[i];
            }
        }
        if (minDistance < DistanceToFollow) {
            TargetUnit = closestUnit;
            SetState(EnemyState.WalkToUnit);
        }
    }

    public void TakeDamage(int damageValue) {
        Health -= damageValue;
        _healthBar.SetHealth(Health, _maxHealth);
        if (Health <= 0) {
            // Die
            Destroy(gameObject);
        }
    }

    private void OnDestroy() {
        if (_healthBar) {
            Destroy(_healthBar.gameObject);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected() {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position, Vector3.up, DistanceToAttack);
        Handles.color = Color.yellow;
        Handles.DrawWireDisc(transform.position, Vector3.up, DistanceToFollow);
    }
#endif

}
