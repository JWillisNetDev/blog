using blog.Services.Validation;
using Xunit.Abstractions;

namespace blog.Services.Tests.Validation;

public class ValidatorTests
{
    public ITestOutputHelper Output { get; }
    
    public ValidatorTests(ITestOutputHelper output)
    {
        Output = output;
    }
    
    private class StringLengthMax10
    {
        [StringLength(10)]
        public string? Value { get; set; }
    }

    private class StringLengthMax10Min5
    {
        [StringLength(10, MinLength = 5)]
        public string? Value { get; set; }
    }
    
    private class StringLengthMax10AllowNullFalse
    {
        [StringLength(10, AllowNull = false)]
        public string? Value { get; set; }
    }
    
    [Theory]
    [InlineData("test")]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void StringLengthAttribute_IsValidMaxLength_ReturnsValidResult(string? value)
    {
        var validator = new Validator<StringLengthMax10>();
        
        var result = validator.Validate(new() { Value = value });

        Assert.True(result.IsValid);
    }

    [Fact]
    public void StringLengthAttribute_IsOverMaxLength_ReturnsOverMaxLengthResult()
    {
        const string overMaxLength = "This is greater than 10 characters.";
        Validator<StringLengthMax10> validator = new();

        var result = validator.Validate(new() { Value = overMaxLength });

        Assert.False(result.IsValid);
        Assert.Collection(result.Errors, (kvp) =>
        {
            Assert.Equal(nameof(StringLengthMax10.Value), kvp.Key);
            Assert.Collection(kvp.Value, error =>
            {
                Assert.False(string.IsNullOrWhiteSpace(error));
                Assert.Equal(
                    MessageTemplateHandler.Interpolate(StringLengthAttribute.OverMaxLengthMessageTemplate, nameof(StringLengthMax10.Value), 10, overMaxLength.Length),
                    error);
                Output.WriteLine(error);
            });
        });
    }

    [Fact(Skip = "Blah blah")]
    public void StringLengthAttribute_IsUnderMinLength_ReturnsUnderMinLengthResult()
    {
        const string overMaxLength = "This is greater than 10 characters.";
        Validator<StringLengthMax10> validator = new();

        var result = validator.Validate(new() { Value = overMaxLength });

        Assert.False(result.IsValid);
        Assert.Collection(result.Errors, (kvp) =>
        {
            Assert.Equal(nameof(StringLengthMax10.Value), kvp.Key);
            Assert.Collection(kvp.Value, error =>
            {
                Assert.False(string.IsNullOrWhiteSpace(error));
                Assert.Equal(
                    MessageTemplateHandler.Interpolate(StringLengthAttribute.OverMaxLengthMessageTemplate, nameof(StringLengthMax10.Value), 10, overMaxLength.Length),
                    error);
            });
        });
    }

}