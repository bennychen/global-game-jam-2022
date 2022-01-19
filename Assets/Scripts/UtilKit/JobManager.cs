using System.Collections;
using System;
using System.Collections.Generic;

public class JobManager : MonoSingleton<JobManager>
{
    public void StopAll()
    {
        StopAllCoroutines();
        _jobs.Clear();
    }

    public void AddJob(Job job)
    {
        _jobs.Add(job);
    }

    public void RemoveJob(Job job)
    {
        _jobs.Remove(job);
    }

    public static void Stop(object client)
    {
        if (IsQuiting)
        {
            return;
        }

        List<Job> jobsToStop = new List<Job>();
        for (int i = 0; i < _jobs.Count; i++)
        {
            if (_jobs[i].Client.IsAlive && 
                _jobs[i].Client.Target == client)
            {
                jobsToStop.Add(_jobs[i]);
            }
        }

        for (int i = 0; i < jobsToStop.Count; i++)
        {
            jobsToStop[i].Kill();
        }
    }

    private static List<Job> _jobs = new List<Job>();
}

public class Job
{
    public event System.Action<bool> OnComplete;

    public WeakReference Client { get; private set; }
    public bool Running { get; private set; }
    public bool Paused { get; private set; }

    public Job(IEnumerator coroutine, bool startImmediately, object client)
        : this(coroutine, true, startImmediately, client)
    { }

    public Job(IEnumerator coroutine, bool shouldStart, bool startImmediately, object client)
    {
        _coroutine = coroutine;
        _startImmediately = startImmediately;
        if (!object.ReferenceEquals(client, null))
        {
            Client = new WeakReference(client);
            JobManager.Instance.AddJob(this);
        }
        if (shouldStart)
            Start();
    }

    public void Start()
    {
        Running = true;
        JobManager.Instance.StartCoroutine(DoWork());
    }

    public IEnumerator StartAsCoroutine()
    {
        Running = true;
        yield return JobManager.Instance.StartCoroutine(DoWork());
    }

    public void Pause()
    {
        Paused = true;
    }

    public void Unpause()
    {
        Paused = false;
    }

    public void Kill()
    {
        _jobWasKilled = true;
        Running = false;
        Paused = false;
    }

    public void Kill(float delayInSeconds)
    {
        var delay = (int)(delayInSeconds * 1000);
        new System.Threading.Timer(obj =>
        {
            lock (this)
            {
                Kill();
            }
        }, null, delay, System.Threading.Timeout.Infinite);
    }

    //If you need maintain coroutine's life cycle, you need pass client parameter, and 
    // call JobManager.Stop(client) to stop this coroutine at the proper time.

    public static Job MakeImmediately(IEnumerator coroutine, object client = null)
    {
        return new Job(coroutine, true, client);
    }

    public static Job Make(IEnumerator coroutine, object client = null)
    {
        return new Job(coroutine, false, client);
    }

    public static Job Make(IEnumerator coroutine, bool shouldStart, bool startImmediately, object client)
    {
        return new Job(coroutine, shouldStart, startImmediately, client);
    }

    private IEnumerator DoWork()
    {
        if (!_startImmediately)
        {
            // null out the first run through in case we start paused
            yield return null;
        }

        while (Running)
        {
            if (Paused)
            {
                yield return null;
            }
            else
            {
                // run the next iteration and stop if we are done
                if (_coroutine.MoveNext())
                {
                    yield return _coroutine.Current;
                }
                else
                {
                    Running = false;
                }
            }
        }

        if (Client != null)
        {
            JobManager.Instance.RemoveJob(this);
        }

        if (OnComplete != null)
        {
            OnComplete(_jobWasKilled);
        }
    }

    private IEnumerator _coroutine;
    private bool _jobWasKilled;
    private bool _startImmediately;
}