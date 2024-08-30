using System.Collections;
using UnityEngine;

public class HandCardDisplay : CardDisplay
{
    public delegate void PlayCardHandler(CardDisplay card);

    public static event PlayCardHandler OnPlayCard;
    private Vector3 originalScale;
    private readonly float transitionDuration = 0.1f;

    new void Start()
    {
        base.Start();
        originalScale = transform.localScale;
    }

    void OnMouseDown()
    {
        OnPlayCard?.Invoke(this);
    }

    void OnMouseEnter()
    {
        CardDetails.Instance.Show(this.card);
        StartCoroutine(SmoothScale(1.1f));
    }

    void OnMouseExit()
    {
        StartCoroutine(SmoothScale(1f));
    }

    IEnumerator SmoothScale(float targetScaleFactor)
    {
        float elapsedTime = 0;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;

            float progress = elapsedTime / transitionDuration;
            transform.localScale = Vector3.Lerp(originalScale, originalScale * targetScaleFactor, progress);

            yield return null;
        }
        transform.localScale = originalScale * targetScaleFactor;
    }
}
