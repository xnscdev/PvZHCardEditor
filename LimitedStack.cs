using System;

namespace PvZHCardEditor
{
    public class LimitedStack<T>
    {
        private readonly int _maxCapacity;
        private readonly T[] _data;
        private int _head;
        private int _count;

        public int Count => _count;

        public LimitedStack(int maxCapacity)
        {
            _maxCapacity = maxCapacity;
            _data = new T[maxCapacity];
            _head = -1;
            _count = 0;
        }

        public void Push(T value)
        {
            if (_count == _maxCapacity)
            {
                for (var i = 1; i < _count; i++)
                    _data[i - 1] = _data[i];
                _head--;
                _count--;
            }

            _data[++_head] = value;
            _count++;
        }

        public T Pop()
        {
            if (_count == 0)
                throw new InvalidOperationException("Cannot pop empty stack");
            _count--;
            return _data[_head--];
        }

        public T Peek()
        {
            if (_count == 0)
                throw new InvalidOperationException("Cannot peek empty stack");
            return _data[_head];
        }

        public bool TryPop(out T result)
        {
            if (_count == 0)
            {
                result = default!;
                return false;
            }
            else
            {
                result = Pop();
                return true;
            }
        }

        public bool TryPeek(out T result)
        {
            if (_count == 0)
            {
                result = default!;
                return false;
            }
            else
            {
                result = Peek();
                return true;
            }
        }

        public void Clear()
        {
            _head = -1;
            _count = 0;
        }
    }
}
