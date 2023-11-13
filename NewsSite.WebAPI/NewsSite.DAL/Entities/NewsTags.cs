namespace NewsSite.DAL.Entities
{
    public class NewsTags
    {
        public Guid PieceOfNewsId { get; set; }
        public PieceOfNews? PieceOfNews { get; set; }

        public Guid TagId { get; set; }
        public Tag? Tag { get; set; }
    }
}
