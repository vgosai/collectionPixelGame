using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using System.Linq;
using System;

public static class RoomOrganizer
{
    public static void ClassifyRooms(Dictionary<Vector2Int, Dictionary<Vector2Int, HashSet<Vector2Int>>> roomCorridors, HashSet<Vector2Int> corridors)
    {
        int numRooms = roomCorridors.Count;
        Dictionary<Vector2Int, int> dist = Dijkstras(roomCorridors, corridors, numRooms);
        foreach (var room in roomCorridors)
        {

        }
        //PriorityQueue<Vector2Int, int> pq = new PriorityQueue<Vector2Int, int>();

    }

    public static Dictionary<Vector2Int, int> Dijkstras(Dictionary<Vector2Int, Dictionary<Vector2Int, HashSet<Vector2Int>>> roomCorridors, HashSet<Vector2Int> corridors, int numRooms)
    {
        Dictionary<Vector2Int, int> roomDistances = new Dictionary<Vector2Int, int>();
        Vector2Int spawnRoomLocation = roomCorridors.ElementAt(Random.Range(1, numRooms)).Key;
        roomDistances.Add(spawnRoomLocation, 0);
        foreach (var room in roomCorridors)
        {
            if (room.Key != spawnRoomLocation)
            {
                roomDistances.Add(room.Key, 10000);
            }
        }
        PriorityQueue<(Vector2Int, int), int> pq = new PriorityQueue<(Vector2Int, int), int>();
        pq.Enqueue((spawnRoomLocation, 0), 0);
        while (pq.Count != 0)
        {
            var vertex = pq.Pop();
            if (vertex.Item2 > roomDistances[vertex.Item1])
            {
                continue;
            }
            foreach (var adj in roomCorridors[vertex.Item1]) //gets each adjacent room's room center and connecting corridor hashset
            {
                if (roomDistances[vertex.Item1] + roomCorridors[vertex.Item1][adj.Key].Count < roomDistances[adj.Key])
                {
                    roomDistances[adj.Key] = roomDistances[vertex.Item1] + roomCorridors[vertex.Item1][adj.Key].Count;
                    pq.Enqueue((adj.Key, roomDistances[adj.Key]), roomDistances[adj.Key]);
                }
            }
        }
        return roomDistances;
    }
    /*
    function Dijkstra(G, s):
    for v in V: dist[v] = +∞
    dist[s] = 0
    PQ = min-priority-queue()
    PQ.push((0, s))

    while PQ not empty:
        (du, u) = PQ.popMin()
        if du > dist[u]: continue
        for (u, v, w) in G.adj[u]:
            if dist[u] + w < dist[v]:
                dist[v] = dist[u] + w
                PQ.push((dist[v], v))

    return dist
    */

    public class PriorityQueue<TElement, TPriority>
    {
        private readonly List<(TPriority p, TElement e)> _heap = new();
        private readonly IComparer<TPriority> _cmp;
        public int Count => _heap.Count;

        public PriorityQueue(IComparer<TPriority> comparer = null)
        {
            _cmp = comparer ?? Comparer<TPriority>.Default; // min-heap by default
        }

        public void Enqueue(TElement element, TPriority priority)
        {
            _heap.Add((priority, element));
            SiftUp(_heap.Count - 1);
        }

        // Pop the next-by-priority item (throws if empty)
        public TElement Pop()
        {
            var (e, _) = Dequeue();
            return e;
        }

        // Dequeue returns both element and priority (throws if empty)
        public (TElement element, TPriority priority) Dequeue()
        {
            if (_heap.Count == 0) throw new SystemException("Queue is empty");

            var root = _heap[0];
            var last = _heap[^1];
            _heap.RemoveAt(_heap.Count - 1);
            if (_heap.Count > 0)
            {
                _heap[0] = last;
                SiftDown(0);
            }
            return (root.e, root.p);
        }

        public bool TryDequeue(out TElement element, out TPriority priority)
        {
            if (_heap.Count == 0) { element = default; priority = default; return false; }
            (element, priority) = Dequeue();
            return true;
        }

        public (TElement element, TPriority priority) Peek()
        {
            if (_heap.Count == 0) throw new InvalidOperationException("Queue is empty");
            var (p, e) = _heap[0];
            return (e, p);
        }

        private void SiftUp(int i)
        {
            while (i > 0)
            {
                int parent = (i - 1) / 2;
                if (_cmp.Compare(_heap[i].p, _heap[parent].p) >= 0) break;
                (_heap[i], _heap[parent]) = (_heap[parent], _heap[i]);
                i = parent;
            }
        }

        private void SiftDown(int i)
        {
            int n = _heap.Count;
            while (true)
            {
                int l = 2 * i + 1, r = l + 1, m = i;
                if (l < n && _cmp.Compare(_heap[l].p, _heap[m].p) < 0) m = l;
                if (r < n && _cmp.Compare(_heap[r].p, _heap[m].p) < 0) m = r;
                if (m == i) break;
                (_heap[i], _heap[m]) = (_heap[m], _heap[i]);
                i = m;
            }
        }
    }
}
