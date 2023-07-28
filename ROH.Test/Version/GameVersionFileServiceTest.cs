using FluentValidation;
using FluentValidation.Results;

using Moq;

using ROH.Domain.Version;
using ROH.Interfaces.Repository.Version;
using ROH.Interfaces.Services.Version;
using ROH.Services.Version;

namespace ROH.Test.Version
{
    public class GameVersionFileServiceTest
    {
        private readonly GameVersion _version = new(1, 1, 1, 1, true, DateTime.Today);
        private readonly GameVersionFile _file = new(1, 1, "testFile", 26354178, "~/testFolder", "format", "wertfgby834ht348ghrfowefj234fh32urf3fh23rfhfh83");


        [Fact]
        public async Task GetFiles_Returns_Files()
        {
            // Arrange
            var files = new List<GameVersionFile> { _file };
            var mockRepository = new Mock<IGameVersionFileRepository>();
            var mockVersionService = new Mock<IGameVersionService>();
            mockRepository.Setup(x => x.GetFiles(_version)).ReturnsAsync(files);
            var mockValidator = new Mock<IValidator<GameVersionFile>>();
            var service = new GameVersionFileService(mockRepository.Object, mockValidator.Object, mockVersionService.Object);

            // Act
            var result = await service.GetFiles(_version);

            // Assert
            Assert.Equal(files, result.ObjectResponse);
        }

        [Fact]
        public async Task NewFile_With_Valid_File_Creates_File_And_Saves()
        {
            // Arrange
            _file.GameVersion = _version;
            var mockValidator = new Mock<IValidator<GameVersionFile>>();
            mockValidator.Setup(x => x.ValidateAsync(_file, CancellationToken.None)).ReturnsAsync(new ValidationResult());
            var mockRepository = new Mock<IGameVersionFileRepository>();
            var mockVersionService = new Mock<IGameVersionService>();
            mockVersionService.Setup(x => x.VerifyIfVersionExist(_version)).Returns(Task.FromResult(true));
            mockRepository.Setup(x => x.SaveFile(_file)).Returns(Task.CompletedTask);
            var service = new GameVersionFileService(mockRepository.Object, mockValidator.Object, mockVersionService.Object);

            // Act
            var result = service.NewFile(_file).IsCompletedSuccessfully;

            // Assert
            Assert.True(result);

            await Task.CompletedTask;
        }

        [Fact]
        public async Task DownloadFile_Returns_File()
        {
            // Arrange
            _file.GameVersion = _version;

            var mockRepository = new Mock<IGameVersionFileRepository>();
            mockRepository.Setup(x => x.GetFile(_file.Id)).ReturnsAsync(_file);

            var mockVersionService = new Mock<IGameVersionService>();
            mockVersionService.Setup(x => x.VerifyIfVersionExist(_version)).ReturnsAsync(true);

            var mockValidator = new Mock<IValidator<GameVersionFile>>();
            mockValidator.Setup(x => x.ValidateAsync(_file, CancellationToken.None)).ReturnsAsync(new ValidationResult());

            var service = new GameVersionFileService(mockRepository.Object, mockValidator.Object, mockVersionService.Object);

            // Act
            var result = await service.DownloadFile(_file.Id);

            // Assert
            Assert.Equal("wertfgby834ht348ghrfowefj234fh32urf3fh23rfhfh83", result.ObjectResponse);

        }
    }
}
