using System;
using System.Collections.Generic;
using BeardedManStudios.Forge.Networking;

public static class ThreadPipe
{
    struct ThreadAction
    {
        public Action<NetWorker> ActionOne;
        public Action<NetworkingPlayer, NetWorker> ActionTwo;
        public NetworkingPlayer Player;
        public NetWorker Sender;
    }

    static object threadLock = new object();
    static Queue<ThreadAction> Actions = new Queue<ThreadAction>();

    public static NetWorker.PlayerEvent Pipe(Action<NetworkingPlayer, NetWorker> action)
    {
        return (NetworkingPlayer player, NetWorker sender) =>
        {
            lock (threadLock)
            {
                Actions.Enqueue(new ThreadAction
                {
                    ActionTwo = action,
                    Player = player,
                    Sender = sender
                });
            }
        };
    }

    public static NetWorker.BaseNetworkEvent Pipe(Action<NetWorker> action)
    {
        return (NetWorker sender) =>
        {
            lock (threadLock)
            {
                Actions.Enqueue(new ThreadAction
                {
                    ActionOne = action,
                    Player = null,
                    Sender = sender
                });
            }
        };
    }

    // To be called from Main Thread (Update) !
    public static void ExecuteQueue()
    {
        lock (threadLock)
        {
            while (Actions.Count > 0)
            {
                ThreadAction next = Actions.Dequeue();
                if (next.Player == null)
                {
                    next.ActionOne(next.Sender);
                }
                else
                {
                    next.ActionTwo(next.Player, next.Sender);
                }
            }
        }
    }
}