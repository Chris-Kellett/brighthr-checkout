using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckoutClassLibrary
{
    public static class Logging
    {
        /// <summary>
        /// Log a Debug message, written out only in non-Live environments.
        /// </summary>
        /// <param name="message">Log Message</param>
        public static void Debug(string message)
        {
            PerformLogging(LogLevel.Debug, message);
        }

        /// <summary>
        /// Log an Event
        /// </summary>
        /// <param name="message">Log Message</param>
        public static void Event(string message)
        {
            PerformLogging(LogLevel.Event, message);
        }

        /// <summary>
        /// Log an instance of an Error
        /// </summary>
        /// <param name="ex">The thrown Exception</param>
        public static void Error(Exception ex)
        {
            PerformLogging(LogLevel.Error, ex.ToString());
        }

        /// <summary>
        /// Private to the Logging class, used to execute the logging of the message and perform any
        /// other required tasks associated with the Logging.
        /// </summary>
        /// <param name="logLevel">Enum of the Logging Level</param>
        /// <param name="message">The Log Message</param>
        private static void PerformLogging(LogLevel logLevel, string message)
        {
            // Format the Logging entry
            string loggingLine = $"{DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff")} [{logLevel.ToString().ToUpper()}] :: {message}";     

            // Write the Logging entry to the Console
            if (Config.DebugMode)
            {
                Console.WriteLine(loggingLine);
            }

            // Ensure the LogLocation exists
            if (!Directory.Exists(Config.LogLocation))
            {
                try
                {
                    Directory.CreateDirectory(Config.LogLocation);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"CRITICAL LOGGING ERROR: {ex.ToString()}");
                    return; // If we can't write to the LogLocation, back out of further processing.
                }
            }

            // Write to the LogLocation file
            WriteToLogFile(loggingLine);
        }

        private static void WriteToLogFile(string loggingLine)
        {
            // First we need to obtain a FileInfo list of every file currently in the LogLocation. Unfortauntely we cannot directly 
            // obtain a list of FileInfo objects via a LINQ Query, and we need the FileInfo object to determine the last modified time
            // of the file.
            List<FileInfo> files = new();
            try
            {
                string[] fileNames = Directory.GetFiles(Config.LogLocation);
                foreach (string file in fileNames)
                {
                    files.Add(new FileInfo(file));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CRITICAL LOGGING ERROR: {ex.ToString()}");
                return; // An error here means we can't continue processing, exit the function.
            }

            // Now we need to work out whether we want to append the LoggingLine to the most recent logging file, or create a new one.
            string appendToFile = "";
            if (files.Count > 0)
            {
                // Get the most recently written-to file.
                FileInfo? recentFile = files.OrderByDescending(f => f.LastWriteTime).FirstOrDefault();
                if (recentFile != null)
                {
                    // Check whether the FileSize of the most recent file is within our limits. If it IS we can append to this file.
                    if (recentFile.Length <= Config.MaxLogFileSizeMb * 1024 * 1024)
                    {
                        appendToFile = recentFile.FullName;
                    }
                }
            }

            // If by this point we don't have a value in "appendToFile", we need to write to a new file. Create the File Name.
            if (appendToFile == "")
            {
                appendToFile = $"{Config.LogLocation}{Config.LogFileName}_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.txt";
            } 
            else
            {
                // As this is an existing file, we want to make sure it's written on a new line.
                loggingLine = Environment.NewLine + loggingLine;
            }

            // Now we have a file to write to, we can append our log to the end
            try
            {
                // Use StreamWriter in append mode
                using (StreamWriter writer = new StreamWriter(appendToFile, true))
                {
                    writer.WriteLine(loggingLine); 
                }
            }
            catch (Exception ex)
            {
                // No real need to return; here as the work has already concluded. Though we should notify Debugging users that the 
                // write to a log file failed.
                Console.WriteLine($"CRITICAL LOGGING ERROR: {ex.ToString()}");
            }
        }
    }
}
