using System.Collections.Generic;

namespace Noodle.Models.Brain
{
    public class WordModel
    {
        public WordModel()
        {
            if (Emotions == null)
                Emotions = new List<EmotionalScoreModel>();

            if (Alignments == null)
                Alignments = new List<AlignmentScoreModel>();
        }

        public string Word { get; set; }
        public List<EmotionalScoreModel> Emotions { get; set; }
        public List<AlignmentScoreModel> Alignments { get; set; }
    }

    public class EmotionalScoreModel
    {
        public BrainEnums.Emotion Emotion { get; set; }
        public int Score { get; set; }
    }

    /// <summary>
    /// Positive/Negative
    /// </summary>
    public class AlignmentScoreModel
    {
        public BrainEnums.Alignment Alignment { get; set; }
        public int Score { get; set; }
    }
}