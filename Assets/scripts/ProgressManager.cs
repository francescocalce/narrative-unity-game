using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProgressManager : MonoBehaviour
{
    [Header("UI")]
    public Image progressBarFill;
    public TextMeshProUGUI progressText;
    
    [Header("Tappe del gioco")]
    private int totalSteps = 4; // Totale tappe: visore, video, puzzle, (eventuale altra)
    private int currentStep = 0;
    
    void Start()
    {
        UpdateProgressBar();
    }
    
    public void CompleteStep(string stepName)
    {
        currentStep++;
        Debug.Log($"Completato: {stepName} ({currentStep}/{totalSteps})");
        UpdateProgressBar();
    }
    
    void UpdateProgressBar()
    {
        float progress = (float)currentStep / totalSteps;
        
        if (progressBarFill != null)
            progressBarFill.fillAmount = progress;
        
        if (progressText != null)
            progressText.text = $"{Mathf.RoundToInt(progress * 100)}%";
    }
    
    public void ResetProgress()
    {
        currentStep = 0;
        UpdateProgressBar();
    }
}