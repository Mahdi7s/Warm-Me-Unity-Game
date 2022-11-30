using UnityEngine;
using System.Collections;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

public class InitializeScript : MonoBehaviour
{
    public Transform loadingPrefab;
    public Vector3 loadingPosition;
    public Font font;
    public int fontSize;
    public Color fontColor;
    public Rect rectangle;
    public bool clearPrefsBeforeInit, showEmailButton, displayLog;
    public Rect buttonPosition;
    private static string debugger;
    private float delay;
    private bool readyToDelete;
    public static string Debugger                               // Just set this property 
    {                                                           // to display a string on
        set                                                     // device LCD.
        {
            debugger += value + '\n';
        }
    }

    void Awake()
    {
        GameState.staticProgressBarPrefab = loadingPrefab;      // Set the loading prefab.
        GameState.progressBarPosition = loadingPosition;        // Set the loading prefab position on scene.
        StoreItemScript.dict.Clear();                           // This field is for debugging, nothing special!
        /*Pause it*/
        if (clearPrefsBeforeInit)                               // this is good for testing
        {
            PlayerPrefs.DeleteAll();
        }
        GameState.IntializeProperties();
        debugger = string.Empty;                                // This will empty debugger string, every time that the scene changes.
    }

    void OnGUI()
    {
        if (displayLog)
        {
            GUI.skin.font = font;                                   //| This is for debugging on devices,
            GUI.skin.label.fontSize = fontSize;                     //| this will use to print a string
            GUI.contentColor = fontColor;                           //| on the device dispaly. Use this
            GUI.Label(rectangle, InitializeScript.debugger);        //| idea in every game.
        }
        if (showEmailButton)                                    // Send log button handler, this will use to send log through mail.
        {
            if (File.Exists(Application.persistentDataPath + "/Log.txt"))
            {
                if (GUI.Button(buttonPosition, "Send Logs"))
                {
                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress("Choc01ate.Corporation.2013@gmail.com");
                    mail.To.Add("msdisatis@gmail.com");
                    mail.CC.Add("virtualworld72@gmail.com");
                    mail.Subject = "Warm Me Log V1";
                    Attachment log = new Attachment(Application.persistentDataPath + "/Log.txt");
                    mail.Attachments.Add(log);
                    mail.Body = "Warm Me Logging V1\nWarm me log file is attached. Download it please.";
                    SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
                    smtpServer.Port = 587;
                    smtpServer.Credentials = new System.Net.NetworkCredential("Choc01ate.Corporation.2013@gmail.com", "ch9133559060") as ICredentialsByHost;
                    smtpServer.EnableSsl = true;
                    ServicePointManager.ServerCertificateValidationCallback =
                        delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                        { return true; };
                    smtpServer.Send(mail);
                    readyToDelete = true;
                }
            }
            else
            {
                Debugger = "Log file does not exist";
                showEmailButton = false;
            }
        }
    }

    public static string Save()                         // Saves the debugger string into a log file.
    {
        string path = Application.persistentDataPath + "/Log.txt";
        StreamWriter writer;
        if (File.Exists(path))
        {
            writer = new StreamWriter(path, true);
        }
        else
        {
            writer = File.CreateText(path);
        }
        writer.Write(debugger);
        writer.Close();
        return path;
    }

    void Update()
    {
        if (readyToDelete)
        {
            delay -= Time.deltaTime;
            if (delay <= 0)
            {
                File.Delete(Application.persistentDataPath + "/Log.txt");
                delay = 3f;
                readyToDelete = false;
                Debugger = "Sent";
            }
        }
    }
}