using AwsLambdaMeasurer.Utils;
using FluentAssertions;
using Xunit;

namespace AwsLambdaMeasurer.Tests.Utils
{
    public class Base64ExtensionsTests
    {
        [Fact]
        public void ToBase64_GivenString_ShouldConvertToBase64Correctly()
        {
            // Given
            const string plainText = "Some example plain text.";
            const string expectedEncodedText = "U29tZSBleGFtcGxlIHBsYWluIHRleHQu";

            // When
            var encoded = plainText.ToBase64String();

            // Then
            encoded.Should().Be(expectedEncodedText);
        }
    }
}