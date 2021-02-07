#!/bin/bash
if [ -z $(echo $1 | grep -P 'v\d+\.\d+\.\d') ]
then
    echo "Version is required to be passed. Format should be ./build-all.sh vX.X.X"
    echo "What was received was $1"
    exit 1
fi

AppName="regexnet"
BuildVersion=$(echo $1 | cut -d 'v' -f 2)
Bin=$(dirname "$(readlink -f $0)")/bin
OutPathBase=$Bin/LinuxPackages
PkgName="$AppName-$BuildVersion-amd64"
PkgNameArm="$AppName-$BuildVersion-arm64"
OutPath="$OutPathBase/$PkgName"
OutPathArm64="$OutPathBase/$PkgNameArm"

if [ ! -d $OutPath/DEBIAN ]
then
    mkdir -p "$OutPath/DEBIAN"
fi
if [ ! -d "$OutPathArm64/DEBIAN" ]
then
    mkdir -p "$OutPathArm64/DEBIAN"
fi

rm -f "$Bin/*.zip"
rm -f "$Bin/*.deb"

cat > $OutPath/DEBIAN/control << EOF
Package: ${AppName}
Version: ${BuildVersion}
Section: custom
Architecture: amd64
Maintainer: https://github.com/troygeiger/${AppName}
Description: Provides regular expression processing to the command line.
EOF

cat > $OutPathArm64/DEBIAN/control << EOF
Package: ${AppName}
Version: ${BuildVersion}
Section: custom
Architecture: arm64
Maintainer: https://github.com/troygeiger/${AppName}
Description: Provides regular expression processing to the command line.
EOF


dotnet publish -c Release -r linux-x64 -o $OutPath/usr/bin -p:Version=$BuildVersion
dotnet publish -c Release -r linux-arm64 -o $OutPathArm64/usr/bin -p:Version=$BuildVersion
dotnet publish -c Release -r win-x64 -o $Bin/publish -p:IncludeNativeLibrariesForSelfExtract=true
dpkg-deb --build $OutPath
dpkg-deb --build $OutPathArm64
mv "$OutPathBase/$PkgName.deb" "$Bin/"
mv "$OutPathBase/$PkgNameArm.deb" "$Bin/"

zip -j "$Bin/linux-x64.zip" "$OutPath/usr/bin/$AppName"
zip -j "$Bin/linux-arm64.zip" "$OutPathArm64/usr/bin/$AppName"
zip -j "$Bin/windows-x64.zip" "$Bin/publish/$AppName.exe"

# Clean up
rm -rf "$OutPathBase"
rm -rf "$Bin/Release"
rm -rf "$Bin/publish"