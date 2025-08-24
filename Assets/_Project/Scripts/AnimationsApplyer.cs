using System.Collections;
using DG.Tweening;
using UnityEngine;

public class AnimationsApplyer
{
    public IEnumerator AnimateBrushTakesMakeup(Transform brush, Vector3 makeupPosition, float duration, float zRotation, float moveHeight)
    {
        float startZRotation = brush.rotation.z;
        brush.DOMove(makeupPosition, duration, true);
        Tween tween1 = brush.DORotate(new Vector3(brush.rotation.x, brush.rotation.y, zRotation), duration);
        yield return tween1.WaitForCompletion();
        yield return ApplyTakingMakeup(brush, duration, startZRotation, moveHeight);
        Tween tween2 = brush.DORotate(new Vector3(brush.rotation.x, brush.rotation.y, startZRotation), duration);
        yield return tween2.WaitForCompletion();
    }

    public IEnumerator AnimateMakeupApplying(Transform makeupApplyer, RectTransform makeupImage, float duration, float yOffset = 0f)
    {
        Vector3 startPosition = makeupImage.position - new Vector3(makeupImage.sizeDelta.x / 2, yOffset, 0f);
        makeupApplyer.position = startPosition;
        Sequence sequence = DOTween.Sequence();
        
        for (int i = 0; i < 3; i++)
        {
            sequence.Append(makeupApplyer.DOMoveX(startPosition.x + makeupImage.sizeDelta.x, duration / 6));
            sequence.Append(makeupApplyer.DOMoveX(startPosition.x, duration / 6));
        }
        
        sequence.Play();
        yield return sequence.WaitForCompletion();
    }
    
    private IEnumerator ApplyTakingMakeup(Transform brush, float duration, float startZRotation, float moveHeight)
    {
        Vector3 startPosition = brush.position;
        Sequence sequence = DOTween.Sequence();
        
        for (int i = 0; i < 3; i++)
        {
            sequence.Append(brush.DOMoveY(startPosition.y + moveHeight, duration / 2));
            sequence.Append(brush.DOMoveY(startPosition.y, duration / 2));
        }

        Tween tween = sequence.Play();
        yield return tween.WaitForCompletion();
    }
}