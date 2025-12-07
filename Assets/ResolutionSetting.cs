using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ResolutionSettings : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;

    private List<(int width, int height)> _resolutions = new List<(int, int)>();

    void Start()
    {
        resolutionDropdown.ClearOptions();

        HashSet<string> seen = new HashSet<string>();
        
        foreach (var res in Screen.resolutions)
        {
            string key = $"{res.width}x{res.height}";

            if (!seen.Contains(key))
            {
                seen.Add(key);
                _resolutions.Add((res.width, res.height));
            }
        }
        
        List<string> options = new List<string>();
        int currentResIndex = 0;

        for (int i = 0; i < _resolutions.Count; i++)
        {
            var (w, h) = _resolutions[i];
            options.Add($"{w} x {h}");
            
            if (w == Screen.currentResolution.width &&
                h == Screen.currentResolution.height)
            {
                currentResIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResIndex;
        resolutionDropdown.RefreshShownValue();

        resolutionDropdown.onValueChanged.AddListener(SetResolution);
    }

    public void SetResolution(int index)
    {
        var (width, height) = _resolutions[index];

        // Set refresh rate to 60
        Screen.SetResolution(width, height, Screen.fullScreenMode, 60);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
}