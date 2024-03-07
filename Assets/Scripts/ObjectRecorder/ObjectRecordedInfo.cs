using System;

[Serializable]
public class ObjectRecordedInfo
{
    private bool status;
    private float number;
    private int integer;

    public bool Status => status;
    public float Number => number;
    public int Integer => integer;

    public ObjectRecordedInfo(bool _status = false, float _number = 0.0f, int _integer = 0)
    {
        status = _status;
        number = _number;
        integer = _integer;
    }
}