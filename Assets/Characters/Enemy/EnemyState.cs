
public interface EnemyState
{
    EnemyState HandleTransition(Enemy enemy);
    void HandleUpdate(Enemy enemy);
    void OnEnter(Enemy enemy);
    void OnExit(Enemy enemy);
}
