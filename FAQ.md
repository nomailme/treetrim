# What is Tree Trim? #
Tree Trim is a tool that trims directories containing source code (source trees).  When working in source trees, tools often generate files and folders that are not neccessary when you just want the source.

# Why would I use it? #
Often, you want to back-up or email your project.  Even a small project directory can become very large when it contains intermediate files, cached files, user settings, source control bindings etc.  Emailing or backing up this junk takes space and time.

# How do I use it? #
You can either run it from the command line or from Windows Explorer.  The setup program intalls context menu commands when right-clicking on folders in Windows Explorer.

# Where would I use it? #
You can either run it from Windows Explorer or from the command line.  The command line is useful for automated environments, such as Continuous Integration environments (Cruise Control/Team City etc.)

# What's included with the tool? #
The tool has two applications - the command line app and the GUI app.  It also has a bunch of plugins that do the actual work.  When using Explorer's context menu commands, the GUI is used.  The GUI currently just runs the commands specified on the command line and displays information if any errors occured.  The console application simply outputs any information to the console - which is good, as it won't bring everything to a halt if it's used in a CI environment.

# How does it work #
The tool works from command line parameters passed to either the gui or the console app.  The command line states what actions are to be performed on the tree and any parameters for the actions.  For instance, this simply deletes debug files and folders and any temporary files.
` treetrim.console.exe c:\dev\myproject -deleteFromDisk `

Here, ` deleteFromDisk ` is a task.  It deletes temporary stuff from disk.

Here's another

` treetrim.console.exe c:\dev\myproject -workingCopy -deleteFromDisk -zip -email `

Now there's four tasks.  It first makes a **working copy** of your source tree, then it **deletes temporary stuff from the working copy**, then it **zips the working copy**, then it **e-mails the zip file**.

You can see from the example above that plugins are run consectively and that the output from one plugin is used in the next plugin.

# How do I pass parameters to a plugin? #
Plugins either take parameters from the command line or from their settings file.  If a required parameter **isn't** passed on the command line, the plugin will use its **settings file**.

For instance, the [e-mail plugin](EmailPlugin.md) takes a bunch of parameters, such as

` -email:sendersName:"Steve Dunn"+sendersEmail:"me@mine.com"+recipients:"fred@somewhere.com,wilma@somewhere.com" `

The email plugin will use the parameters if they're passed on the command line.  If they're **not** passed, it'll use the settings file.  Here's the default settings file for the e-mail plugin
```
<?xml version="1.0" encoding="utf-8" ?>
<Settings>
    <Sender>
        <DisplayName>Your Name</DisplayName>
        <EmailAddress>you@yours.com</EmailAddress>
    </Sender>
    <Subject>Source is attached.</Subject>
    <Recipients>
        <Recipient>wilma@somewhere.com</Recipient>
    </Recipients>
    <Body>Cleaned and zipped using Steve Dunn's Tree Trim utility.</Body>
    <Smtp>
        <ServerAddress>mail.yourhost.co.uk</ServerAddress>
        <TimeoutInSeconds>120</TimeoutInSeconds>
        <Credentials>
            <Username>admin</Username>
            <Password>password123</Password>
        </Credentials>
    </Smtp>
    <AdditionalAttachments>
        <Attachment>c:\temp\copyright.txt</Attachment>
        <Attachment>c:\temp\disclaimer.txt</Attachment>
    </AdditionalAttachments>
</Settings>
```

# How do I write my own plugin? #
Reference `Dunn.TreeTrim.dll` and create a class derived from the `IPlugin` interface.  Look at the plugins in the source for more information.  Also take a look the CreatingPlugins page.  To have your plugin picked up by the tool, just plonk the DLL in the same directory as the exe.