using System;
using CodeDog.Core;
using CodeDog.Core.ConsoleExtensions;

namespace CodeDog.MemoryWoof.Models {

    public abstract class ComparerConsole : ComparerBase {

        protected ConsoleEx X = Program.X;
        protected ConsoleProgress P;

        protected void Msg(string s = null, params object[] arg) => X.Write(s, arg);
        protected void MsgLine(string s = null, params object[] arg) => X.WriteLine(s, arg);

        protected override void AllocMem() {
            base.AllocMem();
        }

        protected override void OnInitializing() {
            MsgLine($":: <special1>{Name}</special1> :: <special2>{Description}</special2>");
            MsgLine();
            MsgLine($"Sample size: {Memory.ToGibi(Sample.Size)}GB");
            Msg("Creating test pattern...");
        }

        protected override void OnInitialized() {
            MsgLine("DONE.");
        }

        protected override void OnStarted() {
            base.OnStarted();
            if (!String.IsNullOrWhiteSpace(TestMessage)) Msg(TestMessage);
            P = new ConsoleProgress(X);
        }

        protected override void OnProgress() {
            base.OnProgress();
            lock (ConsoleState.Lock) P.Dot();
        }

        protected override void OnError(ComparerError e) {
            base.OnError(e);
            if (!IsNewError) return;
            lock (ConsoleState.Lock) {
                MsgLine(
                    "  <error-details> * A: [{0}] = {1}, B: [{2}] = {3}, A ^ B = {4}</error-details>",
                    e.OffsetA.ToString("x16"),
                    e.A.ToString("x16"),
                    e.OffsetB.ToString("x16"),
                    e.B.ToString("x16"),
                    (e.A ^ e.B).ToString("x16")
                );
                X.Boop();
            }
        }

        protected override void OnDone() {
            base.OnDone();
            lock (ConsoleState.Lock) {
                var status = IsNewError ? "<error>ERROR!</error>" : "<ok>OK.</ok>";
                var speed = $"<special1>{State.Speed:0.000}GB/s</special1>";
                P.Done($" {status} {speed}");
            }
        }

        protected override void OnStats(ComparerStats e) {
            base.OnStats(e);
            MsgLine();
            if (e.ErrorRatePer1TB > 0) {
                MsgLine(
                    "<error>Error rate: {0:0} / 1TB.</error> {1:0.0}GB tested in {2:0.0}s. Average test speed: {3:0.0}GB/s.",
                    e.ErrorRatePer1TB, e.Tested, e.Time, e.Speed
                );
            } else {
                MsgLine(
                    "<strong>No errors.</strong> {0:0.0}GB tested in {1:0.0}s. Average test speed: {2:0.0}GB/s.",
                    e.Tested, e.Time, e.Speed
                );
            }
        }

    }

}