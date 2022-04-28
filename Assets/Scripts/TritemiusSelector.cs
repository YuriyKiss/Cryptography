using TMPro;
using UnityEngine;

public class TritemiusSelector : MonoBehaviour
{
    private TMP_Dropdown dropdown;

    public GameObject options;

    private void Awake()
    {
        dropdown = GetComponent<TMP_Dropdown>();
    }

    public void OnSelectorChanged()
    {
        int value = dropdown.value;
        DisableAllOptions();

        switch (value)
        {
            case 1: case 2: case 3:
                options.transform.GetChild(value - 1).gameObject.SetActive(true);
                break;
        }
    }
    
    private void DisableAllOptions()
    {
        foreach(Transform option in options.transform)
        {
            option.gameObject.SetActive(false);
        }
    }
}
