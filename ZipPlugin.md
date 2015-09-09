# Introduction #

A plugin that zips files and folders.


---

# Command line #
## Example ##
` TreeTrim.Console c:\mytree -zip -email `

Creates a zip file in a temporary location, e-mails it and then deletes the zip file.

` TreeTrim.Console c:\mytree -zip:dontCleanUp:writeTo:"c:\temp\mysource.zip" `

Creates a zip file in `c:\temp\mysource.zip` and leaves it there!

## Task Parameters ##
  * **writeTo** if present, the resulting zip file will be written here.  If not specified, then a temporary location will be used.
  * **dontCleanUp** if present, the resulting zip will not be deleted.

---

# Settings file #
_This plugin doesn't have a settings file_.