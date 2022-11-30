using System;
using System.Collections;
using System.Collections.Generic;
using Prime31;



#if UNITY_IPHONE
public enum StoreKitDownloadState
{
	Waiting,
	Active,
	Paused,
	Finished,
	Failed,
	Cancelled
}


public class StoreKitDownload
{
    public StoreKitDownloadState downloadState;
    public double contentLength;
    public string contentIdentifier;
    public string contentURL;
	public string contentVersion;
	public string error;
	public float progress;
	public double timeRemaining;
	public StoreKitTransaction transaction;
	
	
	public static List<StoreKitDownload> downloadsFromJson( string json )
	{
		var downloadList = new List<StoreKitDownload>();
		
		var downlaods = json.listFromJson();
		if( downlaods == null )
			return downloadList;
		
		foreach( Dictionary<string,object> dict in downlaods )
			downloadList.Add( downloadFromDictionary( dict ) );
		
		return downloadList;
	}
	

    public static StoreKitDownload downloadFromDictionary( Dictionary<string,object> dict )
    {
        var download = new StoreKitDownload();
		
		if( dict.ContainsKey( "downloadState" ) )
        	download.downloadState = (StoreKitDownloadState)int.Parse( dict["downloadState"].ToString() );
		
		if( dict.ContainsKey( "contentLength" ) )
        	download.contentLength = double.Parse( dict["contentLength"].ToString() );
		
		if( dict.ContainsKey( "contentIdentifier" ) )
        	download.contentIdentifier = dict["contentIdentifier"].ToString();
		
		if( dict.ContainsKey( "contentURL" ) )
        	download.contentURL = dict["contentURL"].ToString();
		
		if( dict.ContainsKey( "contentVersion" ) )
			download.contentVersion = dict["contentVersion"].ToString();
		
		if( dict.ContainsKey( "error" ) )
			download.error = dict["error"].ToString();
		
		if( dict.ContainsKey( "progress" ) )
			download.progress = float.Parse( dict["progress"].ToString() );
		
		if( dict.ContainsKey( "timeRemaining" ) )
        	download.timeRemaining = double.Parse( dict["timeRemaining"].ToString() );
		
		if( dict.ContainsKey( "transaction" ) )
        	download.transaction = StoreKitTransaction.transactionFromDictionary( dict["transaction"] as Dictionary<string,object> );

        return download;
    }
	
	
	public override string ToString()
	{
		return String.Format( "<StoreKitDownload> downloadState: {0}\n contentLength: {1}\n contentIdentifier: {2}\n contentURL: {3}\n contentVersion: {4}\n error: {5}\n progress: {6}\n transaction: {7}",
			downloadState, contentLength, contentIdentifier, contentURL, contentVersion, error, progress, transaction );
	}
}
#endif
