#!/bin/bash

android update project --path '/Users/bpadget/unity-facebook/CUI/facebook-android-sdk/facebook'
ant clean debug

rm -rf ~/unity-facebook/CUI/facebook-android-sdk/facebook/bin
rm -rf ~/unity-facebook/CUI/SDKPackage/Assets/Plugins/Android/facebook
mkdir ~/unity-facebook/CUI/SDKPackage/Assets/Plugins/Android/facebook
cp ~/unity-facebook/CUI/facebook-android-sdk/facebook/* ~/unity-facebook/CUI/SDKPackage/Assets/Plugins/Android/facebook/
cp -r ~/unity-facebook/CUI/facebook-android-sdk/facebook/bin ~/unity-facebook/CUI/SDKPackage/Assets/Plugins/Android/facebook/
cp -r ~/unity-facebook/CUI/facebook-android-sdk/facebook/gen ~/unity-facebook/CUI/SDKPackage/Assets/Plugins/Android/facebook/
cp -r ~/unity-facebook/CUI/facebook-android-sdk/facebook/res ~/unity-facebook/CUI/SDKPackage/Assets/Plugins/Android/facebook/
