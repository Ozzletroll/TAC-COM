
namespace TAC_COM.Services
{
    /// <summary>
    /// Class responsible for formatting and showing error message dialogs.
    /// </summary>
    public class DebugService
    {
        /// <summary>
        /// Static method to format a given Exception and display it as
        /// an error message dialog.
        /// </summary>
        /// <param name="e"> The <see cref="Exception"/> to format and display.</param>
        public static void ShowErrorMessage(Exception e)
        {
            Dictionary<string, object> errorDict = [];

            Dictionary<string, string?> outerExceptionDict = new()
            {
                { "Message", e.Message },
                { "Source", e.Source },
                { "Target Site", e.TargetSite?.ToString() },
                { "Stack Trace", e.StackTrace },
            };

            errorDict.Add("Exception", outerExceptionDict);

            Exception? currentException = e.InnerException;
            int innerLevel = 1;
            while (currentException != null)
            {
                Dictionary<string, string?> innerExceptionDict = new()
                {
                    { "Message", e.Message },
                    { "Source", e.Source },
                    { "Target Site", e.TargetSite?.ToString() },
                    { "Stack Trace", e.StackTrace },
                };

                errorDict.Add($"Inner Exception {currentException}", innerExceptionDict);

                currentException = currentException.InnerException;
                innerLevel++;
            }

            WindowService.Instance.OpenErrorWindow(errorDict);
        }
    }
}
