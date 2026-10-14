using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PuzzleDropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public int cellIndex;
    public PuzzleGame puzzleGame;
    
    private Image image;
    private Color normalColor;
    private Color highlightColor = new Color(0.29f, 0.62f, 0.87f, 0.18f);

    void Awake()
    {
        image = GetComponent<Image>();
        normalColor = image.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            image.color = highlightColor;
        }
    }

public void OnPointerExit(PointerEventData eventData)
{
    // Non cambiare il colore se c'è già uno sprite
    if (image.sprite == null)
    {
        image.color = normalColor;
    }
}

    public void OnDrop(PointerEventData eventData)
    {
        image.color = normalColor;
        
        PuzzlePiece piece = eventData.pointerDrag.GetComponent<PuzzlePiece>();
        if (piece != null)
        {
            puzzleGame.PlacePiece(piece.pieceIndex, cellIndex);
        }
    }
}