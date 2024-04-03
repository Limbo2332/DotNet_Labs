using System.Reflection;
using Microsoft.AspNetCore.Http;
using NewsSite.DAL.DTO.Response;

namespace NewsSite.DAL.DTO.Page
{
    public class PageSettings
    {
        public PageSorting? PageSorting { get; set; }

        public PageFiltering? PageFiltering { get; set; }

        public PagePagination? PagePagination { get; set; }
        
        public static ValueTask<PageSettings> BindAsync(HttpContext httpContext, ParameterInfo parameter)
        {
            var sortingProperty = httpContext
                .Request
                .Query[$"{nameof(PageSorting)}.{nameof(PageSorting.SortingProperty)}"]
                .FirstOrDefault() ?? string.Empty;
            
            var sortingOrderString = httpContext
                .Request
                .Query[$"{nameof(PageSorting)}.{nameof(PageSorting.SortingOrder)}"]
                .FirstOrDefault() ?? "asc";
            var sortingOrder = sortingOrderString.Contains("desc") 
                ? SortingOrder.Descending 
                : SortingOrder.Ascending;

            var propertyName = httpContext
                .Request
                .Query[$"{nameof(PageFiltering)}.{nameof(PageFiltering.PropertyName)}"]
                .FirstOrDefault() ?? string.Empty;
            var propertyValue = httpContext
                .Request
                .Query[$"{nameof(PageFiltering)}.{nameof(PageFiltering.PropertyValue)}"]
                .FirstOrDefault() ?? string.Empty;

            var pageSize = int.TryParse(httpContext.Request.Query[$"{nameof(PagePagination)}.{nameof(PagePagination.PageSize)}"].FirstOrDefault(), out var parsedPageSize)
                ? parsedPageSize
                : PageList<TagResponse>.DefaultPageSize;
            var pageNumber = int.TryParse(httpContext.Request.Query[$"{nameof(PagePagination)}.{nameof(PagePagination.PageNumber)}"].FirstOrDefault(), out var parsedPageNumber)
                ? parsedPageNumber
                : 1;

            return ValueTask.FromResult(
                new PageSettings
                {
                    PageSorting = new PageSorting
                    {
                        SortingOrder = sortingOrder,
                        SortingProperty = sortingProperty
                    },
                    PageFiltering = new PageFiltering
                    {
                        PropertyName = propertyName,
                        PropertyValue = propertyValue
                    },
                    PagePagination = new PagePagination
                    {
                        PageNumber = pageNumber,
                        PageSize = pageSize
                    }
                }
            );
        }

    }
}
