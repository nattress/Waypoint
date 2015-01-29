using System;
using System.Reflection;
using System.IO;

namespace q
{
    class Program
    {
        const string LIST_FILE = "directories.txt";
        const string MAGIC_VALUE = "magicwdotbat";

        static void Write(string s)
        {
            Console.WriteLine(":" + s);
        }

        static void NoWDotBat()
        {
            Console.WriteLine("You must call this tool from its companion, w.bat.");
        }

        static void ShowUsage()
        {
            Write("w.bat [command|index]");
            Write("Stores a list of directories, allowing you to easily move");
            Write("around the file-system.");
            Write("Commands:");
            Write("w\t\tLists saved directories with their index numbers");
            Write("w s\t\tStores current directory");
            Write("w <index>\tPushd's to the directory at index <index>");
            Write("w c *\t\tDeletes all saved directories");
            Write("w c <index>\tDeletes directory at index <index>");
        }

        static void Main(string[] args)
        {
            string appPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);
            DirectoryList dl = new DirectoryList(Path.Combine(appPath, LIST_FILE));

            if (args.Length == 0)
            {
                NoWDotBat();
                return;
            }

            if (!args[0].Equals(MAGIC_VALUE))
            {
                NoWDotBat();
                return;
            }

            if (args.Length == 1)
            {
                if (dl.Directories.Count == 0)
                {
                    // No directories - show usage to be helpful to new users
                    ShowUsage();
                }
                else
                {
                    for (int i = 1; i <= dl.Directories.Count; i++)
                    {
                        Write(i + "\t" + dl.Directories[i - 1]);
                    }
                }
            } else if (args[1].Equals("s",StringComparison.OrdinalIgnoreCase))
            {
                dl.AddPath(Environment.CurrentDirectory);
            }
            else if (args[1].Equals("c", StringComparison.OrdinalIgnoreCase))
            {
                if (args.Length == 3)
                {
                    if (args[2].Equals("*"))
                    {
                        dl.DeleteAllPaths();
                    }
                    else
                    {
                        int index = 0;
                        if (!int.TryParse(args[2], out index))
                        {
                            Write("Specify a path index, or * for all paths");
                        }
                        else
                        {
                            if (index < 1 || index > dl.Directories.Count)
                            {
                                Write("Invalid index");
                            }
                            else
                            {
                                dl.DeletePath(index - 1);
                            }
                        }
                    }
                }
                else
                {
                    Write("Specify a path index, or * for all paths");
                }
            }
            else if (args[1].Equals("/?"))
            {
                ShowUsage();
            }
            else
            {
                // Default case, see if user entered a number to go to.
                int index = 0;
                if (!int.TryParse(args[1], out index))
                {
                    Write("Specify a path index");
                }
                else
                {
                    if (index < 1 || index > dl.Directories.Count)
                    {
                        Write("Invalid index");
                    }
                    else
                    {
                        Write("*" + dl.Directories[index - 1]);
                    }
                }
            }
        }
    }
}
