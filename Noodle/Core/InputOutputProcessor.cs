namespace Noodle.Managers
{
    public class InputOutputProcessor
    {
        public string[] InputWords { get; set; }

        public void ParsePhrase(string input)
        {
            // 1st try common phrase matcher
            // TODO: invent phrase algorithym - example: what is {0}?   or what does {0} mean?   who are you?

            // 2nd option parse words from phrase
            InputWords = input.Split(' ');
        }
    }
}