using System;
using UnityEngine;

namespace Core
{
    [Serializable]
    public struct V2
    {
        public float X;
        public float Y;

        public V2(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static V2 operator +(V2 obj1, V2 obj2)
        {
            return new V2(obj1.X + obj2.X, obj1.Y + obj2.Y);
        }

        public static bool operator ==(V2 obj1, V2 obj2)
        {
            return Mathf.Approximately(obj1.X,obj2.X) && Mathf.Approximately(obj1.Y,obj2.Y);
        }

        public static bool operator !=(V2 obj1, V2 obj2)
        {
            return Math.Abs(obj1.X - obj2.X) > 0.0001f || Math.Abs(obj1.Y - obj2.Y) > 0.0001f;
        }
    }
}