using System;
using System.Collections;

/// <summary>
/// Abstract class for static binary heap
/// </summary>
/// <typeparam name="TItem">Item in heap.</typeparam>
public class Heap<TItem> : IEnumerator, IEnumerable where TItem : IHeapItem<TItem>
{
    #region F/P
    TItem[] items = null;

    int currentItemCount = 0,
        currentIndex = -1;

    public TItem this[int _index]
    {
        get => _index < 0 || _index >= items.Length ? default(TItem) : items[_index];
        set
        {
            if (_index < 0 || _index >= items.Length)
                return;

            items[_index] = value;
        }
    }

    public int Count => currentItemCount;

    public object Current => items[currentIndex];
    #endregion

    #region Constructor
    public Heap(int _maxHeapSize)
    {
        items = new TItem[_maxHeapSize];
    }
    #endregion

    #region Methods

    #region Indexing
    int ParentIndex(int _index) => (_index - 1) / 2;

    int LeftChildIndex(int _index) => _index * 2 + 1;

    int RightChildIndex(int _index) => _index * 2 + 2;
    #endregion

    #region Sorting
    void SortUp(TItem _item)
    {
        int _parentIndex = ParentIndex(_item.HeapIndex);
        TItem _parent = items[_parentIndex];

        while (_item.CompareTo(_parent) < 0)
        {
            Swap(_item, _parent);
            _parentIndex = ParentIndex(_item.HeapIndex);
            _parent = items[_parentIndex];
        }
    }

    void SortDown(TItem _item)
    {
        int _leftChildIndex = LeftChildIndex(_item.HeapIndex),
            _rightChildIndex = RightChildIndex(_item.HeapIndex),
            _swapIndex = _leftChildIndex;

        while (_leftChildIndex < currentItemCount)
        {
            if (_rightChildIndex < currentItemCount)
                _swapIndex = items[_leftChildIndex].CompareTo(items[_rightChildIndex]) > 0 ? _rightChildIndex : _leftChildIndex;

            if (_item.CompareTo(items[_swapIndex]) <= 0)
                return;

            Swap(_item, items[_swapIndex]);
            _leftChildIndex = LeftChildIndex(_item.HeapIndex);
            _rightChildIndex = RightChildIndex(_item.HeapIndex);
            _swapIndex = _leftChildIndex;
        }
    }

    void Swap(TItem _itemA, TItem _itemB)
    {
        items[_itemA.HeapIndex] = _itemB;
        items[_itemB.HeapIndex] = _itemA;

        int _temp = _itemA.HeapIndex;
        _itemA.HeapIndex = _itemB.HeapIndex;
        _itemB.HeapIndex = _temp;
    }
    #endregion

    #region Adding
    /// <summary>
    /// Add an item at sort t in the heap.
    /// </summary>
    /// <param name="_item">Item to add</param>
    public void Add(TItem _item)
    {
        _item.HeapIndex = currentItemCount;
        items[currentItemCount] = _item;
        SortUp(_item);
        currentItemCount++;
    }
    #endregion

    #region Extracting
    public TItem ExtractFirst() => Extract(0);

    /// <summary>
    /// Remove item at index passed in parametersand returns it.
    /// Replace extracted item by last item and sort down it.
    /// </summary>
    /// <param name="_index">Items index</param>
    /// <returns>Item</returns>
    public TItem Extract(int _index)
    {
        if (_index < 0 || _index >= items.Length)
            return default(TItem);

        TItem _extractedItem = items[_index];
        currentItemCount--;
        items[_index] = items[currentItemCount];
        items[_index].HeapIndex = _index;
        SortDown(items[_index]);
        return _extractedItem;
    }

    public TItem Extract(TItem _item) => Extract(_item.HeapIndex);

    public TItem ExtractLast()
    {
        TItem _lastItem = items[currentItemCount];
        currentItemCount--;
        return _lastItem;
    }
    #endregion

    #region CollectionCommun
    public bool Contains(TItem _item)
    {
        if (_item.HeapIndex < 0 || _item.HeapIndex >= currentItemCount)
            return false;

        return Equals(items[_item.HeapIndex], _item);
    }

    public void Clear() => currentItemCount = 0;
    #endregion

    #region Enumerator
    public bool MoveNext()
    {
        currentIndex++;

        if(currentIndex >= Count)
        {
            Reset();
            return false;
        }
        return true;
    }

    public void Reset() => currentIndex = -1;

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this;
    }
    #endregion
    #endregion
}

/// <summary>
/// Interface that every item to stock in a heap must implement.
/// Contains heap index.
/// </summary>
/// <typeparam name="TItem">Item to stock in the heap.</typeparam>
public interface IHeapItem<TItem> : IComparable<TItem>
{
    public int HeapIndex { get; set; }
}
