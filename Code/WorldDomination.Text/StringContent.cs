using System;
using System.Collections.Generic;
using System.Linq;

namespace WorldDomination.Text
{
    public class StringContent
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

        public List<string> PhrasesThatExist(string content, IList<string> phraseList = null)
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

            // Find any bad words that exist in this cleaned content.
            var results = new List<string>();
            foreach (var phrase in (phraseList ?? PhraseList)
                .Where(phrase => cleanedContent.IndexOf(phrase, StringComparison.InvariantCultureIgnoreCase) >= 0)
                .Where(phrase => !results.Contains(phrase)))
            {
                results.Add(phrase);
            }

            return results.Count <= 0 ? null : results;
        }
    }
}