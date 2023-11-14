namespace NewsSite.DAL.Entities
{
    public class NewsRubrics
    {
        public Guid PieceOfNewsId { get; set; }
        public PieceOfNews? PieceOfNews { get; set; }

        public Guid RubricId { get; set; }
        public Rubric? Rubric { get; set; }
    }
}
