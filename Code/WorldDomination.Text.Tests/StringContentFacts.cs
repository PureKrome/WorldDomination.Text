using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                               "white",
                               "ass",
                               "fuck"
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
                    "This is a bad review. &^*(sd79ad hskah sad (*&(&97s a9 I noticed adult living that there were a lot of white people living in Ivanhoe. This doesn't mean this is a racist suburb, though.";

                var stringContent = new StringContent();

                // Act.
                var results = stringContent.PhrasesThatExist(content, PhraseList);

                // Assert.
                Assert.NotNull(results);
                Assert.NotEmpty(results);
                Assert.Equal(3, results.Count);
                Assert.Equal("adult living", results.First().Phrase);
                Assert.Equal(65, results.First().IndexOn);
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

            [Fact]
            public void GivenSomeContentThatContainsAPhraseInsideAWord_PhrasesThatExist_ReturnsNull()
            {
                // Arrange.
                // NOTE: phrase 'ass' exists but it's inside a word.
                const string content = "How quickly can I get a passport as I need to travel overseas in the next 2 wks for business?";
                var stringContent = new StringContent(PhraseList);

                // Act.
                var results = stringContent.PhrasesThatExist(content);

                // Assert.
                Assert.Null(results);
            }

            [Fact]
            public void GivenSomeContentThatContainsTheMultipleSamePhrases_PhrasesThatExist_ReturnsSomeFoundPhrases()
            {
                // Arrange.
                const string content = "How quickly can I get a passport as I need white to travel overseas ass crapasshat in the next fuck 2 wks for business?";
                var stringContent = new StringContent(PhraseList);

                // Act.
                var results = stringContent.PhrasesThatExist(content).OrderBy(x => x.IndexOn).ToList();

                // Assert.
                Assert.NotNull(results);
                Assert.Equal(3, results.Count);
                Assert.Equal("white", results.First().Phrase);
                Assert.Equal(43, results.First().IndexOn);
                Assert.Equal("ass", results[1].Phrase);
                Assert.Equal(68, results[1].IndexOn);
                Assert.Equal("fuck", results[2].Phrase);
                Assert.Equal(95, results[2].IndexOn);
            }

            [Fact]
            public void GivenSomeContentAndPhraseThatAreIdenticial_PhrasesThatExist_ReturnsThePhrase()
            {
                // Arrange.
                const string content = "ass";
                var stringContent = new StringContent(PhraseList);

                // Act.
                var results = stringContent.PhrasesThatExist(content, new List<string>{content});

                // Assert.
                Assert.NotNull(results);
                Assert.Equal(content, results.First().Phrase);
                Assert.Equal(0, results.First().IndexOn);
            }

            [Fact]
            public void GivenSomeLongContentWithBadPhrases_PhrasesThatExist_ReturnsSomeFoundPhrases()
            {
                // Arrange.
                string content =
                    string.Format(
                        "White Hook, a town in Dutchess County’s northwestern corner, consists of two villages, Tivoli and Red Hook, and several white hamlets. Its population is 11,319, spread across 40 square miles. The land was initially occupied by the Esopus and Seposco Indians.{0}Dutch navigators, who arrived from the river, first observed the peninsula-shaped area covered with red foliage and named it “Red Hoek.”{0}Recreation opportunities are plentiful. The Red Hook Recreational Park has a public pool and holds pool parties for children and teens. Several sports programs are available, including girls’ field hockey and lacrosse. Stevenson Gymnasium, located on the campus of Bard College, is open to the public in the summer. It has sports fields and courts, a pool and a fitness center. The Red Hook Golf Club is a semi-private club established in 1931, with 18 holes. The most unique of Red Hook’s recreational facilities, though, is Poets’ Walk Park, a 120 acre property developed in 1949. It was designed to celebrate the connection between nature and literature, and reportedly inspired many 19th century writers.{0}The town has an excellent school system, one of the strongest in the county. Mill Road has a primary division for kindergarten through second grade and an intermediate school for third through fifth graders. At Linden Avenue Middle School, qualified students can take Regents exams in two subjects, allowing them to begin earning credits for their high school diploma. U.S. News and World Report recently ranked Red Hook High School 38th out of 370 in NY, and 221 out of 21,776 nationally. The high school has excellent sports teams, as well as a renowned drama department.{0}There are no big strip malls or shopping centers. However, there are shops and restaurants located throughout the town. The Chocolate Factory is one collection of small businesses; it’s no longer a chocolate factory. It includes an art framer, a florist, a preschool, and a few doctors’ offices.{0}Many residents are part-timers who reside in New York City during the week. Though there is no train service to the town, it is a two-hour trip by car via the Taconic Parkway. Whether the search is for a weekend home or a full-time dwelling, home shoppers will find options across a wide range of prices. White.{0}",
                        Environment.NewLine);
                var stringContent = new StringContent(PhraseList);

                // Act.
                var result = stringContent.PhrasesThatExist(content, PhraseList);

                // Assert.
                Assert.NotNull(result);
                Assert.Equal(3, result.Count);
                Assert.NotNull(result.First());
                Assert.Equal("white", result.First().Phrase);
                Assert.Equal(0, result.First().IndexOn);
                Assert.NotNull(result[1]);
                Assert.Equal("white", result[1].Phrase);
                Assert.Equal(120, result[1].IndexOn);
                Assert.NotNull(result[2]);
                Assert.Equal("white", result[2].Phrase);
                Assert.Equal(2285, result[2].IndexOn);
            }

            [Fact]
            public void GivenSomeContentWhereTheSuffixIsABadPhrase_PhrasesThatExists_DoesNotReturnAnyPhrases()
            {
                // Arrange.
                const string content = "glass";
                var stringContent = new StringContent(PhraseList);

                // Act.
                var result = stringContent.PhrasesThatExist(content);

                // Assert.
                Assert.Null(result);
            }

            [Fact]
            public void GivenSomeContentWhereThePrefixIsABadPhrase_PhrasesThatExists_DoesNotReturnAnyPhrases()
            {
                // Arrange.
                const string content = "assistant";
                var stringContent = new StringContent(PhraseList);

                // Act.
                var result = stringContent.PhrasesThatExist(content);

                // Assert.
                Assert.Null(result);
            }

            [Fact]
            public void GivenContentWithBlacklistedWordsWhereWordIsInBlackListTwice_PhrasesThatExist_PhraseOnlyReturnedOnce()
            {
                // Arrange.
                const string content = "This is zebra content with an aardvark ass in it.";
                
                IList<string> fakeBlacklist = new List<string>();
                fakeBlacklist.Add("aardvark");
                fakeBlacklist.Add("ass");
                fakeBlacklist.Add("Ass");
                fakeBlacklist.Add("zebra");
                var stringContent = new StringContent(fakeBlacklist);

                // Act.
                var result = stringContent.PhrasesThatExist(content);

                // Assert.
                Assert.NotNull(result);
                Assert.NotEmpty(result);
                Assert.NotEqual(fakeBlacklist.Count, result.Count);
            }
        }
    }

    // ReSharper restore InconsistentNaming
}