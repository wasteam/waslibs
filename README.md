#Windows App Studio Libraries

This repository contains the source code of the libraries used in [Windows App Studio](http://appstudio.windows.com) to create our generated apps.

A sample implementation of these libraries is available in the Windows Store: [Windows App Studio Uwp Samples](https://www.microsoft.com/en-us/store/apps/windows-app-studio-uwp-samples/9nblggh4r0w1). You can see the implementation details in the [source code](https://github.com/wasteam/waslibs/tree/master/samples).

There are three libraries: DataProviders, Uwp and Common. The libraries are also available as Nuget packages. 
```
https://www.nuget.org/packages/WindowsAppStudio.Uwp
https://www.nuget.org/packages/WindowsAppStudio.DataProviders
https://www.nuget.org/packages/WindowsAppStudio.Common
```
# Table of Contents <a name="table-of-contents"><a>
- [Uwp Library](#uwp)
    - [Layout Controls](#layout-controls)
        - [ResponsiveGridView](#responsive-grid-view)
        - [Pivorama](#pivorama)
        - [VariableSizedGrid](#variable-sized-grid)
        - [Carousel](#carousel)
        - [SliderView](#slider-view)
        - [SectionList](#section-list)
        - [SplitterCard](#splitter-card)
    - [Foundation Controls](#foundation-controls)
        - [HtmlBlock](#html-block)
        - [VisualBreakpoints](#visual-breakpoints)
        - [ImageEx](#image-ex)
        - [GifControl](#gif-control)
        - [VirtualBox](#virtual-box)
        - [RelativeBox](#relative-box)
        - [RelativeBox and VirtualBox](#relative-and-virtual-box)
        - [SearchBox](#search-box)
        - [InfiniteScroll](#infinite-scroll)
    - [App Services](#app-services)
        - [Navigation](#navigation)
        - [AppCache](#appcache)
    - [Utilities](#utilities)
        - [ErrorNotification](#error-notification)
        - [ActionsCommandBar](#actions-command-bar)
        - [AnimationExtensions](#animation-extensions)
        - [Converters](#converters)
    - [Labs](#labs)
        - [ResponsiveGridView](#responsive-grid-view-labs)
        - [Accordion](#accordion)
        - [SlideShow](#slide-show)
        - [ShapeImage](#shape-image)
        - [AutoHide](#auto-hide)
        - [Mosaic](#mosaic)
- [Data Providers Library](#data-providers)
    - [Facebook](#facebook)
    - [Twitter](#twitter)
    - [Flickr](#flickr)
    - [YouTube](#youtube)
    - [WordPress](#wordpress)
    - [Rss](#rss)
    - [Bing](#bing)
    - [LocalStorage](#local-storage)
    - [REST API](#rest-api)
- [Common](#common)

# Uwp Library <a name="uwp"></a>
This library contains XAML controls for Windows 10 apps **only**.

##Layout Controls <a name="layout-controls"></a>
###ResponsiveGridView Control <a name="responsive-grid-view"></a>
The ResponsiveGridView control allows to present information within a Grid View perfectly adjusting the total display available space. It reacts to changes in the layout as well as the content so it can adapt to different form factors automatically. The number and the width of items are calculated based on the screen resolution in order to fully leverage the available screen space. The property ItemsHeight define the items fixed height and the property DesiredWidth sets the minimum width for the elements to add a new column.

*View code*  
https://github.com/wasteam/waslibs/tree/master/src/AppStudio.Uwp/Controls/ResponsiveGridView

###Pivorama Control <a name="pivorama"></a>
The Pivorama control allows to visualize a set of elements optimizing the available space to display them. The Pivorama behaves differently based on the screen size: in big screens, it maximizes the number of items displayed using a table kind layout, with the ability to slide horizontally; in small screens, it behaves more like a Pivot control, showing the items in groups, with a heading inviting to slide horizontally. The Pivorama control is ideal to display big number of items in the best way depending on the device form factor.

*View code*  
https://github.com/wasteam/waslibs/tree/master/src/AppStudio.Uwp/Controls/Pivorama

###VariableSizedGrid Control <a name="variable-sized-grid"></a>
The VariableSizedGrid control allows to display items from a list using different values for Width and Height item properties. You can control the number of rows and columns to be displayed as well as the items orientation in the panel. Finally, the AspectRatio property allow us to control the relation between Width and Height.

*View code*  
https://github.com/wasteam/waslibs/tree/master/src/AppStudio.Uwp/Controls/VariableSizedGrid

###Carousel Control <a name="carousel"></a>
The Carousel offer an alternative to image visualization adding horizontal scroll to a set of items. The Carousel control is responsive by design, optimizing the image visualization in the different form factors. You can control properties like the AspectRatio, MaxItems, MinHeight, MaxHeight, GradientOpacity and AlignmentX to properly behave depending on the resolution and space available.

*View code*  
https://github.com/wasteam/waslibs/tree/master/src/AppStudio.Uwp/Controls/Carousel

###SliderView Control <a name="slider-view"></a>
The SliderView control displays a set of images in horizontal layout allowing the user to slide one by one image horizontally. The SliderView control is responsive by design and you can control images the Height and Width as well as optionally decide if you want to show arrows or not to slide the images.

*View code*  
https://github.com/wasteam/waslibs/tree/master/src/AppStudio.Uwp/Controls/SliderView

###SectionList Control <a name="section-list"></a>
The SectionList control enables the horizontal item visualization adjusting the number of items shown to the available space. A SectionList is composed by one or more SectionListItem which can contain any XAML code. Each SectionListItem may have a Header and a ViewAll button to navigate to the list of items shown. The control shows a progress indicator while loading. If an exception occurs during the load, an error message is shown.

*View code*  
https://github.com/wasteam/waslibs/tree/master/src/AppStudio.Uwp/Controls/SectionList

###SplitterCard Control <a name="splitter-card"></a>
The SplitterCard control applies a specific design to strings. For any given string, the controls split the text by spaces. The first two words in the text are displayed one over the other using a specific style and highlighting the first one.

*View code*  
https://github.com/wasteam/waslibs/tree/master/src/AppStudio.Uwp/Controls/SplitterCard

##Foundation Controls <a name="foundation-controls"></a>
###HtmlBlock <a name="html-block"></a>
The HtmlBlock renders Html formatted content using a native Xaml representation. The control is optimized to friendly visualize Html content within your Apps creating a Xaml reading view for your content. It applies a set of default styles to represent each Html tag, the default styles can be overriden as your preference. It supports most common Html tags as well as videos from YouTube and Channel9 embedded using an iframe tag. Following are the currently supported Html tags: a, article, blockQuote, cite, code, dd, details, div, dl, dt, em, figCaption, figure, footer, h1, h2, h3, h4, h5, h6, header, i, img, label, li, main, mark, ol, p, pre, q, section, span, strong, summary, table, td, th, time, tr, ul. Finally, the control does not interpret or parse JavaScript code as is not thought download content from Internet (use the WebView control instead).

*View code*  
https://github.com/wasteam/waslibs/tree/master/src/AppStudio.Uwp/Controls/HtmlBlock

###VisualBreakpoints <a name="visual-breakpoints"></a>
Enables the definition of different visual breakpoints for a certain layout as well as modifies the properties of child controls based on the breakpoints defined. In other words, adds the ability to change the layout and property values of controls based on the defined 'breakpoint values'. There are two main advantages over the platform VisualStateManager control: 1) all the visual breakpoints are defined in a centralized JSON file, which can reference other JSON files; 2) allows the modification of properties inside controls that belongs to DataTemplates.

*View code*  
https://github.com/wasteam/waslibs/tree/master/src/AppStudio.Uwp/Controls/VisualBreakpoints

###ImageEx Control <a name="image-ex"></a>
The ImageEx control extends the default Image platform control improving the performance and responsiveness of your Apps. Source images are downloaded asynchronously showing a load indicator while in progress. Once downloaded, the source image is stored in the App local cache to preserve resources and load time next time the image needs to be displayed.

*View code*  
https://github.com/wasteam/waslibs/tree/master/src/AppStudio.Uwp/Controls/ImageEx

###GifControl Control <a name="gif-control"></a>
The GifControl allows animated gif renderization in XAML. [ImageEx Control](#image-ex) determines the format of the inbound image and uses Gif Control in case the image is an animated gif.

*View code*  
https://github.com/wasteam/waslibs/tree/master/src/AppStudio.Uwp/Controls/GifControl

###VirtualBox Control <a name="virtual-box"></a>
The VirtualBox control allows to redimension any XAML DataTemplate proportionaly. The control works directly with the underlying ContentPresenter to have a smooth behavior and high optimized performance.

*View code*  
https://github.com/wasteam/waslibs/tree/master/src/AppStudio.Uwp/Controls/Virtualbox

###RelativeBox Control <a name="relative-box"></a>
The RelativeBox control uses XAML to apply a responsive behavior to the DataTemplate configured.

*View code*  
https://github.com/wasteam/waslibs/tree/master/src/AppStudio.Uwp/Controls/Relativebox

###Relative and VirtualBox <a name="relative-and-virtual-box"></a>
The RelativeBox control can be used together with the VirtualBox control to apply a better responsive behavior to the DataTemplate configured.

###SearchBox Control <a name="search-box"></a>
The SearchBox control allows to handle the App search feature. You can decide how the control will be displayed using properties Foreground, Background, PlaceHolderText, FontSize, etc. The SearchCommand property establish the command to execute when the user press enter or click the magnifier.

*View code*  
https://github.com/wasteam/waslibs/tree/master/src/AppStudio.Uwp/Controls/SearchBox

###InfiniteScroll Control <a name="infinite-scroll"></a>
To create the Infinite Scroll behavior we use the EndOfScrollCommand attached property. Applying the EndOfScrollCommand to a Xaml control (ListView, GridView or ScrollViewer) we look for changes in the VerticalOffset property. When the VerticalOffset reaches the end of the scrollable size it executes a command in C#. In the sample, the command execution results in loading more items and add them at the end of the item collection.

*View code*  
https://github.com/wasteam/waslibs/blob/master/src/AppStudio.Uwp/Commands/EndOfScrollCommand.cs

##App Services <a name="app-services"></a>
###Navigation Service <a name="navigation-service"></a>
The NavigationService service handles the complexity of navigating among App pages. It can be used in the main rootFrame as well as in other internal frames, like a ShellControl with Hamburger button.

The navigation can be implemented in multiple ways: using the .NET Type of the page we want to navigate to; using a string with the page name as target page; or through an element which implements the INavigable interface. We can use the NavigationService to open the web browser as well.

*View code*  
https://github.com/wasteam/waslibs/tree/master/src/AppStudio.Uwp/Navigation

###AppCache Service <a name="appcache-service"></a>
The AppCache is a service class which use local storage to maintain persisten information among App executions hiding the complexity of saving to and load from the local storage the elements added to memory.

*View code*  
https://github.com/wasteam/waslibs/tree/master/src/AppStudio.Uwp/Cache

##Utilities <a name="utilities"></a>
###ErrorNotification Control <a name="error-notification"></a>
The ErrorNotificationControl allows to show a message when an exception occurs. You can configure the message background color and the user can close it once read.

*View code*  
https://github.com/wasteam/waslibs/tree/master/src/AppStudio.Uwp/Controls/ErrorNotificationControl

###ActionsCommandBar Control <a name="actions-command-bar"></a>
The ActionsCommandBar control extends the system Windows.UI.Xaml.Controls.CommandBar allowing to dynamically load the AppBarButtons through binding the control to a List of ActionInfo items created from C#. The control can define the AppBarButtons in XAML as well.

*View code*  
https://github.com/wasteam/waslibs/tree/master/src/AppStudio.Uwp/Actions

###Animation Extensions <a name="animation-extensions"></a>
The Animation Extensions are a set of extension Methods on the FrameworkElement control which allows to execute animations from C# code. The animations can be invoked asynchronously.

*View code*
https://github.com/wasteam/waslibs/tree/master/src/AppStudio.Uwp/Extensions

###Converters <a name="converters"></a>
Converters allow you to transform a certain value from a data type in other different value. Using Converters leads to have a cleaner XAML code. In the samples shown, the first converter is used to change the Visibility of an image based in the value a Toggle control. If the Toggle is True (Boolean value), the converter returns 'Visible' for the Visibility property of the image control. When the Toggle is False, it returns 'Collapsed' for the Visibility property.

*View code*
https://github.com/wasteam/waslibs/tree/master/src/AppStudio.Uwp/Converters

##Labs <a name="labs"></a>
This section contains the information for those controls developed but still not being used in the Apps generated by Windows App Studio.  
###ResponsiveGridView Contro <a name="responsive-grid-view-labs"></a>
Improved experience and performance for the exsisting ResponsiveGridView control. 

*View code*
https://github.com/wasteam/waslibs/tree/master/src/AppStudio.Uwp/Labs/ResponsiveGridView

###Accordion Control <a name="accordion"></a>
The Accordion Control display a set of images stacked vertically, showing the selected image using the full size. The "stacked" images below the selected one are pre-visualized in the stack waiting to be slided vertically. By default, four images are shown in the stack. If the collection have more than four images, the remain ones will appear as the user slide the existing ones.

*View code*
https://github.com/wasteam/waslibs/tree/master/src/AppStudio.Uwp/Labs/Accordion

###SlideShow Control <a name="slide-show"></a>
The SlideShow Control creates animated transitions between a set of images. You can control the transitions using the DelayInterval and FadeIntervale properties. The Delay Interval define how many time an image remain displayed before start the transition to the next one; The FadeInterval defines the duration of the face-in efect to display the next image.

*View code*
https://github.com/wasteam/waslibs/tree/master/src/AppStudio.Uwp/Labs/SlideShow

###ShapeImage Control <a name="shape-image"></a>
Allows to show an image using a specific shape to display it. Currently it supports tree kind of pre-defined shapes: Border, Elipse and Rectangle. 

*View code*
https://github.com/wasteam/waslibs/tree/master/src/AppStudio.Uwp/Labs/ShapeImage

###AutoHide Control <a name="auto-hide"></a>
This is generic control which allows you to hide / show a certain UI control with fade-out / fade-in effect. It is based on the existing user interaction done by mouse movement. If the user does not move the mouse for a certain duration the control contained will be hidden. If mouse move is detected, the control contained will appear.

*View code*
https://github.com/wasteam/waslibs/tree/master/src/AppStudio.Uwp/Labs/AutoHide

###Mosaic Control <a name="mosaic"></a>
This responsive control allows to create a random mosaic of images making size variations. The control uses an specific with to create the mosaic.

*View code*
https://github.com/wasteam/waslibs/tree/master/src/AppStudio.Uwp/Labs/Mosaic

#Data Providers Library <a name="data-providers"></a>
This library contains the implementation of all the data sources available in 
Windows App Studio apps.

This is a Portable Class Library that can be used in:
- Windows 10
- Windows 8.1
- Windows Phone 8.1 

Among the Data Provider classes implemented in Windows App Studio, those which access third party data, help handling the complexity of each particular provider (API calls, authentication and authorization requirements, data parsing, etc.) so provides a smooth and uniform way to access the content from these providers.  All Data Providers take advantage of the AppCache service to improve the App performance.

##Facebook Data Provider <a name="facebook"></a> 
The FacebookDataProvider allow to retrieve Facebook data through its API. You have to be registered in Facebook Apps to be able to interact with the Facebook API and you must obtain an AppId and AppSecret to configure the data access. Finally, you must use the Page ID from what you want to get the information.

*Further info*  
https://developers.facebook.com/apps/  
https://developers.facebook.com/docs/graph-api/using-graph-api  

*View Code*  
https://github.com/wasteam/waslibs/tree/master/src/AppStudio.DataProviders/Facebook  

##Twitter Data Provider <a name="twitter"></a>
The TwitterDataProvider retrieve data using the Twitter API. To be able to request Twitter data, the user must be registered in Twitter Apps and obtain a ConsumerSecret, an AccessToken and an AccessTokenSecret. This Data Provider can retrieve the user TimeLine or gather data by Twitter User Name or Hashtag.
*Further info*  
https://apps.twitter.com/  
https://dev.twitter.com/rest/public  

*View Code*  
https://github.com/wasteam/waslibs/tree/master/src/AppStudio.DataProviders/Twitter

##Flickr Data Provider <a name="flickr"></a>
The FlickrDataProvider gets images using the Flickr API. You can retrieve content based on tags or you can access the images from a Flickr account by using the UserID. To resolve which UserID is assigned to a certain Flickr account use http://idgettr.com/

*Further info*  
https://www.flickr.com/services/feeds/  
http://idgettr.com/  

*View code*  
https://github.com/wasteam/waslibs/tree/master/src/AppStudio.DataProviders/Flickr  

##YouTube Data Provider <a name="youtube"></a>
The YouTube Data Provider retrieve data through the YouTube API by using the YouTube ChannelID, the PlaylistID, or a search term. To be able to request data using this provider, you must be registered for Google Developers Console and get an API Key.

*Further info*  
https://console.developers.google.com/project
https://dev.twitter.com/rest/public

*View code*  
https://github.com/wasteam/waslibs/tree/master/src/AppStudio.DataProviders/YouTube

##WordPress Data Provider <a name="wordpress"></a>
The WordPress Data Provider retrieve data from the Word Press blog configured in the property WordPressQuery. You can configure the search in the Data Provider by using one of the following options: Posts, Categories or Tags. The information is read in JSON format and transformed to the WordPressSchema entity. This Data Provider relies on the REST API to access the source content. If the target blog is self-hosted (not in Wordpress.com) it must have the JetPack plug-in installed and the JSON API enabled.

*View code*  
https://github.com/wasteam/waslibs/tree/master/src/AppStudio.DataProviders/WordPress

##Rss Data Provider <a name="rss"></a>
The RssDataProvider retrieve information from the configured RSS Url, the data is read form the source in XML format and transformed to a RssSchema entity.

*View code*  
https://github.com/wasteam/waslibs/tree/master/src/AppStudio.DataProviders/Rss

##Bing Data Provider <a name="bing"></a>
The Bing DataProvider allows you to retreive Microsoft Bing web search engine results direct to your App.

*View code*  
https://github.com/wasteam/waslibs/tree/master/src/AppStudio.DataProviders/Bing

##LocalStorage Data Provider <a name="local-storage"></a>
This Data Provider access data from the LocalStorage. You can configure which file will be used as content source. The information, stored in JSON format, is transformed to the specified data type.

*View code*  
https://github.com/wasteam/waslibs/tree/master/src/AppStudio.DataProviders/LocalStorage

##REST API Data Provider <a name="rest-api"></a>
Among the Data Provider classes implemented in Windows App Studio, those which access third party data, help handling the complexity of each particular provider (API calls, authentication and authorization requirements, data parsing, etc.) so provides a smooth and uniform way to access the content from these providers.  All Data Providers take advantage of the AppCache service to improve the App performance.

The RestApiDataProvider retrieve information from the configured endpoint Url. You can configure de pagination type and items per page.

*View code*  
https://github.com/wasteam/waslibs/tree/master/src/AppStudio.DataProviders/RestApi

# Common
This library contains utility classes to create XAML applications. 
This is a Portable Class Library that can be used in:
- Windows 10
- Windows 8.1
- Windows Phone 8.1
