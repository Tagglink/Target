using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class settingsMenuManager : MonoBehaviour {

    public struct Resolution
    {
        public int XScale;
        public int YScale;
    }

    public List<Resolution> Resolutions = new List<Resolution>();

    private Resolution currentResolution;
    private int currentResolutionIndex;
    
    private List<int> AntiAliasing = new List<int>();
    private int currentAntiAlias = 1;

    private List<int> QualityLevels = new List<int>();
    private int currentQualityLevel = 1;

    public GameObject ResolutionCurrentText;
    public GameObject VsyncCurrentText;
    public GameObject FullScreenCurrentText;
    public GameObject AntiAliasCurrentText;
    public GameObject QualityCurrentText;

    public GameObject AudioPanel;
    public GameObject ControlsPanel;
    public GameObject GraphicsPanel;

    private GameObject currentPanel;

    void Start()
    {
        // PlayerPrefs
        if (!PlayerPrefs.HasKey("Settings_Graphics_Resolution"))
            PlayerPrefs.SetInt("Settings_Graphics_Resolution", 0);

        if (!PlayerPrefs.HasKey("Settings_Graphics_VSync"))
            PlayerPrefs.SetInt("Settings_Graphics_VSync", 1);

        if (Screen.fullScreen)
            FullScreenCurrentText.GetComponent<Text>().text = "On";
        if (QualitySettings.vSyncCount == 1)
            VsyncCurrentText.GetComponent<Text>().text = "On";

        foreach (UnityEngine.Resolution a in Screen.resolutions) {
            Resolutions.Add(new Resolution { XScale = a.width, YScale = a.height });
        }

        currentResolutionIndex = PlayerPrefs.GetInt("Settings_Graphics_Resolution");

        for (int i = 0; i < 8; i += 2)
        {
            if (i == 6)
                i = 8;
            AntiAliasing.Add(i);
        }

        for (int i = 0; i < 5; i++)
        {
            QualityLevels.Add(i);
        }

        setCurrentPanel(AudioPanel);
        setCurrentResolution();
        applyResolution();
    }

    void changeCurrentResolutionIndex(bool Direction)
    {
        if (Direction && Resolutions.Count > currentResolutionIndex + 1)
            currentResolutionIndex += 1;
        else if (!Direction && currentResolutionIndex - 1 >= 0)
            currentResolutionIndex -= 1;
        PlayerPrefs.SetInt("Settings_Graphics_Resolution", currentResolutionIndex);
    }

    void setCurrentResolution()
    {
        currentResolution = Resolutions[currentResolutionIndex];
        ResolutionCurrentText.GetComponent<Text>().text = currentResolution.XScale + "x" + currentResolution.YScale;
    }

    void applyResolution()
    {
        if (Screen.fullScreen)
        {
            Screen.SetResolution(currentResolution.XScale, currentResolution.YScale, false);
            Screen.fullScreen = true;
        }
        else
            Screen.SetResolution(currentResolution.XScale, currentResolution.YScale, false);
        }

	public void ChangeResolution(bool Direction)
    {
        changeCurrentResolutionIndex(Direction);
        setCurrentResolution();
        applyResolution();
    }

    // Vsync

    public void ToggleVsync()
    {
        QualitySettings.vSyncCount += 1;
        if (QualitySettings.vSyncCount == 2)
            QualitySettings.vSyncCount = 0;

        PlayerPrefs.SetInt("Settings_Graphics_VSync", QualitySettings.vSyncCount);

        switch (QualitySettings.vSyncCount) {
            case 0:
                VsyncCurrentText.GetComponent<Text>().text = "Off";
                return;
            case 1:
                VsyncCurrentText.GetComponent<Text>().text = "On";
                return;
            default:
                return;
            }
    }

    // Fullscreen

    public void ToggleFullscreen()
    {
        Screen.fullScreen = !Screen.fullScreen;

        if (Screen.fullScreen)
            FullScreenCurrentText.GetComponent<Text>().text = "Off";
        else
            FullScreenCurrentText.GetComponent<Text>().text = "On";
    }

    // Anti-Alias

    public void ToggleAntiAlias(bool Direction)
    {
        if (Direction && AntiAliasing.Count > currentAntiAlias + 1)
            currentAntiAlias++;
        else if (!Direction && currentAntiAlias > 0)
            currentAntiAlias--;

        QualitySettings.antiAliasing = AntiAliasing[currentAntiAlias];

        switch (AntiAliasing[currentAntiAlias])
        {
            case 0:
                AntiAliasCurrentText.GetComponent<Text>().text = "Off";
                return;
            case 2:
                AntiAliasCurrentText.GetComponent<Text>().text = "Low";
                return;
            case 4:
                AntiAliasCurrentText.GetComponent<Text>().text = "Medium";
                return;
            case 8:
                AntiAliasCurrentText.GetComponent<Text>().text = "High";
                return;
        }
    }
    
    // Quality

    public void ToggleQuality(bool Direction)
    {
        if (Direction && AntiAliasing.Count > currentQualityLevel + 1)
            currentQualityLevel++;
        else if (!Direction && currentQualityLevel > 0)
            currentQualityLevel--;

        QualitySettings.SetQualityLevel(currentQualityLevel, true);

        switch (QualityLevels[currentQualityLevel])
        {
            case 0:
                QualityCurrentText.GetComponent<Text>().text = "Low";
                return;
            case 1:
                QualityCurrentText.GetComponent<Text>().text = "Medium";
                return;
            case 2:
                QualityCurrentText.GetComponent<Text>().text = "High";
                return;
            case 3:
                QualityCurrentText.GetComponent<Text>().text = "Ultra";
                return;
        }
    }

    // Selected Settings

    public void setCurrentPanel(GameObject panel)
    {
        if (currentPanel)
        {
            currentPanel.GetComponent<CanvasGroup>().interactable = false;
            currentPanel.GetComponent<CanvasGroup>().alpha = 0;
        }

        currentPanel = panel;

        currentPanel.GetComponent<CanvasGroup>().interactable = true;
        currentPanel.GetComponent<CanvasGroup>().alpha = 1;
    }
}
