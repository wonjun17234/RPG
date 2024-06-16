using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPiece : MonoBehaviour
{
    List<GameObject> _tiles = new List<GameObject>();

    [SerializeField]
    List<GameObject> _spawnableTiles = new List<GameObject>();


    HashSet<Vector3> _spawnables = new HashSet<Vector3>();
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            _tiles.Add(transform.GetChild(i).gameObject);
            _spawnables.Add(transform.GetChild(i).localPosition);
        }

        foreach(GameObject tile in _tiles)
        {
            Vector3 vector = tile.transform.localPosition;
            vector.y += 1f;

            tile.layer = LayerMask.NameToLayer("Ground");
            if (!(_spawnables.Contains(vector) || vector.y > 0))
            {
                _spawnableTiles.Add(tile);
            }
        }
        List<GameObject> enemys = Managers.Instance.DataManager.Enemys;
        foreach (GameObject spawnable in _spawnableTiles)
        {
            int random = Random.Range(0,4);

            if(random == 0)
            {
                int random2 = Random.Range(0, enemys.Count);
                Vector3 vector3 = spawnable.transform.position;
                vector3.y += 2f + enemys[random2].GetComponent<EnemyController>().SpawnOffSet;
                Instantiate<GameObject>(enemys[random2], vector3, Quaternion.identity);
            }
        }
    }
}
