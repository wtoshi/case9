using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public int selectedTowerId = -1;
    public int gameLevel = 1;
    public TowerState towerState = null;

    [System.Serializable]
    public class TowerState
    {        
        public int level = 1; 
        public int experience = 0;
    }
}