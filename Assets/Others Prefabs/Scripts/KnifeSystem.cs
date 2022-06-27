using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KnifeSystem : MonoBehaviour
{
    public static KnifeSystem knife;
    [SerializeField] private LevelManager levelManager;

    [Header("View Settings")]
    public Image fillImage;
    public TextMeshProUGUI damageText;
    
    [Header("Connect Settings")]
    public GameObject vievGroup;

    [Header("Main Settings")]
    public bool readyToUse = false;
    [Tooltip("Work Time")] public float knifeDuration = 5f;
    private Coroutine currentCoroutine;

    public float knifeDamage = 0;
    [SerializeField] private float knifeAddCoefficient = 1;

    [Header("Clamp Settings")]
    [SerializeField] private int minValue = 1;
    [SerializeField] private int maxValue = 40;

    public void Awake()
    {
        knife = this;
        vievGroup.SetActive(false);
    }
    
    public void UseKnife()
    {
        if (currentCoroutine != null)
        {
            knifeDamage += knifeAddCoefficient;
            StopCoroutine(currentCoroutine);
        }

        knifeDamage = Mathf.Clamp(knifeDamage, minValue, maxValue);
        currentCoroutine = StartCoroutine(UsingKnife(knifeDuration));
    }

    private IEnumerator UsingKnife(float time)
    {
        readyToUse = true;
        vievGroup.SetActive(true);
        fillImage.fillAmount = 1;

        levelManager.knifeDamage = Mathf.RoundToInt(knifeDamage);
        damageText.text = "x" + levelManager.knifeDamage.ToString();

        bool timerRinning = false;
        float currentTime = time;
        float coefficient = 1f / time;

        while (!timerRinning)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            currentTime -= Time.deltaTime;

            fillImage.fillAmount = currentTime * coefficient;

            if (currentTime <= 0)
                timerRinning = true;
        }
        
        readyToUse = false;

        knifeDamage = 0;
        levelManager.knifeDamage = Mathf.RoundToInt(knifeDamage);
        damageText.text = "x" + levelManager.knifeDamage.ToString();


        currentCoroutine = null;
        vievGroup.SetActive(false);
    }
}
