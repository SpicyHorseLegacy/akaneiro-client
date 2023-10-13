//
//  PushNotificationManager.m
//  PlaynomicsUnityPlugin
//
//  Created by Jared Jenkins on 6/24/13.
//  Copyright (c) 2013 Jared Jenkins. All rights reserved.
//

#import "Marshal.h"
#import "PushNotificationManager.h"
#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

#define pushInteractionUrl @"ti"
#define pushIgnoreActiveState @"pushIgnored"
#define pushTokenCacheKey @"com.playnomics.lastDeviceToken"


char * _token = 0;
char * _registrationError = 0;
char * _pushClickUrl = 0;
char * _listenerName = 0;

static void onPushTokenUpdated(const char* token)
{
    UnitySendMessage(_listenerName, "OnPushTokenUpdated", token);
}

static void onPushMessageReceived(const char* baseUrl)
{
    UnitySendMessage(_listenerName, "OnPushMessageClicked", baseUrl);
}


void _setPushListener(char* listenerName)
{
    free(_listenerName);
    _listenerName = 0;
	
    int len = strlen(listenerName);
	_listenerName = malloc(len+1);
	strcpy(_listenerName, listenerName);
	
	if(_token) {
		onPushTokenUpdated(_token);
		free(_token);
        _token = 0;
	}
    
	if(_pushClickUrl) {
        onPushMessageReceived(_pushClickUrl);
		free(_pushClickUrl);
        _pushClickUrl = 0;
	}
    
}

const char * _getDeviceToken()
{
    return makeStringCopy([[PushNotificationManager pushManager] getDeviceToken]);
}

void _enablePushNotifications()
{
    [[PushNotificationManager pushManager] registerForPushNotifications];
}

@implementation PushNotificationManager

static PushNotificationManager* pushManager = nil;
+(PushNotificationManager*) pushManager {
    if(pushManager == nil){
        pushManager = [[super alloc] init];
    }
    return pushManager;
}

//we received a push notification token, send the token to the server
- (void) onDidRegisterForRemoteNotificationsWithDeviceToken:(NSData *)devToken{
    NSMutableString* token = [NSMutableString stringWithString:[devToken description]];
	
	//Remove <, >, and spaces
	[token replaceOccurrencesOfString:@"<" withString:@"" options:1 range:NSMakeRange(0, [token length])];
	[token replaceOccurrencesOfString:@">" withString:@"" options:1 range:NSMakeRange(0, [token length])];
	[token replaceOccurrencesOfString:@" " withString:@"" options:1 range:NSMakeRange(0, [token length])];
    
    if(![[self getDeviceToken]isEqualToString:token]){
        [self setDeviceToken: token];
        
        const char* tokenStr = [token UTF8String];
        if(!_listenerName)
        {
            _token = malloc(strlen(tokenStr) + 1);
            strcpy(_token, tokenStr);
            return;
        }
        onPushTokenUpdated(tokenStr);
    }
}

//we received a push notification token, send the token to the server
- (void) onDidFailToRegisterForRemoteNotificationsWithError:(NSError *)error{
    NSLog(@"Error registering for push notifications : %@", error.description);
}

//we received a push notification token, send the token to the server
- (void) onDidAcceptPushNotification:(NSDictionary *)payload {
    if ([payload valueForKeyPath:pushInteractionUrl]!=nil) {
       
        //we received a push notification, so report it
        NSString* callbackUrl = [payload valueForKeyPath:pushInteractionUrl];
        
        UIApplicationState state = [[UIApplication sharedApplication] applicationState];
        if(state == UIApplicationStateActive){
            callbackUrl = [callbackUrl stringByAppendingFormat:@"&%@", pushIgnoreActiveState];
        }
        
        const char* callbackUrlStr = [callbackUrl UTF8String];
        if(!_listenerName)
        {
            _pushClickUrl = malloc(strlen(callbackUrlStr) + 1);
            strcpy(_pushClickUrl, callbackUrlStr);
            return;
        }
        onPushMessageReceived(callbackUrlStr);
    }
}

- (void) registerForPushNotifications {
    [[UIApplication sharedApplication] registerForRemoteNotificationTypes:(UIRemoteNotificationTypeBadge | UIRemoteNotificationTypeSound |UIRemoteNotificationTypeAlert)];
}

- (void) clearNotifications{
    //reset the application badge number
    [[UIApplication sharedApplication] setApplicationIconBadgeNumber:0];
};

- (NSString*) getDeviceToken{
    return [[NSUserDefaults standardUserDefaults] stringForKey:pushTokenCacheKey];
}

- (void) setDeviceToken: (NSString*) token{
    [[NSUserDefaults standardUserDefaults] setObject:token forKey:pushTokenCacheKey];
    [[NSUserDefaults standardUserDefaults] synchronize];
}
@end
