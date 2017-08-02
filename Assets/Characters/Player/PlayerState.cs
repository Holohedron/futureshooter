
namespace Player
{
	public interface PlayerState
    {
        PlayerState HandleTransition(PlayerCharacter player);
        void OnEnter(PlayerCharacter player);
        void OnExit(PlayerCharacter player);
        void HandleUpdate(PlayerCharacter player);
        void HandleHit(PlayerCharacter player);
        
    }
}
