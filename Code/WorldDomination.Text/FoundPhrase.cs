using System;

namespace WorldDomination.Text
{
    public class FoundPhrase
    {
        public FoundPhrase(string phrase, int indexOn)
        {
            if (string.IsNullOrEmpty(phrase))
            {
                throw new ArgumentNullException("phrase");
            }

            if (indexOn < 0)
            {
                throw new ArgumentOutOfRangeException("indexOn");
            }

            Phrase = phrase;
            IndexOn = indexOn;
        }

        public string Phrase { get; private set; }
        public int IndexOn { get; private set; }
    }
}