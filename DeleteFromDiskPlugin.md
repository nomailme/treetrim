# Introduction #

This plugin deletes files or folders from the disk.  Files and folders matching the patterns in the settings file will be deleted.

**If you have any suggestions for what files or folders this plugin should recognise and delete, please visit [this page](http://groups.google.com/group/treetrim/t/f095b73a667834).**


---

# Command line #
## Examples ##
` TreeTrim.Console c:\mytree -deleteFromDisk `

Deletes the files and folders specified in the settings.

` TreeTrim.Console c:\mytree -workingCopy -deleteFromDisk `

Makes a working copy and deletes the files and folders specified in the settings.

## Task Parameters ##
_This plugin doesn't take any parameters.  Everything is specified in the settings file._


---

# Settings file #
## Example ##
```
<?xml version="1.0" encoding="utf-8" ?>
<Settings>
    
    <!-- 
        KEEP THE CONTENTS OF THE NODES BELOW ON THE SAME LINE AS THE NODE.
        ANY WHITE SPACE WILL BUGGER UP THE EXPRESSIONS.
    -->

    <FilePattern>\.scc$|\.vssscc$|\.user$|\.vspscc$|\.suo$</FilePattern>
    <FolderPattern>^bin$|^obj$|^Debug$|^Release$|^_ReSharper\..*$|^_sgbak$</FolderPattern>

</Settings>
```
## Fields ##
  * **FilePattern** This mandatory field specifies patterns that will identify what files to delete.
  * **FolderPattern** As above, but for folders.

---
