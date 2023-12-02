using System.Globalization;
using NewsSite.BLL.Interfaces;
using NewsSite.BLL.Services;
using NewsSite.DAL.DTO.Page;
using NewsSite.DAL.DTO.Response;
using NewsSite.DAL.Repositories.Base;
using NewsSite.UnitTests.Systems.Services.Abstract;
using NewsSite.UnitTests.TestData;
using System.Linq.Dynamic.Core;
using NewsSite.BLL.Exceptions;
using NewsSite.BLL.Extensions;
using NewsSite.UnitTests.TestData.PageSettings.Authors;
using NewsSite.DAL.DTO.Request.Author;

namespace NewsSite.UnitTests.Systems.Services
{
    public class AuthorsServiceTests : BaseServiceTests
    {
        private readonly Mock<IAuthorsRepository> _authorsRepositoryMock;
        private readonly IQueryable<Author> _authorsQueryableMock;
        private readonly IAuthorsService _sut;

        public AuthorsServiceTests()
        {
            _authorsRepositoryMock = new Mock<IAuthorsRepository>();
            _authorsQueryableMock = RepositoriesFakeData.Authors.BuildMock();

            _authorsRepositoryMock
                .Setup(ar => ar.GetAll())
                .Returns(_authorsQueryableMock);

            _sut = new AuthorsService(
                _userManagerMock.Object,
                _mapper,
                _authorsRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAuthorsAsync_ShouldReturnAllAuthors_WhenNoPageSettings()
        {
            // Arrange
            PageSettings? pageSettings = null;
            var authorsResponse = _mapper.Map<List<AuthorResponse>>(RepositoriesFakeData.Authors);

            // Act
            var result = await _sut.GetAuthorsAsync(pageSettings);

            // Assert
            _authorsRepositoryMock.Verify(ar => ar.GetAll(), Times.Once);

            using (new AssertionScope())
            {
                result.Items.Should().BeEquivalentTo(authorsResponse);
                result.Items.Should().BeInAscendingOrder(i => i.UpdatedAt);
                result.TotalCount.Should().Be(RepositoriesFakeData.AUTHORS_ITEMS_COUNT);
                result.PageSize.Should().Be(PageList<AuthorResponse>.DEFAULT_PAGE_SIZE);
                result.PageNumber.Should().Be(1);
                result.HasNextPage.Should().BeFalse();
                result.HasPreviousPage.Should().BeFalse();
            }
        }

        [Fact]
        public async Task GetAuthorsAsync_ShouldReturnPagedList_WhenPagePagination()
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
            var authorsResponse = 
                _mapper.Map<List<AuthorResponse>>(
                RepositoriesFakeData.Authors
                    .Skip(pageSize)
                    .Take(pageNumber));

            // Act
            var result = await _sut.GetAuthorsAsync(pageSettings);

            // Assert
            _authorsRepositoryMock.Verify(ar => ar.GetAll(), Times.Once);

            using (new AssertionScope())
            {
                result.Items.Should().BeEquivalentTo(authorsResponse);
                result.Items.Should().BeInAscendingOrder(i => i.UpdatedAt);
                result.TotalCount.Should().Be(RepositoriesFakeData.AUTHORS_ITEMS_COUNT);
                result.PageSize.Should().Be(pageSize);
                result.PageNumber.Should().Be(pageNumber);
                result.HasNextPage.Should().BeFalse();
                result.HasPreviousPage.Should().BeTrue();
            }
        }

        [Theory]
        [ClassData(typeof(AuthorsFilteringData))]
        public async Task GetAuthorsAsync_ShouldReturnPagedList_WhenPageFiltering(string propertyName, string propertyValue)
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
            var authorsResponse = 
                _mapper.Map<List<AuthorResponse>>(
                    _authorsQueryableMock
                        .Where($"{propertyName}.ToLowerInvariant().Contains(@0)", 
                            propertyValue.ToLowerInvariant()));

            // Act
            var result = await _sut.GetAuthorsAsync(pageSettings);

            // Assert
            _authorsRepositoryMock.Verify(ar => ar.GetAll(), Times.Once);

            using (new AssertionScope())
            {
                result.Items.Should().BeEquivalentTo(authorsResponse);
                result.Items.Should().BeInAscendingOrder(i => i.UpdatedAt);
                result.TotalCount.Should().Be(1);
                result.PageSize.Should().Be(PageList<AuthorResponse>.DEFAULT_PAGE_SIZE);
                result.PageNumber.Should().Be(1);
                result.HasNextPage.Should().BeFalse();
                result.HasPreviousPage.Should().BeFalse();
            }
        }

