using Sudoku.Types;
using System;
using Xunit;

namespace Sudoku.UnitTests.Types
{
    public class When_using_BoardType
    {
        [Fact]
        public void Should_instantiate_with_null_int_array()
        {
            Board board = new Board();

            foreach (var tile in board.Tiles)
            {
                Assert.True(tile == null);
            }
        }

        [Fact]
        public void Should_set_the_correct_tile()
        {
            Board board = new Board();
            var success = board.TrySet(1, 0, 0);

            Assert.True(success);
            Assert.Equal(1, board.Tiles[0, 0]);
        }

        [Fact]
        public void Should_not_set_if_already_set()
        {
            Board board = new Board();
            board.TrySet(5, 0, 0);

            var success = board.TrySet(1, 0, 0);

            Assert.False(success);
        }

        [Fact]
        public void Should_not_set_if_negative()
        {
            Board board = new Board();

            var success = board.TrySet(-1, 0, 0);

            Assert.False(success);
        }

        [Fact]
        public void Should_not_set_if_over_9()
        {
            Board board = new Board();

            var success = board.TrySet(10, 0, 0);

            Assert.False(success);
        }

        [Fact]
        public void Should_throw_exception_if_index_out_of_bounds()
        {
            Board board = new Board();

            Assert.Throws<IndexOutOfRangeException>(() => board.TrySet(7, 9, 0));
        }

        [Fact]
        public void Should_not_set_if_over_0()
        {
            Board board = new Board();

            var success = board.TrySet(0, 0, 0);

            Assert.False(success);
        }

        [Theory]
        [InlineData(3, 0, 1, 8)]
        [InlineData(2, 3, 4, 2)]
        [InlineData(5, 2, 7, 1)]
        public void Should_not_set_if_one_all_ready_in_row(int value, int x1, int x2, int y)
        {
            Board board = new Board();

            board.TrySet(value, x1, y);

            var success = board.TrySet(value, x2, y);

            Assert.False(success);
        }

        [Theory]
        [InlineData(1, 2, 3, 4)]
        [InlineData(3, 2, 8, 5)]
        [InlineData(9, 3, 7, 1)]
        public void Should_not_set_if_one_all_ready_in_column(int value, int x, int y1, int y2)
        {
            Board board = new Board();

            board.TrySet(value, x, y1);

            var success = board.TrySet(value, x, y2);

            Assert.False(success);
        }

        [Theory]
        [InlineData(1, 0, 2, 2, 1)]
        [InlineData(9, 8, 8, 7, 7)]
        [InlineData(7, 4, 5, 3, 4)]
        [InlineData(4, 0, 8, 2, 7)]
        [InlineData(9, 8, 3, 7, 4)]
        [InlineData(5, 4, 7, 3, 6)]
        public void Should_not_set_if_one_all_ready_in_area(int value, int x1, int y1, int x2, int y2)
        {
            Board board = new Board();

            board.TrySet(value, x1, y1);

            var success = board.TrySet(value, x2, y2);

            Assert.False(success);
        }
    }
}
