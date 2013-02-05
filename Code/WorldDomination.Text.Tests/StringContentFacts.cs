using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace WorldDomination.Text.Tests
{
    // ReSharper disable InconsistentNaming

    public class StringContentFacts
    {
        public class PhrasesThatExistFacts
        {
            public IList<string> PhraseList
            {
                get
                {
                    return new List<string>
                           {
                               "able-bodied",
                               "adult community",
                               "adult living",
                               "adult park",
                               "adult only",
                               "african",
                               "aids",
                               "alchoholics",
                               "american indians",
                               "asian",
                               "racist",
                               "white"
                           };
                }
            }

            [Fact]
            public void GivenSomeContentWithBadPhrases_PhrasesThatExist_ReturnsAListOfBadPhrasesFound()
            {
                // Arrange.
                const string content =
                    "This is a bad review. I noticed that there were a lot of white people living in Ivanhoe. This doesn't mean this is a racist suburb, though.";

                var stringContent = new StringContent(PhraseList);

                // Act.
                var results = stringContent.PhrasesThatExist(content);

                // Assert.
                Assert.NotNull(results);
                Assert.NotEmpty(results);
                Assert.Equal(2, results.Count);
            }

            [Fact]
            public void GivenSomeContentWithBadPhrasesSuppliedSeparately_PhrasesThatExist_ReturnsAListOfBadPhrasesFound()
            {
                // Arrange.
                const string content =
                    "This is a bad review. I noticed that there were a lot of white people living in Ivanhoe. This doesn't mean this is a racist suburb, though.";

                var stringContent = new StringContent();

                // Act.
                var results = stringContent.PhrasesThatExist(content, PhraseList);

                // Assert.
                Assert.NotNull(results);
                Assert.NotEmpty(results);
                Assert.Equal(2, results.Count);
            }

            [Fact]
            public void GivenSomeContentWithNoBadPhrases_PhrasesThatExist_ReturnsANullList()
            {
                // Arrange.
                const string content =
                    "foo  1 12l3j1kl;j434;5 3klkrj 987*&^897^DF hsfd l;ajf;o2u3r8fjdasf jdsajf ;aljsfj asf jas;df 8asuf 80sdf 9d (& 8H ";

                var stringContent = new StringContent(PhraseList);

                // Act.
                var results = stringContent.PhrasesThatExist(content);

                // Assert.
                Assert.Null(results);
            }

            [Fact]
            public void GivenSomeContentWithNoBadWordsAndBadPhrasesSuppliedSeparately_PhrasesThatExist_ReturnsAListOfBadPhrasesFound()
            {
                // Arrange.
                const string content =
                    "sdhfasjdfh sadfo8as 68sa6t &%7tsTSOtafdsf dsakf haspdf y78 6* ";

                var stringContent = new StringContent();

                // Act.
                var results = stringContent.PhrasesThatExist(content, PhraseList);

                // Assert.
                Assert.Null(results);
            }

            [Fact]
            public void GivenSomeContentButNoPhraseList_PrasesThatExist_ThrowsAnException()
            {
                // Arrange.
                var stringContent = new StringContent();

                // Act & Assert.
                var result = Assert.Throws<InvalidOperationException>(() => stringContent.PhrasesThatExist("foo"));
                Assert.Equal("Both the PhraseList (class property) and the phraseList (argument) are null or empty. We need at least -some- phrase list, before we can find all phrases within the content. Please provide either/or.", result.Message);
            }
        }
    }

    // ReSharper restore InconsistentNaming
}