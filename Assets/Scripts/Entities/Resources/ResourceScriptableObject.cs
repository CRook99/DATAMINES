using UnityEngine;

namespace Entities
{
    [CreateAssetMenu(menuName = "Resource", fileName = "Resource")]
    public class ResourceScriptableObject : ScriptableObject
    {
        public int Id;
        public string Name;
        public Sprite Sprite;

        public override bool Equals(object obj)
        {
            if (obj is ResourceScriptableObject other)
            {
                return Id == other.Id && Name == other.Name;
            }

            return false;
        }
        
        public override int GetHashCode()
        {
            return (Id, name).GetHashCode();
        }
    }
}