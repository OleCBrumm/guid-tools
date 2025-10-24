using System;
using System.Linq;

namespace SitecoreIdConverter
{
    /// <summary>
    /// Handles GUID processing and conversion logic
    /// </summary>
    public static class GuidProcessor
    {
        /// <summary>
        /// Processes input text and converts it to GUID format
        /// </summary>
        public static Guid? ProcessInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return null;

            try
            {
                string cleanId = CleanInput(input);
                
                if (cleanId.IsValidGuid())
                {
                    return new Guid(cleanId);
                }
            }
            catch
            {
                // Invalid GUID format
            }
            
            return null;
        }

        /// <summary>
        /// Removes all non-alphanumeric characters and converts to lowercase
        /// </summary>
        public static string CleanInput(string input) =>
            new string(input.ToLower()
                .Where(char.IsLetterOrDigit).ToArray());
    }
}
