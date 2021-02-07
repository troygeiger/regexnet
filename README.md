# Regexnet
Regexnet is a program for performing regular expression and returning the results with multiple output options. It is written in C# with .NET 5.0.
It compiles as a single executable and is currently setup to build for Windows and Linux; along with an Arm64 Linux build. 

Pre-compiled versions can be found in the [Releases](https://github.com/troygeiger/regexnet/releases) page.

# Usage
First off, let's execute the help.

```
$ regexnet --help
regexnet 1.0.0
Copyright (C) 2021 Troy Geiger

  -s, --string           The input string to process. (This can also be piped in. Ex. echo "Hello" | ./regexnet -e '\w')

  -e, --expression        Required. The regular expression query.

  -a, --match-all        If set, matching will continue until no more matches are found.

  -g                     Specify the group(s) to return in a comma separated list. Ex. -g 1,2

  --ml                   Return group results as multi-line; otherwise single-lines are returned.

  -i, --ignore           Ignore Case option is applied to the expression.

  -m, --multiline        Multi-Line option is applied to the expression.

  -n, --no-line-break    Don't output a line break at end of output.

  --help                 Display this help screen.

  --version              Display version information.
  ```

## Example time
Below are some examples the ways regexnet can be used.
```
regexnet -s "Hello, World" -e ",\s\w+$" -ml
, World

world=$(regexnet -s "Hello, World" -e ",\s\w+$")
echo $world
, World

echo "Hello, World" | regexnet -e ",\s\w+$" -ml
, World

cat /proc/version | regexnet -e "\w+\sversion\s(\d+\.)+\d+" -a -ml
Linux version 5.4.0
gcc version 9.3.0

```
Another use case would be to return only a certain groups from a string.
Lets pull just the version version match by modifying to last example from above.
```
 cat /proc/version | regexnet -e "\w+\sversion\s((\d+\.)+\d+)" -g 1
 5.4.0
 ```
 How about multiple groups.
 ```
 cat /proc/version | regexnet -e "(\w+)\sversion\s((\d+\.)+\d+)" -g 1,2
 Linux5.4.0
 ```

# Building
First off, you will need to have the Microsoft .NET 5.0.x SDK installed. 

To build from a Debian based distro, you can simply execute the `build_all.sh` script. This will compile and build .deb packages. You will need to provide a version to the script with a format of vX.X.X. 
```
./build_all.sh v1.0.0
```

 
