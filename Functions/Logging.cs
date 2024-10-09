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
            string appendToFile;
            if (files.Count > 0)
            {
                
            }
        }
    }
}
