using UnityEngine;

public class TowerLevel : MonoBehaviour
{
    public int currentLevel = 1;
    public int experience = 0;

    private void Start()
    {
        EventManager.Subscribe(GameEntries.GAME_EVENTS.TowerGotExp.ToString(), OnTowerGotExp);
    }

    void OnTowerGotExp(object exp)
    {
        experience += (int)exp;
        if (experience >= currentLevel * 100)
        {
            LevelUp();
        }
    }

    public void Set(int level, int exp)
    {
        currentLevel = level;
        experience = exp;
    }

    public void LevelUp()
    {
        currentLevel++;
        EventManager.Trigger(GameEntries.GAME_EVENTS.TowerLevelUp.ToString(), currentLevel);
    }

    private void OnDestroy()
    {
        EventManager.Unsubscribe(GameEntries.GAME_EVENTS.TowerGotExp.ToString(), OnTowerGotExp);
    }
}
