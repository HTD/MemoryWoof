using System;
using CodeDog.MemoryWoof.Models;

namespace CodeDog.MemoryWoof.Interfaces {

    public interface IComparer {

        string Name { get; }
        string Description { get; }
        string TestMessage { set; }

        void Start();
        void GetStats();
        void Test(double gb);

        event EventHandler Initializing;
        event EventHandler Initialized;
        event EventHandler<ComparerStarted> Started;
        event EventHandler<ComparerProgress> Progress;
        event EventHandler<ComparerError> Error;
        event EventHandler<ComparerState> Done;
        event EventHandler<ComparerStats> Stats;

    }

}
