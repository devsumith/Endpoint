using SimpleNLG;

namespace Endpoint.Core.Services
{

    public class TenseConverter : ITenseConverter
    {
        public string Convert(string value, bool pastTense = true)
        {
            Lexicon lexicon = Lexicon.getDefaultLexicon();

            NLGFactory phraseFactory = new NLGFactory(lexicon);

            var clause = phraseFactory.createVerbPhrase(value);

            clause.setFeature(Feature.TENSE.ToString(), pastTense ? Tense.PAST : Tense.PRESENT);

            DocumentElement documentElement = phraseFactory.createSentence(clause);

            return new Realiser(lexicon).realise(documentElement).getRealisation().Replace(".", "");

        }
    }
}
