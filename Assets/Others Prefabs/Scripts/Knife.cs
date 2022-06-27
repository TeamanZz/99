using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Knife : MonoBehaviour
{
    public static Knife knife;

    [Header("View Settings")]
    public Image fillImage;

    [Header("Connect Settings")]
    public GameObject knifeGroup;

    [Header("Main Settings")]
    //[SerializeField] private int capacityPoints = 50;
    //public int currentPoints = 0;

    public bool readyToUse = false;
    public float knifeDuration = 5f;
    private Coroutine currentCoroutine;

    public void Awake()
    {
        knife = this;
        knifeGroup.SetActive(false);
        //CheckState(); 
        
        //AddFillPoint(0);
    }
    
    //[ContextMenu("Check State")]
    //public void CheckState()
    //{
    //    if (isOpen)
    //        knifeGroup.SetActive(true);
    //    else
    //        knifeGroup.SetActive(false);

    //    if (currentPoints == capacityPoints)
    //        useKnifeButton.interactable = true;
    //    else
    //        useKnifeButton.interactable = false;
    //}

    //public void AddFillPoint(int value)
    //{
    //    if (!isOpen || currentCoroutine != null)
    //    {
    //        return;
    //    }

    //    currentPoints += value;
    //    currentPoints = Mathf.Clamp(currentPoints, 0, capacityPoints);

    //    float coefficient = 1f / capacityPoints;
    //    fillImage.fillAmount = coefficient * currentPoints;

    //    CheckState();
    //    //if (currentPoints == capacityPoints)
    //    //    useKnifeButton.interactable = true;
    //}

    public void UseKnife()
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        currentCoroutine = StartCoroutine(UsingKnife(knifeDuration));

        //useKnifeButton.interactable = false;
    }

    private IEnumerator UsingKnife(float time)
    {
        readyToUse = true;
        knifeGroup.SetActive(true);
        fillImage.fillAmount = 1;

        //useKnifeButton.interactable = false;

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
        
        readyToUse = false;
        //currentPoints = 0;

        currentCoroutine = null;
        knifeGroup.SetActive(false);
    }
}
