using System.Collections.Generic;
using System.IO;

namespace q
{
    class DirectoryList
    {
        List<string> m_directories = new List<string>();
        string m_directoryListFile;

        public DirectoryList(string directoryListFile)
        {
            m_directoryListFile = directoryListFile;
            if (File.Exists(directoryListFile))
            {
                LoadFromFile();
            }
        }

        private void LoadFromFile()
        {
            TextReader tr = File.OpenText(m_directoryListFile);
            
            while(true)
            {
                string newDirectory = tr.ReadLine();

                if (newDirectory == null)
                    break;
                m_directories.Add(newDirectory);
            }
            tr.Close();
        }

        private void SaveToFile()
        {
            File.WriteAllLines(m_directoryListFile, m_directories);
        }

        public void AddPath(string path)
        {
            if (!m_directories.Contains(path))
            {
                m_directories.Add(path);
                SaveToFile();
            }
        }

        public void DeleteAllPaths()
        {
            m_directories = new List<string>();
            SaveToFile();
        }

        public void DeletePath(int index)
        {
            m_directories.RemoveAt(index);
            SaveToFile();
        }

        public List<string> Directories
        {
            get
            {
                return m_directories;
            }
        }
    }
}
