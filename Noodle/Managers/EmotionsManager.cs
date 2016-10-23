using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Noodle.Models.Brain;

namespace Noodle.Managers
{
    public class EmotionsManager
    {
        public BrainEnums.Emotion CurrentEmotion { get; set; }
        public List<BrainEnums.Emotion> PositiveEmotions { get; set; }
        public List<BrainEnums.Emotion> NegativeEmotions { get; set; }
        public List<BrainEnums.Emotion> NeutralEmotions { get; set; }
        public List<BrainEnums.SecondayEmotion> PositiveSecondaryEmotions { get; set; }
        public List<BrainEnums.SecondayEmotion> NegativeSecondaryEmotions { get; set; }
        public List<BrainEnums.SecondayEmotion> NeutralSecondaryEmotions { get; set; }

        public EmotionsManager()
        {
            // CORE MODEL
            PositiveEmotions = new List<BrainEnums.Emotion>
                                    {
                                        BrainEnums.Emotion.Joy,
                                        BrainEnums.Emotion.Surprise
                                    };
            NegativeEmotions = new List<BrainEnums.Emotion>
                                    {
                                        BrainEnums.Emotion.Anger,
                                        BrainEnums.Emotion.Fear,
                                        BrainEnums.Emotion.Disgust,
                                        BrainEnums.Emotion.Sadness
                                    };
            NeutralEmotions = new List<BrainEnums.Emotion>
                                    {
                                        BrainEnums.Emotion.Anticipation,
                                        BrainEnums.Emotion.Trust
                                    };

            //SECONDARY MODEL - MAY BE TOO COMPLEX
            //TODO: Use, change or throw away secondary model
            PositiveSecondaryEmotions = new List<BrainEnums.SecondayEmotion>
                                    {
                                        BrainEnums.SecondayEmotion.Calm,
                                        BrainEnums.SecondayEmotion.Friendly,
                                        BrainEnums.SecondayEmotion.Couragous,
                                        BrainEnums.SecondayEmotion.Confident,
                                        BrainEnums.SecondayEmotion.Kind,
                                        BrainEnums.SecondayEmotion.Sympathetic,
                                        BrainEnums.SecondayEmotion.Love
                                    };
            NegativeSecondaryEmotions = new List<BrainEnums.SecondayEmotion>
                                    {
                                        BrainEnums.SecondayEmotion.Anger,
                                        BrainEnums.SecondayEmotion.Hate,
                                        BrainEnums.SecondayEmotion.Fear,
                                        BrainEnums.SecondayEmotion.Shame,
                                        BrainEnums.SecondayEmotion.Cruel,
                                        BrainEnums.SecondayEmotion.Jealous,
                                        BrainEnums.SecondayEmotion.Envy
                                    };
            NeutralSecondaryEmotions = new List<BrainEnums.SecondayEmotion>
                                    {
                                        BrainEnums.SecondayEmotion.Funny,
                                        BrainEnums.SecondayEmotion.Mischievous,
                                        BrainEnums.SecondayEmotion.Spontaneous,
                                        BrainEnums.SecondayEmotion.Inspired,
                                        BrainEnums.SecondayEmotion.Creative,
                                        BrainEnums.SecondayEmotion.Trusting,
                                        BrainEnums.SecondayEmotion.Hopeful
                                    };
        }
    }
}
