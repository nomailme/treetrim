# Introduction #

This plugin removes the Subversion source control directories.  Directories removed are those named **`.svn`** and **`_svn`**


---

# Command line #
## Example ##
` TreeTrim.Console c:\mytree -removeSubversionDirectories `

Removes the subversion directories.

` TreeTrim.Console c:\mytree -workingCopy:dontCleanUp -removeSubversionDirectories `

Creates a working copy in a temp location and removes the subversion directories _without removing the working copy_.

## Task Parameters ##
_This plugin doesn't take any parameters_

---

# Settings file #
_This plugin doesn't have a settings file_.