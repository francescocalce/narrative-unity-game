using UnityEngine;

public class LampeggioSfera : MonoBehaviour
{
    public Color baseColor = new Color(0f, 0.3f, 1f);
    public float minEmission = 2f;
    public float maxEmission = 8f;
    public float minLightIntensity = 1f;  // Intensità minima luce
    public float maxLightIntensity = 5f;  // Intensità massima luce
    public float velocita = 2f;

    private Material mat;
    private Light pointLight;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
        mat.EnableKeyword("_EMISSION");
        
        // Trova la Point Light (o aggiungila se non c'è)
        pointLight = GetComponent<Light>();
        if (pointLight == null)
        {
            pointLight = gameObject.AddComponent<Light>();
            pointLight.type = LightType.Point;
            pointLight.color = new Color(0f, 0.59f, 1f); // Blu elettrico
            pointLight.range = 5f;
        }
    }

    void Update()
    {
        float pulse = Mathf.PingPong(Time.time * velocita, 1f);
        
        // Lampeggio materiale (come prima)
        float emissionIntensity = Mathf.Lerp(minEmission, maxEmission, pulse);
        Color emissionColor = baseColor * emissionIntensity;
        mat.SetColor("_EmissionColor", emissionColor);
        
        // Lampeggio luce ambiente
        if (pointLight != null)
        {
            pointLight.intensity = Mathf.Lerp(minLightIntensity, maxLightIntensity, pulse);
        }
    }
}