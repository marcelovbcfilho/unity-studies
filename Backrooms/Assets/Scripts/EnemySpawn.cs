using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [Tooltip("Enemy reference")]
    public GameObject enemy;

    private void Awake() {
        InvokeRepeating("SpawnEnemy", 0f, 5f);
    }

    void SpawnEnemy() {
        Instantiate(this.enemy, this.transform.position, this.enemy.transform.rotation).transform.parent = this.gameObject.transform;
    }
}