        [Fact]
        public async Task GetAuthorsAsync_ShouldReturnPagedList_WhenPageFilteringBirthDate()
        {
            var propertyName = nameof(Author.BirthDate);
            var propertyValue = RepositoriesFakeData.Authors.First().BirthDate.ToString(CultureInfo.InvariantCulture);

            // Arrange
            var pageSettings = new PageSettings
            {
                PageFiltering = new PageFiltering
                {
                    PropertyName = propertyName,
                    PropertyValue = propertyValue
                }
            };
            var authorsResponse =
                _mapper.Map<List<AuthorResponse>>(
            _authorsQueryableMock
                        .Where(a => propertyValue.IsDateTime() 
                                    && a.BirthDate >= Convert.ToDateTime(propertyValue, CultureInfo.InvariantCulture)));

            // Act
            var result = await _sut.GetAuthorsAsync(pageSettings);

            // Assert
            _authorsRepositoryMock.Verify(ar => ar.GetAll(), Times.Once);

            using (new AssertionScope())
            {
                result.Items.Should().BeEquivalentTo(authorsResponse);
                result.Items.Should().BeInAscendingOrder(i => i.UpdatedAt);
                result.TotalCount.Should().Be(1);
                result.PageSize.Should().Be(PageList<AuthorResponse>.DEFAULT_PAGE_SIZE);
                result.PageNumber.Should().Be(1);
                result.HasNextPage.Should().BeFalse();
                result.HasPreviousPage.Should().BeFalse();
            }
        }

        [Fact]
        public async Task GetAuthorsAsync_ShouldReturnPagedList_WhenPageFilteringWrongData()
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
            var authorsResponse = _mapper.Map<List<AuthorResponse>>(_authorsQueryableMock);

            // Act
            var result = await _sut.GetAuthorsAsync(pageSettings);

            // Assert
            _authorsRepositoryMock.Verify(ar => ar.GetAll(), Times.Once);

            using (new AssertionScope())
            {
                result.Items.Should().BeEquivalentTo(authorsResponse);
                result.Items.Should().BeInAscendingOrder(i => i.UpdatedAt);
                result.TotalCount.Should().Be(RepositoriesFakeData.AUTHORS_ITEMS_COUNT);
                result.PageSize.Should().Be(PageList<AuthorResponse>.DEFAULT_PAGE_SIZE);
                result.PageNumber.Should().Be(1);
                result.HasNextPage.Should().BeFalse();
                result.HasPreviousPage.Should().BeFalse();
            }
        }

        [Theory]
        [ClassData(typeof(AuthorsSortingData))]
        public async Task GetAuthorsAsync_ShouldReturnPagedList_WhenPageSortingProperty(SortingOrder order, string sortingProperty)
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
            var authorsResponse =
                _mapper.Map<List<AuthorResponse>>(
                    _authorsQueryableMock
                        .OrderBy($"{sortingProperty} {(order == SortingOrder.Ascending ? "asc" : "desc")}"));

            // Act
            var result = await _sut.GetAuthorsAsync(pageSettings);

            // Assert
            _authorsRepositoryMock.Verify(ar => ar.GetAll(), Times.Once);

