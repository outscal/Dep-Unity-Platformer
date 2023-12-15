using UnityEngine;

namespace Platformer.Drop
{
    [CreateAssetMenu(fileName = "DropScriptableObject", menuName = "ScriptableObjects/DropScriptableObject")]
    public class DropScriptableObject : ScriptableObject
    {
        public DropType dropType;
        public DropView dropView;
        public Vector3 spawnPosition;
        public float dropRadius;

        //an editor script can be written for defining certain properties that can only be changed based on dropType
        public int coinValue;
    }
}