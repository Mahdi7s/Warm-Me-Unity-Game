using UnityEngine;
using System.Collections;
// mod= Mouse Object Data
// mps= Mouse Path Script
// mhs= Mouse House Script
// mhcs= Mouse House Container Script

/// <summary>
/// All data related to a mouse will be store in this fucking class.
/// </summary>
public class MouseObjectsData
{
    public static MouseHouseContainerScript mhcs;
    public MousePathScript path;
    public float triggerTime;
    public MouseScript mouseScript;
    public Transform trap;
    public bool isGone;

    public MouseObjectsData(float triggerTime, MousePathScript path)
    {
        this.path = path;
        this.triggerTime = triggerTime;
        mouseScript = null;
        trap = null;
        isGone = false;
    }

    // Return the oriention of the mouse when it arrives to a specific point in its path.
    public Quaternion GetOriention(int pointNumber)
    {
        Vector3 rotation = Vector3.zero;
        Vector2 directionVector = path.GetPath()[pointNumber] - path.GetPath()[pointNumber - 1];
        float m, teta = 0; // m= Direction Vector Slope, teta= زاویه خط
        if (directionVector.x != 0f)
        {
            m = directionVector.y / directionVector.x; // محاسبه شیب بردار جهت 
            teta = Mathf.Atan(m);                       // محاسبه زاویه بردار جهت (این زاویه بین منفی پی دوم تا پی دوم تغییر میکند.
        }
        else if (directionVector.y > 0f)                // اگر بردار جهت یک خط عمود باشد، و به سمت بالا باشد.
        {
            teta = Mathf.PI / 2f;
            directionVector.y = 1f;
        }
        else if (directionVector.y < 0f)                // اگر بردار جهت یک خط عمود باشد، و به سمت پایین باشد.
        {
            teta = Mathf.PI / -2f;
            directionVector.y = -1f;
        }
        if (teta != Mathf.PI / 2f && teta != Mathf.PI / -2f)                   // اگر بردار جهت یک خط عمود نبود.
        {
            if (directionVector.x < 0f)                 // اگر بردار جهت در ربع دوم یا سوم باشد
            {
                directionVector.x = -1f;
                teta += Mathf.PI;           // چون زاویه بین منفی پی دوم تا مثبت پی دوم ارزیابی شده بود، باید تصیح شود، پی رادیان به زاویه اضافه می‌شود.
                if (directionVector.y > 0f)
                {
                    directionVector.y = 1f;
                }
                else if (directionVector.y < 0f)
                {
                    directionVector.y = -1f;
                }
            }
            else if (directionVector.x > 0f)            // اگر بردار جهت در ربع اول یا چهارم باشد
            {
                directionVector.x = 1f;
                if (directionVector.y > 0f)
                {
                    directionVector.y = 1f;
                }
                else if (directionVector.y < 0f)
                {
                    directionVector.y = -1f;
                }
            }
        }
        rotation.z = teta * Mathf.Rad2Deg + 270;        // تبدیل رادیان به درجه، 270 مقدار ثابتی است که به خاطر اینکه ممد تله موش را به سمت بالا طراحی کرده باید اضافه شود.
        return Quaternion.Euler(rotation);
    }

    public void Remove()
    {
        mhcs.Remove(this);
    }
    // تمامی موشهایی که مسیر این موش را دارند و هنوز راه نیافتاده اند را از بین می برد.
    public void RemoveAllMousesWithCurrentPath()
    {
        mhcs.RemoveAllMousesInPath(path);
    }
}