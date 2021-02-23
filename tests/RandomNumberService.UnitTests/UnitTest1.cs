using System;
using FluentAssertions;
using RandomNumberService.Application.Common.Exceptions;
using Xunit;
using RandomNumberService = RandomNumberService.Infrastructure.RandomNumberService;

namespace RandomNumberService.UnitTests
{
    public class RandomNumberServiceTest
    {
        [Fact]
        public void Generate_DifferentMinMaxValues_ReturnsNumberBetweenMinAndMax()
        {
            // arrange
            var min = 4;
            var max = 200;

            var sut = new Infrastructure.RandomNumberService();

            // act
            var number = sut.Generate(min, max);

            // assert
            number.Should().BeGreaterOrEqualTo(min);
            number.Should().BeLessOrEqualTo(max);
        }

        [Fact]
        public void Generate_SameMinAndMaxValue_ReturnsValueRangeException()
        {
            var minAndMax = 1;

            var sut = new Infrastructure.RandomNumberService();

            // act
            // assert
            var exception = Assert.Throws<ValueRangeException>(() =>
            {
                sut.Generate(minAndMax, minAndMax);
            });
            exception.Message.Should()
                .Be($"Value range invalid. Min value ({minAndMax}) must be less than max value ({minAndMax}).");
        }

        [Fact]
        public void Generate_BiggerMinThanMaxValue_ReturnsValueRangeException()
        {
            // arrange
            var min = 12;
            var max = 3;

            var sut = new Infrastructure.RandomNumberService();

            // act
            // assert
            var exception = Assert.Throws<ValueRangeException>(() =>
            {
                sut.Generate(min, max);
            });
            exception.Message.Should()
                .Be($"Value range invalid. Min value ({min}) must be less than max value ({max}).");
        }
    }
}
