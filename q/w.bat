@echo off
REM
REM     q.exe
REM
REM     Command line utility for storing a list of paths you want to CD to quickly
REM
REM     Since you can't change the cmd.exe's current directory from a .Net app,
REM     this wrapper batch file parses the q.exe output and if it sees a line
REM     output with an asterisk at the front, it pushd's to the directory listed
REM     after the *.


REM EnableDelayedExpansion is necessary for the For loop to work since the entire 
REM contents of the For are evaluated on each iteration before running.
setlocal EnableDelayedExpansion

SET Q_OUTPUT=

REM This For statement runs the q.exe command and iterates over the lines of 
REM text returned to standard out.  usebackq allows us to support paths with spaces.
REM tokens=* reads the entire line into a since token, %%i
REM
REM Note: I saw some strange behaviour with the output parsing here.  If q.exe returned
REM raw paths, ie c:\foo\bar\baz, the script would only see \foo\bar\baz.  It seems everything
REM before a colon is encountered gets chopped off.  I can find no documentation of this
REM behaviour.  As a hack, q.exe sticks a colon on the front of all lines it returns.
FOR /F "usebackq tokens=*" %%i in (`q.exe magicwdotbat %1 %2 %3`) do (
    REM We want to do substring manipulation which isn't possible with a %%i style variable,
    REM so assign to a regular variable.
    set var=!%%i!
    REM Strip off the first character
    set var=!var:~1!
    REM Set var2 to the new first character - for detecting the starting *
    set var2=!var:~0,1!
    REM Set var3 to the remainder of the string after the first character.  When the string
    REM starts with a *, var3 will be the path we want to pushd to
    set var3=!var:~1!
    
    REM Else gave syntax errors here so I just use 2 If statements.  I think it may be an 
    REM issue with being nested inside a For statement and the cmd parser not liking it
    if "!var2!" == "*" (
        SET Q_OUTPUT=!var3!
    )
    if not "!var2!" == "*" (
        echo !var!
    )
    
)

REM This is a trick to pass a variable through the endlocal barrier.  Inside parentheses,
REM %Q_OUTPUT%'s value is immediately expanded so it will still be available after endlocal
REM is called while still inside the brackets.  We set it immediately back to Q_OUTPUT and 
REM it is now in a global variable.
(
    endlocal 
    SET Q_OUTPUT=%Q_OUTPUT%
)

if not "%Q_OUTPUT%" == "" (
    REM pushd instead of CD so we can change drives and use network shares
    pushd %Q_OUTPUT%
    REM Clean up since our trick to pass through the endlocal boundary set a global
    SET Q_OUTPUT=
)

