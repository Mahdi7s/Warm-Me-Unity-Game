//
//  P31SharedTools.h
//  P31SharedTools
//
//  Created by Mike Desaro on 8/10/13.
//  Copyright (c) 2013 prime31. All rights reserved.
//


#if TARGET_OS_IPHONE
#import <UIKit/UIKit.h>


@interface P31 : NSObject

+ (BOOL)isValidJsonObject:(NSObject*)object;

+ (NSString*)jsonStringFromObject:(NSObject*)object;

+ (NSObject*)objectFromJsonString:(NSString*)json;

+ (const char *)jsonFromError:(NSError*)error;

+ (void)unityPause:(BOOL)shouldPause;

+ (UIViewController*)unityViewController;

@end

#endif