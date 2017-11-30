using System;

namespace Sheep.Model.Content.Entities
{
    public static class ReplyExtensions
    {
        /// <summary>
        ///     计算投票的得分。
        /// </summary>
        /// <param name="reply">回复。</param>
        /// <returns>得分。</returns>
        public static float CalculateVotesScore(this Reply reply)
        {
            return Math.Min(1.0f, reply.YesVotesCount / 10.0f) - Math.Min(1.0f, reply.NoVotesCount / 10.0f);
        }

        /// <summary>
        ///     计算内容质量的得分。
        /// </summary>
        /// <param name="reply">回复。</param>
        /// <param name="votesWeight">投票的权重。</param>
        /// <returns>得分。</returns>
        public static float CalculateContentQuality(this Reply reply, float votesWeight = 1.0f)
        {
            return CalculateVotesScore(reply) * votesWeight;
        }
    }
}