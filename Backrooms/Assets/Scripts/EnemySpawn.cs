using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [Tooltip("Enemy reference")] public GameObject enemy;
    [Tooltip("Player reference")] public GameObject player;
    [Tooltip("Terrain controller")] private Terrain.Controller _terrainController;

    private void Awake()
    {
        _terrainController = FindObjectOfType<Terrain.Controller>();
        InvokeRepeating(nameof(SpawnEnemy), 0f, 5f);
    }

    private void SpawnEnemy()
    {
        int xSpawn = (int) player.transform.position.x - 5;
        int zSpawn = (int) player.transform.position.z - 5;
        int ySpawn = (int) _terrainController.GetChunkHeight(xSpawn, zSpawn);
        Vector3 spawnPoint = new Vector3(xSpawn, ySpawn, zSpawn);
        Instantiate(enemy, spawnPoint, enemy.transform.rotation).transform.parent = gameObject.transform;
    }
}