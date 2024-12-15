using TMPro;
using UnityEngine;

public class RoundUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI roundText;

    private void OnEnable()
    {
        EventManager.Subscribe(GameEntries.GAME_EVENTS.RoundStarted.ToString(), OnRoundStarted);
    }

    private void OnRoundStarted(object parameter)
    {
        roundText.text = $"Round {parameter}";
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe(GameEntries.GAME_EVENTS.RoundStarted.ToString(), OnRoundStarted);
    }
}
