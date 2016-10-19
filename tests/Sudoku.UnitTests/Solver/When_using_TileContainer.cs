using Moq;
using Sudoku.Solver.Interfaces;
using Sudoku.Solver.Types;
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

        [Fact]
        public void Should_chain_observer_updates_if_multiple_solutions()
        {
            var tileContainer2 = new TileContainer(1, 0);
            for (var i = 1; i < 7; i++)
                tileContainer2.RemovePossibleValue(i, true);

            tileContainer2.RemovePossibleValue(9, true);

            var tileContainer1 = new TileContainer(2, 0);
            for (var i = 1; i < 8; i++)
                tileContainer1.RemovePossibleValue(i, true);

            tileContainer2.Subscribe(tileContainer1);
            tileContainer1.Subscribe(tileContainer2);

            tileContainer1.Subscribe(_tileConainer);

            tileContainer1.Update(new TileContainer(0, 0, 9), false);
            tileContainer1.IsSolved(false);

            Assert.Null(tileContainer2.PossibleValues);
            Assert.Equal(7, tileContainer2.Solution);
        }

        [Fact]
        public void Should_remove_observer_if_it_was_not_a_guess()
        {
            var solvedTileContainer = new TileContainer(0, 0, 8);

            _tileConainer.Subscribe(solvedTileContainer);

            _tileConainer.Update(solvedTileContainer, true);

            Assert.False(_tileConainer.Observers.ContainsKey(solvedTileContainer.GetKey()));
        }

        [Fact]
        public void Should_remove_possible_value_if_update_called_and_not_solved()
        {
            var solvedTileContainer = new TileContainer(0, 0, 8);

            _tileConainer.Update(solvedTileContainer, false);

            Assert.False(_tileConainer.PossibleValues.Contains(8));
        }

        #region Test Observer
        [Fact]
        public void Should_add_subscriber_when_subscribe_is_called()
        {
            Mock<ISudokuObservable<TileContainer>> observer = new Mock<ISudokuObservable<TileContainer>>();
            observer.Setup(o => o.GetKey()).Returns("1-0");

            _tileConainer.Subscribe(observer.Object);
            Assert.Equal(1, _tileConainer.Observers.Count);
        }

        [Fact]
        public void Should_remove_subscriber_when_unsubscribe_is_called()
        {
            Mock<ISudokuObservable<TileContainer>> observer = new Mock<ISudokuObservable<TileContainer>>();
            observer.Setup(o => o.GetKey()).Returns("1-0");

            _tileConainer.Subscribe(observer.Object);
            Assert.Equal(1, _tileConainer.Observers.Count);

            _tileConainer.UnSubscribe(observer.Object);
            Assert.Equal(0, _tileConainer.Observers.Count);
        }

        [Fact]
        public void Should_call_subscriber_it_when_it_is_solved()
        {
            Mock<ISudokuObservable<TileContainer>> observer = new Mock<ISudokuObservable<TileContainer>>();
            observer.Setup(o => o.GetKey()).Returns("1-0");

            _tileConainer.Subscribe(observer.Object);

            //Lets solve it
            for(var i = 1;i < 9;i++)
            {
                _tileConainer.RemovePossibleValue(i, false);
                _tileConainer.IsSolved(false);
            }

            observer.Verify(o => o.Update(_tileConainer, false), Times.Once);
        }
        #endregion

        #region Test ICopyable 
        [Fact]
        public void Should_create_new_non_refernce_copy_of_container()
        {
            _tileConainer.PossibleValues.Remove(2);
            TileContainer copy = _tileConainer.Copy();
            _tileConainer.PossibleValues.Remove(3);

            Assert.Equal(_tileConainer.Row, copy.Row);
            Assert.Equal(_tileConainer.Column, copy.Column);
            Assert.Equal(_tileConainer.Solution, copy.Solution);
            Assert.False(copy.PossibleValues.Contains(2));
            Assert.True(copy.PossibleValues.Contains(3));

            copy.Row = 1;
            Assert.NotEqual(_tileConainer.Row, copy.Row);

        }
        #endregion

        #region Test IMomento
        [Fact]
        public void Should_be_able_to_revert_to_momento_state()
        {
            _tileConainer.Solution = 7;
            _tileConainer.SaveMomento();

            _tileConainer.PossibleValues.Remove(1);
            _tileConainer.Column = 9;
            _tileConainer.Row = 7;

            Assert.True(_tileConainer.TryRevertToMomento());

            Assert.Equal(9, _tileConainer.PossibleValues.Count);
            Assert.Equal(0, _tileConainer.Row);
            Assert.Equal(0, _tileConainer.Column);
            Assert.Equal(7, _tileConainer.Solution);
        }

        [Fact]
        public void Should_return_false_if_momento_was_never_saved()
        {
            Assert.False(_tileConainer.TryRevertToMomento());

        }
        #endregion
    }
}
