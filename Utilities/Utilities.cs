using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TheGrid
{
    public static class Extensions
    {
        /// <summary>
        /// Load all content within a certain folder. The function
        /// returns a dictionary where the file name, without type
        /// extension, is the key and the texture object is the value.
        ///
        /// The contentFolder parameter has to be relative to the
        /// game.Content.RootDirectory folder.
        /// </summary>
        /// <typeparam name="T">The content type.</typeparam>
        /// <param name="contentManager">The content manager for which content is to be loaded.</param>
        /// <param name="contentFolder">The game project root folder relative folder path.</param>
        /// <returns>A list of loaded content objects.</returns>
        public static List<KeyValuePair<string, T>> LoadContent<T>(this ContentManager contentManager, string contentFolder)
        {
            var directory = ReadManifest(contentManager);
            //Load directory info, abort if none
            //DirectoryInfo dir = new DirectoryInfo(contentManager.RootDirectory + "\\" + contentFolder);
            //if (!dir.Exists)
            //    throw new DirectoryNotFoundException();
            //Init the resulting list
            // Dictionary<String, T> result = new Dictionary<String, T>();
            var result = new List<KeyValuePair<String, T>>();

            //Load all files that matches the file filter
            List<string> list = directory.GetFilesInFolder(contentFolder);
            list.Sort();

            foreach (var filename in list)
            {
                result.Add(new KeyValuePair<string, T>(filename, contentManager.Load<T>(contentFolder + "\\" + filename)));
            }    //Return the result
            return result;
        }

        public static List<string> LoadContentList(this ContentManager contentManager, string contentFolder)
        {
            var directory = ReadManifest(contentManager);
            List<string> list = directory.GetFilesInFolder(contentFolder);
            list.Sort();
            List<string> result = new List<string>();
            foreach (var filename in list)
            {
                result.Add(contentFolder + "\\" + filename);
            }    //Return the result
            return result;

        }

        static object _lock = new object();
        static Directory ReadManifest(ContentManager content)
        {
            lock (_lock)
            {
                var root = new Directory();
                var filelist = content.Load<List<string>>("Manifest");
                foreach (var filename in filelist)
                {
                    root.Push(filename);
                }
                return root;
            }
        }

        public class Directory
        {
            public List<string> Filenames
            {
                get;
                private set;
            }

            public IDictionary<string, Directory> Children
            {
                get;
                private set;
            }

            public void Push(string filename)
            {
                int ich = filename.IndexOf('\\');
                if (ich > 0)
                    PushDirectory(filename);
                else
                    PushFile(filename);
            }

            private void PushFile(string filename)
            {
                if (Filenames == null)
                    Filenames = new List<string>();
                Filenames.Add(filename);
                Filenames.Sort();
            }

            private void PushDirectory(string filename)
            {
                int ich = filename.IndexOf('\\');
                var directoryName = filename.Substring(0, ich);
                if (Children == null)
                    Children = new Dictionary<string, Directory>();
                if (!Children.ContainsKey(directoryName))
                {
                    var directory = new Directory();
                    Children.Add(directoryName, directory);
                }
                Children[directoryName].Push(filename.Substring(ich + 1));                
            }


            internal List<string> GetFilesInFolder(string contentFolder)
            {
                Directory targetdir = this;
                foreach (var dirname in contentFolder.Split('\\'))
                {
                    targetdir = targetdir.Children[dirname];
                }
                return targetdir.Filenames;
            }
        }
    }
}
