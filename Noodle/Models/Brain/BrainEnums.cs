namespace Noodle.Models.Brain
{
    public class BrainEnums
    {
        public enum Alignment
        {
            Positive,
            Negative
        }

        /// <summary>
        /// BASIC EMOTIONS: CORE MODEL
        /// Theorists: Ekman, Friesen, and Ellsworth
        /// </summary>
        public enum Emotion
        {
            Anger,
            Disgust,
            Fear,
            Joy,
            Sadness,
            Surprise,
            Anticipation,
            Trust
        }

        /// <summary>
        /// SECONDARY MODEL - MAY BE TOO COMPLEX
        /// Book Two of Aristotle's "Rhetoric"
        /// </summary>
        public enum SecondayEmotion
        {
            // pattern: emotion, opposite emotion, neutral emotions at the end
            Calm, Anger,
            Friendly, Hate,
            Couragous, Fear,
            Confident, Shame,
            Kind, Cruel,
            Sympathetic, Jealous,
            Love, Envy,
            Funny, Mischievous, Spontaneous, Inspired, Creative, Trusting, Hopeful
        }
        public enum ZodiacSign
        {
            Aries,
            Taurus,
            Gemini,
            Cancer,
            Leo,
            Virgo,
            Libra,
            Scorpio,
            Sagittarius,
            Capricorn,
            Aquarius,
            Pisces
        }

        public enum ThoughtPriority
        {
            Emergency,
            High,
            Neutral,
            Low,
            None
        }
    }
}