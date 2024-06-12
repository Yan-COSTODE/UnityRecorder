using System;
using UnityEngine;

[Serializable]
public class UnityRecordedInfo
{
    private bool status;
    private float number;
    private int integer;
    private Vector3 vector1;
    private Vector3 vector2;

    public bool Status => status;
    public float Number => number;
    public int Integer => integer;
    public Vector3 Vector1 => vector1;
    public Vector3 Vector2 => vector2;

    public UnityRecordedInfo(bool _status = false, float _number = 0.0f, int _integer = 0, Vector3 _vector1 = default, Vector3 _vector2 = default)
    {
        status = _status;
        number = _number;
        integer = _integer;
        vector1 = _vector1;
        vector2 = _vector2;
    }
}