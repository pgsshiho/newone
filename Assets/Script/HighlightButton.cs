using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class HighlightButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TMP_Text textMeshPro;

    // ����� ����
    public Color highlightColor = Color.yellow;
    // ���� ����
    private Color originalColor;

    void Start()
    {
        textMeshPro = GetComponentInChildren<TMP_Text>();
        if (textMeshPro != null)
        {
            originalColor = textMeshPro.color;
        }
        else
        {
            Debug.LogError("TMP_Text ������Ʈ�� ã�� �� �����ϴ�.");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (textMeshPro != null)
        {
            textMeshPro.color = highlightColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (textMeshPro != null)
        {
            textMeshPro.color = originalColor;
        }
    }
}