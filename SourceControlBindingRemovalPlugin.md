# Introduction #

This plugin removes source control bindings from Visual Studio **2003** projects and solutions.
**THIS PLUGIN IS JUST FOR VISUAL STUDIO 2003 PROJECTS**.  I don't yet have examples of 2005/2008 projects that have source control bindings.  I'll update this once I do.


---

# Command line #
## Example ##
` TreeTrim.Console c:\mytree -removeSourceControlBindings `

Removes the source control bindings from any Visual Studio project or solution files.

## Task Parameters ##
_This plugin doesn't take any parameters_

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
    <Pattern >\.(cs|vb)proj$|\.sln$</Pattern>
</Settings>
```
## Fields ##
  * **Pattern** The regular expression pattern that matches project or solution files.