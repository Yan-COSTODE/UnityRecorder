using System;
using System.Collections.Generic;

[Serializable]
public class ObjectRecorded
{
    private float fTime;
    private List<ObjectRecordedItem> items;

    public float Time => fTime;
    public List<ObjectRecordedItem> Items => items;

    public ObjectRecorded(float _time)
    {
        fTime = _time;
        items = new List<ObjectRecordedItem>();
    }

    /// <summary>
    /// Add object to the list of recorded object on this frame
    /// </summary>
    /// <param name="_item">Object recorded this frame</param>
    public void Add(ObjectRecordedItem _item) => items.Add(_item);
}
