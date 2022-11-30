using UnityEngine;
using System.Collections;

public class ThermometerScript : PausableBehaviour  // MonoBehaviour
{
    public GameObject degreeSymbol;
    public GameObject degreeCapSymbol;
    public float temperatureDecreaseStep;
    [Tooltip("سرعت رسیدن دماسنح به تعادل را در هر بار تغییر دما مشخص میکند.")]
    public float raiseSpeed;
    [Tooltip("کمترین سایز y نماد درجه را مشخص می‌کند.")]
    public float maxScaleOfDegreeSymbol;
    [Tooltip("بیشترین سایز y نماد درجه را مشخص می‌کند.")]
    public float minScaleOfDegreeSymbol;
    [Tooltip("اگر نماد Degree Cap کاملا بالای دماسنج قرار نمیگیرد این متغیر را دستکاری کنید.")]
    public float yCorrection;

    Vector3 position;
    Vector3 currentPosition;
    Vector3 scale;

    float raiseScalePerDegree;
    float lastTemp;
    float currentTemp;
    float y;
    float temp;
    Animator anim;

    float minTemp;
    float maxTemp;

    void Start()
    {
        ChapterLevelScript chapterLevelScript = Camera.main.GetComponent<ChapterLevelScript>();
        maxTemp = chapterLevelScript.MaxTemprature; // This is needed for calculating raise step per degree.
        minTemp = 0;                                // This is needed for calculating raise step per degree.
        if (temperatureDecreaseStep > 0)            // Temprature decrease step should be negative.
        {
            temperatureDecreaseStep *= -1;
        }
        raiseScalePerDegree = (maxScaleOfDegreeSymbol - minScaleOfDegreeSymbol) / (maxTemp - minTemp);  // How much we should decrease or increase
        currentTemp = lastTemp = minTemp;                                                               // degree transform scale per degree.
        scale = degreeSymbol.transform.localScale;
        scale.y = minScaleOfDegreeSymbol;
        position = currentPosition = degreeSymbol.transform.position;
        degreeSymbol.transform.localScale = scale;
        y = degreeSymbol.GetComponent<SpriteRenderer>().sprite.bounds.size.y;
        anim = GetComponent<Animator>();
    }

    protected override void PFixedUpdate()
    {
        temp = GameState.GetTemperature();
        var animState = anim.GetCurrentAnimatorStateInfo(0);

        if (currentTemp < temp - 1)             // اگر دما هنوز به میزان مورد نظر نرسیده و باید بالا رود
        {
            currentTemp = Mathf.Lerp(currentTemp, temp, raiseSpeed * Time.deltaTime);
            scale.y += (currentTemp - lastTemp) * raiseScalePerDegree;  // نماد degree را به اندازه مورد نظر سایزش را زیاد کن
            anim.SetInteger("Heat", 1);                                 // انیمشن بالا رفتن دمای دماسنج
        }
        else if (currentTemp > temp + 1)        // اگر دما هنوز به میزان مورد نظر نرسیده و باید پایین بیاید
        {
            currentTemp = Mathf.Lerp(temp, currentTemp, 1.0f - (raiseSpeed * Time.deltaTime));
            scale.y -= (lastTemp - currentTemp) * raiseScalePerDegree;  // نماد degree را به اندازه مورد نظر سایزش را کم کن
            anim.SetInteger("Heat", -1);                                 // انیمشن پایین آمدن دمای دماسنج
        }
        else// اگر دمای مورد نظر کمتر از ۱ درجه با دمای فعلی تفاوت دارد
        {
            currentTemp = temp; 
            scale.y = (temp - minTemp) * raiseScalePerDegree;
            anim.SetInteger("Heat", 0);
        }
        degreeSymbol.transform.localScale = scale;
        position = currentPosition;
        position.y += scale.y * (y - yCorrection);
        degreeCapSymbol.transform.position = position;
        lastTemp = currentTemp;
        GameState.ChangeTemperature(Time.deltaTime * temperatureDecreaseStep);
    }

    protected override void POnGUI() { }
    protected override void PUpdate() { }
}