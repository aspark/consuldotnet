using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Consul.Test
{
    public class OptionsTest
    {
        [Fact]
        public void Equal()
        {
            var options = FilterOptions.CreateEqual("ID", 0);

            Assert.Equal("(ID == 0)", options.GetValue());

            options = FilterOptions.CreateEqual("ID", "");

            Assert.Equal("(ID == \"\")", options.GetValue());
        }

        [Fact]
        public void NotEqual()
        {
            var options = FilterOptions.CreateNotEqual("ID", 0);

            Assert.Equal("(ID != 0)", options.GetValue());
        }

        [Fact]
        public void IsEmpty()
        {
            var options = FilterOptions.CreateIsEmpty("ID");

            Assert.Equal("(ID is empty)", options.GetValue());
        }

        [Fact]
        public void IsNotEmpty()
        {
            var options = FilterOptions.CreateIsNotEmpty("ID");

            Assert.Equal("(ID is not empty)", options.GetValue());
        }

        [Fact]
        public void In()
        {
            var options = FilterOptions.CreateIn(0, "ID");

            Assert.Equal("(0 in ID)", options.GetValue());
        }

        [Fact]
        public void NotIn()
        {
            var options = FilterOptions.CreateNotIn(0, "ID");

            Assert.Equal("(0 not in ID)", options.GetValue());
        }

        [Fact]
        public void Contains()
        {
            var options = FilterOptions.CreateContains("ID", 0);

            Assert.Equal("(ID contains 0)", options.GetValue());
        }

        [Fact]
        public void NotContains()
        {
            var options = FilterOptions.CreateNotContains("ID", 0);

            Assert.Equal("(ID not contains 0)", options.GetValue());
        }

        [Fact]
        public void Composite()
        {
            var options = FilterOptions.CreateNotContains("ID", 0);
            options = options.And(FilterOptions.CreateEqual("ID", 1));
            Assert.Equal("((ID not contains 0) and (ID == 1))", options.GetValue());

            options = options.Or(FilterOptions.CreateIsEmpty("ID"));
            Assert.Equal("(((ID not contains 0) and (ID == 1)) or (ID is empty))", options.GetValue());

            options = options.And(FilterOptions.CreateContains("ID", 2).Not());
            Assert.Equal("((((ID not contains 0) and (ID == 1)) or (ID is empty)) and (not (ID contains 2)))", options.GetValue());
        }
    }
}