using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class AddDamageSystem : MonoBehaviour
{
    public static AddDamageSystem damageSystem;
    [SerializeField] private LevelManager levelManager;

    [Header("View Settings")]
    public Image fillImage;
    public TextMeshProUGUI damageText;

    [Header("Connect Settings")]
    public GameObject vievGroup;

    [Header("Main Settings")]
    [Tooltip("Work Time")]public float damageDuration = 1f;
    private Coroutine currentCoroutine;

    public float damage = 0;
    [SerializeField] private float damageAddCoefficient = 2;

    [Header("Clamp Settings")]
    [SerializeField] private int minValue = 3;
    [SerializeField] private int maxValue = 40;

    public void Awake()
    {
        damageSystem = this;
        vievGroup.SetActive(false);
    }

    public void UseDamage()
    {
        if (currentCoroutine != null)
        {
            damage += damageAddCoefficient;
            StopCoroutine(currentCoroutine);
        }

        damage = Mathf.Clamp(damage, minValue, maxValue);
        currentCoroutine = StartCoroutine(UsingDamageSystem(damageDuration));
    }

    private IEnumerator UsingDamageSystem(float time)
    {
        vievGroup.SetActive(true);
        fillImage.fillAmount = 1;

        levelManager.allDamageValue = Mathf.RoundToInt(damage);
        damageText.text = "+" + levelManager.allDamageValue.ToString();

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

        damage = 0;
        levelManager.allDamageValue = Mathf.RoundToInt(damage);
        damageText.text = "+" + levelManager.allDamageValue.ToString();

        currentCoroutine = null;
        vievGroup.SetActive(false);
    }
}
