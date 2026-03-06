using Xunit;
using Moq;
using Training.Core;
using FluentAssertions;

namespace TestWeather
{
    public class WheatherTest
    {
        [Fact]
        public void CheckWeather_ShouldReturnSunny_WhenTempIs25()
        {
            // Arrange
            var mockWeatherService = new Mock<IWeatherService>();

            mockWeatherService
                .Setup(x => x.GetTemp())
                .Returns(25);

            var logic = new WeatherService(mockWeatherService.Object);

            // Act
            var result = logic.CheckWeather();

            // Assert
            Assert.Equal("天氣晴朗", result);

            //FluentAssertions寫法
            //result.Should().Be("天氣晴朗");

            
        }
    }
}