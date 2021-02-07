#!/bin/bash
if [ -z $(echo $1 | grep -P 'v\d+\.\d+\.\d') ]
then
    echo "Version is required to be passed. Format should be ./build-all.sh vX.X.X"
    echo "What was received was $1"
    exit 1
fi

BuildVersion=$(echo $1 | cut -d 'v' -f 2)
OutPathBase=$(dirname "$(readlink -f $0)")/bin/LinuxPackages
OutPath="$OutPathBase/regexnet-$BuildVersion-amd64"
OutPathArm64="$OutPathBase/regexnet-$BuildVersion-arm64"

if [ ! -d $OutPath/DEBIAN ]
then
    mkdir -p "$OutPath/DEBIAN"
fi
if [ ! -d "$OutPathArm64/DEBIAN" ]
then
    mkdir -p "$OutPathArm64/DEBIAN"
fi

cat > $OutPath/DEBIAN/control << EOF
Package: regexnet
Version: ${BuildVersion}
Section: custom
Architecture: amd64
Maintainer: https://github.com/troygeiger/regexy
Description: Provides regular expression processing to the command line.
EOF

cat > $OutPathArm64/DEBIAN/control << EOF
Package: regexnet
Version: ${BuildVersion}
Section: custom
Architecture: arm64
Maintainer: https://github.com/troygeiger/regexy
Description: Provides regular expression processing to the command line.
EOF


dotnet publish -c Release -r linux-x64 -o $OutPath/usr/bin -p:Version=$BuildVersion
dotnet publish -c Release -r linux-arm64 -o $OutPathArm64/usr/bin -p:Version=$BuildVersion
dpkg-deb --build $OutPath
dpkg-deb --build $OutPathArm64