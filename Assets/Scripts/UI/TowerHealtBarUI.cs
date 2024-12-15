using UnityEngine;
using UnityEngine.UI;

public class TowerHealtBarUI : MonoBehaviour
{
    [SerializeField] Slider Slider;
    float progress = 1f;

    private void Start()
    {
        EventManager.Subscribe(GameEntries.GAME_EVENTS.UpdateTowerHealthUI.ToString(), UpdateHealthBar);
    }

    private void UpdateHealthBar(object parameter)
    {
        progress = (float)parameter;
        Slider.value = progress;
    }

    private void OnDestroy()
    {
        EventManager.Unsubscribe(GameEntries.GAME_EVENTS.UpdateTowerHealthUI.ToString(), UpdateHealthBar);
    }
}
