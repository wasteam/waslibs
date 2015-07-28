#Windows App Studio Libraries

This repository contains the source code of the libraries used by [Windows App Studio](http://appstudio.windows.com) in the generated apps.
There are three libraries:

The libraries are also available as Nuget packages. To reference them you need to use the
feed hosted at MyGet.org:

```
https://www.myget.org/F/wasbeta/api/v2
```

## Common

This library contains utility classes to create XAML applications. 
This is a Portable Class Library that can be used in:
- Windows 10
- Windows 8.1
- Windows Phone 8.1

## DataProviders

This library contains the implementation of all the data sources available in 
Windows App Studio apps.

This is a Portable Class Library that can be used in:
- Windows 10
- Windows 8.1
- Windows Phone 8.1 

## Controls  

This library contains XAML controls for Windows 10 apps **only** .

