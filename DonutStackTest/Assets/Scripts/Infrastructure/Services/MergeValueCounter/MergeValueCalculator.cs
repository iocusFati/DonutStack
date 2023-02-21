using System;
using Gameplay.DonutFolder.Merging;

namespace Infrastructure.Services.MergeValueCounter
{
    public class MergeValueCalculator : IMergeValueCalculatorService
    {
        private ValueInt _frontValue = new ValueInt();
        private ValueInt _backValue = new ValueInt();
        private ValueInt _rightValue = new ValueInt();
        private ValueInt _leftValue = new ValueInt();

        private ValueInt _currentValue;
        private ValueInt[] _values;

        public void Initialize()
        {
            _values = new[]
            {
                _frontValue,
                _backValue,
                _rightValue,
                _leftValue
            };
        }

        public void Start(MergeDirection direction)
        {
            _currentValue = direction switch
            {
                MergeDirection.Front => _frontValue,
                MergeDirection.Back => _backValue,
                MergeDirection.Right => _rightValue,
                MergeDirection.Left => _leftValue,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }

        public void RaiseValue()
        {
            _currentValue.Value++;
        }
    }
}