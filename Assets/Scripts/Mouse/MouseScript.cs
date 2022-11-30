using UnityEngine;
using System.Collections;

public class MouseScript : PausableBehaviour
{
    public static Transform mouseTrap = null;   // This is for instantiating mouse trap.
    public MouseObjectsData mod = null;         // mod= Mouse Object Data
    public AudioClip mouseTrapClip;             // Sound of killing mouse.
    public float speed;                         // Mouse move speed
    public float reactionDistance;              // Minimum distance between mouse and trap, to fall in trap.
    Transform mt;                               // mt= Mouse Trap
    int lineNo;                                 // number of line in path, that mouse is runnig on that.
    float m, teta, soundTimer;                  // m= شیب خطی که موش روی آن حرکت می‌کند, teta= زاویه خطی که موش روی آن حرکت می‌کند, soundTimer= مدت زمانی که بین دو صدای موش فاصله میافتد
    MousePathScript mps;                        // mouse path script
    Vector3[] path;                             // points of the path
    Vector3 rotation, movement, hatPos;         // hatPos= جای کلاهی که موش به سمت آن میرود
    Vector2 speedVector, directionVector;       // speedVector= بردار سرعت، سرعت خطی روی این بردار تصویر میشود, directionVector= بردار جهت.
    HatScript hatScript;                        // اسکریپت کلاهی که موش به سمت آن میرود
    bool wait, shouldSoundPlay;                 // wait= آیا موش باید بایستد یا خیر.

    /// <summary>
    /// تا این تابع صدا زده نشود موش راه نمی‌افتد.
    /// </summary>
    /// <param name="mps"></param>
    /// <param name="home"></param>
    /// <param name="trap"></param>
    public void SetPathAndHome(MousePathScript mps, GameObject home, Transform trap)
    {
        mt = trap; // اگر تله قبل از راه افتادن موش کار گذاشته شده باشد اینجا به آن داده می شود.
        speedVector = Vector2.zero;
        rotation = Vector3.zero;
        this.mps = mps;
        path = mps.GetPath();           // مسیر موش را در متغیر path میریزیم.
        Vector3 position = path[0];
        transform.position = position;
        lineNo = 1;
        GetPartLine();                  // به توضیحات بالای تابع نگاه کنید
        speedVector = GetSpeed();
        SetOriention();
        movement.x = speedVector.x;
        movement.y = speedVector.y;
        movement.z = 0;
        if (mps.GetHat() == null)   // اگرکلاه این موش قبلا افتیده بود
        {
            Destroy(gameObject);
        }
        else
        {
            hatPos = mps.GetHatPosition();
            hatScript = mps.GetHat().GetComponent<HatScript>();
        }
        wait = false;
        if (!_isPaused)
        {
            audio.Play();
            shouldSoundPlay = false;
            soundTimer = 5.0f;
        }
    }

    void Start()
    {

    }

    protected override void PUpdate()
    {
        audio.mute = !GameState.AudioFx;
        audio.pitch = Time.timeScale;
        if (wait)   // این شرط باعث می‌شود موش تا زمانیکه کلاه نیافتاده است از پشت آن جم نخورد
        {
            wait = !hatScript.IsFalling();
            hatScript.Fall();
        }
        else
        {
            Vector3 position = transform.position + movement * Time.deltaTime;      // حرکت موش با این خطوط انجام می‌شود
            transform.position = position;
            if (mt != null) // اگر تله ای در مسیر موش هست
            {
                if (Vector3.Distance(transform.position, mt.position) <= reactionDistance) // اگر فاصله موش تا تله کمتر از مقدار تعیین شده است.
                {
                    // موش در تله میافتد
                    // Appropriate Action
                    mt.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("Close");
                    mod.trap = mt;
                    Destroy(mt.gameObject, 5.0f);
                    Destroy(gameObject);
                }
            }
            else if (Vector3.Distance(hatPos, position) <= reactionDistance) // اگر تله ای در مسیر موش نبود و فاصله موش تا کلاه مورد نظر کمتر از مقذار تعیین شده است
            {
                if (hatScript != null)  // اگر کلاه نیافتاده بود
                {
                    hatScript.Fall();   // کلاه را بنداز
                    mod.RemoveAllMousesWithCurrentPath();   // تمام موشهایی که مقصدشان این کلاه است و هنوز راه نیافتاده اند را نابود می‌کند.
                    wait = true;                            // صبر کن تا کلاه بیافتد
                }
            }
            if (PassedTheLine())    // اگر این پاره خط توسط کاملا رد شده است
            {
                lineNo++;           // برو خط بعد
                GetPartLine();      // جهت و زاویه موش و... را محاسبه کن
                speedVector = GetSpeed(); // سرعت موش روی هر محور را محاسبه کن
                SetOriention();             // جهت موش را به جهت محاسبه شده تنظیم کن
                movement.x = speedVector.x; // حرکت موش را با توجه به سرعت موش تنظیم کن
                movement.y = speedVector.y;
            }
        }
        if (shouldSoundPlay)    // اگر صدای موش باید پخش شود
        {
            if (!audio.isPlaying)
            {
                audio.Play();
            }
            shouldSoundPlay = false;
            soundTimer = 5.0f;
        }
        else
        {
            soundTimer -= Time.deltaTime;
            if (soundTimer <= 0)
            {
                shouldSoundPlay = true;
            }
        }
    }

