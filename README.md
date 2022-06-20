# NippyWard.NCurses
A multi-platform NCurses .NET wrapper with UTF-8 support 

## Rationale
After looking for a decent NCurses wrapper supporting Windows and UTF-8, not finding one - except one based on the (back then) outdated PDCurses -, I decided to write my own wrapper.

This project started without any prior experience writing wrappers and during the first months of .NET Core 1.0. It has evolved from using an IL generator to using a source generator today.

## Installation
### Linux
Ensure NCurses is installed! This should be the default for most if not all distributions. At the time of writing NippyWard.NCurses supports ubuntu.16.04-upwards and Debian-8-upwards.

Any other distribution can be added in https://github.com/sebaFlame/NippyWard.NCurses/blob/2e587a1e3b0e38d587ea290b7be66d6b32e2e34b/src/NippyWard.NCurses/Interop/Constants.cs#L48-L105, but most should Just Work™.

### Windows
Create and install a NuGet package from [runtime.win.NCurses](deps/runtime.win.NCurses/).

Building this package downloads the required DLL files (x86 & x64) from http://invisible-island.net, the official NCurses website.

You're gonna have to manually check the *$VERSION* the build downloaded in the bin\Debug directory.

Nuget push pushes the NuGet package to your local cache. This way it can be consumed by any project.

```cmd
cd deps\NippyWard.NCurses.Tasks
dotnet build
cd ..\runtime.win.NCurses
dotnet build
dotnet pack
dotnet nuget push bin\Debug\runtime.win.NCurses.$VERSION.nupkg -s %USERPROFILE%\.nuget\packages
```
This nuget package should get auto-referenced when on Windows and referencing NippyWard.NCurses. You need to guarantee it's in your cache, because it is not available on NuGet.org.

### MacOS
I have no mac.

## Usage
The class [Ncurses](src/NippyWard.NCurses//NCurses.cs) is the starting point of all operations.

A call to NCurses.Start:
- initilaizes NCurses
- overrides the current Console
- returns the Standard Screen.

The Standard Screen (stdscr) is an [IWindow](src/NippyWard.NCurses/IWindow.cs) which you can use like any window, except that it spans the entire screen.
```C#
IWindow stdScr = NCurses.Start();
```
A window is used to manipulate - write text to - the screen. Windows can be created and destoyed (except the stdscr). These windows can be optimized to only support single byte characters (ASCII). By default multi-byte windows are created unless explicitly stated.
```C#
IWindow win1 = NCurses.CreateWindow(20, 20, 0, 0);
```
Many operations can be used on a window, check [IWindow](src/NippyWard.NCurses/IWindow.cs) for a full list. The API is very similar to Console, the major difference being a 2nd call to IWindow.Refresh or IWindow.NoOutRefresh/NCurses.Update to actually update the screen.

The [NCurses documentation](https://invisible-island.net/ncurses/man/ncurses.3x.html) is relevant to some degree. Especially check the sections about [refresh](https://invisible-island.net/ncurses/man/curs_refresh.3x.html) to prevent screen flickering.
```C#
win1.Write("Hello World");
win1.Refresh(); //update window, screen and render

//update multiple windows
IWindow win2 = NCurses.CreateWindow(20, 20, 20, 20);
win1.Write("Hello Galaxy");
win1.NoOutRefresh(); //update window
win2.Write("Hello Universe");
win2.NoOutRefresh(); //update window
NCurses.Update(); //update screen and render
```
Check [test](test/NippyWard.NCurses.Tests/) for more usage examples (e.g. panels, pads).
For UTF-8 examples, check [MultiByte](test/NippyWard.NCurses.Tests/MultiByte/).

When using UTF-8 on Windows, keep in mind it gets converted to UTF-16 to render.

NippyWard.NCurses is NOT thread-safe, unless locking gets enabled explicitly using NCurses.EnableLocking ! When enabled, a set of operations on 1 ore more windows can be thread-safely grouped using NCurses.CreateThreadSafeDisposable.

Remember to dispose when your're done using the window.
```C#
win1.Dispose();
```
When your program ends, a call to Ncurses.End destroys NCurses and restores the Console.
```C#
NCurses.End();
```