using System.Text;

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
            WindowService.Instance.OpenErrorWindow(GetExceptionDetails(e).ToString());
        }

        /// <summary>
        /// Static method to recursively format a given <see cref="Exception"/>
        /// and all nested inner exceptions as a string.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private static string GetExceptionDetails(Exception e)
        {
            if (e == null) return string.Empty;

            var exceptionDetails = new StringBuilder();

            exceptionDetails.AppendLine($"Exception: {e.Message}");
            exceptionDetails.AppendLine();
            exceptionDetails.AppendLine($"Source: {e.Source}");
            exceptionDetails.AppendLine();
            exceptionDetails.AppendLine($"Type: {e.GetType()}");
            exceptionDetails.AppendLine();

            if (!string.IsNullOrEmpty(e.StackTrace))
            {
                exceptionDetails.AppendLine("StackTrace:");
                exceptionDetails.AppendLine(e.StackTrace);
            }
            if (e.InnerException != null)
            {
                exceptionDetails.AppendLine("Inner Exception:");
                exceptionDetails.AppendLine(GetExceptionDetails(e.InnerException));
                exceptionDetails.AppendLine();
            }

            return exceptionDetails.ToString();
        }
    }
}
