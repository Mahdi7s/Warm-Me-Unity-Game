using UnityEngine;
using System.Collections;

public static class ConnectionUtility
{
    /// <summary>
    /// A combo method that determines if the device is connected to internet.
    /// Important Note: This method should be tested!!!
    /// </summary>
    /// <returns><c>true</c> if is connected to internet; otherwise, <c>false</c>.</returns>
    public static bool IsConnectedToInternet()
    {
        bool isConnectedToInternet = false;

#if (UNITY_EDITOR || (!UNITY_IPHONE && !UNITY_ANDROID))
        if (Network.player.ipAddress.ToString() != "127.0.0.1")
        {
            isConnectedToInternet = true;
        }
#endif
#if (UNITY_IPHONE)
		if (iPhoneSettings.internetReachability == iPhoneNetworkReachability.ReachableViaWiFiNetwork)
		{
			isConnectedToInternet = true;
		}
#endif
#if (UNITY_ANDROID)
        if (iPhoneSettings.internetReachability == iPhoneNetworkReachability.ReachableViaWiFiNetwork)
        {
            isConnectedToInternet = true;
        }
#endif

        return isConnectedToInternet;
        //////////////////////////////////////////////////////////////////////////////////////////////////////
        /*
        // Start/Poll the connection test, report the results in a label and 
        // react to the results accordingly
        connectionTestResult = Network.TestConnection();
        switch (connectionTestResult) {
            case ConnectionTesterStatus.Error: 
                testMessage = "Problem determining NAT capabilities";
                doneTesting = true;
                break;
                
            case ConnectionTesterStatus.Undetermined: 
                testMessage = "Undetermined NAT capabilities";
                doneTesting = false;
                break;
                
            case ConnectionTesterStatus.PublicIPIsConnectable:
                testMessage = "Directly connectable public IP address.";
                useNat = false;
                doneTesting = true;
                break;
                
                // This case is a bit special as we now need to check if we can 
                // circumvent the blocking by using NAT punchthrough
            case ConnectionTesterStatus.PublicIPPortBlocked:
                testMessage = "Non-connectable public IP address (port " +
                    serverPort +" blocked), running a server is impossible.";
                useNat = false;
                // If no NAT punchthrough test has been performed on this public 
                // IP, force a test
                if (!probingPublicIP) {
                    connectionTestResult = Network.TestConnectionNAT();
                    probingPublicIP = true;
                    testStatus = "Testing if blocked public IP can be circumvented";
                    timer = Time.time + 10;
                }
                // NAT punchthrough test was performed but we still get blocked
                else if (Time.time > timer) {
                    probingPublicIP = false;        // reset
                    useNat = true;
                    doneTesting = true;
                }
                break;
            case ConnectionTesterStatus.PublicIPNoServerStarted:
                testMessage = "Public IP address but server not initialized, "+
                    "it must be started to check server accessibility. Restart "+
                        "connection test when ready.";
                break;
                
            case ConnectionTesterStatus.LimitedNATPunchthroughPortRestricted:
                testMessage = "Limited NAT punchthrough capabilities. Cannot "+
                    "connect to all types of NAT servers. Running a server "+
                        "is ill advised as not everyone can connect.";
                useNat = true;
                doneTesting = true;
                break;
                
            case ConnectionTesterStatus.LimitedNATPunchthroughSymmetric:
                testMessage = "Limited NAT punchthrough capabilities. Cannot "+
                    "connect to all types of NAT servers. Running a server "+
                        "is ill advised as not everyone can connect.";
                useNat = true;
                doneTesting = true;
                break;
                
            case ConnectionTesterStatus.NATpunchthroughAddressRestrictedCone:
            case ConnectionTesterStatus.NATpunchthroughFullCone:
                testMessage = "NAT punchthrough capable. Can connect to all "+
                    "servers and receive connections from all clients. Enabling "+
                        "NAT punchthrough functionality.";
                useNat = true;
                doneTesting = true;
                break;
                
            default: 
                testMessage = "Error in test routine, got " + connectionTestResult;
        }
        if (doneTesting) {
            if (useNat)
                shouldEnableNatMessage = "When starting a server the NAT "+
                    "punchthrough feature should be enabled (useNat parameter)";
            else
                shouldEnableNatMessage = "NAT punchthrough not needed";
            testStatus = "Done testing";
        }
        */
    }
}