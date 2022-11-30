//
//  AdMobManager.h
//  AdMobTest
//
//  Created by Mike on 1/27/11.
//  Copyright 2011 Prime31 Studios. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "GADBannerView.h"
#import "GADInterstitial.h"


typedef enum
{
	AdMobBannerTypeiPhone_320x50,
	AdMobBannerTypeiPad_728x90,
	AdMobBannerTypeiPad_468x60,
	AdMobBannerTypeiPad_320x250,
	AdMobBannerTypeSmartPortrait,
	AdMobBannerTypeSmartLandscape
} AdMobBannerType;

typedef enum
{
	AdMobAdPositionTopLeft,
	AdMobAdPositionTopCenter,
	AdMobAdPositionTopRight,
	AdMobAdPositionCentered,
	AdMobAdPositionBottomLeft,
	AdMobAdPositionBottomCenter,
	AdMobAdPositionBottomRight
} AdMobAdPosition;


@interface AdMobManager : NSObject <GADBannerViewDelegate, GADInterstitialDelegate>
@property (nonatomic, retain) GADBannerView *adView;
@property (nonatomic, retain) GADInterstitial *interstitialAd;
@property (nonatomic, retain) NSString *publisherId;
@property (nonatomic) BOOL isTesting;
@property (nonatomic) BOOL isShowingSmartBanner;
@property (nonatomic) AdMobAdPosition bannerPosition;
@property (nonatomic, retain) NSMutableArray *testDevices;
@property (nonatomic) BOOL tagForChildDirectedTreatment;


+ (AdMobManager*)sharedManager;


- (void)createBanner:(AdMobBannerType)bannerType withPosition:(AdMobAdPosition)position andAdUnitId:(NSString*)adUnitId;

- (void)destroyBanner;

- (void)requestInterstitalAd:(NSString*)interstitialUnitId;

- (void)showInterstitialAd;

@end
