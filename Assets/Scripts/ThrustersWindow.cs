using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrustersWindow : MonoBehaviour
{
    private Transform barTemplate;

    private void Awake()
    {
        barTemplate = transform.transform.Find("BarTemplate");
        barTemplate.gameObject.SetActive(false);
    }

    private void CreateBar(Vector2 anchoredPosition, Vector2 size)
    {
        Transform barTransform = Instantiate(barTemplate, transform);
        barTransform.gameObject.SetActive(true);
        RectTransform barRectTransform = barTransform.GetComponent<RectTransform>();
        barRectTransform.anchoredPosition = anchoredPosition;
        barRectTransform.sizeDelta = size;
    }
}
