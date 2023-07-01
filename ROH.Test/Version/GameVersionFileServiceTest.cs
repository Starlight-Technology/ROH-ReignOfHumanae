using FluentValidation;
using FluentValidation.Results;

using Moq;

using ROH.Domain.Version;
using ROH.Interfaces.Repository.Version;
using ROH.Services.Version;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ROH.Test.Version
{
    public class GameVersionFileServiceTest
    {
        private readonly GameVersion version = new GameVersion(1, 1, 1, 1, true, DateTime.Today);
        private readonly GameVersionFile file = new GameVersionFile(1, 1, "testFile", 26354178, "~/testFolder", "format", "wertfgby834ht348ghrfowefj234fh32urf3fh23rfhfh83");
        

        [Fact]
        public async Task GetFiles_Returns_Files()
        {
            // Arrange
            var files = new List<GameVersionFile> { new GameVersionFile(1,1,"testFile", 26354178, "~/testFolder", "format", "wertfgby834ht348ghrfowefj234fh32urf3fh23rfhfh83") };
            var mockRepository = new Mock<IGameVersionFileRepository>();
            mockRepository.Setup(x => x.GetFiles(version)).ReturnsAsync(files);
            var mockValidator = new Mock<IValidator<GameVersionFile>>();
            var service = new GameVersionFileService(mockRepository.Object, mockValidator.Object);

            // Act
            var result = await service.GetFiles(version);

            // Assert
            Assert.Equal(files, result.ObjectResponse);
        }

        [Fact]
        public async Task NewFile_With_Valid_File_Creates_File_And_Saves()
        {
            // Arrange
            file.GameVersion = version;
            var mockValidator = new Mock<IValidator<GameVersionFile>>();
            mockValidator.Setup(x => x.ValidateAsync(file,  CancellationToken.None)).ReturnsAsync(new ValidationResult());
            var mockRepository = new Mock<IGameVersionFileRepository>();
            mockRepository.Setup(x => x.SaveFile(file)).Returns(Task.CompletedTask);
            var service = new GameVersionFileService(mockRepository.Object, mockValidator.Object);

            // Act
            await service.NewFile(file);

            // Assert
            mockRepository.Verify(x => x.SaveFile(file), Times.Once);
        }
    }
}
