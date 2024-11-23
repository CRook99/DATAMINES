using Entities.Player;

namespace System
{
    public interface IInteractable
    {
        public void Interact(PlayerInteraction player);
    }
}