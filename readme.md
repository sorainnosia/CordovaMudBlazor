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

Once all successful, you have a Cordova www folder.
Build CordovaMudBlazorPatch project and copy the output (exe, dll, and json files) to the root of CordovaMudBlazor project

Copy published Blazor WASM wwwroot folder's content and copy into the Cordova www folder and run command
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
* Build the CordovaMudBlazorPatch project and copy the output (exe, dll, and json files) to the root of the Cordova project

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

## Important Note
Whenever you input a Href or Link attribute, the value should be prepended with "/android_asset/www/"
Example
```
<MudNavMenu>
            <MudNavLink Match="NavLinkMatch.All" Href="@(BrowserService.wwwasset + "Products")">Products</MudNavLink>
            <MudNavLink Match="NavLinkMatch.All" Href="@(BrowserService.wwwasset + "Stocks")">Stocks</MudNavLink>
</MudNavMenu>
```
Where you have a class name BrowserService.cs
```
public class BrowserService
{
    // Compile to APK (Cordova)
    public static string wwwasset = "/android_asset/www/";
    // Compile to Web
    //public static string wwwasset = "/";
}
```
However whenever you are using NavigationManager.NavigateTo, you don't need to specify the BrowserService.wwwasset
```
[Inject] protected NavigationManager navMan { get; set; }
public void GoLogin()
{
    navMan.NavigateTo("login");
}
```
CordovaMudBlazorPatch will patch index.html to have
```
<base href="file:///android_asset/www/">
```
this is needed so that the dynamic routing of @page "email/{folder?}" will work. Without setting the base href in index.html and only relying on prepending "android_asset/www" to relative url will only make pages that do not have dynamic/complex routing to work and leaving pages with dynamic routing becomes error.

## MudBlazor.AdminDashboard Sample
<img src="https://user-images.githubusercontent.com/19261780/128962021-f68e7aa7-a054-4418-af1a-a20f18a74e96.gif" width="200">

CordovaMudBlazorPatch works with MudBlazor 5.1.0 using ThemeManager 1.0.5 with some minor changes, for example it works with MudBlazor.AdminDashboard
* Try getting the source code of AdminDashboard https://github.com/Garderoben/MudBlazor.Templates/tree/master/MudBlazor.Template.AdminDashboard
* Open the solution file using Visual Studio, and then create a class in AdminDashboard.Wasm project
```
public class BrowserService
{
    // Compile to APK (Cordova)
    public static string wwwasset = "/android_asset/www";
    // Compile to Web
    //public static string wwwasset = "/";
}
```
* Search in AdminDashboard.wasm project for the word "application" then when it is a Link or Href, modify it to become below (MudNavLink is example) which prepends it with BrowserService.www string
```
<MudNavLink Href="@(BrowserService.www + "/application/chat")" 
```
* Do the same for the word "pages" and "personal", you might notice some link does not starts with "/", add it to the link because BrowserService.www does not ends with a slash "/"
```
<MudNavLink Href="@(BrowserService.www + "/application/chat")" Icon="@Icons.Material.Outlined.Forum">Chat</MudNavLink>
...
<MudNavLink Href="@(BrowserService.www + "/personal/account")" Icon="@Icons.Material.Outlined.Person">Account</MudNavLink>
...
<MudNavLink Href="@(BrowserService.www + "/pages/authentication/login")" Icon="@Icons.Material.Outlined.InsertDriveFile">Login</MudNavLink>
...
```
* Open Dashboard.razor file, under below
```
@page "/personal/dashboard"
```
* Makes it to have another route
```
@page "/personal/dashboard"
@page "/index.html"
```
* Build AdminDashboard.Wasm project
* Create a new Cordova Android project describe in How to Use section
* Publish AdminDashboard.Wasm project, then go to the published folder, there is a folder name wwwroot, copy all files inside to the www folder of the cordova project (by first emptying the www folder)
* Build CordovaMudBlazorPatch project from this repository and copy the output (exe, dll, and json files) to the root of cordova project where www folder resides then run below command
```
CordovaMudBlazorPatch.exe /all
```
* Then you can build the cordova project and run the APK, which will show above result
 
## Thanks to
[Blazor.Cordova](https://github.com/BickelLukas/Blazor.Cordova)
[MudBlazor.ThemeManager](https://github.com/Garderoben/MudBlazor.ThemeManager)
[MudBlazor.Templates](https://github.com/Garderoben/MudBlazor.Templates)
[MudBlazor](https://github.com/Garderoben/MudBlazor)

Special thanks to Garderoben to upgrade the ThemeManager package and AdminDashboard package so that now MudBlazor components can run in Cordova with Theme enabled.
