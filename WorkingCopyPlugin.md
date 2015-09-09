# Introduction #

This plugin makes a working copy of the folder specified


---

# Command line #
## Example ##
` TreeTrim.Console c:\mytree -workingCopy -zip -email `

Creates a working copy in a temp location, zips it, and e-mails it.  It the removes the working copy

` TreeTrim.Console c:\mytree -workingCopy:dontCleanUp -zip -email `

Creates a working copy in a temp location, zips it, and e-mails it _without removing the working copy_.

` TreeTrim.Console c:\mytree -workingCopy:writeTo:"c:\temp" -zip -email `

Creates a working copy in the c:\temp directory, zips it, and e-mails it and then removes the working copy.

## Task Parameters ##
  * **writeTo** if present, the resulting folder will be copied here.  If not specified, then a temporary location will be used.
  * **dontCleanUp** if present, the resulting folder will not be deleted.

---

# Settings file #
_This plugin doesn't have a settings file_.