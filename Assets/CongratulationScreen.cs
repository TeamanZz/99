using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class CongratulationScreen : MonoBehaviour
{
    public Image backgroundImage;
    public TextMeshProUGUI topText;
    public TextMeshProUGUI bottomText;
    public Transform newPlateImage;
    Sequence imageSeq;
    public Button doneButton;
    public TextMeshProUGUI doneText;

    private void OnEnable()
    {
        backgroundImage.color = new Color(0, 0, 0, 0);
        backgroundImage.DOFade(0.06f, 0.5f);

        newPlateImage.localScale = Vector3.zero;
        doneButton.transform.localScale = Vector3.zero;
        doneText.color = new Color(255, 255, 255, 0);

        topText.transform.localScale = Vector3.zero;
        bottomText.transform.localScale = Vector3.zero;

        topText.color = new Color(255, 255, 255, 0);
        bottomText.color = new Color(255, 255, 255, 0);

        topText.transform.DOScale(1, 1);
        bottomText.transform.DOScale(1, 2);

        topText.DOFade(1, 1);
        bottomText.DOFade(1, 2);

        imageSeq = DOTween.Sequence();
        newPlateImage.DOMoveY(newPlateImage.position.y + 3, 3f).SetEase(Ease.InBounce).SetLoops(-1, LoopType.Yoyo);
        imageSeq.Append(newPlateImage.DOScale(0.9f, 2).SetEase(Ease.OutBack).SetLoops(-1, LoopType.Yoyo));
        imageSeq.Append(newPlateImage.DOScale(1.2f, 2).SetEase(Ease.InOutBack).SetLoops(-1, LoopType.Yoyo));
        imageSeq.Append(newPlateImage.DOScale(1f, 2).SetEase(Ease.InOutBack).SetLoops(-1, LoopType.Yoyo));
        imageSeq.SetLoops(-1, LoopType.Yoyo);

        StartCoroutine(DoneButtonAppear());
    }

    private IEnumerator DoneButtonAppear()
    {
        yield return new WaitForSeconds(3);
        doneButton.transform.DOScale(1, 1);
        doneText.DOFade(1, 2);
    }

    private void OnDisable()
    {
        imageSeq.Kill();
    }
}