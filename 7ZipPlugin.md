# Introduction #

This plugin compresses the input into 7zip format.


---

# Command line #
## Example ##
` TreeTrim.Console c:\mytree -7zip:password:abc123+WriteTo:"c:\temp\my folder\output.7z" `

## Task Parameters ##
  * **WriteTo** if present, the resulting file will be written here.  If not specified, then then the WriteTo field in the static settings file will be used.  If this isn't present, then a temporary location will be used.
  * **Password** if present, the resulting file will be password protected.  If not specified, then then the Password field in the static settings file will be used.  If this isn't present, then no password will be used.

---

# Settings file #
## Example ##
```
<?xml version="1.0" encoding="utf-8" ?>
<Settings>
    <!-- 
        KEEP THE CONTENTS OF THE NODE BELOW ON THE SAME LINE AS THE NODE.
        ANY WHITE SPACE WILL BUGGER UP THE EXPRESSIONS.
    -->
    <Password>abc123</Password>
    <WriteTo>c:\temp\whatever.7z</WriteTo>
    <PathTo7ZipBinary>C:\Program Files\7-Zip\7z.dll</PathTo7ZipBinary>
</Settings>
```
## Fields ##
  * **PathTo7ZipBinary** This mandatory field points to the 7z.dll file on the machine.  This must be the platform specific (x86/x64) DLL in your installation of 7zip.
  * **WriteTo** The path to where the compressed file will be written. If not specified, a temporary location will be used.
  * **Password** The password.

---
