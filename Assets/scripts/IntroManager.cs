using UnityEngine;
using UnityEngine.InputSystem;

public class IntroManager : MonoBehaviour
{
    public GameObject pannelloIntro;
    public GameObject triggerVideo;
    public GameObject overlayVisore; // Overlay blu
    public GameObject vignetteVisore; // Vignetta scura bordi
    public GameObject minimapUI;
    public GameObject glitchContainer; // Container per effetto glitch (tagli orizzontali)
    public ProgressManager progressManager;
    
    private bool introCompletato = false;

    void Start()
    {
        pannelloIntro.SetActive(true);
        triggerVideo.SetActive(false);
        
        // Disattiva tutti gli elementi del visore all'inizio
        if (overlayVisore != null)
            overlayVisore.SetActive(false);
        if (vignetteVisore != null)
            vignetteVisore.SetActive(false);
        if (minimapUI != null)
            minimapUI.SetActive(false);
        if (glitchContainer != null)
            glitchContainer.SetActive(false); // Disattiva glitch
    }

    void Update()
    {
        if (!introCompletato && Keyboard.current != null && Keyboard.current.xKey.wasPressedThisFrame)
        {
            AttivaVisore();
        }
    }

    void AttivaVisore()
    {
        introCompletato = true;
        pannelloIntro.SetActive(false);
        triggerVideo.SetActive(true);
        
        // Attiva overlay blu
        if (overlayVisore != null)
            overlayVisore.SetActive(true);
        
        // Attiva vignetta scura
        if (vignetteVisore != null)
            vignetteVisore.SetActive(true);
        
        // Attiva mini-mappa
        if (minimapUI != null)
            minimapUI.SetActive(true);
        
        // Attiva effetto glitch
        if (glitchContainer != null)
            glitchContainer.SetActive(true);
        
        // Aggiorna barra avanzamento
        if (progressManager != null)
            progressManager.CompleteStep("Visore attivato");
        
        Debug.Log("Visore attivato con effetto completo!");
    }
}