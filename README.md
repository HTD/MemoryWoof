# Memory Woof
PC Motherboard Diagnostic Tool

This is a kind of scientific work...

## Thesis:
Memory I/O in modern PC systems can be unreliable due to invalid motherboard configuration or a component failure
and this unreliability can be detected with diagnostic software running in protected mode of Windows operating system.

## Proof:
This program tests memory I/O using following algorithm:
- fills all available memory with test pattern (XorShift* Marsaglia PRNG used)
- compares every previously written location with computed pattern data
- given damaged or misconfigured motherboard - this program detects and describes differences between stored and computed data

## Important findings:
- BIOS settings like memory timing, clock frequencies, supply voltages and various CPU settings can be responsible for memory I/O unreliability
- some memory unreliability problems can be fixed with firmware settings alone when reliably detected
- some memory unreliability problems CANNOT BE DETECTED with standard memory testing tools like memtest86
- some memory unreliability problems can be detected EXCLUSIVELY WITH PSEUDO RANDOM SEQUENCE transfered to and from memory
- memory unreliability problems undetected with memtest86 are unlikely related to memory modules themselves
- 64-bit operations gives over 2 times better performance with slightly less detection capability due to decreased test duration
    
## Aditional findings:
- allocating amout of memory close to amout reported as "Free physical memory" by Windows systems can be dangerous, lead to uncontrolled swap file usage, make system unresponsive, cause system crash and, last but not least - DAMAGE SSD DRIVE
- to mitigate the risk of Windows memory exhaustion one should never allocate more than 512MB less amount than "Free physical memory" reported by Windows system.

## System requirements / code description:
This software is intended to run only on 64-bit version of Microsoft Windows 8.1 or newer.
More than 4GB of RAM is highly recomended, though it's tested for 4GB configuration and works.

Special thanks to [SpankyJ](http://blogs.msdn.com/b/joshwil/archive/2005/08/10/450202.aspx) for [BigArray<T>](http://blogs.msdn.com/b/joshwil/archive/2005/08/10/450202.aspx) idea.
I tested varius approaches to iterate over large blocks of memory, including usafe pointer code but his "memory paging" algorithm is the fastest you can get in C#. The key thing is indexer which performs only bit shifting to find cell location.

## About the test

I had to work (using my PC). I had to download and unzip some files. Nothing special, just about 0.5GB of data.
Unexpectedly I got CRC error! I downloaded the archive again, and again I got CRC error, on the different file this time.
I redownloaded the file and this time there was no CRC errors. Weird, huh?

It seemed like my PC was broken. So I started to diagnose it. First of course: [memtest86](http://www.memtest86.com/download.htm).
After devastating hours of waiting for test to complete I got nothing. Seems like at least DRAM in my PC is fine.

Then maybe - some RAM - CPU issue? So I tried [System Stability Tester](http://sourceforge.net/projects/systester/). Again, nothing.
According to this fine tool my PC is perfectly fine.

No. It wasn't. If I took any big file (even like 0.5GB) and copied it, then compared the copies or checked CRC -
quite often the copies differed. So my PC was broken, but no diagnostic tool I knew could even detect it.

Oh, BTW, I tried gaming. Over 100 hours of Fallout 4 and almost no crashes. **No crashes in Bethesda game!** Again: weird.

So I wrote this diagnostic tool to measure estimated memory I/O error rate.
Then I fixed my broken PC by fine-tuning my memory timing settings in BIOS and some advanced CPU settings. Totally worth it.

Bonus - it benchmarks your motherboard, see GB/s spped result.

The program accepts a number as an only argument. This is test size in gigabytes. Default is 100 (when no argument given).
You can specify numbers like 12.34 or 1000. Be careful though, for lower values the detection rate would be next to zero,
for higher - it would take ages to complete.
