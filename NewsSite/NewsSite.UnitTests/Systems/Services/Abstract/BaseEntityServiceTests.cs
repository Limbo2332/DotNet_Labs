using System.Linq.Dynamic.Core;
using NewsSite.BLL.Services.Abstract;
using NewsSite.DAL.DTO.Page;
using NewsSite.DAL.DTO.Response.Abstract;
using NewsSite.DAL.Entities.Abstract;
using NewsSite.DAL.Repositories.Base;
using NewsSite.UnitTests.TestData;

namespace NewsSite.UnitTests.Systems.Services.Abstract
{
    public abstract class BaseEntityServiceTests<TEntry, TResult> : BaseServiceTests
        where TEntry : BaseEntity
        where TResult : BaseResponse
    {
        private readonly IQueryable<TEntry> _queryableMock;

        protected abstract BaseEntityService<TEntry, TResult> _sut { get; }

        protected BaseEntityServiceTests()
        {
            var repositoryMock = new Mock<IGenericRepository<TEntry>>();
            _queryableMock = RepositoriesFakeData.GetEntities<TEntry>().BuildMock();

            repositoryMock
                .Setup(ar => ar.GetAll())
                .Returns(_queryableMock);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAll_WhenNoPageSettings()
        {
            // Arrange
            PageSettings? pageSettings = null;
            var response = _mapper.Map<List<TResult>>(RepositoriesFakeData.GetEntities<TEntry>());

            // Act
            var result = await _sut.GetAllAsync(_queryableMock, pageSettings);

            // Assert
            using (new AssertionScope())
            {
                result.Items.Should().BeEquivalentTo(response);
                result.Items.Should().BeInAscendingOrder(i => i.UpdatedAt);
                result.TotalCount.Should().Be(RepositoriesFakeData.ITEMS_COUNT);
                result.PageSize.Should().Be(PageList<TResult>.DEFAULT_PAGE_SIZE);
                result.PageNumber.Should().Be(1);
                result.HasNextPage.Should().BeFalse();
                result.HasPreviousPage.Should().BeFalse();
            }
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnPagedList_WhenPagePagination()
        {
            // Arrange
            var pageNumber = 2;
            var pageSize = 3;

            var pageSettings = new PageSettings
            {
                PagePagination = new PagePagination
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                }
            };
            var response =
                _mapper.Map<List<TResult>>(
                RepositoriesFakeData.GetEntities<TEntry>()
                    .Skip(pageSize)
                    .Take(pageNumber));

            // Act
            var result = await _sut.GetAllAsync(_queryableMock, pageSettings);

            // Assert
            using (new AssertionScope())
            {
                result.Items.Should().BeEquivalentTo(response);
                result.Items.Should().BeInAscendingOrder(i => i.UpdatedAt);
                result.TotalCount.Should().Be(RepositoriesFakeData.ITEMS_COUNT);
                result.PageSize.Should().Be(pageSize);
                result.PageNumber.Should().Be(pageNumber);
                result.HasNextPage.Should().BeFalse();
                result.HasPreviousPage.Should().BeTrue();
            }
        }

        public virtual async Task GetAllAsync_ShouldReturnPagedList_WhenPageFiltering(string propertyName, string propertyValue)
        {
            // Arrange
            var pageSettings = new PageSettings
            {
                PageFiltering = new PageFiltering
                {
                    PropertyName = propertyName,
                    PropertyValue = propertyValue
                }
            };
            var response =
                _mapper.Map<List<TResult>>(
                    _queryableMock
                        .Where($"{propertyName}.ToLowerInvariant().Contains(@0)",
                            propertyValue.ToLowerInvariant()));

            // Act
            var result = await _sut.GetAllAsync(_queryableMock, pageSettings);

            // Assert
            using (new AssertionScope())
            {
                result.Items.Should().BeEquivalentTo(response);
                result.Items.Should().BeInAscendingOrder(i => i.UpdatedAt);
                result.TotalCount.Should().Be(1);
                result.PageSize.Should().Be(PageList<TResult>.DEFAULT_PAGE_SIZE);
                result.PageNumber.Should().Be(1);
                result.HasNextPage.Should().BeFalse();
                result.HasPreviousPage.Should().BeFalse();
            }
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnPagedList_WhenPageFilteringWrongData()
        {
            var propertyName = "wrongName";
            var propertyValue = "wrongValue";

            // Arrange
            var pageSettings = new PageSettings
            {
                PageFiltering = new PageFiltering
                {
                    PropertyName = propertyName,
                    PropertyValue = propertyValue
                }
            };
            var response = _mapper.Map<List<TResult>>(_queryableMock);

            // Act
            var result = await _sut.GetAllAsync(_queryableMock, pageSettings);

            // Assert
            using (new AssertionScope())
            {
                result.Items.Should().BeEquivalentTo(response);
                result.Items.Should().BeInAscendingOrder(i => i.UpdatedAt);
                result.TotalCount.Should().Be(RepositoriesFakeData.ITEMS_COUNT);
                result.PageSize.Should().Be(PageList<TResult>.DEFAULT_PAGE_SIZE);
                result.PageNumber.Should().Be(1);
                result.HasNextPage.Should().BeFalse();
                result.HasPreviousPage.Should().BeFalse();
            }
        }

        public virtual async Task GetAllAsync_ShouldReturnPagedList_WhenPageSortingProperty(SortingOrder order, string sortingProperty)
        {
            // Arrange
            var pageSettings = new PageSettings
            {
                PageSorting = new PageSorting
                {
                    SortingOrder = order,
                    SortingProperty = sortingProperty
                }
            };
            var response =
                _mapper.Map<List<TResult>>(
                    _queryableMock
                        .OrderBy($"{sortingProperty} {(order == SortingOrder.Ascending ? "asc" : "desc")}"));

            // Act
            var result = await _sut.GetAllAsync(_queryableMock, pageSettings);

            // Assert
            using (new AssertionScope())
            {
                result.Items.Should().ContainInConsecutiveOrder(response);
                result.TotalCount.Should().Be(RepositoriesFakeData.ITEMS_COUNT);
                result.PageSize.Should().Be(PageList<TResult>.DEFAULT_PAGE_SIZE);
                result.PageNumber.Should().Be(1);
                result.HasNextPage.Should().BeFalse();
                result.HasPreviousPage.Should().BeFalse();
            }
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnPagedList_WhenPageSortingPropertyNotSpecified()
        {
            // Arrange
            var pageSettings = new PageSettings
            {
                PageSorting = new PageSorting
                {
                    SortingOrder = SortingOrder.Ascending,
                    SortingProperty = "sortingProperty"
                }
            };
            var response = _mapper.Map<List<TResult>>(_queryableMock);

            // Act
            var result = await _sut.GetAllAsync(_queryableMock, pageSettings);

            // Assert
            using (new AssertionScope())
            {
                result.Items.Should().ContainInConsecutiveOrder(response);
                result.TotalCount.Should().Be(RepositoriesFakeData.ITEMS_COUNT);
                result.PageSize.Should().Be(PageList<TResult>.DEFAULT_PAGE_SIZE);
                result.PageNumber.Should().Be(1);
                result.HasNextPage.Should().BeFalse();
                result.HasPreviousPage.Should().BeFalse();
            }
        }
    }
}
