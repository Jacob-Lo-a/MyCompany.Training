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
            var mockWeatherService = new Mock<WeatherService>();

            var temp = mockWeatherService.Object.GetTemp();

            // Act

            var result = (temp == 25) ? "天氣晴朗" : "天氣不明";

            // Assert
            Assert.Equal("天氣晴朗", result);

            //FluentAssertions寫法
            //result.Should().Be("天氣晴朗");


        }
    }
}