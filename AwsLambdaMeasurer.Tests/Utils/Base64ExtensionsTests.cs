using AwsLambdaMeasurer.Utils;
using FluentAssertions;
using Xunit;

namespace AwsLambdaMeasurer.Tests.Utils
{
    public class Base64ExtensionsTests
    {
        [Fact]
        public void EncodeBase64_GivenString_ShouldConvertToBase64Correctly()
        {
            // Given
            const string plainText = "Some example plain text.";
            const string expectedEncodedText = "U29tZSBleGFtcGxlIHBsYWluIHRleHQu";

            // When
            var encoded = plainText.EncodeBase64();

            // Then
            encoded.Should().Be(expectedEncodedText);
        }
        
        [Fact]
        public void DecodeBase64_GivenString_ShouldConvertToPlainStringCorrectly()
        {
            // Given
            const string encodedText = "U29tZSBleGFtcGxlIHBsYWluIHRleHQu";
            const string expectedPlainText = "Some example plain text.";

            // When
            var decoded = encodedText.DecodeBase64();

            // Then
            decoded.Should().Be(expectedPlainText);
        }
    }
}