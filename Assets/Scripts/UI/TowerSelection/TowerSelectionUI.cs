using TMPro;
using UnityEngine;

public class TowerSelectionUI : MonoBehaviour
{
    [SerializeField] GameObject listTowerItemPF;
    [SerializeField] Transform holder;

    [SerializeField] TextMeshProUGUI towerDescription;

    private void OnEnable()
    {
        ClearHolder();

        var towers = GameManager.Instance.GetTowers();
        foreach (var tower in towers)
        {
            var towerItem = Instantiate(listTowerItemPF, holder);
            towerItem.GetComponent<ListTowerItem>().Set(tower);
        }

        SetDescription("");
    }

    public void SetSelectedTower(ListTowerItem towerItem)
    {
        foreach (Transform child in holder)
        {
            child.GetComponent<ListTowerItem>().SetSelected(false);
        }
        towerItem.SetSelected(true);

        SetDescription(towerItem.towerDescription);
    }

    public void OnSelectTower()
    {
        foreach (Transform child in holder)
        {
            if (child.GetComponent<ListTowerItem>().IsSelected)
            {
                EventManager.Trigger(GameEntries.GAME_EVENTS.TowerSelected.ToString(), child.GetComponent<ListTowerItem>().towerId);
                break;
            }
        }
    }

    void ClearHolder()
    {
        foreach (Transform child in holder)
        {
            Destroy(child.gameObject);
        }
    }

    void SetDescription(string description)
    {
        Debug.Log(description);
        towerDescription.text = description;
    }
}
