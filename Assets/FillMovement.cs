using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FillMovement : MonoBehaviour
{
    public Transform fillImage;
    public Transform fillImageBack;
    public Transform particles;

    void Start()
    {
        fillImage.DOMoveY(fillImage.position.y + 1, 3f).SetEase(Ease.OutExpo).SetLoops(-1, LoopType.Yoyo);
        fillImageBack.DOMoveY(fillImageBack.position.y + 1, 3f).SetEase(Ease.OutExpo).SetLoops(-1, LoopType.Yoyo);
        particles.DOMoveY(particles.position.y + 1, 3f).SetEase(Ease.OutExpo).SetLoops(-1, LoopType.Yoyo);
    }
}