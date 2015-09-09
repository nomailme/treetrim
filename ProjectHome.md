# Tree Trim #

This is a command line tool that trims your source code tree.  It removes debug files, source control bindings, and temporary files.

An example would be:

` treetrim.console.exe c:\my\tree -deletefromdisk `

This would delete the debug files and folders in ` c:\my\tree `

---

Updates:

**v.1.0.1**

  1. Includes a 'show in explorer' plugin which opens up an Explorer window showing the output of the previous plugin.
  1. The shell's context menu entries are now written to the correct key in the registry.  Thanks to Matt (http://sticklebackplastic.com/) for the tip.

---




It is based on the excellent [CleanSourcesPlus project](http://www.codinghorror.com/blog/archives/000368.html).

It can compress, email, or copy the output.  The tool is plugin based so can be extended to include whatever functionality you wish.  It can be included as a task in your CI environment.

**For more information, please see the [FAQ](FAQ.md) page**


---

## Examples of the command line ##

Here's a couple of examples of the command line:


` treetrim.console c:\my\tree -deletefromdisk `

This removes the debug files and folders found in your source tree


` treetrim.console c:\my\tree -workingcopy -deletefromdisk `

This makes a working copy and removes the debug files and folders **from the working copy**.


` treetrim.console c:\my\tree -workingcopy -deletefromdisk -zip:writeTo:c:\out.zip `

This makes a working copy, removes the debug stuff, and writes the output to zipped file.


` treetrim.console c:\my\tree -workingcopy -deletefromdisk -zip -email`

This makes a working copy, removes the debug stuff, zips it up in a temporary file, and emails it.

---

The order of the **tasks** on the command line dictates the order in which the items are run.
**workingcopy** is a **task** - it makes a working copy.
**zip** is a task - it compresses the output of the previous task.

The output from one task is the input to the next task.  In the example above, it went:
  * **make a working copy**
  * **remove the debug** -- _in the temporary folder that the previous task created_
  * **zip it** -- _zips the contents of that folder_
  * **email it** -- _emails the content of the previous task, which is a zip file_


Please also take a look at [Tree Trim discussion group](http://groups.google.com/group/treetrim).

---

Tree Trim is free to use - but if you like it, please consider [donating to this charity](http://www.justgiving.com/30kdrop)

[![](http://d2cme1q4f44ryr.cloudfront.net/Utils/imaging.ashx?width=160&height=160&square=160&imageType=frpphoto&img=42013/89e1846b-128b-479b-9958-0ee482dd90a7.jpg)](http://www.justgiving.com/30kdrop)