//
//  PushNotificationManager.h
//  PlaynomicsUnityPlugin
//
//  Created by Jared Jenkins on 6/24/13.
//  Copyright (c) 2013 Jared Jenkins. All rights reserved.
//


@interface PushNotificationManager : NSObject
+ (id)pushManager;
//successfully registered push notification
- (void) onDidRegisterForRemoteNotificationsWithDeviceToken:(NSString *)token;
//failed to register for push notification
- (void) onDidFailToRegisterForRemoteNotificationsWithError:(NSError *)error;

- (void) onDidAcceptPushNotification: (NSDictionary*) payload;

- (void) registerForPushNotifications;

- (NSString*) getDeviceToken;

- (void) clearNotifications;

@end

