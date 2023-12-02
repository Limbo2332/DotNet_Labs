namespace NewsSite.DAL.DTO.Response
{
    public class AuthorResponse
    {
        public Guid Id { get; set; }

        public string Email { get; set; } = string.Empty;

        public string FullName { get; set; } = string.Empty;

        public bool? Sex { get; set; }

        public string? PublicInformation { get; set; }

        public DateTime BirthDate { get; set; }

        public DateTime UpdatedAt { get; set; }

        public override bool Equals(object? obj)
        {
            var anotherAuthor = obj as AuthorResponse;

            if(anotherAuthor is null)
            {
                return false;
            }

            return Id == anotherAuthor.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
