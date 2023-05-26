using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro.Examples;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject Menu;
    [SerializeField]
    private GameObject SettingsPanel;
    [SerializeField]
    private GameObject crosshair;
    private bool isOpen=true;
    //private bool isOpenSettings = true;
    [SerializeField]
    private Slider sens;
    private float TMPsens;
    [SerializeField]
    private KeyCode openCloseMenu;
    public void Play() 
    {
        SceneManager.LoadScene(1);
    }

    public void Quit() 
    {
        Application.Quit();
    }
    public void BackMenu() 
    {
        SceneManager.LoadScene(0);
    }

    public void Settings() 
    {
    Menu.SetActive(false);
        SettingsPanel.SetActive(true);

    }
    private void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex==1)
        Menu.SetActive(false);
    }
    private void Update()
    {
            Open();
    }

    public void SetSensitivity() 
    {
         TMPsens= sens.value;
        Camera_Movement.sensitivityMouse = TMPsens;
    }

    private void Open()
    {
        if (Input.GetKeyDown(openCloseMenu))
        {
            if (SceneManager.GetActiveScene().buildIndex == 1)
            {
                isOpen = !isOpen;
                if (isOpen)
                {
                    Menu.SetActive(true);
                    crosshair.SetActive(false);
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    Camera_Movement.sensitivityMouse = 0f;
                }
                else
                {
                    Menu.SetActive(false);
                    SettingsPanel.SetActive(false);
                    crosshair.SetActive(true);
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    Camera_Movement.sensitivityMouse = 250;
                }
            }
        }
    }
    public void CloseSettings() 
    {
        Menu.SetActive(true);
        SettingsPanel.SetActive(false);
    }
}
