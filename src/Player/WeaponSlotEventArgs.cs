using System;

namespace RGB.Player
{
    public class WeaponSlotEventArgs : EventArgs
    {
        public WeaponSlotEventArgs(WeaponSlot value)
        {
            Value = value;   
        }

        public WeaponSlot Value { get; init; }
    }
}