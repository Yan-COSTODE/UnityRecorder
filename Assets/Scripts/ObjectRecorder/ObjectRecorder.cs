using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRecorder : SingletonTemplate<ObjectRecorder>
{
    #region Fields & Properties
    #region Fields
    public event Action OnPlaybackStarted;
    public event Action OnPlaybackEnded;
    
    [SerializeField, Range(1, 144)] private int iRefreshRate = 30;
    [SerializeField, Range(0.0f, 10.0f)] private float fMaxRecordedTime = 5.0f;
    [SerializeField] private KeyCode input = KeyCode.LeftShift;
    [SerializeField] private LayerMask toIgnore;
    private List<int> layerIgnore = new ();
    private readonly List<ObjectRecorded> objectRecorded = new ();
    private float fRecordedTime = 0.0f;
    private float fPlaybackTime = 0.0f;
    private float fCurrentPlaybackTime = 0.0f;
    private bool bIsRecording = true;
    #endregion

    #region Fields
    private float FrameTime => 1.0f / iRefreshRate;
    private float TimeRecorded => objectRecorded.Count > 0 ? objectRecorded[^1].Time - objectRecorded[0].Time : 0.0f;
    #endregion
    #endregion

    #region Methods

    /// <summary>
    /// Invoke the perform method at a specific frame rate
    /// </summary>
    private void Start()
    {
        InitLayer();
        InvokeRepeating(nameof(Perform), 0.0f, FrameTime);
    }
    
    /// <summary>
    /// Check input to launch the playback with max time
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(input))
            LaunchPlayBack(fMaxRecordedTime);
    }
    
    /// <summary>
    /// Init the List of all the layers that we need to ignore
    /// </summary>
    private void InitLayer()
    {
        const int _layers = 64;
        
        for (int _i = 0; _i < _layers; _i++)
        {
            if ((toIgnore & (1 << _i)) != 0)
                layerIgnore.Add(_i);
        }
    }
    
    /// <summary>
    /// Record or Playback depending on the situation
    /// </summary>
    private void Perform()
    {
        if(bIsRecording)
            Record();
        else
            Playback();
    }

    /// <summary>
    /// Start the playback and stop recording
    /// </summary>
    /// <param name="_time">Duration of playback, max clamp to the total time recorded</param>
    private void LaunchPlayBack(float _time)
    {
        if (objectRecorded.Count <= 0 || !bIsRecording)
            return;
        
        ObjectStatus(false);
        bIsRecording = false;
        fPlaybackTime = _time > TimeRecorded ? TimeRecorded : _time;
        fCurrentPlaybackTime = 0.0f;
        OnPlaybackStarted?.Invoke();
    }
    
    /// <summary>
    /// Record all information of all object in scene that aren't ignore by the layer at an exact frame
    /// </summary>
    private void Record()
    {
        while (TimeRecorded > fMaxRecordedTime)
            objectRecorded.RemoveAt(0);
        
        Transform[] _transforms = FindObjectsByType<Transform>(FindObjectsSortMode.None);
        int _transformCount = _transforms.Length;
        int _layersCount = layerIgnore.Count;
        int _j;
        ObjectRecorded _objectRecorded = new ObjectRecorded(fRecordedTime);
        
        for (int _i = 0; _i < _transformCount; _i++)
        {
            Transform _transform = _transforms[_i];

            for (_j = 0; _j < _layersCount; _j++)
            {
                if (_transform.gameObject.layer == layerIgnore[_j])
                    break;
            }
            
            if (_j == _layersCount)
                _objectRecorded.Add(new ObjectRecordedItem(_transform.gameObject, _transform));
        }
        
        objectRecorded.Add(_objectRecorded);
        fRecordedTime += FrameTime;
    }

    /// <summary>
    /// Playback the information of all recorded object at the last frame recorded
    /// </summary>
    private void Playback()
    {
        ObjectRecordedItem[] _objectRecordedItems = objectRecorded[^1].Items.ToArray();
        int _objectCount = _objectRecordedItems.Length;

        for (int _i = 0; _i < _objectCount; _i++)
            _objectRecordedItems[_i].Replay();
        
        fCurrentPlaybackTime += FrameTime;
        
        if(objectRecorded.Count == 1 || fCurrentPlaybackTime >= fPlaybackTime)
            ObjectStatus(true);
        
        objectRecorded.RemoveAt(objectRecorded.Count - 1);

        if (fCurrentPlaybackTime < fPlaybackTime && objectRecorded.Count != 0)
            return;
        
        objectRecorded.Clear();
        bIsRecording = true;
        fCurrentPlaybackTime = 0.0f;
        OnPlaybackEnded?.Invoke();
    }

    /// <summary>
    /// Set the status of all component on object recorded
    /// </summary>
    /// <param name="_status">The status to apply to all objects</param>
    private void ObjectStatus(bool _status)
    {
        ObjectRecordedItem[] _objectRecordedItems = objectRecorded[0].Items.ToArray();
        int _objectCount = _objectRecordedItems.Length;

        for (int _i = 0; _i < _objectCount; _i++)
        {
            ObjectRecordedItem _objectRecordedItem = _objectRecordedItems[_i];
            _objectRecordedItem.SetStatus(_status);
        }
    }
    #endregion
}