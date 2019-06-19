'SendEmail
'*******************************************************
'Parameter
'(1)From
'(2)To : Split by ","  
'(3)CC
'(4)BCC
'(5)Subject
'(6)BodyType : S=String, T=TextFile, H=HtmlFile
'(7)Body : String(value/path)
'(8)Attachment : Split by ","
'*******************************************************
if WScript.Arguments.Count <> 8 then
	 LF = chr(10)
	 wscript.echo "Send email Error : " +  LF _
                    & "Missing Parameter (double quotation marks & Split by space)" + LF _
	            & "(1)From" + LF _
	            & "(2)To : Split by "+""""+","+""""+" eg."+""""+"a@mail.com,b@mail.com"+"""" + LF _
	            & "(3)CC : none =" + """" + """" + LF _
	            & "(4)BCC" + LF _
	            & "(5)Subject" + LF _
	            & "(6)BodyType : S/T/H  S=String, T=TextFile, H=HtmlFile" + LF _
	            & "(7)Body : String (value/path)" + LF _
	            & "(8)Attachment : Split by "+""""+","+"""" + LF + LF _
	            & "eg.CISendEmail "+""""+"bas@taifex.com.tw"+""" "++""""+"aa@taifex.com.tw"+""" "++""""+""" "+""""+""" "+""""+"Test Subject"+""" "+""""+""+""" "+""""+""" "+""""+""" " + LF + LF _
	            & "eg.CISendEmail "+""""+"bas@taifex.com.tw"+""" "++""""+"aa@taifex.com.tw"+""" "++""""+""" "+""""+""" "+""""+"Test Subject"+""" "+""""+"S"+""" "+""""+"Test Body"+""" "+""""+"D:\temp\test1.txt,D:\temp\test2.txt " 
	 'wscript.echo "CISendEmail.vbs ¡¨a@taifex.com.tw¡¨ ¡¨b@mail.com,c@mail.com¡¨ ¡¨CC¡¨ ¡¨BCC¡¨ ¡¨Subject¡¨ ¡¨BodyType:S/T/H¡¨ ¡¨TextBody¡¨ ¡¨AttachFile1,¡¨AttachFile2¡¨"
	 wscript.quit
end if

strFrom = WScript.Arguments(0)
strTO = WScript.Arguments(1)           
strCC = WScript.Arguments(2)
strBCC = WScript.Arguments(3)
strSubject = WScript.Arguments(4)
strBodyType = WScript.Arguments(5) 
strBody = WScript.Arguments(6)
strAttac = WScript.Arguments(7)        


Set cdoConfig = CreateObject("CDO.Configuration") 
sch = "http://schemas.microsoft.com/cdo/configuration/"
With cdoConfig.Fields 
    .Item(sch & "sendusing") = 2 ''cdoSendUsingPort 
    .Item(sch & "smtpserver") = "smtp.taifex.com.tw"
    .update 
End With 
'.BodyPart.ContentTransferEncoding = "7bit" 
Set objEmail = CreateObject("CDO.Message")
set objEmail.configuration = cdoConfig
log_attach_file=infa_log_file

objEmail.From = strFrom
if strTO <> "" then
   objEmail.To = strTO
end if
if strCC <> "" then
   objEmail.CC = strCC
end if
if strBCC <> "" then
   objEmail.BCC = strBCC
end if
objEmail.Subject = strSubject

if strBody <> "" then
   if strBodyType = "T" then
   	 'Text File
      Set fso = CreateObject("Scripting.FileSystemObject")
      objEmail.Textbody = fso.OpenTextFile(strBody, 1).ReadAll 
   elseif strBodyType = "H" then
   	 'Html File
      Set fso = CreateObject("Scripting.FileSystemObject")
      objEmail.htmlbody = fso.OpenTextFile(strBody, 1).ReadAll 
   else
   	 'String
      objEmail.Textbody = strBody
   end if
end if

if strAttac <> "" then
   strAttacFile = Split(strAttac,",")
   for each item in strAttacFile
       objEmail.AddAttachment item
   next
end if

objEmail.Send	  
'If Err.Number <> 0 Then WScript.Echo "SendEmail Error" 

wscript.quit Err.Number
