using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ArrowUISectionSwitcher : MonoBehaviour
{
    [Header("UI Sections (Panels with CanvasGroup)")]
    public CanvasGroup[] sections;
    [Header("Arrow Buttons")]
    public Button nextArrowButton;   
    public Button prevArrowButton;  

    private int currentIndex = 0;
    private bool isTransitioning = false;

    void Start()
    {
        sections = FindObjectsOfType<CanvasGroup>();
        if (sections == null || sections.Length == 0)
        {
            Debug.LogError("No UI sections assigned!");
            return;
        }

        if (nextArrowButton == null || prevArrowButton == null)
        {
            Debug.LogError("Arrow buttons not assigned!");
            return;
        }

        // Show only the first section
        ShowSectionImmediate(currentIndex);

        // Add listeners
        nextArrowButton.onClick.AddListener(() => SwapSection(1));
        prevArrowButton.onClick.AddListener(() => SwapSection(-1));
    }

    void SwapSection(int direction)
    {
        if (isTransitioning) return;

        int nextIndex = (currentIndex + direction + sections.Length) % sections.Length;
        StartCoroutine(TransitionSections(currentIndex, nextIndex));
        currentIndex = nextIndex;
    }

    IEnumerator TransitionSections(int fromIndex, int toIndex)
    {
        isTransitioning = true;

        CanvasGroup from = sections[fromIndex];
        CanvasGroup to = sections[toIndex];

        to.gameObject.SetActive(true);

        float duration = 0.5f;
        float t = 0;

        while (t < duration)
        {
            t += Time.deltaTime;
            float alpha = t / duration;

            from.alpha = 1 - alpha;
            to.alpha = alpha;

            yield return null;
        }

        from.alpha = 0;
        from.gameObject.SetActive(false);
        to.alpha = 1;

        isTransitioning = false;
    }

    void ShowSectionImmediate(int index)
    {
        for (int i = 0; i < sections.Length; i++)
        {
            sections[i].gameObject.SetActive(i == index);
            sections[i].alpha = i == index ? 1 : 0;
        }
    }
}