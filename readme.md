Summary
=======
This program reads a CSV tab separated flow of data begining by timestamps (ms)  and outputs a new replay file which simulates sampling from a network engine as well as latency and packetloss.

Running the program
===================

In order to compile and run the program, the .NET Core SDK (v2) is required.

For instance, to run the program with the data source D1.csv (in the current directory) with 10% packet loss, a minimum latency of 100ms, 
a max latency of 130ms and a sampling rate of 50ms, then output the result to test.csv, run the following command in the directory.

    dotnet run --inputPath="D1.csv" --outputPath="D1-out.csv" --packetLoss=0.1 --minLatency=100 --maxLatency=130 --sampling=50 >>test.csv

Format of the output file
=========================

