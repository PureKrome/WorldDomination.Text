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

            var results = new List<FoundPhrase>();

            // Find any bad words that exist in this cleaned content.
            foreach (var phrase in (phraseList != null ? phraseList.AsParallel() : PhraseList.AsParallel()))
            {
                int index = 0;
                while ((index = (content.IndexOf(phrase, index, StringComparison.InvariantCultureIgnoreCase))) >= 0)
                {
                    // "How quickly can I get a passport as I need white to travel overseas ass crapasshat in the next fuck 2 wks for business?";

                    // We have found an offending phrase .. but it is 'stand alone' ?
                    // Check the characters to 'either side' of the content where the phrase is found. 
                    // If it's letter/digit then it's considered NOT FOUND.
                    // NOTE: We're only testing characters to either side of the found word -IF- there is some character before or after.
                    //       This handles words found at the start and/or end or if the content = the phrase.
                    var hasCharactersAfterWord = index + phrase.Length <= content.Length - 1;
                    var isNextCharacterALetterOrDigit = hasCharactersAfterWord && char.IsLetterOrDigit(content[index + phrase.Length]);
                    var hasCharacterBeforeWord = index > 0;
                    var isPreviousCharacterALetterOrDigit = hasCharacterBeforeWord && char.IsLetterOrDigit(content[index - 1]);
                    if ( 
                        (!hasCharacterBeforeWord || !isPreviousCharacterALetterOrDigit) &&
                        (!hasCharactersAfterWord || !isNextCharacterALetterOrDigit)
                        )
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