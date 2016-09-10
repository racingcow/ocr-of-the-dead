using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = System.Random;
using Racingcow.OcrOfTheDead.Models;

namespace Assets.Scripts.WordSources
{
    public class FileWordSource : IWordsSource
    {
        public event RetrieveWordsCompletedEventHandler RetrieveWordsCompleted;

        public void BeginRetrieveWords(int count)
        {
            var directory = new DirectoryInfo(Path.Combine(Application.dataPath, "Images/Words"));
            var files = directory.GetFiles("*.png");
            
            var rand = new Random();
            var words = new List<IWord>();
            for (var i = 0; i < count; i++)
            {
                var file = files[rand.Next(0, files.Length)];

                var fileName = Path.GetFileNameWithoutExtension(file.Name);
                var tokens = fileName.Split('_');
                var wordText = tokens[0];
                var width = tokens[1];
                var height = tokens[2];
                
                words.Add(new Word
                {
                    Id = file.FullName,
                    Value = wordText,
                    Image = File.ReadAllBytes(file.FullName),
                    ImageWidth = Convert.ToInt32(width),
                    ImageHeight = Convert.ToInt32(height)
                });
            }

            if (RetrieveWordsCompleted != null)
            {
                RetrieveWordsCompleted(this, new RetrieveWordsCompletedEventArgs
                {
                    Words = words.ToArray()
                });
            }
        }
    }
}
