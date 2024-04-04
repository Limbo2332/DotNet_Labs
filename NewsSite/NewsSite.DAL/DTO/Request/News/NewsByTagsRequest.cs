using static System.Guid;

namespace NewsSite.DAL.DTO.Request.News;

public class NewsByTagsRequest
{
    public List<Guid> TagsIds { get; set; } = new();

    public static bool TryParse(string value, IFormatProvider? formatProvider, out NewsByTagsRequest? newsByTagsRequest)
    {
        newsByTagsRequest = new NewsByTagsRequest
        {
            TagsIds = new List<Guid>()
        };

        if (string.IsNullOrEmpty(value))
        {
            return false;
        }
        
        var tagsIdsList = new List<Guid>();
        var trimmedValue = value.TrimStart('(').TrimEnd(')');
        var segments = trimmedValue.Split(
            ',',
            StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        
        foreach (var segment in segments)
        {
            Guid.TryParse(segment, out var id);
            tagsIdsList.Add(id);
        }

        newsByTagsRequest = new NewsByTagsRequest
        {
            TagsIds = tagsIdsList
        };
        
        return true;
    }
}