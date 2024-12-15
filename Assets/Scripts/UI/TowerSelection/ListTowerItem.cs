using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Timeline;

public class ListTowerItem : MonoBehaviour
{
    public TextMeshProUGUI towerNameTxt;
    public Image towerIcon;
    public GameObject focus;

    [HideInInspector]
    public int towerId;
    [HideInInspector]
    public string towerName;
    [HideInInspector]
    public string towerDescription;

    TowerSelectionUI towerSelectionUI;

    public bool IsSelected => isSelectedTower;
    bool isSelectedTower;

    private void Awake()
    {
        towerSelectionUI = GetComponentInParent<TowerSelectionUI>();
    }

    public void Set(Tower tower)
    {
        this.towerId = tower.id;
        this.towerName = tower.towerName;
        this.towerDescription = tower.towerDescription;

        towerNameTxt.text = tower.towerName;

        towerIcon.sprite = tower.towerIcon;

        focus.SetActive(false);
    }

    public void SetSelected(bool _isSelected = true)
    {
        isSelectedTower = _isSelected;

        focus.SetActive(isSelectedTower);
    }

    public void OnClick()
    {
        towerSelectionUI.SetSelectedTower(this);
    }

}
