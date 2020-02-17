dotnet pack

//with nuget >= 3.3
nuget add bin/Debug/runtime.win.NCurses.version.nupkg -expand -source $HOME/.nuget/packages
//or with dotnet
dotnet nuget push bin/Debug/runtime.win.NCurses.version.nupkg -s $HOME/.nuget/packages