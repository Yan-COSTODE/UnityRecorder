using UnityEngine;

public class ExplosionClicker : MonoBehaviour
{
    #region Fields
    [SerializeField] private Camera currentCamera;
    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private float fSpawnOffset;
    [SerializeField] private float fExplosionRange;
    [SerializeField] private float fExplosionForce;
    #endregion

    #region Methods
    /// <summary>
    /// Check input to explode
    /// </summary>
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
            TryExplode();
    }

    /// <summary>
    /// Check if hit something and then explode at this point
    /// </summary>
    private void TryExplode()
    {
        Ray _ray = currentCamera.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(_ray, out RaycastHit _hit)) 
            return;
        
        Vector3 _position = _hit.point;
        Instantiate(explosionEffect, _position + Vector3.up * fSpawnOffset, Quaternion.Euler(-90.0f, 0.0f, 0.0f));
        ExplodeForce(_position);
    }

    /// <summary>
    /// Apply explosion force to all rigidbody in range
    /// </summary>
    /// <param name="_position">Position of explosion</param>
    private void ExplodeForce(Vector3 _position)
    {
        Collider[] _colliders = Physics.OverlapSphere(_position, fExplosionRange);
        int _colliderLength = _colliders.Length;
        
        if (_colliderLength <= 0) 
            return;
        
        for (int i = 0; i < _colliderLength; i++)
        {
            if(_colliders[i].TryGetComponent(out Rigidbody _rigidbody))
                _rigidbody.AddExplosionForce(fExplosionForce, _position, fExplosionRange);
        }
    }
    #endregion
}
