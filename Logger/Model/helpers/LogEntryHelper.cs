namespace Logger.Model.helpers
{
    using System;
    using System.Text;

    public class LogEntryHelper
    {

        /// <summary>
        /// Return a formatted log message.
        /// </summary>
        /// <returns></returns>
        public string formatLogMessage(string logMessage)
        {
            // 
            StringBuilder sb = new StringBuilder();

            if ( !String.IsNullOrEmpty(logMessage) )
            {
                sb.Append(logMessage).Append(". ");
            }
            return sb.ToString();

        }

    }

}