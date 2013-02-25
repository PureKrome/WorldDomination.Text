using System.Collections.Generic;

namespace WorldDomination.Text
{
    public interface IStringContent
    {
        IList<FoundPhrase> PhrasesThatExist(string content, IList<string> phraseList = null);
    }
}