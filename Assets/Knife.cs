using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Knife : MonoBehaviour
{
    public static Knife knife;

    [Header("View Settings")]
    public Image fillImage;
    public Button useKnifeButton;

    [Header("Connect Settings")]
    public bool isOpen = false;
    public GameObject knifeGroup;

    [Header("Main Settings")]
    [SerializeField] private int capacityPoints = 50;
    public int currentPoints = 0;

    public bool readyToUse = false;
    public float knifeDuration = 5f;
    private Coroutine currentCoroutine;

    public void Awake()
    {
        knife = this;
        CheckState();
        CheckButtonState();
    }
    
    [ContextMenu("Check State")]
    public void CheckState()
    {
        if (isOpen)
            knifeGroup.SetActive(true);
        else
            knifeGroup.SetActive(false);
    }

    public void AddFillPoint(int value)
    {
        if (!isOpen)
            return;

        currentPoints += value;
        currentPoints = Mathf.Clamp(currentPoints, 0, capacityPoints);

        CheckButtonState();
    }

    private  void CheckButtonState()
    {
        if (currentPoints == capacityPoints)
            useKnifeButton.interactable = true;
        else
            useKnifeButton.interactable = false;

        float coefficient = 1f / capacityPoints;
        fillImage.fillAmount = coefficient * currentPoints;
    }

    public void UseKnife()
    {
        currentCoroutine = StartCoroutine(UsingKnife(knifeDuration));
    }

    private IEnumerator UsingKnife(float time)
    {
        readyToUse = true;

        bool timerRinning = false;
        float currentTime = time;
        float coefficient = 1f / time;

        while (!timerRinning)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            currentTime -= Time.deltaTime;

            fillImage.fillAmount = /*1f - */(currentTime * coefficient);

            if (currentTime <= 0)
                timerRinning = true;
        }

        yield return new WaitForSeconds(time);
        
        readyToUse = false;
        currentPoints = 0;
        currentCoroutine = null;

        CheckButtonState();
    }
}
