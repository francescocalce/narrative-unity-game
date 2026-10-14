using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PuzzlePiece : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int pieceIndex;
    public PuzzleGame puzzleGame;
    
    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 originalPosition;
    private Transform originalParent;
    private bool isUsed = false;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isUsed) return;
        
        originalPosition = rectTransform.anchoredPosition;
        originalParent = transform.parent;
        
        // Porta in primo piano
        transform.SetParent(canvas.transform);
        canvasGroup.alpha = 0.7f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isUsed) return;
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isUsed) return;
        
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        
        // Torna alla posizione originale
        transform.SetParent(originalParent);
        rectTransform.anchoredPosition = originalPosition;
    }

        public void MarkAsUsed()
        {
            isUsed = true;
            Destroy(gameObject); // Distrugge completamente il pezzo dal pannello
        }
}