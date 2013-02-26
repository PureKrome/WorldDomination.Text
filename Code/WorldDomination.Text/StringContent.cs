using System;
using System.Collections.Generic;
using System.Linq;

namespace WorldDomination.Text
{
    public class StringContent : IStringContent
    {
        public StringContent() : this(null)
        {
        }

        public StringContent(IList<string> phraseList)
        {
            // Optional.
            PhraseList = phraseList;
        }

        public IList<string> PhraseList { get; private set; }

        public IList<FoundPhrase> PhrasesThatExist(string content, IList<string> phraseList = null)
        {
            if (string.IsNullOrEmpty(content))
            {
                throw new ArgumentNullException("content");
            }

            if ((PhraseList == null ||
                PhraseList.Count <= 0) &&
                (phraseList == null ||
                phraseList.Count <= 0))
            {
                throw new InvalidOperationException(
                    "Both the PhraseList (class property) and the phraseList (argument) are null or empty. We need at least -some- phrase list, before we can find all phrases within the content. Please provide either/or.");
            }

            // Remove all silly characters that could mess shit up.
            var cleanedContent = content.Clean();

            var results = new List<FoundPhrase>();

            // Find any bad words that exist in this cleaned content.
            foreach (var phrase in (phraseList != null ? phraseList.AsParallel() : PhraseList.AsParallel()))
            {
                int index = 0;
                while ((index = (cleanedContent.IndexOf(phrase, index, StringComparison.InvariantCultureIgnoreCase))) >= 0)
                {
                    // "How quickly can I get a passport as I need white to travel overseas ass crapasshat in the next fuck 2 wks for business?";

                    // We have found an offending phrase .. but it is 'stand alone' ?
                    // Check the characters to 'either side' of the content where the phrase is found. 
                    // If it's letter/digit then it's considered NOT FOUND.
                    var ri = index + phrase.Length + 1 <= cleanedContent.Length;
                    var right = char.IsLetterOrDigit(cleanedContent[index + phrase.Length]);
                    var li = index > 0;
                    var left = char.IsLetterOrDigit(cleanedContent[index - 1]);
                    if ((index > 0 && !char.IsLetterOrDigit(cleanedContent[index - 1])) &&
                         (index + phrase.Length <= cleanedContent.Length && !char.IsLetterOrDigit(cleanedContent[index + phrase.Length])))
                    {
                        // Phrase is stand alone. so record this, please.
                        results.Add(new FoundPhrase(phrase, index));
                    }

                    
                    
                    index = index + phrase.Length;
                }
            }

            return results.Count <= 0 ? null : results;
        }
    }
}