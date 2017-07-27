
public interface PlayerState {
    PlayerState HandleTransition(PlayerCharacter player);
    void HandleUpdate(PlayerCharacter player);
    void OnEnter(PlayerCharacter player);
    void OnExit(PlayerCharacter player);
}