    // این تابع سرعت موش را روی دو محور تنظیم میکند.
    public Vector2 GetSpeed()
    {
        speedVector.x = Mathf.Abs(speed * Mathf.Cos(teta)) * directionVector.x;
        speedVector.y = Mathf.Abs(speed * Mathf.Sin(teta)) * directionVector.y;
        return speedVector;
    }

    // این تابع مشخص میکند آیا پاره خط فعلی توسط موش پیموده شده است یا خیر.
    public bool PassedTheLine()
    {
        Vector2[] partLine = GetPartLine();
        if (Vector2.Distance(transform.position, partLine[0]) >=
            Vector2.Distance(partLine[1], partLine[0])) // اگر فاصله موش تا ابتدای پاره خط بیشتر از فاصله نقطه انتهای پاره خط تا ابتدای آن بود
        {
            return true;
        }
        return false;
    }

    // چرخش موش را تعیین میکند.
    public void SetOriention()
    {
        rotation.z = teta * Mathf.Rad2Deg + 90;// تبدیل رادیان به درجه، 270 مقدار ثابتی است که به خاطر اینکه ممد موش را به سمت بالا طراحی کرده باید اضافه شود
        transform.rotation = Quaternion.Euler(rotation);
    }

    // این تابع در کلاس Mouse Object Data با نام GetOriention آمده است، توضیحاتش را آنجا بخوانید.
    public Vector2[] GetPartLine()
    {
        Vector2[] partLine = new Vector2[2];
        partLine[0] = Vector2.zero;
        partLine[1] = Vector2.zero;
        path = mps.GetPath();
        if (lineNo < path.Length && lineNo > 0)
        {
            partLine[0] = path[lineNo - 1];
            partLine[1] = path[lineNo];
        }
        else
        {
            mod.Remove();
            GameObject.Destroy(gameObject);
        }
        directionVector = partLine[1] - partLine[0];
        if (directionVector.x != 0f)
        {
            m = directionVector.y / directionVector.x;
            teta = Mathf.Atan(m);
        }
        else if (directionVector.y > 0f)
        {
            teta = Mathf.PI / 2f;
            directionVector.y = 1f;
        }
        else if (directionVector.y < 0f)
        {
            teta = Mathf.PI / -2f;
            directionVector.y = -1f;
        }
        if (teta != Mathf.PI / 2f && teta != Mathf.PI / -2f)
        {
            if (directionVector.x < 0f)
            {
                directionVector.x = -1f;
                teta += Mathf.PI;
                if (directionVector.y > 0f)
                {
                    directionVector.y = 1f;
                }
                else if (directionVector.y < 0f)
                {
                    directionVector.y = -1f;
                }
            }
            else if (directionVector.x > 0f)
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
        return partLine;
    }


    public void FallInTrap()
    {
        if (mt == null)// اگر موش تله ای ندارد
        {
            Vector3 rotation = this.rotation;   // چرخش حال حاضر موش را بگیر و z آن را با ۱۸۰ جمع کن
            rotation.z += 180;                  // جمع با ۱۸۰ به خاطر مسایل طراحی است و اینکه ممد جهت طراحی هایش ثابت نیست
            //maxDistanceToFallInTrap = 0.1f;
            if (lineNo < path.Length - 1)       // اگر موش روی آخرین پاره خط مسیرش نیست
            {
                if (path.Length >= 2)
                {
                    mt = (Transform)Instantiate(mouseTrap, path[UnityEngine.Random.Range(lineNo, path.Length - 2)], Quaternion.Euler(rotation)); // تله موش را بصورت رندم در یکی از نقاط مسیرش بنداز
                }
                else
                {
                    mt = (Transform)Instantiate(mouseTrap, path[lineNo], Quaternion.Euler(rotation));// تله موش را در اولین نقطه مسیرش بنداز
                }
            }
            else
            {
                mt = (Transform)Instantiate(mouseTrap, transform.position, Quaternion.Euler(rotation)); // تله موش را بنداز در موقعیت کنونی موش
            }
            foreach (ParticleSystem particleSystem in mt.GetComponentsInChildren<ParticleSystem>())
            {
                particleSystem.Play();
            }
            audio.Stop();
            audio.clip = mouseTrapClip;
            audio.Play();
            mod.trap = mt;
        }
    }

    protected override void PFixedUpdate()
    {
    }

    protected override void POnGUI()
    {
    }
}