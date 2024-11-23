using UnityEngine;

namespace Entities
{
    [CreateAssetMenu(menuName = "Resource", fileName = "Resource")]
    public class ResourceScriptableObject : ScriptableObject
    {
        public string Name;
        public Sprite Sprite;
    }
}