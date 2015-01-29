# Waypoint
Waypoint is a simple aid for navigating the file-system from the Windows command line.

# Why I made it
If your workflow typically has a set of several directories you find yourself swapping between frequently you might use pushd and popd.  That's great for swapping between two directories but beyond that you end up scrolling through previous commands to find the right 'cd' you used previously. I do this all the time and so I wanted a handy list of my favourite directories I can swap between quickly with a few key strokes.

# How to use it
Build from source using a relatively recent version of Visual Studio (2012 and beyond are fine).  In the build output you will see w.bat and q.exe.  You will be using w.bat, or simply `w` from now on to invoke the tool.

  - Save the current working directory with `w s`
  - See your list of saved directories with `w`
  - Navigate to the desired directory with `w <number>` where `number` matches the entry number listed by running `w` with no parameters
  - Delete entries with `w c <number>` or all entries with `w c *`
  - Get help with `w /?`

# Why the need for an executable binary and a BATCH file?

I wanted no dependencies on other scripting engines so I chose BATCH. I did not want to manage  loading and saving lists of folder names from files with BATCH so I wrote the main logic in C#.  Applications started from the command line inherit the environment from the command line, but cannot persist changes to the current working directory back to the starting command line session. The BATCH file is thus needed to change the working directory returned by the managed executable.