using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    public struct Position
    {
        public Transform Marker { get; set; }
        public int SoldierType { get; set; }

        public Position(Transform marker, int soldierType)
        {
            Marker = marker;
            SoldierType = soldierType;
        }
    }
}
