using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ObjectRecordedItem
{
    private GameObject item;
    private bool status;
    private Vector3 position;
    private Vector3 rotation;
    private Vector3 scale;
    private Dictionary<Type, ObjectRecordedInfo> info;

    public ObjectRecordedItem(GameObject _item, Transform _transform)
    {
        item = _item;
        status = _transform.gameObject.activeSelf;
        position = _transform.localPosition;
        rotation = _transform.localEulerAngles;
        scale = _transform.localScale; 
        info = new Dictionary<Type, ObjectRecordedInfo>();
        RegisterStatus();
    }

    /// <summary>
    /// Replay how the object was on this frame with active, transform and other components
    /// </summary>
    public void Replay()
    {
        if (!item)
            return;
            
        item.SetActive(status);
        item.transform.localPosition = position;
        item.transform.localEulerAngles = rotation;
        item.transform.localScale = scale;
        ReplayStatus();
    }

    /// <summary>
    /// Replay the status of all the components on this frame
    /// </summary>
    private void ReplayStatus()
    {
        if (item.TryGetComponent(out ParticleSystem _particleSystem))
        {
            ObjectRecordedInfo _info = info[typeof(ParticleSystem)];
            _particleSystem.Simulate(_info.Number, true, true);
            
            if (_info.Status)
                _particleSystem.Play();
            else
                _particleSystem.Stop();
        }
    }

    /// <summary>
    /// Register the status of all component on this frame
    /// </summary>
    private void RegisterStatus()
    {
        if (item.TryGetComponent(out ParticleSystem _particleSystem))
            info.Add(typeof(ParticleSystem), new ObjectRecordedInfo(_particleSystem.isPlaying, _particleSystem.time));

        if (item.TryGetComponent(out Rigidbody _rigidbody))
            info.Add(typeof(Rigidbody), new ObjectRecordedInfo(_rigidbody.useGravity, 0.0f, _rigidbody.isKinematic ? 0 : 1));
    }
     
    /// <summary>
    /// Set status of all components on the object
    /// </summary>
    /// <param name="_status">Status to apply to all the components</param>
    public void SetStatus(bool _status)
    {
        if (!item)
            return;

        if (item.TryGetComponent(out Collider _collider))
            _collider.enabled = _status;

        if (item.TryGetComponent(out Rigidbody _rigidbody))
        {
            if (!_status)
            {
                _rigidbody.useGravity = false;
                _rigidbody.isKinematic = true;
            }
            else
            {
                ObjectRecordedInfo _info = info[typeof(Rigidbody)];
                _rigidbody.useGravity = _info.Status;
                _rigidbody.isKinematic = _info.Integer == 0;
            }
        }
    }
}
