using System;
using System.Collections.Generic;
using CodeDog.MemoryWoof.Interfaces;
using CodeDog.System;

namespace CodeDog.MemoryWoof.Models {

    public abstract class ComparerBase : IComparer {

        public abstract string Name { get; }
        public abstract string Description { get; }
        public string TestMessage { protected get; set; }

        protected ComparerState State;
        protected MemBlock<ulong> Sample;
        protected ulong Iteration;
        protected ulong Iterations;
        protected const ulong Updates = 50;
        protected ulong T = 0; // Tick every T iterations

        private List<ComparerError> ErrorList = new List<ComparerError>();
        protected bool IsNewError;

        public event EventHandler Initializing;
        public event EventHandler Initialized;
        public event EventHandler<ComparerStarted> Started;
        public event EventHandler<ComparerProgress> Progress;
        public event EventHandler<ComparerError> Error;
        public event EventHandler<ComparerState> Done;
        public event EventHandler<ComparerStats> Stats;

        protected virtual void AllocMem() {
            Sample = new MemBlock<ulong>(Memory.Available);
            State = new ComparerState {
                SampleSize = Sample.Size,
                Time_FirstStarted = DateTime.Now
            };
        }

        protected abstract void CreatePattern();

        protected virtual void Initialize() {
            if (Sample == null) AllocMem();
            if (Iterations < 1) {
                OnInitializing();
                CreatePattern();
                T = Iterations / Updates;
                OnInitialized();
            }
            OnStarted();
        }

        /// <summary>
        /// Must call Initialize(), OnProgress(), OnDone()!
        /// </summary>
        public abstract void Start();

        public virtual void Test(double gb) {
            Initialize();
            ulong target = (ulong)(gb * (double)0x40000000);
            var count = (int)Math.Ceiling(target / (double)Sample.Size);
            for (int i = 0; i < count; i++) {
                TestMessage = $"Test {i + 1:00}/{count:00} ";
                Start();
            }
            GetStats();
            Collect();
        }

        public virtual void GetStats() {
            OnStats(new ComparerStats {
                Tested = Memory.ToGibi(State.Tested_Total),
                Errors = State.Errors,
                Time = (State.Time_LastDone - State.Time_FirstStarted).TotalSeconds,
                Speed = Memory.ToGibi(State.Tested_Total) / (State.Time_LastDone - State.Time_FirstStarted).TotalSeconds,
                ErrorRatePer1TB = 0x10000000000 * State.Errors / (double)State.Tested_Total
            });
        }

        public virtual void Collect() {
            Sample = null;
            GC.Collect();
        }

        protected abstract void OnInitializing();

        protected abstract void OnInitialized();

        protected virtual void OnStarted() {
            State.Time_LastStarted = DateTime.Now;
            var handler = Started;
            if (handler != null) handler(this, new ComparerStarted { SampleSize = Sample.Size, Time = State.Time_LastStarted });
        }

        protected virtual void OnProgress() {
            var handler = Progress;
            if (handler != null) handler(this, new ComparerProgress { Offset = Iteration, Total = Sample.Length });
        }

        protected virtual void OnError(ComparerError e) {
            if (!ErrorList.Contains(e)) {
                IsNewError = true;
                ErrorList.Add(e);
                State.Errors++;
            }
            else IsNewError = false;
            var handler = Error;
            if (IsNewError && handler != null) handler(this, e);
        }

        protected virtual void OnDone() {
            State.Tested_Last = Sample.Size;
            State.Tested_Total += State.Tested_Last;
            State.Time_LastDone = DateTime.Now;
            var time = (State.Time_LastDone - State.Time_LastStarted).TotalSeconds;
            State.Speed = Memory.ToGibi(State.Tested_Last) / (double)time;
            var handler = Done;
            if (handler != null) handler(this, State);
        }

        protected virtual void OnStats(ComparerStats e) {
            var handler = Stats;
            if (handler != null) handler(this, e);
        }

    }

}