using System.Collections.Generic;

namespace WorldDomination.Text
{
    public interface IStringContent
    {
        List<string> PhrasesThatExist(string content, IList<string> phraseList = null);
    }
}