using Sudoku.Solver.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Sudoku.UnitTests.Solver
{
    public class When_using_TileContainer
    {
        private readonly TileContainer _tileConainer;

        public When_using_TileContainer()
        {
            _tileConainer = new TileContainer(0, 0);
        }

        #region Test ICopyable 
        [Fact]
        public void Should_create_new_non_refernce_copy_of_container()
        {
            _tileConainer.PossibleValues.Remove(2);
            TileContainer copy = _tileConainer.Copy();
            _tileConainer.PossibleValues.Remove(3);

            Assert.Equal(_tileConainer.Row, copy.Row);
            Assert.Equal(_tileConainer.Column, copy.Column);
            Assert.False(copy.PossibleValues.Contains(2));
            Assert.True(copy.PossibleValues.Contains(3));

            copy.Row = 1;
            Assert.NotEqual(_tileConainer.Row, copy.Row);

        }
        #endregion
    }
}
