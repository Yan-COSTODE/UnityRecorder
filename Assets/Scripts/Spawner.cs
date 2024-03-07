using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    #region Fields & Properties
    #region Fields
    [SerializeField] private GameObject[] prefab;
    [SerializeField] private Vector3 bounds;
    [SerializeField, Range(100, 10000)] private int iCount = 100;
    [SerializeField, Range(1, 100)] private int iNumberOfBatch = 1;
    [SerializeField, Range(0.0f, 1.0f)] private float fDelay = 0.1f;
    #endregion
    
    #region Properties
    #endregion
    #endregion

    #region Methods
    /// <summary>
    /// Start the spawn of prefab
    /// </summary>
    private void Start() => StartCoroutine(Spawn());

    /// <summary>
    /// Draw box space where the prefab will spawn
    /// </summary>
    private void OnDrawGizmos() => Gizmos.DrawWireCube(transform.position, bounds * 2);

    /// <summary>
    /// Spawn random prefab on random location inside a box
    /// </summary>
    private IEnumerator Spawn()
    {
        for (int i = 0; i < iCount; i++)
        {
            Vector3 _position = new Vector3(Random.Range(-bounds.x, bounds.x), Random.Range(-bounds.y, bounds.y), Random.Range(-bounds.z, bounds.z));
            Transform _transform = transform;
            Instantiate(prefab[Random.Range(0, prefab.Length)], _transform.position + _position, Quaternion.identity, _transform);
            
            if(i % (iCount / iNumberOfBatch) == 0)
                yield return new WaitForSeconds(fDelay);
        }
    }
    #endregion
}
