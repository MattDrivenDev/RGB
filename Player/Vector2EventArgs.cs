using System;
using Microsoft.Xna.Framework;

namespace RGB.Player
{
    public class Vector2EventArgs : EventArgs
    {
        public Vector2EventArgs(Vector2 value)
        {
            Value = value;   
        }

        public Vector2 Value { get; init; }
    }
}