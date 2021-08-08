# CordovaMudBlazor

## Sample
<img src="https://user-images.githubusercontent.com/19261780/128637079-a01d05e0-1ba7-468f-b956-4cb59168bf18.gif" width="200">

## Description
A patching program that patch the Publish output of Blazor WASM created using .NET Core 5.0.8 and MudBlazor component to be able to run in Cordova project

## How to use
Run command prompt in Administrator mode, navigate to root of this project

Create a Cordova Project
```
cordova create CordovaMudBlazor com.johnkenedy.pharga CordovaMudBlazor 
```

CD into the root directory of the project
```
cd CordovaMudBlazor
```

Add Android platform to the cordova project
```
cordova platform add android
```

Add below plugins to the cordova project
```
cordova plugin add cordova-plugin-android-permissions
cordova plugin add cordova-plugin-file
cordova plugin add cordova-plugin-file-transfer
cordova plugin add cordova-plugin-advanced-http
```

Once all successful, you have a Cordova www folder

Copy Blazor WASM wwwroot folder into the Cordova www folder and run command
```
CordovaMudBlazorPatch.exe /all
```
Now you are ready to build the cordova project and test building APK and test running the APK


## CordovaMudBlazorPatch
This program is a rough patching software, it patches build.gradle, AndroidManifest.xml, index.html, MudBlazor css file, MudBlazor js file and other css files.

The included CordovaMudBlazor.Library is a sample project that is successfully run under Cordova using CordovaMudBlazorPatch, to compile the sample Blazor WASM project
* Open CordovaMudBlazor.sln
* Edit BrowserService.cs under CordovaMudBlazor.Library, uncomment to become like below
```
// Compile to APK (Cordova)
public static string wwwasset = "/android_asset/www/";
```
and the other portion becomes like below
```
// Compile to Web
//public static string wwwasset = "/";
```
* Edit Index.razor in CordovaMudBlazor.Library, make sure the @page is pointed to "/index.html"
```
@page "/index.html"
```
* Build the solution and Publish CordovaMudBlazor.Server project into a folder, the output contains a wwwroot folder, copy all files in this folder into the Cordova www folder

Run the command
```
CordovaMudBlazorPatch.exe /all
```

Now you can test it by running
```
cordova build android
cordova run android
```

When you want the Blazor WASM project to run in browser, make sure wwwasset is "/"
```
// Compile to Web
public static string wwwasset = "/";
```

and make sure Index.razor is
```
@page "/"
```
Then test running it in the browser.
