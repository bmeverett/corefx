// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text;
using Xunit;

namespace System.Globalization.Tests
{
    public class StringNormalizationTests
    {
        [Theory]
        [InlineData("\u00C4\u00C7", NormalizationForm.FormC, true)]
        [InlineData("\u00C4\u00C7", NormalizationForm.FormD, false)]
        [InlineData("A\u0308C\u0327", NormalizationForm.FormC, false)]
        [InlineData("A\u0308C\u0327", NormalizationForm.FormD, true)]
        public void IsNormalized(string value, NormalizationForm normalizationForm, bool expected)
        {
            if (normalizationForm == NormalizationForm.FormC)
            {
                Assert.Equal(expected, value.IsNormalized());
            }
            Assert.Equal(expected, value.IsNormalized(normalizationForm));
        }

        [Fact]
        [PlatformSpecific(~TestPlatforms.Windows)]
        public void IsNormalized_Invalid_NonWindows()
        {
            Assert.Throws<ArgumentException>(() => "\uFB01".IsNormalized((NormalizationForm)10));

            Assert.Throws<ArgumentException>("strInput", () => "\uFFFE".IsNormalized()); // Invalid codepoint
            Assert.Throws<ArgumentException>("strInput", () => "\uD800\uD800".IsNormalized()); // Invalid surrogate pair

            Assert.Throws<ArgumentNullException>("strInput", () => StringNormalizationExtensions.IsNormalized(null));

            Assert.Throws<ArgumentNullException>("strInput", () => ((string)null).IsNormalized());
        }

        [Fact]
        [PlatformSpecific(TestPlatforms.Windows)]
        [SkipOnTargetFramework(TargetFrameworkMonikers.NetFramework | TargetFrameworkMonikers.Netcoreapp)]
        public void IsNormalized_Invalid_NetCore()
        {
            Assert.Throws<ArgumentException>(() => "\uFB01".IsNormalized((NormalizationForm)10));

            Assert.Throws<ArgumentException>("strInput", () => "\uFFFE".IsNormalized()); // Invalid codepoint
            Assert.Throws<ArgumentException>("strInput", () => "\uD800\uD800".IsNormalized()); // Invalid surrogate pair

            Assert.Throws<ArgumentNullException>("strInput", () => StringNormalizationExtensions.IsNormalized(null));

            Assert.Throws<ArgumentNullException>("strInput", () => ((string)null).IsNormalized());
        }

        [Fact]
        [PlatformSpecific(TestPlatforms.Windows)]
        [SkipOnTargetFramework(~(TargetFrameworkMonikers.NetFramework | TargetFrameworkMonikers.Netcoreapp))]
        public void IsNormalized_Invalid_Desktop()
        {
            Assert.Throws<ArgumentException>(() => "\uFB01".IsNormalized((NormalizationForm)10));

            Assert.Throws<ArgumentException>("strInput", () => "\uFFFE".IsNormalized()); // Invalid codepoint
            Assert.Throws<ArgumentException>("strInput", () => "\uD800\uD800".IsNormalized()); // Invalid surrogate pair

            Assert.Throws<ArgumentNullException>("strInput", () => StringNormalizationExtensions.IsNormalized(null));

            // On desktop IsNormalized is not extension method, trying to do ((string)null).IsNormalized()
            // will get NullReferenceException, in .Net Core we use extension method which will throw
            // ArgumentNullException
            Assert.Throws<NullReferenceException>(() => ((string)null).IsNormalized());
        }

        [Theory]
        [InlineData("", NormalizationForm.FormC, "")]
        [InlineData("\u00C4\u00C7", NormalizationForm.FormD, "A\u0308C\u0327")]
        [InlineData("A\u0308C\u0327", NormalizationForm.FormC, "\u00C4\u00C7")]
        [InlineData("\uFB01", NormalizationForm.FormC, "\uFB01")]
        [InlineData("\uFB01", NormalizationForm.FormD, "\uFB01")]
        [InlineData("\uFB01", NormalizationForm.FormKC, "fi")]
        [InlineData("\uFB01", NormalizationForm.FormKD, "fi")]
        [InlineData("\u1E9b\u0323", NormalizationForm.FormC, "\u1E9b\u0323")]
        [InlineData("\u1E9b\u0323", NormalizationForm.FormD, "\u017f\u0323\u0307")]
        [InlineData("\u1E9b\u0323", NormalizationForm.FormKC, "\u1E69")]
        [InlineData("\u1E9b\u0323", NormalizationForm.FormKD, "\u0073\u0323\u0307")]
        public void Normalize(string value, NormalizationForm normalizationForm, string expected)
        {
            if (normalizationForm == NormalizationForm.FormC)
            {
                Assert.Equal(expected, value.Normalize());
            }
            Assert.Equal(expected, value.Normalize(normalizationForm));
        }

        [Fact]
        [PlatformSpecific(~TestPlatforms.Windows)]
        public void Normalize_Invalid_NonWindows()
        {
            //Assert.True(false, "Framework not skipped");
            Assert.Throws<ArgumentException>(() => "\uFB01".Normalize((NormalizationForm)7));

            Assert.Throws<ArgumentException>("strInput", () => "\uFFFE".Normalize()); // Invalid codepoint
            Assert.Throws<ArgumentException>("strInput", () => "\uD800\uD800".Normalize()); // Invalid surrogate pair

            Assert.Throws<ArgumentNullException>("strInput", () => StringNormalizationExtensions.Normalize(null));

            Assert.Throws<ArgumentNullException>("strInput", () => ((string)null).Normalize());
        }

        [Fact]
        [PlatformSpecific(TestPlatforms.Windows)]
        [SkipOnTargetFramework(TargetFrameworkMonikers.NetFramework | TargetFrameworkMonikers.Netcoreapp)]
        public void Normalize_Invalid_NetCore()
        {
            Assert.Throws<ArgumentException>(() => "\uFB01".Normalize((NormalizationForm)7));

            Assert.Throws<ArgumentException>("strInput", () => "\uFFFE".Normalize()); // Invalid codepoint
            Assert.Throws<ArgumentException>("strInput", () => "\uD800\uD800".Normalize()); // Invalid surrogate pair

            Assert.Throws<ArgumentNullException>("strInput", () => StringNormalizationExtensions.Normalize(null));

            Assert.Throws<ArgumentNullException>("strInput", () => ((string)null).Normalize());
        }

        [Fact]
        [PlatformSpecific(TestPlatforms.Windows)]
        [SkipOnTargetFramework(~(TargetFrameworkMonikers.NetFramework | TargetFrameworkMonikers.Netcoreapp))]
        public void Normalize_Invalid_Desktop()
        {
            Assert.Throws<ArgumentException>(() => "\uFB01".Normalize((NormalizationForm)7));

            Assert.Throws<ArgumentException>("strInput", () => "\uFFFE".Normalize()); // Invalid codepoint
            Assert.Throws<ArgumentException>("strInput", () => "\uD800\uD800".Normalize()); // Invalid surrogate pair

            Assert.Throws<ArgumentNullException>("strInput", () => StringNormalizationExtensions.Normalize(null));

            // On desktop Normalize is not extension method, trying to do ((string)null).Normalize()
            // will get NullReferenceException, in .Net Core we use extension method which will throw
            // ArgumentNullException
            Assert.Throws<NullReferenceException>(() => ((string)null).Normalize());
        }
    }
}
