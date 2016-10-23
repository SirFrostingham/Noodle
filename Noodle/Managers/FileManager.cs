using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Noodle.Models;
using Formatting = System.Xml.Formatting;

namespace Noodle.Managers
{
    public class FileManager
    {

        public const string JobsFilename = @"_{0}_jobs.json";

        public void DeletePreviousJobsFile()
        {
            var file = string.Format(JobsFilename, "Jobs");

            //delete previous file
            File.Delete(file);
        }

        public void GetJobsProcessSaveJobsToDisk(List<Job> jobs)
        {
            var file = string.Format(JobsFilename, "Jobs");

            var fs = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);

            using (fs)
            {
                WriteJobsStreamJson(jobs, fs);
            }
        }

        public void SaveSingleJobToDisk(Job job)
        {
            if (Globals.Current.Jobs == null)
                Globals.Current.Jobs = new List<Job>();

            if (Globals.Current.Jobs.Count == 0)
                GetJobs();

            var dupeFound = false;

            foreach (var singleJob in Globals.Current.Jobs)
            {
                if (job.Command == singleJob.Command)
                {
                    dupeFound = true;
                    break;
                }
            }

            if (!dupeFound)
            {
                //Add
                Globals.Current.Jobs.Add(job);
            }
            else
            {
                //Edit
                var newList = new List<Job>();
                foreach (var cJob in Globals.Current.Jobs)
                {
                    newList.Add(cJob.Command == job.Command ? job : cJob);
                }

                Globals.Current.Jobs = newList;
            }

            SaveJobsToDisk();
        }

        public static void SaveJobsToDisk()
        {
            if (Globals.Current.Jobs == null) return;

            FileStream fs;

            var file = string.Format(JobsFilename, "Jobs");

            //delete previous file
            File.Delete(file);

            using (fs = File.Open(file, FileMode.OpenOrCreate))
            {
                WriteJobsStreamJson(Globals.Current.Jobs, fs);
            }
        }

        private static void WriteJobsStreamJson(List<Job> jobs, FileStream fs)
        {
            //TODO: Does this need to be async?  If so, uncomment this:
            //App.Current.Dispatcher.Invoke((Action)delegate // <--- HERE
            //{
            //    using (var sw = new StreamWriter(fs))
            //    using (JsonWriter jw = new JsonTextWriter(sw))
            //    {
            //        jw.Formatting = (Newtonsoft.Json.Formatting)Formatting.Indented;
            //        var serializer = new JsonSerializer();
            //        serializer.Serialize(jw, jobs.ToList());
            //        //jw.WriteRaw(result);
            //    }
            //});

            using (var sw = new StreamWriter(fs))
            using (JsonWriter jw = new JsonTextWriter(sw))
            {
                jw.Formatting = (Newtonsoft.Json.Formatting)Formatting.Indented;
                var serializer = new JsonSerializer();
                serializer.Serialize(jw, jobs.ToList());
            }
        }

        public static void GetJobs()
        {
            var file = string.Format(JobsFilename, "Jobs");

            if (File.Exists(file))
            {
                Globals.Current.Jobs = ReadJobsStreamJson(file);
            }

            if (Globals.Current.Jobs == null)
                Globals.Current.Jobs = new List<Job>();
        }

        private static List<Job> ReadJobsStreamJson(string fs)
        {
            var jobs = new List<Job>();
            using (var r = new StreamReader(fs))
            {
                string json = r.ReadToEnd();
                jobs = JsonConvert.DeserializeObject<List<Job>>(json);
            }
            return jobs;
        }
    }
}