            using (new AssertionScope())
            {
                result.Items.Should().ContainInConsecutiveOrder(authorsResponse);
                result.TotalCount.Should().Be(RepositoriesFakeData.AUTHORS_ITEMS_COUNT);
                result.PageSize.Should().Be(PageList<AuthorResponse>.DEFAULT_PAGE_SIZE);
                result.PageNumber.Should().Be(1);
                result.HasNextPage.Should().BeFalse();
                result.HasPreviousPage.Should().BeFalse();
            }
        }

        [Fact]
        public async Task GetAuthorsAsync_ShouldReturnPagedList_WhenPageSortingPropertyNotSpecified()
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
            var authorsResponse = _mapper.Map<List<AuthorResponse>>(_authorsQueryableMock);

            // Act
            var result = await _sut.GetAuthorsAsync(pageSettings);

            // Assert
            _authorsRepositoryMock.Verify(ar => ar.GetAll(), Times.Once);

            using (new AssertionScope())
            {
                result.Items.Should().ContainInConsecutiveOrder(authorsResponse);
                result.TotalCount.Should().Be(RepositoriesFakeData.AUTHORS_ITEMS_COUNT);
                result.PageSize.Should().Be(PageList<AuthorResponse>.DEFAULT_PAGE_SIZE);
                result.PageNumber.Should().Be(1);
                result.HasNextPage.Should().BeFalse();
                result.HasPreviousPage.Should().BeFalse();
            }
        }

        [Fact]
        public async Task GetAuthorByIdAsync_ShouldThrowException_WhenNoAuthor()
        {
            // Arrange
            var authorId = Guid.Empty;
            var exceptionMessage = new NotFoundException(nameof(Author), authorId).Message;

            // Act
            var action = async () => await _sut.GetAuthorByIdAsync(authorId);

            // Assert
            await action.Should()
                .ThrowAsync<NotFoundException>()
                .WithMessage(exceptionMessage);
        }

        [Theory]
        [ClassData(typeof(AuthorsTestData))]
        public async Task GetAuthorByIdAsync_ShouldReturnAuthor_WhenAuthorExists(Author author)
        {
            // Arrange
            _authorsRepositoryMock
                .Setup(ar => ar.GetByIdAsync(author.Id))
                .ReturnsAsync(author);

            var authorResponse = _mapper.Map<AuthorResponse>(author);

            // Act
            var result = await _sut.GetAuthorByIdAsync(author.Id);

            // Assert
            _authorsRepositoryMock.Verify(ar => ar.GetByIdAsync(author.Id), Times.Once);

            result.Should().Be(authorResponse);
        }

        [Theory]
        [ClassData(typeof(AuthorsTestData))]
        public async Task UpdateAuthorAsync_ShouldReturnAuthor_WhenAuthorExists(Author author)
        {
            // Arrange
            var updatedAuthor = new UpdatedAuthorRequest
            {
                BirthDate = author.BirthDate,
                Email = author.Email,
                FullName = $"{author.FullName} {author.Email}",
                PublicInformation = author.PublicInformation,
                Sex = true,
                Id = author.Id
            };

            _userManagerMock
                .Setup(um => um.FindByEmailAsync(author.Email).Result)
                .Returns(author.IdentityUser);

            var newAuthor = _mapper.Map<Author>(updatedAuthor);

            _authorsRepositoryMock
                .Setup(ar => ar.GetByIdAsync(author.Id))
                .ReturnsAsync(author);

            _authorsRepositoryMock
                .Setup(ar => ar.UpdateAsync(newAuthor))
                .Returns(Task.CompletedTask);

            _userManagerMock
                .Setup(um => um.SetEmailAsync(newAuthor.IdentityUser, newAuthor.Email))
                .Callback(() => newAuthor.IdentityUser.Email = newAuthor.Email);

            _userManagerMock
                .Setup(um => um.SetUserNameAsync(newAuthor.IdentityUser, newAuthor.FullName))
                .Callback(() => newAuthor.IdentityUser.UserName = newAuthor.FullName);
            
            var authorResponse = _mapper.Map<AuthorResponse>(newAuthor);

            // Act
            var result = await _sut.UpdateAuthorAsync(updatedAuthor);

            // Assert
            using (new AssertionScope())
            {
                result.Should().Be(authorResponse);
                result.Email.Should().Be(newAuthor.IdentityUser.Email);
                result.FullName.Should().Be(newAuthor.IdentityUser.UserName);
            }
        }

        [Theory]
        [ClassData(typeof(AuthorsTestData))]
        public async Task DeleteAuthorAsync_ShouldDeleteAuthor_WhenAuthorExists(Author author)
        {
            // Arrange
            var authors = RepositoriesFakeData.Authors.ToList();

            _authorsRepositoryMock
                .Setup(ar => ar.DeleteAsync(author.Id))
                .Callback(() => authors.Remove(author));

            // Act
            await _sut.DeleteAuthorAsync(author.Id);

            // Assert
            _authorsRepositoryMock.Verify(ar => ar.DeleteAsync(author.Id), Times.Once);

            authors.Should().NotContain(author);
        }

        [Fact]
        public void IsEmailUnique_ShouldReturnFalse_WhenEmailExists()
        {
            // Arrange
            _authorsRepositoryMock
                .Setup(ar => ar.IsEmailUnique(It.IsAny<string>()))
                .Returns(false);

            // Act
            var result = _sut.IsEmailUnique(It.IsAny<string>());

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void IsEmailUnique_ShouldReturnTrue_WhenEmailExists()
        {
            // Arrange
            _authorsRepositoryMock
                .Setup(ar => ar.IsEmailUnique(It.IsAny<string>()))
                .Returns(true);

            // Act
            var result = _sut.IsEmailUnique(It.IsAny<string>());

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsFullNameUnique_ShouldReturnFalse_WhenEmailExists()
        {
            // Arrange
            _authorsRepositoryMock
                .Setup(ar => ar.IsFullNameUnique(It.IsAny<string>()))
                .Returns(false);

            // Act
            var result = _sut.IsFullNameUnique(It.IsAny<string>());

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void IsFullNameUnique_ShouldReturnTrue_WhenEmailExists()
        {
            // Arrange
            _authorsRepositoryMock
                .Setup(ar => ar.IsFullNameUnique(It.IsAny<string>()))
                .Returns(true);

            // Act
            var result = _sut.IsFullNameUnique(It.IsAny<string>());

            // Assert
            result.Should().BeTrue();
        }
    }
}
