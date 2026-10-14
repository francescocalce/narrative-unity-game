using UnityEngine;
using UnityEngine.InputSystem;

public class EasterEggController : MonoBehaviour
{
    [Header("Impostazioni Easter Egg")]
    public AudioClip canzoneSegreto;
    public Transform puntoSegreto; // Posizione dove attivare l'easter egg
    public float raggioAttivazione = 2f; // Quanto vicino devi essere
    
    [Header("Riferimenti")]
    public Transform player;
    
    private AudioSource audioSource;
    private int saltiConsecutivi = 0;
    private float tempoUltimoSalto = 0f;
    private float timeoutSalti = 2f; // Se passan 2 secondi, resetta i salti
    private bool easterEggAttivato = false;
    private bool playerInZona = false;
    
    // Riferimento al CharacterController per rilevare i salti
    private CharacterController characterController;
    private bool eraATerraPrimaFrame = true;

    void Start()
    {
        // Crea AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        
        // Trova il CharacterController del player
        if (player != null)
            characterController = player.GetComponent<CharacterController>();
    }

void Update()
{
    if (easterEggAttivato) return;
    
    // Verifica se il player è nella zona segreta
    VerificaPosizionePlayer();
    
    // Se è nella zona e preme K
    if (playerInZona)
    {
        if (Keyboard.current != null && Keyboard.current.kKey.wasPressedThisFrame)
        {
            AttivaEasterEgg();
        }
    }
}
    
    void VerificaPosizionePlayer()
    {
        if (player == null || puntoSegreto == null) return;
        
        float distanza = Vector3.Distance(player.position, puntoSegreto.position);
        playerInZona = distanza <= raggioAttivazione;
        
        // Debug visivo
        if (playerInZona && saltiConsecutivi > 0)
        {
            Debug.Log($"Nella zona segreta! Salti: {saltiConsecutivi}/2");
        }
    }
    
    void RilevaSalti()
    {
        if (characterController == null) return;
        
        bool aTerraAdesso = characterController.isGrounded;
        
        // Rileva quando atterra (transizione da aria a terra)
        if (aTerraAdesso && !eraATerraPrimaFrame)
        {
            // Ha appena atterrato = ha appena saltato
            saltiConsecutivi++;
            tempoUltimoSalto = Time.time;
            Debug.Log($"Salto rilevato! Totale: {saltiConsecutivi}");
        }
        
        eraATerraPrimaFrame = aTerraAdesso;
    }
    
    void AttivaEasterEgg()
    {
        easterEggAttivato = true;
        
        if (canzoneSegreto != null && audioSource != null)
        {
            audioSource.PlayOneShot(canzoneSegreto);
            Debug.Log("🎵 EASTER EGG ATTIVATO! Canzone segreta in riproduzione!");
        }
        
        // Opzionale: mostra un messaggio sullo schermo
        // Puoi aggiungere un popup o testo UI qui
    }
    
    // Visualizza la zona segreta nell'Editor
    void OnDrawGizmos()
    {
        if (puntoSegreto != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(puntoSegreto.position, raggioAttivazione);
        }
    }
}