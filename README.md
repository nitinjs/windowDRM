# WindowDRM
Just another app to make windows applications screenshot and screenshare protected

#### Core
This app is simple background service which pInvokes `SetWindowDisplayAffinity` to protect the selected apps from screen capture apps

> const uint WDA_MONITOR = 1;<br/>
> SetWindowDisplayAffinity(this.Handle, WDA_MONITOR);

#### References
https://stackoverflow.com/questions/56712964/how-to-use-setwindowdisplayaffinity
https://stackoverflow.com/questions/1363167/how-can-i-get-the-child-windows-of-a-window-given-its-hwnd/28055461#28055461
http://pinvoke.net/default.aspx/user32.EnumChildWindows
https://devblogs.microsoft.com/oldnewthing/20130603-00/?p=4193
