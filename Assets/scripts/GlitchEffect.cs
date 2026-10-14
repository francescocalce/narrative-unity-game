using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GlitchEffect : MonoBehaviour
{
    [Header("Glitch Settings")]
    public float glitchFrequency = 0.15f; // Probabilità di glitch ogni frame (bassa = raro)
    public float minGlitchDuration = 0.05f;
    public float maxGlitchDuration = 0.2f;
    public int minStripes = 2;
    public int maxStripes = 6;
    
    private List<Image> glitchStripes = new List<Image>();
    private Canvas canvas;
    private float glitchTimer = 0f;
    private bool isGlitching = false;

    void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("GlitchEffect deve essere su un GameObject dentro un Canvas!");
            return;
        }
        
        // Pre-crea alcune strisce invisibili
        for (int i = 0; i < maxStripes; i++)
        {
            CreateGlitchStripe();
        }
    }

    void CreateGlitchStripe()
    {
        GameObject stripeObj = new GameObject($"GlitchStripe_{glitchStripes.Count}");
        stripeObj.transform.SetParent(transform, false);
        
        Image stripe = stripeObj.AddComponent<Image>();
        stripe.color = new Color(1f, 1f, 1f, 0.8f); // Bianco semi-trasparente
        stripe.raycastTarget = false;
        
        RectTransform rt = stripeObj.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0, 0.5f);
        rt.anchorMax = new Vector2(1, 0.5f);
        rt.sizeDelta = new Vector2(0, 0); // Larghezza schermo, altezza da definire
        
        stripeObj.SetActive(false);
        glitchStripes.Add(stripe);
    }

    void Update()
    {
        if (isGlitching)
        {
            glitchTimer -= Time.deltaTime;
            if (glitchTimer <= 0f)
            {
                HideGlitch();
            }
        }
        else
        {
            // Random chance di glitch
            if (Random.value < glitchFrequency * Time.deltaTime)
            {
                TriggerGlitch();
            }
        }
    }

    void TriggerGlitch()
    {
        isGlitching = true;
        glitchTimer = Random.Range(minGlitchDuration, maxGlitchDuration);
        
        int numStripes = Random.Range(minStripes, maxStripes + 1);
        
        for (int i = 0; i < numStripes && i < glitchStripes.Count; i++)
        {
            Image stripe = glitchStripes[i];
            RectTransform rt = stripe.GetComponent<RectTransform>();
            
            // Posizione Y casuale
            float yPos = Random.Range(-Screen.height / 2f, Screen.height / 2f);
            
            // Altezza casuale
            float height = Random.Range(5f, 40f);
            
            rt.anchoredPosition = new Vector2(0, yPos);
            rt.sizeDelta = new Vector2(0, height);
            
            // Tagli scuri casuali
            if (Random.value > 0.7f)
                stripe.color = new Color(0f, 0f, 0f, 0.8f); // Nero opaco
            else
                stripe.color = new Color(0f, 0f, 0f, 0.5f); // Nero semi-trasparente
            
            stripe.gameObject.SetActive(true);
        }
    }

    void HideGlitch()
    {
        isGlitching = false;
        
        foreach (Image stripe in glitchStripes)
        {
            stripe.gameObject.SetActive(false);
        }
    }
}