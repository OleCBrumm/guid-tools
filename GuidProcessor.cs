using System;
using System.Linq;

namespace IdConverter
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
                    // Pad 8-character small format with zeros to create full GUID
                    if (cleanId.Length == 8)
                    {
                        cleanId = cleanId.PadRight(32, '0');
                    }

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
            new string(input.Where(char.IsLetterOrDigit).ToArray());
    }
}
