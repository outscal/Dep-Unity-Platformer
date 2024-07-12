using System;
using UnityEngine;

namespace Platformer.Cameras
{
    [Serializable]
    public struct CameraBounds
    {
        public float leftBound;
        public float rightBound;
        public float topBound;
        public float bottomBound;
    }
}