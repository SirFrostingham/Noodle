using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Formatting = System.Xml.Formatting;

namespace ParseToJsonModel
{
    class Program
    {
        //TODO: NOTE THE FILE USED FOR IMPORT IS ATTACHED TO THIS PROJECT
        public static string FileNameInput = @"C:\Users\areed\Downloads\NRC-Emotion-Lexicon-v0.92\NRC-Emotion-Lexicon-v0.92\NRC-emotion-lexicon-wordlevel-alphabetized-v0.92_FOR_EXPORT.txt";
        public static string FileNameOutput = @"serializedJsonModel.json";
        public static List<WordModel> WordModels { get; set; }

        static void Main(string[] args)
        {
            WordModels = new List<WordModel>();

            ReadInFile();

            FileStream fs;
            using (fs = File.Open(FileNameOutput, FileMode.OpenOrCreate))
            {
                WriteJobsStreamJson(WordModels,fs);
            }
        }

        private static void ReadInFile()
        {
            FileStream fs;
            
            string lineOfText = string.Empty;

            using (fs = File.Open(FileNameInput, FileMode.Open))
            {
                var fileStreamReader = new System.IO.StreamReader(fs, System.Text.Encoding.UTF8, true, 128);

                while ((lineOfText = fileStreamReader.ReadLine()) != null)
                {
                    string[] values = lineOfText.Split('\t').Select(sValue => sValue.Trim()).ToArray();

                    SaveSingleWordLineToModelList(values);
                }
            }
        }

        public static void SaveSingleWordLineToModelList(string[] wordLine)
        {
            var word = wordLine[0];
            var emotionOrAlignment = wordLine[1];
            var score = wordLine[2];

            var dupeFound = false;

            foreach (var singleWordModel in WordModels)
            {
                if (word == singleWordModel.Word)
                {
                    dupeFound = true;
                    break;
                }
            }

            if (!dupeFound)
            {
                var model = emotionOrAlignment.ToLowerInvariant().Contains("positive") ||
                            emotionOrAlignment.ToLowerInvariant().Contains("negative")
                    ? new WordModel
                    {
                        Word = word,
                        Alignments =
                            new List<AlignmentScoreModel>
                            {
                                new AlignmentScoreModel
                                {
                                    Alignment = ParseEnum<BrainEnums.Alignment>(emotionOrAlignment),
                                    Score = Convert.ToInt32(score)
                                }
                            }
                    }
                    : new WordModel
                    {
                        Word = word,
                        Emotions =
                            new List<EmotionalScoreModel>
                            {
                                new EmotionalScoreModel
                                {
                                    Emotion = ParseEnum<BrainEnums.Emotion>(emotionOrAlignment),
                                    Score = Convert.ToInt32(score)
                                }
                            }
                    };

                //Add
                WordModels.Add(model);
            }
            else
            {
                //Edit
                var newList = new List<WordModel>();
                foreach (var wordModel in WordModels)
                {
                    if (wordModel.Word == word)
                    {
                        if (emotionOrAlignment.ToLowerInvariant().Contains("positive") ||
                            emotionOrAlignment.ToLowerInvariant().Contains("negative"))
                        {
                            wordModel.Alignments.Add(
                                        new AlignmentScoreModel
                                        {
                                            Alignment = ParseEnum<BrainEnums.Alignment>(emotionOrAlignment),
                                            Score = Convert.ToInt32(score)
                                        });
                        }
                        else
                        {
                            wordModel.Emotions.Add(
                                        new EmotionalScoreModel
                                        {
                                            Emotion = ParseEnum<BrainEnums.Emotion>(emotionOrAlignment),
                                            Score = Convert.ToInt32(score)
                                        });
                        }
                    }
                    newList.Add(wordModel);
                }

                WordModels = newList;
            }
        }

        private static void WriteJobsStreamJson(List<WordModel> model, FileStream fs)
        {
            using (var sw = new StreamWriter(fs))
            using (JsonWriter jw = new JsonTextWriter(sw))
            {
                jw.Formatting = (Newtonsoft.Json.Formatting)Formatting.Indented;
                var serializer = new JsonSerializer();
                serializer.Serialize(jw, model.ToList());
            }
        }
        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), UppercaseFirst(value), true);
        }
        static string UppercaseFirst(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }
    }
}
