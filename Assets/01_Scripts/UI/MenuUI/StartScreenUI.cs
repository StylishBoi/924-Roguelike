using System;
using UnityEngine;

public class StartScreenUI : MonoBehaviour
{
    [Header("Main Panels")]
    [SerializeField] GameObject menuPanel;
    [SerializeField] GameObject manualPanel;
    
    [Header("Manual Panels")]
    [SerializeField] GameObject instructionsPanel;
    [SerializeField] GameObject controlsPanel;
    [SerializeField] GameObject itemsPanel;
    [SerializeField] GameObject enemyPanel;
    
    private void Start()
    {
        AudioManager.Instance.PlayBGM(AudioManager.Instance.menuOST);
    }

    public void StartGame()
    {
        AudioManager.Instance.PlaySfx(AudioManager.Instance.buttonSFX);
        GameManager.GameStart();
    }

    public void ShowMenu()
    {
        AudioManager.Instance.PlaySfx(AudioManager.Instance.buttonSFX);
        manualPanel.SetActive(false);
        menuPanel.SetActive(true);
    }
    
    public void ShowManual()
    {
        AudioManager.Instance.PlaySfx(AudioManager.Instance.buttonSFX);
        menuPanel.SetActive(false);
        instructionsPanel.SetActive(false);
        controlsPanel.SetActive(false);
        itemsPanel.SetActive(false);
        enemyPanel.SetActive(false);
        manualPanel.SetActive(true);
    }
    
    public void ShowInstructions()
    {
        AudioManager.Instance.PlaySfx(AudioManager.Instance.buttonSFX);
        manualPanel.SetActive(false);
        instructionsPanel.SetActive(true);
    }
    
    public void ShowControls()
    {
        AudioManager.Instance.PlaySfx(AudioManager.Instance.buttonSFX);
        manualPanel.SetActive(false);
        controlsPanel.SetActive(true);
    }
    
    public void ShowItems()
    {
        AudioManager.Instance.PlaySfx(AudioManager.Instance.buttonSFX);
        manualPanel.SetActive(false);
        itemsPanel.SetActive(true);
    }
    
    public void ShowEnemy()
    {
        AudioManager.Instance.PlaySfx(AudioManager.Instance.buttonSFX);
        manualPanel.SetActive(false);
        enemyPanel.SetActive(true);
    }
    
    public void QuitGame()
    {
        AudioManager.Instance.PlaySfx(AudioManager.Instance.buttonSFX);
        Application.Quit();
    }
}
