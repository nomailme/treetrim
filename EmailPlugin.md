# Introduction #

This plugin emails the input.


---

# Command line #
## Examples ##
` TreeTrim.Console c:\mytree -zip -email `

Zips the directory and then emails it.  All settings are taken from the associated settings XML file.

` TreeTrim.Console c:\mytree -email:timeoutInSeconds:120 `

Emails the files in the directory.  The timeout is set high as there may be many files to attach.

` TreeTrim.Console c:\mytree -zip -email:additionalAttachments:"c:\copyright.txt;c:\disclaimer.txt"+recipients:person1@whatever.com,person2@whatever.com `

As well as attaching the zipped file, it adds two additional attachments and specifies the recipients.

## Task parameters ##
  * **serverAddress** The address of the SMTP server to use
  * **userName** The user name to log into the SMTP server
  * **password** The password to log into the STMP server
  * **timeout** The timeout _in seconds_ for sending the email
  * **recipients** A comma delimited list of receipients' email addresses
  * **sendersEmail** The email address of the sender
  * **sendersName** The name of the sender
  * **body** The body of the e-mail
  * **subject** The subject of the e-mail
  * **dontAttachOutputOfPreviousTask** Don't attach what the previous task produced.
  * **additionalAttachments** A semi-colon separated list of paths to additional attachments, e.g. ` c:\file1.txt;c:\file2.txt `


---

## Settings file ##
### Example ###
```
<?xml version="1.0" encoding="utf-8" ?>
<Settings>
    <Sender>
        <DisplayName>Steve Dunn</DisplayName>
        <EmailAddress>steve@dunnhq.com</EmailAddress>
    </Sender>
    <Subject>Source is attached.</Subject>
    <CommaSeparatedRecipients>user1@guest.com,user2@guest.com</CommaSeparatedRecipients>
    <Body>Cleaned and zipped using Steve Dunn's Tree Trim utility.</Body>
    <Smtp>
        <ServerAddress>mail.yourserver.com</ServerAddress>
        <TimeoutInSeconds>120</TimeoutInSeconds>
        <Credentials>
            <Username>the-user</Username>
            <Password>abc123</Password>
        </Credentials>
    </Smtp>
    <AdditionalAttachments>
        <Attachment>c:\temp\copyright.txt</Attachment>
        <Attachment>c:\temp\disclaimer.txt</Attachment>
    </AdditionalAttachments>
</Settings>
```

## Fields ##
All nodes are optional as they can be specified in the task's paramters from the command line.

---
