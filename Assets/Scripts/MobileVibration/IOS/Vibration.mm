#import <Foundation/Foundation.h>
#import <AudioToolbox/AudioToolbox.h>
#import <UIKit/UIKit.h>
#import "Vibration.h"

@interface Vibration ()

@end

@implementation Vibration


#pragma mark - Vibrate

+ (void)    vibrate {
    AudioServicesPlaySystemSoundWithCompletion(1352, NULL);
}
+ (void)    vibratePeek {
    AudioServicesPlaySystemSoundWithCompletion(1519, NULL);
}
+ (void)    vibratePop {
    AudioServicesPlaySystemSoundWithCompletion(1520, NULL);
}
+ (void)    vibrateNope {
    AudioServicesPlaySystemSoundWithCompletion(1521, NULL); // three vibrate continuous
}

@end


#pragma mark - "C"

extern "C" 
{
    void    _Vibrate () {
        [Vibration vibrate];
    }
    
    void    _VibratePeek () {
        [Vibration vibratePeek];
    }

    void    _VibratePop () {
        [Vibration vibratePop];
    }

    void    _VibrateNope () {
        [Vibration vibrateNope];
    }
}

