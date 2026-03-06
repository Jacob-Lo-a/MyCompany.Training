using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Training.Core;

namespace TestWeather
{
    public class AverageScoreTest
    {
        [Fact]
        public void CalculateAverageScoreByClass_ShouldReturnCorrectAverage()
        {
            // Arrange
            var students = new List<Student>
            {
                new Student ("A1", 80, "A"),
                new Student ("A2",  100, "A"),
                new Student ("B1", 60, "B"),
                new Student ("B2", 80, "B")
            };

            var service = new Linq();

            // Act
            var result = service.CalculateAverageScoreByClass(students);

            // Assert
            //Assert.Equal(90, result["A"]);
            //Assert.Equal(70, result["B"]);

            //FluentAssertions
            result["A"].Should().Be(90);
            result["B"].Should().Be(70);

        }
        [Fact]
        public void ListIsNull_ShouldNotReturnFalse()
        {
            // Arrange
            var students = new List<Student>();
            

            var service = new Linq();

            // Act
            var result = service.CalculateAverageScoreByClass(students);

            // Assert
            Assert.Equal(90, result["A"]);
            Assert.Equal(70, result["B"]);
            
            //FluentAssertions
            

        }
    }
}
