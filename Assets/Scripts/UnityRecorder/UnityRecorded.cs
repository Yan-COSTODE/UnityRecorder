using System;
using System.Collections.Generic;

[Serializable]
public class UnityRecorded
{
    private float fTime;
    private List<UnityRecordedItem> items;

    public float Time => fTime;
    public List<UnityRecordedItem> Items => items;

    public UnityRecorded(float _time)
    {
        fTime = _time;
        items = new List<UnityRecordedItem>();
    }

    /// <summary>
    /// Add object to the list of recorded object on this frame
    /// </summary>
    /// <param name="_item">Object recorded this frame</param>
    public void Add(UnityRecordedItem _item) => items.Add(_item);
}
