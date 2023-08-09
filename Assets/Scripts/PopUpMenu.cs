using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpMenu : MonoBehaviour
{
    public GameObject menu;
    private bool toggle;

    private void Start()
    {
        toggle = false;
    }

    public void ToggleMenu() {
        toggle = !toggle;
        menu.SetActive(toggle);
    }

    private void OnEnable()
    {
        GameEvents.OnGameOver += ToggleMenu;
    }

    private void OnDisable()
    {
        GameEvents.OnGameOver -= ToggleMenu;
    }
}
