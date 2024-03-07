using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    #region Fields
    [SerializeField] private float fLifeTime = 1.0f;
    #endregion

    #region Methods
    /// <summary>
    /// Destroy this object after fLifeTime seconds
    /// </summary>
    private void Start() => Destroy(gameObject, fLifeTime);
    #endregion
}
