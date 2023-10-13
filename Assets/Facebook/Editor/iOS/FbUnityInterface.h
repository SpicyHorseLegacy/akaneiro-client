
#import <UIKit/UIKit.h>
#import <FacebookSDK/FacebookSDK.h>
#include "NativeDialogModes.cs"


@interface FbUnityInterface : NSObject

@property (strong, nonatomic) FBSession *session;
@property (nonatomic) BOOL isInitializing;
@property (strong, nonatomic) FBFrictionlessRecipientCache *friendCache;
@property (assign, nonatomic) BOOL useFrictionlessRequests;
@property (nonatomic) NativeDialogModes::eModes dialogMode;
@property (nonatomic, strong) NSString *launchURL;

+(FbUnityInterface *)sharedInstance;
-(id)init;
-(id)initWithCookie:(bool)cookie logging:(bool)_logging status:(bool)_status frictionlessRequests:(bool)_frictionlessRequests;
-(void)login:(const char *)scope;
-(void)logout;

@end