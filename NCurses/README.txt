//download binaries
//http://invisible-island.net/datafiles/release/mingw32.zip (bin/libncursesw6.dll & bin/libpanelw6.dll into x86 directory)
//http://invisible-island.net/datafiles/release/mingw64.zip (bin/libncursesw6.dll & bin/libpanelw6.dll into x64 directory)
//change version to correct one in project.json
dotnet build
dotnet pack
//with nuget >= 3.3
nuget add NCurses.version.nupkg -source $HOME/.nuget/packages