using System.Collections.Generic;

public class PriorityQueue<T>
{
    private List<(T item, int priority)> list = new();

    public int Count => list.Count;

    public void Enqueue(T item, int priority)
    {
        list.Add((item, priority));
    }

    public T Dequeue()
    {
        int best = 0;
        for (int i = 1; i < list.Count; i++)
        {
            if (list[i].priority < list[best].priority)
                best = i;
        }

        T item = list[best].item;
        list.RemoveAt(best);
        return item;
    }
}
