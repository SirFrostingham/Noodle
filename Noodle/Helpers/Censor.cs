using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using NLog.Fluent;
using Noodle.Annotations;

namespace Noodle.Helpers
{
    public static class Censor
    {
        private static readonly List<string> _allWords = new List<string>();
        private static readonly List<string> _hardStopWords = new List<string>();

        // Escaped characters for Regex.Match and Regex.Replace
        private static readonly List<string> _specialCharsForRegex = new List<string> { " ", "`", "~", "\\!", "\\@", "#", "\\$", "\\%", "\\^", "&", "\\*", "\\(", "\\)", "_", "'", "\\-", "=", "{", "}", "\\|", "\\:", "\"", "<", ">", "\\?", "\\[", "\\]", "\\\\", "\\;", "'", "\\,", "\\.", "\\/" };

        // Non-escaped special characters for string matching
        private static readonly List<string> _specialCharsForString = new List<string> { " ", "`", "~", "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "_", "'", "-", "=", "{", "}", "|", ":", "\"", "<", ">", "?", "[", "]", "\\", ";", "'", ",", ".", "/" }; 

        static Censor()
        {
            //all words
            _allWords = CensorWords.De;

            if(LoadLowSeverityWords)
                _allWords.AddRange(CensorWords.En);

            _allWords.AddRange(CensorWords.Es);
            _allWords.AddRange(CensorWords.Fr);
            _allWords.AddRange(CensorWords.Hi);
            _allWords.AddRange(CensorWords.It);
            _allWords.AddRange(CensorWords.Ja);
            _allWords.AddRange(CensorWords.Nl);
            _allWords.AddRange(CensorWords.Pt);
            _allWords.AddRange(CensorWords.Ru);
            _allWords.AddRange(CensorWords.Zh);

            //hard stop words
            _hardStopWords = CensorWords.HardStopWords;
        }

        public static bool LoadLowSeverityWords { get; set; }

        /// <summary>
        /// Determines whether [is profanity free] [the specified STR].
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns>
        ///   <c>true</c> if [is profanity free] [the specified STR]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInappropriate(string str)
        {
            if (string.IsNullOrEmpty(str))
                return false;

            List<string> foundWords = new List<string>();

            var strToCheck = " " + str + " ";
            
            //check all words/phrases/patterns
            foreach (var censoredWord in _allWords)
            {
                var isSpecialCharBefore = false;
                var isSpecialCharAfter = false;

                // basic word check
                if (strToCheck.ToLowerInvariant().IndexOf(censoredWord, System.StringComparison.Ordinal) > -1)
                {
                    //check for special character before
                    foreach (var spec in _specialCharsForString)
                    {
                        if (strToCheck.ToLowerInvariant().IndexOf(spec + censoredWord, System.StringComparison.Ordinal) > -1)
                        {
                            isSpecialCharBefore = true;
                            break;
                        }
                    }

                    //check for special character after
                    foreach (var spec in _specialCharsForString)
                    {
                        if (strToCheck.ToLowerInvariant().IndexOf(censoredWord + spec, System.StringComparison.Ordinal) > -1)
                        {
                            isSpecialCharAfter = true;
                            break;
                        }
                    }
                }

                // only add if specialChar before AND after
                if (isSpecialCharBefore && isSpecialCharAfter)
                {
                    foundWords.Add(censoredWord);
                }
            }

            //check without punctuation
            var strWithoutPunctuation = RemoveSpecialCharacters(strToCheck);

            //check hard stop words
            foreach (var hardStopWord in _hardStopWords)
            {
                if (strWithoutPunctuation.IndexOf(hardStopWord, System.StringComparison.Ordinal) > -1)
                {
                    foundWords.Add(hardStopWord);
                }
            }

            //remove dupes
            var foundWordsWithoutDupes = foundWords.Distinct().ToList();

            if (foundWordsWithoutDupes.Count > 0)
                Log.Instance.Trace("\r\n*** String under test: {0} \r\n*** Inappropriate words found: {1}", str, string.Join(",", foundWords));

            // if inappropriate words are found, return true
            return foundWordsWithoutDupes.Count > 0;
        }

        /// <summary>
        /// Censors the text.  Example: This is a test ****.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static string CensorText(string str)
        {
            if (string.IsNullOrEmpty(str))
                return null;

            List<string> foundWords = new List<string>();

            var censoredText = str;

            var isInappropriate = IsInappropriate(censoredText);

            //filter all words/phrases/patterns first
            if (isInappropriate)
            {
                foreach (string censoredWord in _allWords)
                {
                    string regularExpression = ToRegexPattern(censoredWord);

                    // check for preceding special character
                    censoredText = Regex.Replace(censoredText, regularExpression, StarCensoredMatch,
                                                 RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
                }
            }

            //remove dupes
            var foundWordsWithoutDupes = foundWords.Distinct().ToList();

            if (foundWordsWithoutDupes.Count > 0)
                Log.Instance.Trace("\r\n*** String under test: {0} \r\n*** Inappropriate words found: {1}", str, string.Join(",", foundWords));

            return censoredText;
        }

        /// <summary>
        /// Removes the profanity from text.  Example: This is a test     .
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static string RemoveFromText(string str)
        {
            if (string.IsNullOrEmpty(str))
                return null;

            List<string> foundWords = new List<string>();

            var censoredText = str;

            var isInappropriate = IsInappropriate(censoredText);

            //filter all words/phrases/patterns first
            if (isInappropriate)
            {
                foreach (string censoredWord in _allWords)
                {
                    string regularExpression = ToRegexPattern(censoredWord);

                    // check for preceding special character
                    censoredText = Regex.Replace(censoredText, regularExpression, RemoveCensoredMatch,
                                                 RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
                }
            }

            //remove dupes
            var foundWordsWithoutDupes = foundWords.Distinct().ToList();

            if (foundWordsWithoutDupes.Count > 0)
                Log.Instance.Trace("\r\n*** String under test: {0} \r\n*** Inappropriate words found: {1}", str, string.Join(",", foundWords));

            return censoredText;
        }

        private static string StarCensoredMatch(Match m)
        {
            string word = m.Captures[0].Value;

            var wordWithoutSpecialChars = string.Empty;

            var wordUnderTest = new List<string>();
            wordUnderTest.Add(word);

            foreach (var specialChar in _specialCharsForString)
            {
                // word index
                wordWithoutSpecialChars = wordUnderTest.Last().TrimStart(specialChar[0]).TrimEnd(specialChar[0]).Trim();

                // increment index (list) to keep the last word as the "word under test"
                if (wordWithoutSpecialChars != wordUnderTest.Last())
                    wordUnderTest.Add(wordWithoutSpecialChars);
            }

            return word.Replace(wordWithoutSpecialChars, new string('*', wordWithoutSpecialChars.Length));
        }

        private static string RemoveCensoredMatch(Match m)
        {
            string word = m.Captures[0].Value;

            var wordWithoutSpecialChars = string.Empty;

            var wordUnderTest = new List<string>();
            wordUnderTest.Add(word);

            foreach (var specialChar in _specialCharsForString)
            {
                // word index
                wordWithoutSpecialChars = wordUnderTest.Last().TrimStart(specialChar[0]).TrimEnd(specialChar[0]).Trim();

                // increment index (list) to keep the last word as the "word under test"
                if (wordWithoutSpecialChars != wordUnderTest.Last())
                    wordUnderTest.Add(wordWithoutSpecialChars);
            }

            return word.Replace(wordWithoutSpecialChars, new string(' ', wordWithoutSpecialChars.Length));
        }
        
        private static string ToRegexPattern(string wildcardSearch)
        {
            string regexPattern = Regex.Escape(wildcardSearch);
            
            regexPattern = regexPattern.Replace(@"\*", ".*?");
            regexPattern = regexPattern.Replace(@"\?", ".");
            
            if (regexPattern.StartsWith(".*?"))
            {
                regexPattern = regexPattern.Substring(3);
                regexPattern = @"(^\b)*?" + regexPattern;
            }
            
            regexPattern = @"\b" + regexPattern + @"\b";
            
            return regexPattern;    
        }

        private static string RemoveSpecialCharacters(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                // leaving some punctuation, because these could be valid sentences.
                // i.e. "I went to the car waSH.  IT was fun!"
                //      "I like candy, BUT Today I hate candy."      <-- This is not on the HardStopWord list
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == ',')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }
}