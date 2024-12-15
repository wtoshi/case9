using UnityEngine;
using UnityEngine.Events;

public class UIManager : Singleton<UIManager>
{
    [Header("UI Panels")]
    [SerializeField] private GameObject gameUI;
    [SerializeField] private GameObject menuUI;
    [SerializeField] private GameObject towerSelectionUI;

    [Header("Events")]
    public UnityEvent onGameUIOpened;
    public UnityEvent onMenuUIOpened;
    public UnityEvent onTowerSelectionUIOpened;


    public void OpenGameUI()
    {
        DisableAllUI();
        gameUI.SetActive(true);
        onGameUIOpened?.Invoke();
    }

    public void OpenMenuUI()
    {
        DisableAllUI();
        menuUI.SetActive(true);
        onMenuUIOpened?.Invoke();
    }

    public void OpenTowerSelectionUI()
    {
        DisableAllUI();
        towerSelectionUI.SetActive(true);
        onTowerSelectionUIOpened?.Invoke();
    }

    private void DisableAllUI()
    {
        gameUI.SetActive(false);
        menuUI.SetActive(false);
        towerSelectionUI.SetActive(false);
    }

    public void OnClickPlay()
    {
        GameManager.Instance.StartGame();
    }
}
