using System;
using System.Collections.Generic;
using UnityEngine;

public class UnityRecorder : SingletonTemplate<UnityRecorder>
{
    #region Fields & Properties
    #region Fields
    public event Action OnPlaybackStarted;
    public event Action OnPlaybackEnded;
    
    [SerializeField, Range(1, 144)] private int iRefreshRate = 30;
    [SerializeField, Range(0.0f, 10.0f)] private float fMaxRecordedTime = 5.0f;
    [SerializeField] private LayerMask toIgnore;
    private readonly List<int> layerIgnore = new ();
    private readonly List<UnityRecorded> objectRecorded = new ();
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
    public void LaunchPlayBack()
    {
        if (objectRecorded.Count <= 0 || !bIsRecording)
            return;
        
        bIsRecording = false;
        fPlaybackTime = TimeRecorded;
        fCurrentPlaybackTime = 0.0f;
        OnPlaybackStarted?.Invoke();
    }
    
    /// <summary>
    /// Record all information of all object in scene that aren't ignore by the layer at an exact frame
    /// </summary>
    private void Record()
    {
        while (TimeRecorded >= fMaxRecordedTime)
            objectRecorded.RemoveAt(0);
        
        Transform[] _transforms = FindObjectsByType<Transform>(FindObjectsSortMode.None);
        int _transformCount = _transforms.Length;
        int _layersCount = layerIgnore.Count;
        UnityRecorded unityRecorded = new UnityRecorded(fRecordedTime);
        
        for (int _i = 0; _i < _transformCount; _i++)
        {
            int _j;
            Transform _transform = _transforms[_i];            

            for (_j = 0; _j < _layersCount; _j++)
            {
               	if (_transform.gameObject.layer == layerIgnore[_j])
               	    break;
            }

            if (_j == _layersCount)
                unityRecorded.Add(new UnityRecordedItem(_transform.gameObject, _transform));
        }
        
        objectRecorded.Add(unityRecorded);
        fRecordedTime += FrameTime;
    }

    /// <summary>
    /// Playback the information of all recorded object at the last frame recorded
    /// </summary>
    private void Playback()
    {
        UnityRecordedItem[] _objectRecordedItems = objectRecorded[^1].Items.ToArray();
        int _objectCount = _objectRecordedItems.Length;

        for (int _i = 0; _i < _objectCount; _i++)
            _objectRecordedItems[_i].Replay();
        
        fCurrentPlaybackTime += FrameTime;
        objectRecorded.RemoveAt(objectRecorded.Count - 1);

        if (fCurrentPlaybackTime <= fPlaybackTime && objectRecorded.Count != 0)
            return;
        
        objectRecorded.Clear();
        bIsRecording = true;
        fCurrentPlaybackTime = 0.0f;
        OnPlaybackEnded?.Invoke();
    }
    #endregion
}