//
//  Marshal.h
//  PlaynomicsUnityPlugin
//
//  Created by Jared Jenkins on 6/28/13.
//  Copyright (c) 2013 Jared Jenkins. All rights reserved.
//

#define makeStringCopy(s) ( s != NULL && [s isKindOfClass:[NSString class]] ) ? strdup( [s UTF8String] ) : NULL