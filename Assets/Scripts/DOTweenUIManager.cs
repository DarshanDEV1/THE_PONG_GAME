using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using System; // For TextMeshPro support

public class DOTweenUIManager : MonoBehaviour
{
    // Singleton for easy access (optional)
    public static DOTweenUIManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 1. Slide in from left
    public void SlideInFromLeft(RectTransform uiElement, float duration)
    {
        uiElement.anchoredPosition = new Vector2(-Screen.width, uiElement.anchoredPosition.y);
        uiElement.DOAnchorPosX(0, duration).SetEase(Ease.OutExpo);
    }

    // 2. Slide in from right
    public void SlideInFromRight(RectTransform uiElement, float duration)
    {
        uiElement.anchoredPosition = new Vector2(Screen.width, uiElement.anchoredPosition.y);
        uiElement.DOAnchorPosX(0, duration).SetEase(Ease.OutExpo);
    }

    // 3. Slide in from top
    public void SlideInFromTop(RectTransform uiElement, float duration)
    {
        uiElement.anchoredPosition = new Vector2(uiElement.anchoredPosition.x, Screen.height);
        uiElement.DOAnchorPosY(0, duration).SetEase(Ease.OutExpo);
    }

    // 4. Slide in from bottom
    public void SlideInFromBottom(RectTransform uiElement, float duration)
    {
        uiElement.anchoredPosition = new Vector2(uiElement.anchoredPosition.x, -Screen.height);
        uiElement.DOAnchorPosY(0, duration).SetEase(Ease.OutExpo);
    }

    // 5. Pop in (scale)
    public void PopIn(RectTransform uiElement, float duration)
    {
        uiElement.localScale = Vector3.zero;
        uiElement.DOScale(Vector3.one, duration).SetEase(Ease.OutBack);
    }

    // 6. Pop out (scale)
    public void PopOut(RectTransform uiElement, float duration)
    {
        uiElement.DOScale(Vector3.zero, duration).SetEase(Ease.InBack);
    }

    // 7. Fade in UI element
    public void FadeIn(CanvasGroup uiElement, float duration)
    {
        uiElement.alpha = 0;
        uiElement.DOFade(1, duration);
    }

    // 8. Fade out UI element
    public void FadeOut(CanvasGroup uiElement, float duration)
    {
        uiElement.alpha = 1;
        uiElement.DOFade(0, duration);
    }

    // 9. Bounce in (Y-axis bounce effect)
    public void BounceIn(RectTransform uiElement, float duration)
    {
        uiElement.localScale = Vector3.zero;
        uiElement.DOScale(Vector3.one, duration).SetEase(Ease.OutBounce);
    }

    // 10. Shake effect (like a UI button shake)
    public void ShakeUI(RectTransform uiElement, float duration, float strength = 20, int vibrato = 10)
    {
        uiElement.DOShakePosition(duration, strength, vibrato);
    }

    // 11. Boomerang effect (move to a point and back)
    public void Boomerang(RectTransform uiElement, Vector2 targetPos, float duration)
    {
        Vector2 originalPos = uiElement.anchoredPosition;
        uiElement.DOAnchorPos(targetPos, duration / 2).SetEase(Ease.OutQuad)
            .OnComplete(() => uiElement.DOAnchorPos(originalPos, duration / 2).SetEase(Ease.InQuad));
    }

    // 12. Rotate UI Element (like a spinning icon)
    public void RotateUI(RectTransform uiElement, float duration, float rotationAngle = 360f)
    {
        uiElement.DORotate(new Vector3(0, 0, rotationAngle), duration, RotateMode.FastBeyond360);
    }

    // 13. Pulse (scaling up and down)
    public void Pulse(RectTransform uiElement, float duration, float scaleMultiplier = 1.2f)
    {
        Sequence pulseSequence = DOTween.Sequence();
        pulseSequence.Append(uiElement.DOScale(Vector3.one * scaleMultiplier, duration / 2))
            .Append(uiElement.DOScale(Vector3.one, duration / 2))
            .SetLoops(-1, LoopType.Yoyo);
    }

    // 14. Text typing animation (TextMeshPro)
    //public void TypeText(TextMeshProUGUI textComponent, string fullText, float typeDuration)
    //{
    //    textComponent.text = "";
    //    textComponent.DOText(fullText, typeDuration).SetEase(Ease.Linear);
    //}

    // 15. Jump animation (makes the UI element jump)
    public void Jump(RectTransform uiElement, float jumpPower, int numJumps, float duration)
    {
        uiElement.DOJumpAnchorPos(uiElement.anchoredPosition, jumpPower, numJumps, duration);
    }

    // 16. Flip horizontally (like a card flip)
    public void FlipHorizontally(RectTransform uiElement, float duration)
    {
        uiElement.DORotate(new Vector3(0, 180, 0), duration, RotateMode.LocalAxisAdd);
    }

    // 17. Flip vertically
    public void FlipVertically(RectTransform uiElement, float duration)
    {
        uiElement.DORotate(new Vector3(180, 0, 0), duration, RotateMode.LocalAxisAdd);
    }

    // 18. Expand width (like opening a drawer)
    public void ExpandWidth(RectTransform uiElement, float targetWidth, float duration)
    {
        uiElement.DOSizeDelta(new Vector2(targetWidth, uiElement.sizeDelta.y), duration);
    }

    // 19. Expand height
    public void ExpandHeight(RectTransform uiElement, float targetHeight, float duration)
    {
        uiElement.DOSizeDelta(new Vector2(uiElement.sizeDelta.x, targetHeight), duration);
    }

    // 20. Color change animation (for UI Images)
    public void ChangeColor(Image uiElement, Color targetColor, float duration)
    {
        uiElement.DOColor(targetColor, duration);
    }

    // 21. Tween any value
    public void TweenFloatValue(float originValue, float targetValue, float duration, Action<float, bool> action)
    {
        // Tween the value
        DOTween.To(() => originValue, x => originValue = x, targetValue, duration)
               .OnUpdate(() => action?.Invoke(originValue, false))
               .OnComplete(() => action?.Invoke(originValue, true));
    }

    // 22. Scale object by vector
    public void ScaleValue(Transform targetObject, Vector3 targetScale, float duration)
    {
        targetObject.DOScale(targetScale, duration);
    }
}
